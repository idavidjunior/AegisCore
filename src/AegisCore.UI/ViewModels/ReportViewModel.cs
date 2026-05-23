using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace AegisCore.UI.ViewModels;

public class ReportViewModel : INotifyPropertyChanged
{
    private double _ramBefore;
    private double _ramAfter;
    private int _modulesCompleted;
    private int _modulesTotal;
    private int _modulesFailed;
    private ObservableCollection<string> _failedModules;
    private ObservableCollection<string> _logEntries;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ReportViewModel()
    {
        _failedModules = new ObservableCollection<string>();
        _logEntries = new ObservableCollection<string>();

        RestoreCommand = new RelayCommand(() => RestoreDefaults());
        ExportLogCommand = new RelayCommand(() => ExportLog());
        ExitCommand = new RelayCommand(() => Exit());
        NewScanCommand = new RelayCommand(() => NewScan());
    }

    public double RamBefore
    {
        get => _ramBefore;
        set { _ramBefore = value; OnPropertyChanged(); }
    }

    public double RamAfter
    {
        get => _ramAfter;
        set { _ramAfter = value; OnPropertyChanged(); }
    }

    public double RamDifference => RamAfter - RamBefore;

    public int ModulesCompleted
    {
        get => _modulesCompleted;
        set { _modulesCompleted = value; OnPropertyChanged(); }
    }

    public int ModulesTotal
    {
        get => _modulesTotal;
        set { _modulesTotal = value; OnPropertyChanged(); }
    }

    public int ModulesFailed
    {
        get => _modulesFailed;
        set { _modulesFailed = value; OnPropertyChanged(); }
    }

    public ObservableCollection<string> FailedModules
    {
        get => _failedModules;
        set { _failedModules = value; OnPropertyChanged(); }
    }

    public ObservableCollection<string> LogEntries
    {
        get => _logEntries;
        set { _logEntries = value; OnPropertyChanged(); }
    }

    public string StatusMessage => ModulesFailed == 0 
        ? "✓ Scan completed successfully" 
        : $"⚠ Scan completed with {ModulesFailed} error(s)";

    public ICommand RestoreCommand { get; }
    public ICommand ExportLogCommand { get; }
    public ICommand ExitCommand { get; }
    public ICommand NewScanCommand { get; }

    public event Action? OnRestoreRequested;
    public event Action? OnNewScanRequested;
    public event Action? OnExitRequested;

    public void SetReportData(double ramBefore, double ramAfter, int completed, int total, int failed, ObservableCollection<string> failedMods, ObservableCollection<string> logs)
    {
        RamBefore = ramBefore;
        RamAfter = ramAfter;
        ModulesCompleted = completed;
        ModulesTotal = total;
        ModulesFailed = failed;
        
        FailedModules.Clear();
        foreach (var mod in failedMods)
        {
            FailedModules.Add(mod);
        }

        LogEntries.Clear();
        foreach (var log in logs)
        {
            LogEntries.Add(log);
        }

        OnPropertyChanged(nameof(StatusMessage));
        OnPropertyChanged(nameof(RamDifference));
    }

    private void RestoreDefaults()
    {
        var result = MessageBox.Show(
            "This will restore Windows defaults. Are you sure?",
            "Confirm Restore",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            OnRestoreRequested?.Invoke();
        }
    }

    private void ExportLog()
    {
        try
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var logPath = Path.Combine(desktop, $"AegisCore_Log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            
            using (var writer = new StreamWriter(logPath))
            {
                writer.WriteLine("AEGISCORE SCAN REPORT");
                writer.WriteLine($"Generated: {DateTime.Now}");
                writer.WriteLine();
                writer.WriteLine($"RAM Before: {RamBefore:F2} GB");
                writer.WriteLine($"RAM After: {RamAfter:F2} GB");
                writer.WriteLine($"RAM Difference: {RamDifference:F2} GB");
                writer.WriteLine();
                writer.WriteLine($"Modules Completed: {ModulesCompleted}/{ModulesTotal}");
                writer.WriteLine($"Modules Failed: {ModulesFailed}");
                writer.WriteLine();
                
                if (FailedModules.Count > 0)
                {
                    writer.WriteLine("FAILED MODULES:");
                    foreach (var mod in FailedModules)
                    {
                        writer.WriteLine($"  - {mod}");
                    }
                    writer.WriteLine();
                }

                writer.WriteLine("ACTIVITY LOG:");
                foreach (var log in LogEntries)
                {
                    writer.WriteLine($"  {log}");
                }
            }

            MessageBox.Show($"Log exported to: {logPath}", "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to export log: {ex.Message}", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void NewScan()
    {
        OnNewScanRequested?.Invoke();
    }

    private void Exit()
    {
        OnExitRequested?.Invoke();
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
