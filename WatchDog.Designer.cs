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
            tbProcess = new TextBox();
            btnMonitor = new Button();
            btnKill = new Button();
            lbProcesses = new ListBox();
            txtSearch = new TextBox();
            tmrClock = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // tbProcess
            // 
            tbProcess.Location = new Point(353, 33);
            tbProcess.Multiline = true;
            tbProcess.Name = "tbProcess";
            tbProcess.Size = new Size(781, 559);
            tbProcess.TabIndex = 0;
            // 
            // btnMonitor
            // 
            btnMonitor.Dock = DockStyle.Bottom;
            btnMonitor.Location = new Point(0, 643);
            btnMonitor.Name = "btnMonitor";
            btnMonitor.Size = new Size(1134, 29);
            btnMonitor.TabIndex = 1;
            btnMonitor.Text = "Monitorear";
            btnMonitor.UseVisualStyleBackColor = true;
            btnMonitor.Click += btnMonitor_Click;
            // 
            // btnKill
            // 
            btnKill.Dock = DockStyle.Bottom;
            btnKill.Location = new Point(0, 614);
            btnKill.Name = "btnKill";
            btnKill.Size = new Size(1134, 29);
            btnKill.TabIndex = 2;
            btnKill.Text = "Cerrar";
            btnKill.UseVisualStyleBackColor = true;
            // 
            // lbProcesses
            // 
            lbProcesses.FormattingEnabled = true;
            lbProcesses.Location = new Point(0, 33);
            lbProcesses.Name = "lbProcesses";
            lbProcesses.Size = new Size(347, 564);
            lbProcesses.TabIndex = 3;
            // 
            // txtSearch
            // 
            txtSearch.Dock = DockStyle.Top;
            txtSearch.Location = new Point(0, 0);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(1134, 27);
            txtSearch.TabIndex = 4;
            // 
            // tmrClock
            // 
            tmrClock.Tick += tmrClock_Tick;
            // 
            // WatchDog
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1134, 672);
            Controls.Add(txtSearch);
            Controls.Add(lbProcesses);
            Controls.Add(btnKill);
            Controls.Add(btnMonitor);
            Controls.Add(tbProcess);
            Name = "WatchDog";
            Text = "WatchDog";
            Load += WatchDog_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbProcess;
        private Button btnMonitor;
        private Button btnKill;
        private ListBox lbProcesses;
        private TextBox txtSearch;
        private System.Windows.Forms.Timer tmrClock;
    }
}