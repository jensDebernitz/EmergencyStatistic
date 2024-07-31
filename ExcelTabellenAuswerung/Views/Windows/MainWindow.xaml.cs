using System.Windows;
using System.Windows.Media;
using ExcelTabellenAuswerung.DataBase;
using ExcelTabellenAuswerung.ViewModels.Windows;
using System.Windows.Threading;

using Velopack;
using Velopack.Sources;
using Microsoft.VisualBasic;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace ExcelTabellenAuswerung.Views.Windows
{
    public partial class MainWindow : Window, IDisposable
    {
        public static Snackbar Snackbar = new();
        public MainWindowViewModel ViewModel { get; }
        App app = (App)Application.Current;
        private bool isTimerRunning;

        public MainWindow()
        {
            ViewModel = new MainWindowViewModel();
            InitializeComponent();
            DataContext = this;

            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            DataBase.SettingsDataBase settingsDataBase = new DataBase.SettingsDataBase();
            app.InitialTheme = settingsDataBase.LoadTheme();

            switch (app.InitialTheme)
            {
                case BaseTheme.Dark:
                    ModifyTheme(true);
                    break;
                case BaseTheme.Light:
                    ModifyTheme(false);
                    break;
            }

            isTimerRunning = false;
            Snackbar = MainSnackbar;
            StartTimer();
        }

        private UpdateManager _updateManager;

        private async Task StartTimer()
        {
            isTimerRunning = true;
            while (isTimerRunning)
            {
                UpdateManager updateManager = new UpdateManager(new GithubSource("https://github.com/jensDebernitz/EmergencyStatistic", null, false));
                var update = await updateManager.CheckForUpdatesAsync().ConfigureAwait(true);

                if (update != null)
                {
                    Snackbar.MessageQueue?.Enqueue($"Es steht ein neues Update '{update.TargetFullRelease.Version}' zuverfügung. Um dies zu installieren gehen sie bitte in ihre Settings");
                    TimeSpan.FromSeconds(5);
                }

                await Task.Delay(TimeSpan.FromSeconds(60 * 5)); //~approx 5 minutes
            }
        }

        public void StopTimer()
        {
            isTimerRunning = false;
        }
        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //until we had a StaysOpen glag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;

            while (dependencyObject != null)
            {
                if (dependencyObject is Theme.ScrollBar)
                {
                    return;
                }
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }
        }

        private static void ModifyTheme(bool isDarkTheme)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(isDarkTheme ? BaseTheme.Dark : BaseTheme.Light);
            paletteHelper.SetTheme(theme);
        }

        private void MenuToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            ItemsSearchBox.Focus();
        }

        private void OnSelectedItemChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //MainScrollViewer.ScrollToHome();
        }

        public void Dispose()
        {
            StopTimer();
        }
    }
}
