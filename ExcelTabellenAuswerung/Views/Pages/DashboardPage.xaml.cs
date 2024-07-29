using System.Windows.Controls;
using ExcelTabellenAuswerung.ViewModels.Pages;

namespace ExcelTabellenAuswerung.Views.Pages
{
    public partial class DashboardPage : UserControl
    {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage()
        {
            ViewModel = new DashboardViewModel();
            DataContext = this;

            InitializeComponent();
        }
    }
}
