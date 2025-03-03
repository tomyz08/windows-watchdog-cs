﻿using System.ComponentModel;
using System.Diagnostics;
using windows_watchdog_cs.Models;
using windows_watchdog_cs.Services;

namespace windows_watchdog_cs
{
    public partial class WatchDog : Form
    {
        private readonly ProcessService processService = new ProcessService();
        private readonly FileService fileService;
        private List<MonitoredProcess> monitoredProcesses = new List<MonitoredProcess>();

        public WatchDog()
        {
            InitializeComponent();
            fileService = new FileService("monitored-apps.json");
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

        private async void WatchDog_Load(object sender, EventArgs e)
        {
            lbAllProcesses.DataSource = await processService.GetAllProcesses();
            lbAllProcesses.DisplayMember = "ProcessName";

            monitoredProcesses = await fileService.LoadMonitoredProcesses(LogMessage);
            lbMonitoredProcess.DataSource = monitoredProcesses;
            lbMonitoredProcess.DisplayMember = "ProcessName";

            tmrClock.Interval = 5000;
            tmrClock.Start();
        }

        private async void btnMonitor_Click(object sender, EventArgs e)
        {
            string selectedProcess = lbAllProcesses.Text;
            var dialog = MessageBox.Show("Deseas empezar a monitorea esta app/proceso?", "Confirmacion de Monitoreo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog != DialogResult.Yes)
                return;
            string filePath = await processService.GetProcessFilePath(selectedProcess, LogMessage);
            if (filePath == "Acceso denegado" || filePath == "Proceso no encontrado")
            {
                LogMessage($"No se puede monitorear \"{selectedProcess}\". {filePath}");
                return;
            }
            monitoredProcesses.Add(new MonitoredProcess
            {
                ProcessName = selectedProcess,
                FileName = filePath,
                IsMonitored = true
            });
            await fileService.SaveMonitoredProcesses(monitoredProcesses, LogMessage);
            LogMessage($"{selectedProcess} agregado a la lista de monitoreo.");
        }

        private async void tmrClock_Tick(object sender, EventArgs e)
        {
            foreach (var monitored in monitoredProcesses)
            {
                var processes = Process.GetProcessesByName(monitored.ProcessName);
                if (processes.Length == 0)
                {
                    LogMessage($"Se detecto que {monitored.ProcessName} se ha detenido. Iniciandolo nuevamente...");
                    await processService.StartProcess(monitored.FileName, LogMessage);
                }
                else if (processes.Length > 1)
                {
                    LogMessage($"Se detectaron instancias adicionales de {monitored.ProcessName}. Terminando...");
                    await processService.KillAdditionalInstances(processes, monitored.ProcessName, LogMessage);
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
                    await processService.KillProcess(processes[0]);
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
            lbAllProcesses.DataSource = await processService.GetAllProcesses();
            lbAllProcesses.DisplayMember = "ProcessName";
        }
    }
}
