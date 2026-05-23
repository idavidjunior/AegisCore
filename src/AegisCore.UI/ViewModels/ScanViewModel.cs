using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AegisCore.UI.ViewModels;

public class ScanViewModel : INotifyPropertyChanged
{
    private ObservableCollection<ScanModule> _checklistItems;
    private ObservableCollection<LogEntry> _logEntries;
    private double _progress;
    private bool _isScanning;
    private string _currentStatus;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ScanViewModel()
    {
        _checklistItems = new ObservableCollection<ScanModule>();
        _logEntries = new ObservableCollection<LogEntry>();
        _progress = 0;
        _isScanning = false;
        _currentStatus = "Ready";

        StartScanCommand = new RelayCommand(async () => await StartScanAsync());
        CancelScanCommand = new RelayCommand(() => CancelScan());
    }

    public ObservableCollection<ScanModule> ChecklistItems
    {
        get => _checklistItems;
        set { _checklistItems = value; OnPropertyChanged(); }
    }

    public ObservableCollection<LogEntry> LogEntries
    {
        get => _logEntries;
        set { _logEntries = value; OnPropertyChanged(); }
    }

    public double Progress
    {
        get => _progress;
        set { _progress = value; OnPropertyChanged(); }
    }

    public bool IsScanning
    {
        get => _isScanning;
        set { _isScanning = value; OnPropertyChanged(); }
    }

    public string CurrentStatus
    {
        get => _currentStatus;
        set { _currentStatus = value; OnPropertyChanged(); }
    }

    public ICommand StartScanCommand { get; }
    public ICommand CancelScanCommand { get; }

    public void InitializeChecklist(string scanType)
    {
        ChecklistItems.Clear();
        
        var modules = scanType switch
        {
            "Quick" => new[]
            {
                new ScanModule { Name = "Engine Initialization", Status = ModuleStatus.Pending },
                new ScanModule { Name = "Registry Check", Status = ModuleStatus.Pending },
                new ScanModule { Name = "Critical Files Scan", Status = ModuleStatus.Pending },
                new ScanModule { Name = "Service Verification", Status = ModuleStatus.Pending }
            },
            "Full" => new[]
            {
                new ScanModule { Name = "Engine Initialization", Status = ModuleStatus.Pending },
                new ScanModule { Name = "Registry Check", Status = ModuleStatus.Pending },
                new ScanModule { Name = "Critical Files Scan", Status = ModuleStatus.Pending },
                new ScanModule { Name = "Service Verification", Status = ModuleStatus.Pending },
                new ScanModule { Name = "Full System Scan", Status = ModuleStatus.Pending },
                new ScanModule { Name = "Integrity Verification", Status = ModuleStatus.Pending },
                new ScanModule { Name = "Risk Assessment", Status = ModuleStatus.Pending }
            },
            _ => Array.Empty<ScanModule>()
        };

        foreach (var module in modules)
        {
            ChecklistItems.Add(module);
        }
    }

    private async Task StartScanAsync()
    {
        if (IsScanning) return;
        
        IsScanning = true;
        LogEntries.Clear();
        AddLog("INFO", "Scan started");

        for (int i = 0; i < ChecklistItems.Count; i++)
        {
            if (!IsScanning) break;

            var module = ChecklistItems[i];
            module.Status = ModuleStatus.InProgress;
            CurrentStatus = $"Scanning: {module.Name}";
            AddLog("INFO", $"Starting: {module.Name}");

            await Task.Run(async () =>
            {
                try
                {
                    // Simulação real do scan - aqui seria chamado o módulo real
                    await Task.Delay(1500); // Tempo real de processamento
                    
                    // Atualizar status baseado no resultado real
                    module.Status = ModuleStatus.Completed;
                    AddLog("OK", $"{module.Name} - Completed");
                }
                catch (Exception ex)
                {
                    module.Status = ModuleStatus.Failed;
                    AddLog("ERROR", $"{module.Name} - Failed: {ex.Message}");
                }
            });

            Progress = ((i + 1) / (double)ChecklistItems.Count) * 100;
        }

        IsScanning = false;
        CurrentStatus = "Scan completed";
        AddLog("INFO", "Scan finished");
    }

    private void CancelScan()
    {
        IsScanning = false;
        CurrentStatus = "Scan cancelled";
        AddLog("WARN", "Scan cancelled by user");
    }

    private void AddLog(string level, string message)
    {
        var entry = new LogEntry
        {
            Timestamp = DateTime.Now.ToString("HH:mm:ss"),
            Level = level,
            Message = message
        };
        
        LogEntries.Add(entry);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class ScanModule : INotifyPropertyChanged
{
    private string _name;
    private ModuleStatus _status;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    public ModuleStatus Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    public string StatusIcon => Status switch
    {
        ModuleStatus.Pending => "☐",
        ModuleStatus.InProgress => "⏳",
        ModuleStatus.Completed => "✅",
        ModuleStatus.Failed => "❌",
        _ => "☐"
    };

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class LogEntry
{
    public string Timestamp { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public enum ModuleStatus
{
    Pending,
    InProgress,
    Completed,
    Failed
}
