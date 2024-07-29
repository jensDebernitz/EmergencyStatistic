using System.Windows.Controls;
using ExcelTabellenAuswerung.ViewModels.Pages;

namespace ExcelTabellenAuswerung.Views.Pages
{
    public partial class SettingsPage : UserControl
    {
        public SettingsViewModel ViewModel { get; }

        public SettingsPage()
        {
            ViewModel = new SettingsViewModel();
            DataContext = this;

            InitializeComponent();
        }
    }
}
