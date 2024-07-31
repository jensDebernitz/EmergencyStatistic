using DocumentFormat.OpenXml.Spreadsheet;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using ExcelTabellenAuswerung.ViewModels.Pages;
using ExcelTabellenAuswerung.Views.Pages;
using Items = ExcelTabellenAuswerung.Models.Items;

namespace ExcelTabellenAuswerung.ViewModels.Windows;

public partial class MainWindowViewModel : ObservableObject
{
    private string _searchKeyword;
    private readonly ICollectionView _itemsView;
    private Items _selectedItem;
    private int _selectedIndex;
    private ObservableCollection<Items> _items = new ();
    private Items _protocol = null;
    private string _title = $"Einsatz-Dokumentation & Statistik";
    private bool _toggleButton = false;
    private Items _messagePolling;

    public ICommand ClickCommandSettingsButton { get; private set; }

    public MainWindowViewModel()
    {
        foreach (var item in GenerateItems())
        {
            Items.Add(item);
        }

        SelectedItem = Items[0];

        _itemsView = CollectionViewSource.GetDefaultView(Items);
        _itemsView.Filter = ItemsFilter;

        ClickCommandSettingsButton = new DelegateCommand(SettingsButtonIsClicked);
    }

    private bool ItemsFilter(object obj)
    {
        if (string.IsNullOrWhiteSpace(_searchKeyword))
        {
            return true;
        }

        if (obj is Models.Items)
        {
            Models.Items item = (Models.Items)obj;

            return item.Name.ToLower().Contains(_searchKeyword.ToLower());
        }

        return false;
    }


    public ObservableCollection<Models.Items> Items
    {
        get { return _items; }
        private set { _items = value; }
    }

    public Models.Items SelectedItem
    {
        get { return _selectedItem; }
        set
        {
            SetProperty(ref _selectedItem, value);
            ToggleButton = false;

        }
    }

    public int SelectedIndex
    {
        get { return _selectedIndex; }
        set { SetProperty(ref _selectedIndex, value); }
    }

    public bool ToggleButton
    {
        get { return _toggleButton; }
        set { SetProperty(ref _toggleButton, value); }
    }

    public string Title
    {
        get { return _title; }
        set { SetProperty(ref _title, value); }
    }

    public string SearchKeyword
    {
        get { return _searchKeyword; }
        set
        {
            if (SetProperty(ref _searchKeyword, value))
            {
                _itemsView.Refresh();
            }
        }
    }

    private IEnumerable<Models.Items> GenerateItems()
    {
        yield return new Models.Items("Statistik", "Statistik", typeof(DashboardPage));
        yield return new Models.Items("Daten", "Daten", typeof(DataPage));

    }
    public void SettingsButtonIsClicked()
    {
        _messagePolling = new Items("Einstellungen", "Einstellungen", typeof(SettingsPage));
        SelectedIndex = -1;
        SelectedItem = _messagePolling;
    }

}

