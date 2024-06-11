using System.Windows.Media;
using ExcelTabellenAuswerung.DataBase;
using ExcelTabellenAuswerung.ViewModels.Windows;
using System.Windows.Threading;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Velopack;
using Velopack.Sources;
using Microsoft.VisualBasic;

namespace ExcelTabellenAuswerung.Views.Windows
{
    public partial class MainWindow : INavigationWindow, IDisposable
    {
        private ControlAppearance _snackbarAppearance = ControlAppearance.Primary;
        private readonly ISnackbarService _snackbarService;

        public MainWindowViewModel ViewModel { get; }
        private bool isTimerRunning;

        public MainWindow(
            MainWindowViewModel viewModel,
            IPageService pageService,
            INavigationService navigationService,
             IContentDialogService contentDialogService,
             ISnackbarService snackbarService
        )
        {
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));
            ViewModel = viewModel;
            DataContext = this;

            SystemThemeWatcher.Watch(this);

            InitializeComponent();
            
            _snackbarService = snackbarService;

            _snackbarService.SetSnackbarPresenter(SnackbarPresenter);
            SetPageService(pageService);

            navigationService.SetNavigationControl(RootNavigation);
            contentDialogService.SetDialogHost(RootContentDialog);

            SettingsDataBase settingsDataBase = new SettingsDataBase();
            ApplicationThemeManager.Apply(settingsDataBase.LoadTheme());

            isTimerRunning = false;
            StartTimer();
        }

        private UpdateManager _updateManager;

        private async Task StartTimer()
        {
            isTimerRunning = true;
            while (isTimerRunning)
            {
                _updateManager = new UpdateManager(new GithubSource("https://github.com/jensDebernitz/EmergencyStatistic", null, false));
                var update = await _updateManager.CheckForUpdatesAsync().ConfigureAwait(true);

                if (update != null)
                {
                    _snackbarService.Show(
                        "Neues Update Verfügbar",
                        $"Es steht ein neues Update '{update.TargetFullRelease.Version}' zuverfügung. Um dies zu installieren gehen sie bitte in ihre Settings",
                        _snackbarAppearance,
                        new SymbolIcon(SymbolRegular.Fluent24),
                        TimeSpan.FromSeconds(5));
                }

                await Task.Delay(TimeSpan.FromSeconds(60 * 5)); //~approx 5 minutes
            }
        }

        public void StopTimer()
        {
            isTimerRunning = false;
        }

        #region INavigationWindow methods

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        #endregion INavigationWindow methods

        /// <summary>
        /// Raises the closed event.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }

        INavigationView INavigationWindow.GetNavigation()
        {
            throw new NotImplementedException();
        }

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            StopTimer();
        }
    }
}
