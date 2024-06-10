using ExcelTabellenAuswerung.DataBase;
using System.Text;
using Serilog;
using Velopack;
using Velopack.Sources;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace ExcelTabellenAuswerung.ViewModels.Pages
{
    public partial class SettingsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _appVersion = String.Empty;
        
        [ObservableProperty]
        private string _author = String.Empty;

        [ObservableProperty]
        private string _logging = String.Empty;

        [ObservableProperty]
        private ApplicationTheme _currentTheme = ApplicationTheme.Unknown;

        private UpdateManager _updateManager;
        private UpdateInfo _update;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom() { }

        private void InitializeViewModel()
        {
            CurrentTheme = ApplicationThemeManager.GetAppTheme();
            AppVersion = $"Excel Tabellen Auswertung - {GetAssemblyVersion()}";
            Author = "rot weiße Grüße von Jens Debernitz";
            _updateManager = new UpdateManager(new GithubSource("https://github.com/jensDebernitz/EmergencyStatistic", null, false));
            UpdateStatus();
            _isInitialized = true;
        }

        private string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()
                ?? String.Empty;
        }

        [RelayCommand]
        private void OnChangeTheme(string parameter)
        {
            SettingsDataBase settingsDataBase = new SettingsDataBase();

            switch (parameter)
            {
                case "theme_light":
                    if (CurrentTheme == ApplicationTheme.Light)
                        break;

                    ApplicationThemeManager.Apply(ApplicationTheme.Light);
                    CurrentTheme = ApplicationTheme.Light;
                    settingsDataBase.SaveTheme(CurrentTheme);
                    break;

                default:
                    if (CurrentTheme == ApplicationTheme.Dark)
                        break;

                    ApplicationThemeManager.Apply(ApplicationTheme.Dark);
                    CurrentTheme = ApplicationTheme.Dark;
                    settingsDataBase.SaveTheme(CurrentTheme);
                    break;
            }
        }

        private void UpdateStatus()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Velopack version: {VelopackRuntimeInfo.VelopackNugetVersion}");
            sb.AppendLine($"This app version: {(_updateManager.IsInstalled ? _updateManager.CurrentVersion : "(n/a - not installed)")}");
            Log.Information(sb.ToString());
        }
    }
}
