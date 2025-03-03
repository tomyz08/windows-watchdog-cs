namespace windows_watchdog_cs
{
    partial class WatchDog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tbLog = new TextBox();
            btnKill = new Button();
            tmrClock = new System.Windows.Forms.Timer(components);
            tabMenu = new TabControl();
            tpAllProcesses = new TabPage();
            gbAllProcesses = new GroupBox();
            btnRefresh = new Button();
            lbAllProcesses = new ListBox();
            btnMonitor = new Button();
            gbSearchAll = new GroupBox();
            txtSearchAll = new TextBox();
            tpMonitoredProcesses = new TabPage();
            gbMonitoredProcess = new GroupBox();
            lbMonitoredProcess = new ListBox();
            gbSearchMonitored = new GroupBox();
            txtSearchMonitored = new TextBox();
            tabMenu.SuspendLayout();
            tpAllProcesses.SuspendLayout();
            gbAllProcesses.SuspendLayout();
            gbSearchAll.SuspendLayout();
            tpMonitoredProcesses.SuspendLayout();
            gbMonitoredProcess.SuspendLayout();
            gbSearchMonitored.SuspendLayout();
            SuspendLayout();
            // 
            // tbLog
            // 
            tbLog.Location = new Point(411, 12);
            tbLog.Multiline = true;
            tbLog.Name = "tbLog";
            tbLog.ReadOnly = true;
            tbLog.ScrollBars = ScrollBars.Both;
            tbLog.Size = new Size(574, 499);
            tbLog.TabIndex = 0;
            // 
            // btnKill
            // 
            btnKill.Location = new Point(6, 431);
            btnKill.Name = "btnKill";
            btnKill.Size = new Size(373, 29);
            btnKill.TabIndex = 2;
            btnKill.Text = "Cerrar";
            btnKill.UseVisualStyleBackColor = true;
            btnKill.Click += btnKill_Click;
            // 
            // tmrClock
            // 
            tmrClock.Tick += tmrClock_Tick;
            // 
            // tabMenu
            // 
            tabMenu.Controls.Add(tpAllProcesses);
            tabMenu.Controls.Add(tpMonitoredProcesses);
            tabMenu.Location = new Point(12, 12);
            tabMenu.Name = "tabMenu";
            tabMenu.SelectedIndex = 0;
            tabMenu.Size = new Size(393, 499);
            tabMenu.TabIndex = 7;
            // 
            // tpAllProcesses
            // 
            tpAllProcesses.Controls.Add(gbAllProcesses);
            tpAllProcesses.Controls.Add(btnMonitor);
            tpAllProcesses.Controls.Add(gbSearchAll);
            tpAllProcesses.Location = new Point(4, 29);
            tpAllProcesses.Name = "tpAllProcesses";
            tpAllProcesses.Padding = new Padding(3);
            tpAllProcesses.Size = new Size(385, 466);
            tpAllProcesses.TabIndex = 0;
            tpAllProcesses.Text = "Activos";
            tpAllProcesses.UseVisualStyleBackColor = true;
            // 
            // gbAllProcesses
            // 
            gbAllProcesses.Controls.Add(btnRefresh);
            gbAllProcesses.Controls.Add(lbAllProcesses);
            gbAllProcesses.Location = new Point(6, 82);
            gbAllProcesses.Name = "gbAllProcesses";
            gbAllProcesses.Size = new Size(367, 337);
            gbAllProcesses.TabIndex = 9;
            gbAllProcesses.TabStop = false;
            gbAllProcesses.Text = "Procesos Activos";
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(6, 295);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(355, 36);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "Actualizar";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // lbAllProcesses
            // 
            lbAllProcesses.FormattingEnabled = true;
            lbAllProcesses.Location = new Point(6, 26);
            lbAllProcesses.Name = "lbAllProcesses";
            lbAllProcesses.Size = new Size(355, 264);
            lbAllProcesses.TabIndex = 4;
            // 
            // btnMonitor
            // 
            btnMonitor.Location = new Point(6, 425);
            btnMonitor.Name = "btnMonitor";
            btnMonitor.Size = new Size(373, 35);
            btnMonitor.TabIndex = 7;
            btnMonitor.Text = "Monitorear";
            btnMonitor.UseVisualStyleBackColor = true;
            btnMonitor.Click += btnMonitor_Click;
            // 
            // gbSearchAll
            // 
            gbSearchAll.Controls.Add(txtSearchAll);
            gbSearchAll.Location = new Point(6, 6);
            gbSearchAll.Name = "gbSearchAll";
            gbSearchAll.Size = new Size(367, 70);
            gbSearchAll.TabIndex = 8;
            gbSearchAll.TabStop = false;
            gbSearchAll.Text = "Busqueda";
            // 
            // txtSearchAll
            // 
            txtSearchAll.Location = new Point(6, 26);
            txtSearchAll.Name = "txtSearchAll";
            txtSearchAll.Size = new Size(355, 27);
            txtSearchAll.TabIndex = 5;
            txtSearchAll.TextChanged += txtSearchAll_TextChanged;
            // 
            // tpMonitoredProcesses
            // 
            tpMonitoredProcesses.Controls.Add(gbMonitoredProcess);
            tpMonitoredProcesses.Controls.Add(gbSearchMonitored);
            tpMonitoredProcesses.Controls.Add(btnKill);
            tpMonitoredProcesses.Location = new Point(4, 29);
            tpMonitoredProcesses.Name = "tpMonitoredProcesses";
            tpMonitoredProcesses.Padding = new Padding(3);
            tpMonitoredProcesses.Size = new Size(385, 466);
            tpMonitoredProcesses.TabIndex = 1;
            tpMonitoredProcesses.Text = "Monitoreados";
            tpMonitoredProcesses.UseVisualStyleBackColor = true;
            // 
            // gbMonitoredProcess
            // 
            gbMonitoredProcess.Controls.Add(lbMonitoredProcess);
            gbMonitoredProcess.Location = new Point(6, 82);
            gbMonitoredProcess.Name = "gbMonitoredProcess";
            gbMonitoredProcess.Size = new Size(367, 337);
            gbMonitoredProcess.TabIndex = 11;
            gbMonitoredProcess.TabStop = false;
            gbMonitoredProcess.Text = "Procesos Monitoreados";
            // 
            // lbMonitoredProcess
            // 
            lbMonitoredProcess.FormattingEnabled = true;
            lbMonitoredProcess.Location = new Point(6, 26);
            lbMonitoredProcess.Name = "lbMonitoredProcess";
            lbMonitoredProcess.Size = new Size(355, 284);
            lbMonitoredProcess.TabIndex = 4;
            // 
            // gbSearchMonitored
            // 
            gbSearchMonitored.Controls.Add(txtSearchMonitored);
            gbSearchMonitored.Location = new Point(6, 6);
            gbSearchMonitored.Name = "gbSearchMonitored";
            gbSearchMonitored.Size = new Size(367, 70);
            gbSearchMonitored.TabIndex = 10;
            gbSearchMonitored.TabStop = false;
            gbSearchMonitored.Text = "Busqueda";
            // 
            // txtSearchMonitored
            // 
            txtSearchMonitored.Location = new Point(6, 26);
            txtSearchMonitored.Name = "txtSearchMonitored";
            txtSearchMonitored.Size = new Size(355, 27);
            txtSearchMonitored.TabIndex = 5;
            txtSearchMonitored.TextChanged += txtSearchMonitored_TextChanged;
            // 
            // WatchDog
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(997, 523);
            Controls.Add(tabMenu);
            Controls.Add(tbLog);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "WatchDog";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "WatchDog";
            Load += WatchDog_Load;
            tabMenu.ResumeLayout(false);
            tpAllProcesses.ResumeLayout(false);
            gbAllProcesses.ResumeLayout(false);
            gbSearchAll.ResumeLayout(false);
            gbSearchAll.PerformLayout();
            tpMonitoredProcesses.ResumeLayout(false);
            gbMonitoredProcess.ResumeLayout(false);
            gbSearchMonitored.ResumeLayout(false);
            gbSearchMonitored.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbLog;
        private Button btnKill;
        private System.Windows.Forms.Timer tmrClock;
        private TabControl tabMenu;
        private TabPage tpAllProcesses;
        private GroupBox gbAllProcesses;
        private ListBox lbAllProcesses;
        private Button btnMonitor;
        private GroupBox gbSearchAll;
        private TextBox txtSearchAll;
        private TabPage tpMonitoredProcesses;
        private GroupBox gbMonitoredProcess;
        private ListBox lbMonitoredProcess;
        private GroupBox gbSearchMonitored;
        private TextBox txtSearchMonitored;
        private Button btnRefresh;
    }
}