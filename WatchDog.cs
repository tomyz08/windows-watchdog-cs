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

namespace windows_watchdog_cs
{
    public partial class WatchDog : Form
    {
        public WatchDog()
        {
            InitializeComponent();
        }
        private Process process;
        private class _Process
        {
            public int Id { get; set; }
            public string ProcessName { get; set; }
            public string FileName { get; set; }
        }

        private List<_Process> GetProcesses()
        {
            Process[] processes = Process.GetProcesses();
            List<_Process> _processes = new List<_Process>();
            foreach (Process process in processes)
            {
                _processes.Add(new _Process()
                {
                    Id = process.Id,
                    ProcessName = process.ProcessName,
                    FileName = process.StartInfo.FileName
                });
            }
            return _processes;            
        }

        private void WatchDog_Load(object sender, EventArgs e)
        {
            string json = JsonSerializer.Serialize(GetProcesses());
            lbProcesses.DataSource = GetProcesses();
            lbProcesses.DisplayMember = "ProcessName";
            return;
        }

        private void btnMonitor_Click(object sender, EventArgs e)
        {
            string selectedProcess = lbProcesses.Text;
            DialogResult dialog = MessageBox.Show("Deseas empezar a monitorea esta app/proceso?", "Confirmacion de Monitoreo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if(dialog == DialogResult.Yes)
            {
                using (process = new Process())
                {
                    process.StartInfo.FileName = selectedProcess + ".exe";
                }
            }
            else
            {
                return;
            }
        }
    }
}
