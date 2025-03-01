using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

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

        private void WatchDog_Load(object sender, EventArgs e)
        {
            lbProcesses.DataSource = GetAllProcesses();
            lbProcesses.DisplayMember = "ProcessName";
            if (File.Exists(configFile))
            {
                try
                {
                    string json = File.ReadAllText(configFile);
                    monitoredProcesses = JsonSerializer.Deserialize<List<MonitoredProcess>>(json);
                }
                catch (Exception ex)
                {
                    LogMessage($"Error leyendo configuración: {ex.Message}");
                }
            }
        }

        private void btnMonitor_Click(object sender, EventArgs e)
        {
            string selectedProcess = lbProcesses.Text;
            DialogResult dialog = MessageBox.Show("Deseas empezar a monitorea esta app/proceso?", "Confirmacion de Monitoreo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                monitoredProcesses.Add(new MonitoredProcess
                {
                    ProcessName = selectedProcess,
                    IsMonitored = true
                });
                SaveMonitoredProcesses();
                LogMessage($"{selectedProcess} agregado a la lista de monitoreo.");
            }
        }

        private void SaveMonitoredProcesses()
        {
            try
            {
                string json = JsonSerializer.Serialize(monitoredProcesses, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configFile, json);
            }
            catch (Exception ex)
            {
                LogMessage($"Error guardando configuración: {ex.Message}");
            }
        }

        private void tmrClock_Tick(object sender, EventArgs e)
        {
            foreach (var monitored in monitoredProcesses)
            {
                var processes = Process.GetProcessesByName(monitored.ProcessName);
                if (processes.Length == 0)
                {
                    LogMessage($"Se detectó que {monitored.ProcessName} se ha detenido. Iniciándolo nuevamente...");
                    StartProcess(monitored.FileName);
                }
                else if (processes.Length > 1)
                {
                    LogMessage($"Se detectaron instancias adicionales de {monitored.ProcessName}. Terminándolas...");
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
            tbProcess.AppendText($"{DateTime.Now}: {message}{Environment.NewLine}");
        }
    }
}
