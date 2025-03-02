using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private class MonitoredProcess
        {
            public string ProcessName { get; set; }
            public string FileName { get; set; }
            public bool IsMonitored { get; set; }
        }

        private List<MonitoredProcess> GetAllProcesses()
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
        }

        private void StartProcess(string filePath)
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
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = filePath,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                LogMessage($"{Path.GetFileName(filePath)} se ha iniciado correctamente.");
            }
            catch (Exception e)
            {
                LogMessage($"Error iniciando {filePath}: {e.Message}");
            }
        }

        private void KillAdditionalInstances(Process[] instances, string processName)
        {
            for (int i = 1; i < instances.Length; i++)
            {
                try
                {
                    instances[i].Kill();
                    instances[i].WaitForExit();
                }
                catch (Exception e)
                {
                    LogMessage($"Error terminando instancias adicionales de {processName}: {e.Message}");
                }
            }
            LogMessage($"Instancias adicionales de {processName} terminadas.");
        }

        private string GetProcessFilePath(string processName)
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length > 0)
                {
                    return processes[0].MainModule.FileName;
                }
            }
            catch (Exception ex)
            {
                LogMessage($"No se pudo obtener la ruta de {processName}: {ex.Message}");
            }

            return "Acceso denegado";
        }

        private void SaveMonitoredProcesses()
        {
            try
            {
                string directory = Path.GetDirectoryName(configFile);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                string json = JsonSerializer.Serialize(monitoredProcesses, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configFile, json);
                LogMessage("Lista de procesos monitoreados guardada correctamente.");
            }
            catch (Exception ex)
            {
                LogMessage($"Error guardando configuracion: {ex.Message}");
            }
        }

        private void WatchDog_Load(object sender, EventArgs e)
        {
            lbAllProcesses.DataSource = GetAllProcesses();
            lbAllProcesses.DisplayMember = "ProcessName";
            if (File.Exists(configFile))
            {
                try
                {
                    string json = File.ReadAllText(configFile);
                    monitoredProcesses = JsonSerializer.Deserialize<List<MonitoredProcess>>(json);
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
                SaveMonitoredProcesses();
            }
            tmrClock.Interval = 5000;
            tmrClock.Start();
        }

        private void btnMonitor_Click(object sender, EventArgs e)
        {
            string selectedProcess = lbAllProcesses.Text;
            DialogResult dialog = MessageBox.Show("Deseas empezar a monitorea esta app/proceso?", "Confirmacion de Monitoreo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                string filePath = GetProcessFilePath(selectedProcess);
                monitoredProcesses.Add(new MonitoredProcess
                {
                    ProcessName = selectedProcess,
                    FileName = filePath,
                    IsMonitored = true
                });
                SaveMonitoredProcesses();
                LogMessage($"{selectedProcess} agregado a la lista de monitoreo.");
            }
        }

        private void tmrClock_Tick(object sender, EventArgs e)
        {
            foreach (var monitored in monitoredProcesses)
            {
                var processes = Process.GetProcessesByName(monitored.ProcessName);
                if (processes.Length == 0)
                {
                    LogMessage($"Se detecto que {monitored.ProcessName} se ha detenido. Iniciandolo nuevamente...");
                    StartProcess(monitored.FileName);
                }
                else if (processes.Length > 1)
                {
                    LogMessage($"Se detectaron instancias adicionales de {monitored.ProcessName}. Terminando...");
                    KillAdditionalInstances(processes, monitored.ProcessName);
                }
            }
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

        private void btnKill_Click(object sender, EventArgs e)
        {
            string selectedProcess = lbMonitoredProcess.Text;
            try
            {
                Process[] processes = Process.GetProcessesByName(selectedProcess);
                if (processes.Length > 0)
                {
                    processes[0].Kill();
                    processes[0].WaitForExit();
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lbAllProcesses.DataSource = GetAllProcesses();
            lbAllProcesses.DisplayMember = "ProcessName";
        }
    }
}
