using System.Diagnostics;

namespace test
{
    public partial class Form1 : Form
    {
        private PerformanceCounter? cpuCounter;
        private PerformanceCounter? ramCounter;
        private long totalMemory;
        private DriveInfo? systemDrive;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // 初始化 CPU 效能計數器
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                cpuCounter.NextValue(); // 第一次呼叫會返回 0，需要先呼叫一次

                // 初始化記憶體計數器
                ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                
                // 取得總記憶體
                var computerInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();
                totalMemory = (long)(computerInfo.TotalPhysicalMemory / (1024.0 * 1024.0 * 1024.0));

                // 取得系統磁碟
                systemDrive = DriveInfo.GetDrives().FirstOrDefault(d => 
                    d.IsReady && d.DriveType == DriveType.Fixed && d.Name.StartsWith("C"));

                // 啟動計時器
                timerUpdate.Start();
                
                // 立即更新一次
                UpdateSystemInfo();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"初始化錯誤: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            UpdateSystemInfo();
        }

        private void UpdateSystemInfo()
        {
            try
            {
                // 更新 CPU 使用率
                if (cpuCounter != null)
                {
                    float cpuUsage = cpuCounter.NextValue();
                    lblCpuValue.Text = $"{cpuUsage:F1}%";
                    progressBarCpu.Value = Math.Min((int)cpuUsage, 100);
                    
                    // 根據使用率改變顏色 - 專業配色
                    if (cpuUsage > 80)
                        lblCpuValue.ForeColor = Color.FromArgb(244, 67, 54); // 專業紅
                    else if (cpuUsage > 50)
                        lblCpuValue.ForeColor = Color.FromArgb(255, 183, 77); // 專業橙
                    else
                        lblCpuValue.ForeColor = Color.FromArgb(129, 199, 132); // 專業綠
                }

                // 更新記憶體使用
                if (ramCounter != null)
                {
                    float availableMemory = ramCounter.NextValue() / 1024.0f; // 轉換為 GB
                    float usedMemory = totalMemory - availableMemory;
                    float memoryPercent = (usedMemory / totalMemory) * 100;
                    
                    lblMemoryValue.Text = $"{usedMemory:F1} / {totalMemory:F1} GB ({memoryPercent:F0}%)";
                    progressBarMemory.Value = Math.Min((int)memoryPercent, 100);
                    
                    if (memoryPercent > 80)
                        lblMemoryValue.ForeColor = Color.FromArgb(244, 67, 54); // 專業紅
                    else if (memoryPercent > 60)
                        lblMemoryValue.ForeColor = Color.FromArgb(255, 183, 77); // 專業橙
                    else
                        lblMemoryValue.ForeColor = Color.FromArgb(129, 199, 132); // 專業綠
                }

                // 更新磁碟使用
                if (systemDrive != null && systemDrive.IsReady)
                {
                    double totalSize = systemDrive.TotalSize / (1024.0 * 1024.0 * 1024.0);
                    double freeSpace = systemDrive.AvailableFreeSpace / (1024.0 * 1024.0 * 1024.0);
                    double usedSpace = totalSize - freeSpace;
                    double diskPercent = (usedSpace / totalSize) * 100;
                    
                    lblDiskValue.Text = $"{usedSpace:F1} / {totalSize:F1} GB ({diskPercent:F0}%)";
                    progressBarDisk.Value = Math.Min((int)diskPercent, 100);
                    
                    if (diskPercent > 90)
                        lblDiskValue.ForeColor = Color.FromArgb(244, 67, 54); // 專業紅
                    else if (diskPercent > 70)
                        lblDiskValue.ForeColor = Color.FromArgb(255, 183, 77); // 專業橙
                    else
                        lblDiskValue.ForeColor = Color.FromArgb(129, 199, 132); // 專業綠
                }

                // 更新狀態
                lblStatus.Text = $"上次更新: {DateTime.Now:HH:mm:ss}";
                lblStatus.ForeColor = Color.FromArgb(158, 158, 158);
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"更新錯誤: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // 釋放資源
            timerUpdate.Stop();
            cpuCounter?.Dispose();
            ramCounter?.Dispose();
            base.OnFormClosing(e);
        }
    }
}