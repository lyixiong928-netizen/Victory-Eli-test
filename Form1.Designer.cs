namespace test
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblCpuTitle = new System.Windows.Forms.Label();
            this.lblCpuValue = new System.Windows.Forms.Label();
            this.lblMemoryTitle = new System.Windows.Forms.Label();
            this.lblMemoryValue = new System.Windows.Forms.Label();
            this.lblDiskTitle = new System.Windows.Forms.Label();
            this.lblDiskValue = new System.Windows.Forms.Label();
            this.progressBarCpu = new System.Windows.Forms.ProgressBar();
            this.progressBarMemory = new System.Windows.Forms.ProgressBar();
            this.progressBarDisk = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.timerUpdate = new System.Windows.Forms.Timer();
            this.SuspendLayout();
            // 
            // lblCpuTitle
            // 
            this.lblCpuTitle.AutoSize = true;
            this.lblCpuTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblCpuTitle.Font = new System.Drawing.Font("Microsoft JhengHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCpuTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(181)))), ((int)(((byte)(246)))));
            this.lblCpuTitle.Location = new System.Drawing.Point(25, 30);
            this.lblCpuTitle.Name = "lblCpuTitle";
            this.lblCpuTitle.Size = new System.Drawing.Size(99, 19);
            this.lblCpuTitle.TabIndex = 0;
            this.lblCpuTitle.Text = "CPU 使用率:";
            // 
            // lblCpuValue
            // 
            this.lblCpuValue.AutoSize = true;
            this.lblCpuValue.BackColor = System.Drawing.Color.Transparent;
            this.lblCpuValue.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            this.lblCpuValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblCpuValue.Location = new System.Drawing.Point(145, 30);
            this.lblCpuValue.Name = "lblCpuValue";
            this.lblCpuValue.Size = new System.Drawing.Size(36, 19);
            this.lblCpuValue.TabIndex = 1;
            this.lblCpuValue.Text = "0%";
            // 
            // lblMemoryTitle
            // 
            this.lblMemoryTitle.AutoSize = true;
            this.lblMemoryTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblMemoryTitle.Font = new System.Drawing.Font("Microsoft JhengHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblMemoryTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(199)))), ((int)(((byte)(132)))));
            this.lblMemoryTitle.Location = new System.Drawing.Point(25, 100);
            this.lblMemoryTitle.Name = "lblMemoryTitle";
            this.lblMemoryTitle.Size = new System.Drawing.Size(99, 19);
            this.lblMemoryTitle.TabIndex = 2;
            this.lblMemoryTitle.Text = "記憶體使用getsuon:";
            // 
            // lblMemoryValue
            // 
            this.lblMemoryValue.AutoSize = true;
            this.lblMemoryValue.BackColor = System.Drawing.Color.Transparent;
            this.lblMemoryValue.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            this.lblMemoryValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblMemoryValue.Location = new System.Drawing.Point(145, 100);
            this.lblMemoryValue.Name = "lblMemoryValue";
            this.lblMemoryValue.Size = new System.Drawing.Size(70, 19);
            this.lblMemoryValue.TabIndex = 3;
            this.lblMemoryValue.Text = "0 / 0 GB";
            // 
            // lblDiskTitle
            // 
            this.lblDiskTitle.AutoSize = true;
            this.lblDiskTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblDiskTitle.Font = new System.Drawing.Font("Microsoft JhengHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDiskTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(183)))), ((int)(((byte)(77)))));
            this.lblDiskTitle.Location = new System.Drawing.Point(25, 170);
            this.lblDiskTitle.Name = "lblDiskTitle";
            this.lblDiskTitle.Size = new System.Drawing.Size(84, 19);
            this.lblDiskTitle.TabIndex = 4;
            this.lblDiskTitle.Text = "磁碟使用:";
            // 
            // lblDiskValue
            // 
            this.lblDiskValue.AutoSize = true;
            this.lblDiskValue.BackColor = System.Drawing.Color.Transparent;
            this.lblDiskValue.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            this.lblDiskValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblDiskValue.Location = new System.Drawing.Point(130, 170);
            this.lblDiskValue.Name = "lblDiskValue";
            this.lblDiskValue.Size = new System.Drawing.Size(70, 19);
            this.lblDiskValue.TabIndex = 5;
            this.lblDiskValue.Text = "0 / 0 GB";
            // 
            // progressBarCpu
            // 
            this.progressBarCpu.Location = new System.Drawing.Point(25, 58);
            this.progressBarCpu.Name = "progressBarCpu";
            this.progressBarCpu.Size = new System.Drawing.Size(450, 26);
            this.progressBarCpu.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarCpu.TabIndex = 6;
            // 
            // progressBarMemory
            // 
            this.progressBarMemory.Location = new System.Drawing.Point(25, 128);
            this.progressBarMemory.Name = "progressBarMemory";
            this.progressBarMemory.Size = new System.Drawing.Size(450, 26);
            this.progressBarMemory.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarMemory.TabIndex = 7;
            // 
            // progressBarDisk
            // 
            this.progressBarDisk.Location = new System.Drawing.Point(25, 198);
            this.progressBarDisk.Name = "progressBarDisk";
            this.progressBarDisk.Size = new System.Drawing.Size(450, 26);
            this.progressBarDisk.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarDisk.TabIndex = 8;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(158)))), ((int)(((byte)(158)))));
            this.lblStatus.Location = new System.Drawing.Point(25, 240);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(79, 15);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "正在監控中...";
            // 
            // timerUpdate
            // 
            this.timerUpdate.Interval = 1000;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(500, 280);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBarDisk);
            this.Controls.Add(this.progressBarMemory);
            this.Controls.Add(this.progressBarCpu);
            this.Controls.Add(this.lblDiskValue);
            this.Controls.Add(this.lblDiskTitle);
            this.Controls.Add(this.lblMemoryValue);
            this.Controls.Add(this.lblMemoryTitle);
            this.Controls.Add(this.lblCpuValue);
            this.Controls.Add(this.lblCpuTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系統資源監控器 - Professional Edition";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblCpuTitle;
        private Label lblCpuValue;
        private Label lblMemoryTitle;
        private Label lblMemoryValue;
        private Label lblDiskTitle;
        private Label lblDiskValue;
        private ProgressBar progressBarCpu;
        private ProgressBar progressBarMemory;
        private ProgressBar progressBarDisk;
        private Label lblStatus;
        private System.Windows.Forms.Timer timerUpdate;
    }
}