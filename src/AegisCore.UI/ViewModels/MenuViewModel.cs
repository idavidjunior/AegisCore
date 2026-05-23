using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AegisCore.UI.ViewModels;

public class MenuViewModel : INotifyPropertyChanged
{
    private int _selectedIndex;
    private ObservableCollection<MenuItem> _menuItems;

    public event PropertyChangedEventHandler? PropertyChanged;

    public MenuViewModel()
    {
        _selectedIndex = 0;
        _menuItems = new ObservableCollection<MenuItem>
        {
            new MenuItem { Id = 1, Name = "Quick Scan", Description = "Scan critical system areas" },
            new MenuItem { Id = 2, Name = "Full Scan", Description = "Complete system scan" },
            new MenuItem { Id = 3, Name = "Custom Scan", Description = "Select specific folders/files" },
            new MenuItem { Id = 4, Name = "Integrity Check", Description = "Verify file signatures" },
            new MenuItem { Id = 5, Name = "SFC Scan", Description = "System File Checker" },
            new MenuItem { Id = 6, Name = "Restore Defaults", Description = "Restore Windows defaults" },
            new MenuItem { Id = 7, Name = "Exit", Description = "Close application" }
        };

        NavigateUpCommand = new RelayCommand(() => NavigateUp());
        NavigateDownCommand = new RelayCommand(() => NavigateDown());
        SelectCommand = new RelayCommand(() => Select());
    }

    public int SelectedIndex
    {
        get => _selectedIndex;
        set { _selectedIndex = value; OnPropertyChanged(); }
    }

    public ObservableCollection<MenuItem> MenuItems
    {
        get => _menuItems;
        set { _menuItems = value; OnPropertyChanged(); }
    }

    public ICommand NavigateUpCommand { get; }
    public ICommand NavigateDownCommand { get; }
    public ICommand SelectCommand { get; }

    public event Action<int>? OnItemSelected;

    private void NavigateUp()
    {
        if (SelectedIndex > 0)
            SelectedIndex--;
    }

    private void NavigateDown()
    {
        if (SelectedIndex < MenuItems.Count - 1)
            SelectedIndex++;
    }

    private void Select()
    {
        OnItemSelected?.Invoke(MenuItems[SelectedIndex].Id);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
