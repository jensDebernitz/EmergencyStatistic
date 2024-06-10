using Serilog;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velopack;
using static System.Environment;
using Velopack.Sources;

namespace ExcelTabellenAuswerung
{
    public  class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {

                var commonpath = GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
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

                Log.Information("Anwendung gestartet 2");
               

                //// It's important to Run() the VelopackApp as early as possible in app startup.
                VelopackApp.Build().Run();
                UpdateMyApp();

                // We can now launch the WPF application as normal.
                var app = new App();
                app.InitializeComponent();
                app.Run();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.ToString());
            }

            Log.Information("Anwendung wird beendet");
            Log.CloseAndFlush();
        }

        private static void UpdateMyApp()
        {
            Log.Information("Check Updates avaible");
            var mgr = new UpdateManager(new GitHubSource("https://github.com/jensDebernitz/EmergencyStatistic"));


            

            // check for new version
            var newVersion = mgr.CheckForUpdates();
            if (newVersion == null)
            {
                Log.Information("keine neue version gefunden");
                return; // no update available
            }

            Log.Information(newVersion.TargetFullRelease.Version.ToFullString());
            // download new version
            mgr.DownloadUpdates(newVersion);

            // install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);
        }
    }
}
