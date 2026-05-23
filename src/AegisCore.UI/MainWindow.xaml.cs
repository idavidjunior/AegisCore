
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AegisCore.Core;
using AegisCore.UI.ViewModels;
using AegisCore.UI.Views;

namespace AegisCore.UI;

public partial class MainWindow : Window
{
    private readonly IntegrityScanner _scanner;
    private readonly DashboardViewModel _dashboardViewModel;
    private readonly MenuViewModel _menuViewModel;
    private readonly ScanViewModel _scanViewModel;
    private readonly ReportViewModel _reportViewModel;

    private double _initialRam;

    public MainWindow()
    {
        InitializeComponent();
        
        _scanner = new IntegrityScanner();
        
        // Initialize ViewModels
        _dashboardViewModel = new DashboardViewModel();
        _menuViewModel = new MenuViewModel();
        _scanViewModel = new ScanViewModel();
        _reportViewModel = new ReportViewModel();

        // Subscribe to events
        _menuViewModel.OnItemSelected += HandleMenuSelection;
        _reportViewModel.OnRestoreRequested += HandleRestoreRequest;
        _reportViewModel.OnNewScanRequested += HandleNewScanRequest;
        _reportViewModel.OnExitRequested += HandleExitRequest;

        // Handle keyboard input
        this.KeyDown += HandleKeyDown;

        // Show dashboard first
        ShowDashboard();
    }

    private void ShowDashboard()
    {
        var dashboardView = new DashboardView { DataContext = _dashboardViewModel };
        ContentArea.Content = dashboardView;
        
        _dashboardViewModel.StartMonitoringCommand.Execute(null);
        
        // Auto-advance to menu after 3 seconds or on key press
        Task.Delay(3000).ContinueWith(_ => Dispatcher.Invoke(() =>
        {
            if (ContentArea.Content is DashboardView)
            {
                ShowMenu();
            }
        }));
    }

    private void ShowMenu()
    {
        _dashboardViewModel.StopMonitoringCommand.Execute(null);
        
        var menuView = new MenuView { DataContext = _menuViewModel };
        ContentArea.Content = menuView;
    }

    private void HandleMenuSelection(int itemId)
    {
        switch (itemId)
        {
            case 1: // Quick Scan
                StartScan("Quick");
                break;
            case 2: // Full Scan
                StartScan("Full");
                break;
            case 3: // Custom Scan
                MessageBox.Show("Custom scan feature coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                break;
            case 4: // Integrity Check
                MessageBox.Show("Integrity check feature coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                break;
            case 5: // SFC Scan
                MessageBox.Show("SFC scan feature coming soon", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                break;
            case 6: // Restore Defaults
                HandleRestoreRequest();
                break;
            case 7: // Exit
                Close();
                break;
        }
    }

    private async void StartScan(string scanType)
    {
        // Capture initial RAM
        _initialRam = await GetFreeRamAsync();
        
        _scanViewModel.InitializeChecklist(scanType);
        
        var scanView = new ScanView { DataContext = _scanViewModel };
        ContentArea.Content = scanView;
        
        _scanViewModel.StartScanCommand.Execute(null);
        
        // Show report after scan completes
        await Task.Delay(100); // Give scan time to start
        while (_scanViewModel.IsScanning)
        {
            await Task.Delay(100);
        }
        ShowReport();
    }

    private void ShowReport()
    {
        var finalRam = GetFreeRam();
        
        var failedModules = new System.Collections.ObjectModel.ObservableCollection<string>();
        var logEntries = new System.Collections.ObjectModel.ObservableCollection<string>();
        
        foreach (var module in _scanViewModel.ChecklistItems)
        {
            if (module.Status == ModuleStatus.Failed)
            {
                failedModules.Add(module.Name);
            }
        }
        
        foreach (var log in _scanViewModel.LogEntries)
        {
            logEntries.Add($"[{log.Timestamp}] {log.Level}: {log.Message}");
        }
        
        _reportViewModel.SetReportData(
            _initialRam,
            finalRam,
            _scanViewModel.ChecklistItems.Count,
            _scanViewModel.ChecklistItems.Count,
            failedModules.Count,
            failedModules,
            logEntries
        );
        
        var reportView = new ReportView { DataContext = _reportViewModel };
        ContentArea.Content = reportView;
    }

    private void HandleRestoreRequest()
    {
        MessageBox.Show("Restore functionality would call Restore-WindowsDefaults.ps1", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void HandleNewScanRequest()
    {
        ShowMenu();
    }

    private void HandleExitRequest()
    {
        Close();
    }

    private void HandleKeyDown(object sender, KeyEventArgs e)
    {
        // Handle keyboard navigation in menu
        if (ContentArea.Content is MenuView)
        {
            if (e.Key == Key.Up)
            {
                _menuViewModel.NavigateUpCommand.Execute(null);
            }
            else if (e.Key == Key.Down)
            {
                _menuViewModel.NavigateDownCommand.Execute(null);
            }
            else if (e.Key == Key.Enter)
            {
                _menuViewModel.SelectCommand.Execute(null);
            }
        }
        
        // Handle keyboard shortcuts in report
        if (ContentArea.Content is ReportView)
        {
            if (e.Key == Key.R)
            {
                _reportViewModel.RestoreCommand.Execute(null);
            }
            else if (e.Key == Key.E)
            {
                _reportViewModel.ExportLogCommand.Execute(null);
            }
            else if (e.Key == Key.N)
            {
                _reportViewModel.NewScanCommand.Execute(null);
            }
            else if (e.Key == Key.S)
            {
                _reportViewModel.ExitCommand.Execute(null);
            }
        }
    }

    private async Task<double> GetFreeRamAsync()
    {
        return await Task.Run(() => GetFreeRam());
    }

    private double GetFreeRam()
    {
        try
        {
            var ramCounter = new System.Diagnostics.PerformanceCounter("Memory", "Available MBytes");
            return ramCounter.NextValue() / 1024.0; // Convert to GB
        }
        catch
        {
            return 0;
        }
    }
}
