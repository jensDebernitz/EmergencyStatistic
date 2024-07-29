using ExcelTabellenAuswerung.DataBase;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Serilog;
using Velopack;
using Velopack.Sources;
using MaterialDesignThemes.Wpf;

namespace ExcelTabellenAuswerung.ViewModels.Pages
{
    public partial class SettingsViewModel : ObservableObject
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
        private BaseTheme _currentTheme = BaseTheme.Light;

        private UpdateManager _updateManager;
        private UpdateInfo _update;

        public SettingsViewModel()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }
        
        private void InitializeViewModel()
        {
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
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            switch (parameter)
            {
                case "theme_light":
                    if (CurrentTheme == BaseTheme.Light)
                        break;

                    theme.SetBaseTheme(BaseTheme.Light);
                    CurrentTheme = BaseTheme.Light;
                    settingsDataBase.SaveTheme(CurrentTheme);
                    paletteHelper.SetTheme(theme);
                    break;

                default:
                    if (CurrentTheme == BaseTheme.Dark)
                        break;

                    theme.SetBaseTheme(BaseTheme.Dark);
                    CurrentTheme = BaseTheme.Dark;
                    settingsDataBase.SaveTheme(CurrentTheme);
                    paletteHelper.SetTheme(theme);
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
