using System.Configuration;
using System.Data;

using System.Windows;
using System.Windows.Media;
using ExcelTabellenAuswerung.DataBase;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace ExcelTabellenAuswerung
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal BaseTheme InitialTheme { get; set; }
    }

}
