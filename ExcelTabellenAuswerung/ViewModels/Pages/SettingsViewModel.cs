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
        private string _appVersion = string.Empty;
        
        [ObservableProperty]
        private string _author = string.Empty; 
        
        [ObservableProperty]
        private string _textStatus = string.Empty;

        [ObservableProperty]
        private bool _checkUpdateEnable = false;

        [ObservableProperty]
        private bool _downloadUpdateEnable = false;
        
        [ObservableProperty]
        private bool _restartAndApplyEnable = false;

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
        private async void OnCheckForUpdate()
        {
            Working();

            try
            {
                // ConfigureAwait(true) so that UpdateStatus() is called on the UI thread
                _update = await _updateManager.CheckForUpdatesAsync().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error checking for updates");
            }

            UpdateStatus();
        }

        [RelayCommand]
        private async void OnDownloadUpdate()
        {
            Working();

            try
            {
                // ConfigureAwait(true) so that UpdateStatus() is called on the UI thread
                await _updateManager.DownloadUpdatesAsync(_update, Progress).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error downloading updates");
            }
            UpdateStatus();
        }

        [RelayCommand]
        private void OnRestartAndApply()
        {
            _updateManager.ApplyUpdatesAndRestart(_update);
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

        private void Progress(int percent)
        {
            TextStatus = $"Downloading ({percent}%)...";
        }


        private void Working()
        {
            Log.Information("");
            CheckUpdateEnable= false;
            DownloadUpdateEnable = false;
            RestartAndApplyEnable = false;
            TextStatus = "Working...";
        }

        private void UpdateStatus()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Velopack version: {VelopackRuntimeInfo.VelopackNugetVersion}");
            sb.AppendLine($"This app version: {(_updateManager.IsInstalled ? _updateManager.CurrentVersion : "(n/a - not installed)")}");
            Log.Information(sb.ToString());


            if (_update != null)
            {
                sb.AppendLine($"Update available: {_update.TargetFullRelease.Version}");
                DownloadUpdateEnable = true;
            }
            else
            {
                DownloadUpdateEnable = false;
            }

            if (_updateManager.IsUpdatePendingRestart)
            {
                sb.AppendLine("Update ready, pending restart to install");
                RestartAndApplyEnable = true;
            }
            else
            {
                RestartAndApplyEnable = false;
            }

            TextStatus = sb.ToString();
            CheckUpdateEnable = true;

        }
    }
}
