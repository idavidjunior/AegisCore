using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AegisCore.UI.ViewModels;

public class DashboardViewModel : INotifyPropertyChanged
{
    private double _cpuUsage;
    private double _ramTotal;
    private double _ramFree;
    private double _diskUsed;
    private double _diskFree;
    private bool _isMonitoring;

    public event PropertyChangedEventHandler? PropertyChanged;

    public DashboardViewModel()
    {
        StartMonitoringCommand = new RelayCommand(async () => await StartMonitoringAsync());
        StopMonitoringCommand = new RelayCommand(() => StopMonitoringAsync());
    }

    public double CpuUsage
    {
        get => _cpuUsage;
        set { _cpuUsage = value; OnPropertyChanged(); }
    }

    public double RamTotal
    {
        get => _ramTotal;
        set { _ramTotal = value; OnPropertyChanged(); }
    }

    public double RamFree
    {
        get => _ramFree;
        set { _ramFree = value; OnPropertyChanged(); }
    }

    public double RamUsed => RamTotal - RamFree;

    public double DiskUsed
    {
        get => _diskUsed;
        set { _diskUsed = value; OnPropertyChanged(); }
    }

    public double DiskFree
    {
        get => _diskFree;
        set { _diskFree = value; OnPropertyChanged(); }
    }

    public bool IsMonitoring
    {
        get => _isMonitoring;
        set { _isMonitoring = value; OnPropertyChanged(); }
    }

    public ICommand StartMonitoringCommand { get; }
    public ICommand StopMonitoringCommand { get; }

    private async Task StartMonitoringAsync()
    {
        if (IsMonitoring) return;
        IsMonitoring = true;

        await Task.Run(async () =>
        {
            while (IsMonitoring)
            {
                await UpdateSystemStatsAsync();
                await Task.Delay(2000); // Atualiza a cada 2 segundos
            }
        });
    }

    private async Task StopMonitoringAsync()
    {
        IsMonitoring = false;
        await Task.CompletedTask;
    }

    private async Task UpdateSystemStatsAsync()
    {
        await Task.Run(() =>
        {
            try
            {
                // CPU Usage
                var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                cpuCounter.NextValue(); // Primeira leitura retorna 0
                System.Threading.Thread.Sleep(1000);
                CpuUsage = cpuCounter.NextValue();

                // RAM
                var ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                RamFree = ramCounter.NextValue() / 1024.0; // Converter para GB
                
                // Total RAM via WMI
                using (var searcher = new System.Management.ManagementObjectSearcher("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        var totalRamKb = Convert.ToDouble(obj["TotalVisibleMemorySize"]);
                        RamTotal = totalRamKb / 1024.0 / 1024.0; // Convert KB to GB
                        break;
                    }
                }

                // Disco C:
                var driveInfo = new DriveInfo("C:\\");
                DiskUsed = driveInfo.TotalSize / (1024.0 * 1024.0 * 1024.0) - 
                          driveInfo.AvailableFreeSpace / (1024.0 * 1024.0 * 1024.0);
                DiskFree = driveInfo.AvailableFreeSpace / (1024.0 * 1024.0 * 1024.0);
            }
            catch
            {
                // Se falhar, manter valores anteriores ou definir como 0
            }
        });
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
