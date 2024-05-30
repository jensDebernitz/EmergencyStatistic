using ExcelTabellenAuswerung.Services;
using ExcelTabellenAuswerung.ViewModels.Pages;
using ExcelTabellenAuswerung.ViewModels.Windows;
using ExcelTabellenAuswerung.Views.Pages;
using ExcelTabellenAuswerung.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using Wpf.Ui;
using static System.Environment;

namespace ExcelTabellenAuswerung
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c =>
            {
                c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location));
            })
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();

                services.AddSingleton<IPageService, PageService>();
                services.AddSingleton<IThemeService, ThemeService>();
                services.AddSingleton<ITaskBarService, TaskBarService>();
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<IContentDialogService, ContentDialogService>();

                // Main window with navigation
                services.AddSingleton<INavigationWindow, MainWindow>();
                services.AddSingleton<MainWindowViewModel>();

                services.AddSingleton<DashboardPage>();
                services.AddSingleton<DashboardViewModel>();
                services.AddSingleton<DataPage>();
                services.AddSingleton<DataViewModel>();
                services.AddSingleton<SettingsPage>();
                services.AddSingleton<SettingsViewModel>();
            }).Build();


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var commonpath = GetFolderPath(SpecialFolder.CommonApplicationData);
            var path = Path.Combine(commonpath, "ExcelTabellenAuswertung\\logs");

            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Debug()
             .WriteTo.Console()
             .WriteTo.File(path + "\\app.log", rollingInterval: RollingInterval.Day)
             .CreateLogger();

            Log.Information("Anwendung gestartet");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("Anwendung wird beendet");
            Log.CloseAndFlush();
            base.OnExit(e);
        }

        /// <summary>
        /// Gets registered service.
        /// </summary>
        /// <typeparam name="T">Type of the service to get.</typeparam>
        /// <returns>Instance of the service or <see langword="null"/>.</returns>
        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private void OnStartup(object sender, StartupEventArgs e)
        {
            _host.Start();
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}
