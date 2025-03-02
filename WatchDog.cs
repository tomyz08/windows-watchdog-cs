using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using windows_watchdog_cs.Models;

namespace windows_watchdog_cs
{
    public partial class WatchDog : Form
    {
        private List<MonitoredProcess> monitoredProcesses = new List<MonitoredProcess>();
        private string configFile = "monitored-apps.json";

        public WatchDog()
        {
            InitializeComponent();
        }

        private void LogMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => LogMessage(message)));
                return;
            }
            tbLog.AppendText($"{DateTime.Now}: {message}{Environment.NewLine}");
        }

        private async Task<List<MonitoredProcess>> GetAllProcesses()
        {
            return await Task.Run(() =>
            {
                var processes = Process.GetProcesses();
                var _processes = new List<MonitoredProcess>();
                foreach (var process in processes)
                {
                    _processes.Add(new MonitoredProcess()
                    {
                        ProcessName = process.ProcessName,
                        IsMonitored = false
                    });
                }
                return _processes;
            });            
        }

        private async Task StartProcess(string filePath)
        {
            try
            {
                if (filePath == "Acceso denegado")
                {
                    LogMessage("No se puede iniciar el proceso porque el acceso este esta restringido.");
                    return;
                }
                if (!File.Exists(filePath))
                {
                    LogMessage($"El archivo \"{filePath}\" no existe. No se puede iniciar el proceso.");
                    return;
                }
                await Task.Run(() =>
                {
                    Process process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = filePath,
                            CreateNoWindow = true
                        }
                    };
                    process.Start();
                });                
                LogMessage($"{Path.GetFileName(filePath)} se ha iniciado correctamente.");
            }
            catch (Exception e)
            {
                LogMessage($"Error iniciando {filePath}: {e.Message}");
            }
        }

        private async Task KillProcess(Process process)
        {
            process.Kill();
            await process.WaitForExitAsync();
        }

        private async Task KillAdditionalInstances(Process[] instances, string processName)
        {
            for (int i = 1; i < instances.Length; i++)
            {
                try
                {
                    await KillProcess(instances[i]);
                }
                catch (Exception e)
                {
                    LogMessage($"Error terminando instancias adicionales de {processName}: {e.Message}");
                }
            }
            LogMessage($"Instancias adicionales de {processName} terminadas.");
        }

        private async Task<string> GetProcessFilePath(string processName)
        {
            return await Task.Run(() =>
            {
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length > 0)
                {
                    try
                    {
                        return processes[0].MainModule.FileName;
                    }
                    catch (Win32Exception) // Se captura específicamente el error de acceso denegado
                    {
                        return "Acceso denegado";
                    }
                    catch (Exception ex) // Se captura cualquier otro error inesperado
                    {
                        LogMessage($"Error obteniendo la ruta de {processName}: {ex.Message}");
                        return "Acceso denegado";
                    }
                }
                return "Proceso no encontrado";
            });            
        }

        private async Task SaveMonitoredProcesses()
        {
            try
            {
                string directory = Path.GetDirectoryName(configFile);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                await using FileStream writeStream = File.Create(configFile);
                await JsonSerializer.SerializeAsync(writeStream, monitoredProcesses, new JsonSerializerOptions { WriteIndented = true });                
                LogMessage("Lista de procesos monitoreados guardada correctamente.");
            }
            catch (Exception ex)
            {
                LogMessage($"Error guardando configuracion: {ex.Message}");
            }
        }

        private async void WatchDog_Load(object sender, EventArgs e)
        {
            lbAllProcesses.DataSource = await GetAllProcesses();
            lbAllProcesses.DisplayMember = "ProcessName";
            if (File.Exists(configFile))
            {
                try
                {
                    using FileStream openStream = File.OpenRead(configFile);
                    monitoredProcesses = await JsonSerializer.DeserializeAsync<List<MonitoredProcess>>(openStream);
                    lbMonitoredProcess.DataSource = monitoredProcesses;
                    lbMonitoredProcess.DisplayMember = "ProcessName";
                }
                catch (Exception ex)
                {
                    LogMessage($"Error leyendo configuracion: {ex.Message}");
                }
            }
            else
            {
                LogMessage("No se encontro un archivo de configuracion. Se creará un nuevo archivo.");
                monitoredProcesses = new List<MonitoredProcess>();
                await SaveMonitoredProcesses();
            }
            tmrClock.Interval = 5000;
            tmrClock.Start();
        }

        private async void btnMonitor_Click(object sender, EventArgs e)
        {
            string selectedProcess = lbAllProcesses.Text;
            var dialog = MessageBox.Show("Deseas empezar a monitorea esta app/proceso?", "Confirmacion de Monitoreo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog != DialogResult.Yes)
                return;
            try
            {
                string filePath = await GetProcessFilePath(selectedProcess);
                if (filePath == "Acceso denegado")
                {
                    LogMessage($"No se puede monitorear \"{selectedProcess}\" debido a restricciones de acceso.");
                    return;
                }

                if (filePath == "Proceso no encontrado")
                {
                    LogMessage($"No se puede monitorear \"{selectedProcess}\" porque no está en ejecución.");
                    return;
                }
                monitoredProcesses.Add(new MonitoredProcess
                {
                    ProcessName = selectedProcess,
                    FileName = filePath,
                    IsMonitored = true
                });
                await SaveMonitoredProcesses();
                LogMessage($"{selectedProcess} agregado a la lista de monitoreo.");
            }
            catch (Exception ex)
            {
                LogMessage($"Error al intentar monitorear \"{selectedProcess}\": {ex.Message}");
            }
        }

        private async void tmrClock_Tick(object sender, EventArgs e)
        {
            foreach (var monitored in monitoredProcesses)
            {
                var processes = Process.GetProcessesByName(monitored.ProcessName);
                if (processes.Length == 0)
                {
                    LogMessage($"Se detecto que {monitored.ProcessName} se ha detenido. Iniciandolo nuevamente...");
                    await StartProcess(monitored.FileName);
                }
                else if (processes.Length > 1)
                {
                    LogMessage($"Se detectaron instancias adicionales de {monitored.ProcessName}. Terminando...");
                    await KillAdditionalInstances(processes, monitored.ProcessName);
                }
            }
        }

        private async void btnKill_Click(object sender, EventArgs e)
        {
            string selectedProcess = lbMonitoredProcess.Text;
            try
            {
                Process[] processes = Process.GetProcessesByName(selectedProcess);
                if (processes.Length > 0)
                {
                    await KillProcess(processes[0]);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Error terminando el proceso {selectedProcess}: {ex.Message}");
            }
            LogMessage($"Se termino el proceso {selectedProcess} correctamente.");
        }

        private void txtSearchAll_TextChanged(object sender, EventArgs e)
        {
            lbAllProcesses.SelectedIndex = lbAllProcesses.FindString(txtSearchAll.Text);
        }

        private void txtSearchMonitored_TextChanged(object sender, EventArgs e)
        {
            lbMonitoredProcess.SelectedIndex = lbMonitoredProcess.FindString(txtSearchMonitored.Text);
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            lbAllProcesses.DataSource = await GetAllProcesses();
            lbAllProcesses.DisplayMember = "ProcessName";
        }
    }
}
