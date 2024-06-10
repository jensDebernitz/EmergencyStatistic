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

namespace ExcelTabellenAuswerung;

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

            Helpers.DataBase.GlobalLogging = new StringWriter();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.TextWriter(Helpers.DataBase.GlobalLogging)
                .WriteTo.File(path + "\\app.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Anwendung gestartet");
           

            //// It's important to Run() the VelopackApp as early as possible in app startup.
            VelopackApp.Build().Run();
            
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
}

