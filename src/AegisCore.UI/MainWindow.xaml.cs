
using System.Windows;
using AegisCore.Core;

namespace AegisCore.UI;

public partial class MainWindow : Window
{
    private readonly IntegrityScanner _scanner;

    public MainWindow()
    {
        InitializeComponent();
        _scanner = new IntegrityScanner();
    }

    private void ScanButton_Click(object sender, RoutedEventArgs e)
    {
        var results = _scanner.RunQuickScan();
        OutputBox.Text = string.Join("\n", results);
    }
}
