using System.Collections.ObjectModel;
using ExcelTabellenAuswerung.DataBase;
using ExcelTabellenAuswerung.Models;
using Microsoft.Win32;
using Serilog;
using System.Diagnostics;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExcelTabellenAuswerung.Controls;
using ExcelTabellenAuswerung.Views.Windows;
using CsvHelper;
using System.Globalization;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Data;
using CsvHelper.Configuration;

namespace ExcelTabellenAuswerung.ViewModels.Pages
{
    public partial class DataViewModel : ObservableObject
    {
        private bool _isInitialized = false;
        private readonly SynchronizationContext? context = SynchronizationContext.Current;
        private List<string> _randomWaitungInformation = new();

        [ObservableProperty] private Visibility _openedFilePathVisibilityData1 = Visibility.Collapsed;

        [ObservableProperty] private Visibility _openedFilePathVisibilityData2 = Visibility.Collapsed;

        [ObservableProperty] private string _openedFilePathData1 = string.Empty;

        [ObservableProperty] private string _openedFilePathData2 = string.Empty;

        [ObservableProperty] private ObservableCollection<EmergencyCase?> _emergencyCaseList = [];

        [ObservableProperty] private ICollectionView _filteredEmergencyCaseList;

        public bool IsInitialized { get; internal set; }

        public DataViewModel()
        {
            _randomWaitungInformation = GeneratedWaitungInformations().ToList();

            Task.Run(LoadInformation);
        }

        public async void OnDoubleClick(object parameter)
        {
            var
                item = parameter as EmergencyCase; // MyItemType sollte durch den tatsächlichen Typ deines Items ersetzt werden
            if (item != null)
            {

                var editDataContentDialog = new EditDataContentDialog(item.Id);

                editDataContentDialog.ShowDialog();
                if (EmergencyCaseList != null)
                {

                    EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();
                    var found = EmergencyCaseList.FirstOrDefault(x => x != null && x.Id == item.Id);
                    int i = EmergencyCaseList.IndexOf(found);
                    EmergencyCaseList[i] = emergencyCaseDataBase.Get(item.Id);

                }
            }
        }

        private IEnumerable<string> GeneratedWaitungInformations()
        {
            yield return new string("Kein Stress, schnapp dir 'nen Kaffee und lehn dich zurück – während die Excel-Tabelle sich in aller Ruhe importiert. Die Software hat das im Griff, also entspann dich und genieß die Pause! \ud83d\ude04");
            yield return new string("Zeit für eine kleine Siesta! Lass die Software schuften, während die Excel-Tabelle gemütlich importiert wird. \ud83c\udf34");
            yield return new string("Ab in den Chill-Modus! Die Tabelle braucht ein bisschen, um anzukommen. Die Software übernimmt das Ruder. \ud83d\ude80");
            yield return new string("Lehne dich zurück, entspann dich und lass die Magie geschehen – die Excel-Tabelle kommt gleich, versprochen! \u2728");
            yield return new string("Mach's dir bequem und zähl ein paar Schäfchen, während die Software die Tabelle importiert. \ud83d\udc11");
            yield return new string("Zeit für eine kleine Tanzpause! Die Software importiert die Excel-Tabelle, während du eine Runde tanzen kannst. \ud83d\udc83\ud83d\udd7a");
            yield return new string("Schnapp dir einen Snack und relax – die Software kümmert sich um den Tabellen-Import! \ud83c\udf7f");
            yield return new string("Füße hoch und entspannen! Die Excel-Tabelle nimmt sich eine kleine Auszeit, bevor sie ankommt. \ud83d\udecb\ufe0f");
            yield return new string("Mach’s dir gemütlich und lies ein Kapitel deines Lieblingsbuchs, während die Software die Excel-Tabelle importiert. \ud83d\udcd6");
            yield return new string("Jetzt ist der perfekte Moment für eine Tasse Tee – die Excel-Tabelle braucht ein bisschen, um sich zurechtzufinden. \ud83c\udf75");
            yield return new string("Atme tief durch und genieße die Ruhepause, während die Software den Excel-Import erledigt. \ud83c\udf2c\ufe0f");
        }

        private static readonly Random _random = new Random();

        private string GetRandomWaitungInformation()
        {
            if (_randomWaitungInformation == null || _randomWaitungInformation.Count == 0)
            {
                throw new InvalidOperationException("The list is empty or null.");
            }

            // Index eines zufälligen Elements in der Liste generieren
            int index = _random.Next(_randomWaitungInformation.Count);

            // Zufälliges Element abrufen und zurückgeben
            return _randomWaitungInformation[index];
        }

        private void LoadInformation()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();
            List<Models.EmergencyCase> emergencyCases = emergencyCaseDataBase.LoadData();


            EmergencyCaseList = new ObservableCollection<EmergencyCase?>(emergencyCases);

            stopwatch.Stop();
            FilteredEmergencyCaseList = CollectionViewSource.GetDefaultView(EmergencyCaseList);
            // Loggen der Dauer der Operation
            Log.Information("Die LoadInformation dauerte {Duration} Millisekunden.", stopwatch.ElapsedMilliseconds);
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        [RelayCommand]
        public async Task OnStartCsvExport2()
        {
        }

        /// <summary>
            /// 
            /// </summary>
            [RelayCommand]
        public void OnTextSearch()
        {
            FilterEmergencyCases();
        }


        [ObservableProperty]
        private string _searchTerm;
        
        private void FilterEmergencyCases()
        {
            if (string.IsNullOrEmpty(SearchTerm))
            {
                FilteredEmergencyCaseList.Filter = null; // zeigt alle Daten
            }
            else
            {
                FilteredEmergencyCaseList.Filter = item =>
                {
                    var emergencyCase = item as EmergencyCase;

                    if (emergencyCase != null)
                    {
                        if (emergencyCase.GrundStichwort != null
                            && emergencyCase.GrundStichwort.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }

                        if (emergencyCase.Diagnosis != null
                            && emergencyCase.Diagnosis.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }

                        if (emergencyCase.Name != null
                            && emergencyCase.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }

                    return false;
                };
            }
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }

        [RelayCommand]
        public async Task OnOpenFileData1()
        {
            OpenedFilePathVisibilityData1 = Visibility.Collapsed;

            OpenFileDialog openFileDialog =
                new()
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Filter = "Excel Worksheets|*.xls;*.xlsx"
                };

            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            if (!File.Exists(openFileDialog.FileName))
            {
                return;
            }

            Task.Factory.StartNew(() => Thread.Sleep(2500)).ContinueWith(t =>
            {
                //note you can use the message queue from any thread, but just for the demo here we 
                //need to get the message queue from the snackbar, so need to be on the dispatcher
                MainWindow.Snackbar.MessageQueue?.Enqueue(GetRandomWaitungInformation());
            }, TaskScheduler.FromCurrentSynchronizationContext());

            Stopwatch stopwatch = Stopwatch.StartNew();
            Helpers.ExcelReader excelReader = new Helpers.ExcelReader();
            int newImported = 0;

            await Task.Run(() =>
            {
           newImported = excelReader.ReadExcelFileData1(openFileDialog.FileName);
            });

            stopwatch.Stop();

            // Loggen der Dauer der Operation
            Log.Information("Die ReadExcel dauerte {Duration} Millisekunden.", stopwatch.ElapsedMilliseconds);


            EmergencyCaseList.Clear();

            EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();

            stopwatch = Stopwatch.StartNew();

            List<Models.EmergencyCase> emergencyCases = emergencyCaseDataBase.LoadData();
            EmergencyCaseList = new ObservableCollection<EmergencyCase?>(emergencyCases);
            // Loggen der Dauer der Operation
            Log.Information("Die LoadData dauerte {Duration} Millisekunden. Anzahl in Datenbank {1}", stopwatch.ElapsedMilliseconds, emergencyCases.Count);


            OpenedFilePathData1 = newImported.ToString();
            OpenedFilePathVisibilityData1 = Visibility.Visible;
        }

        [RelayCommand]
        public async Task OnOpenFileData2()
        {
            OpenedFilePathVisibilityData2 = Visibility.Collapsed;

            OpenFileDialog openFileDialog =
                new()
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Filter = "Excel Worksheets|*.xls;*.xlsx"
                };

            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            if (!File.Exists(openFileDialog.FileName))
            {
                return;
            }

            Task.Factory.StartNew(() => Thread.Sleep(2500)).ContinueWith(t =>
            {
                //note you can use the message queue from any thread, but just for the demo here we 
                //need to get the message queue from the snackbar, so need to be on the dispatcher
                MainWindow.Snackbar.MessageQueue?.Enqueue(GetRandomWaitungInformation());
            }, TaskScheduler.FromCurrentSynchronizationContext());


            Stopwatch stopwatch = Stopwatch.StartNew();
            Helpers.ExcelReader excelReader = new Helpers.ExcelReader();

            int newImported = excelReader.ReadExcelFileData2(openFileDialog.FileName);

            stopwatch.Stop();

            // Loggen der Dauer der Operation
            Log.Information("Die ReadExcel dauerte {Duration} Millisekunden.", stopwatch.ElapsedMilliseconds);


            EmergencyCaseList.Clear();

            EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();

            stopwatch = Stopwatch.StartNew();

            List<Models.EmergencyCase> emergencyCases = emergencyCaseDataBase.LoadData();
            EmergencyCaseList = new ObservableCollection<EmergencyCase?>(emergencyCases);
            // Loggen der Dauer der Operation
            Log.Information("Die LoadData dauerte {Duration} Millisekunden. Anzahl in dDatenbank {1}", stopwatch.ElapsedMilliseconds, emergencyCases.Count);
            
            OpenedFilePathData2 = newImported.ToString();
            OpenedFilePathVisibilityData2 = Visibility.Visible;
        }

        [RelayCommand]
        public async Task OnOpenFileImportDocuments()
        {
            OpenFolderDialog openFolderDialog = new ()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (openFolderDialog.ShowDialog() != true)
            {
                return;
            }

            if (!Directory.Exists(openFolderDialog.FolderName))
            {
                return;
            }

            Task.Factory.StartNew(() => Thread.Sleep(2500)).ContinueWith(t =>
            {
                //note you can use the message queue from any thread, but just for the demo here we 
                //need to get the message queue from the snackbar, so need to be on the dispatcher
                MainWindow.Snackbar.MessageQueue?.Enqueue(GetRandomWaitungInformation());
            }, TaskScheduler.FromCurrentSynchronizationContext());


            Helpers.ExcelReader excelReader = new Helpers.ExcelReader();
            excelReader.SavePdf(new DirectoryInfo(openFolderDialog.FolderName));
        }

        [RelayCommand]
        public async Task OnStartCsvExport()
        {
            SaveFileDialog saveFileDialog =
                new()
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Filter = "CSV|*.csv"
                };

            if (saveFileDialog.ShowDialog() != true)
            {
                return;
            }

            Dispatcher currentDispatcher = Dispatcher.CurrentDispatcher;

            Task.Run(() =>
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    NewLine = Environment.NewLine,
                    Delimiter = "\t"

                };

                List<EmergencyCase?> tempList = EmergencyCaseList.ToList();

                using (var writer = new StreamWriter(saveFileDialog.FileName))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(tempList);
                }

                currentDispatcher.Invoke(DispatcherPriority.Normal,
                    () =>
                    {
                        Task.Factory.StartNew(() => Thread.Sleep(5000)).ContinueWith(
                            t => { MainWindow.Snackbar.MessageQueue?.Enqueue("Export der Daten ist abgeschlossen"); },
                            TaskScheduler.FromCurrentSynchronizationContext());
                    });
            });


        }
    }
}
