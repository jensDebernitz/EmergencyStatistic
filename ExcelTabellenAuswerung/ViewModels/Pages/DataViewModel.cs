using System.Collections.ObjectModel;
using ExcelTabellenAuswerung.Controls;
using ExcelTabellenAuswerung.DataBase;
using ExcelTabellenAuswerung.Models;
using Microsoft.Win32;
using Serilog;
using System.Diagnostics;
using System.IO;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace ExcelTabellenAuswerung.ViewModels.Pages
{
    public partial class DataViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;
        private readonly SynchronizationContext? context = SynchronizationContext.Current;
        private readonly IContentDialogService _contentDialogService;

        [ObservableProperty] private Visibility _openedFilePathVisibilityData1 = Visibility.Collapsed;

        [ObservableProperty] private Visibility _openedFilePathVisibilityData2 = Visibility.Collapsed;

        [ObservableProperty] private string _openedFilePathData1 = string.Empty;

        [ObservableProperty] private string _openedFilePathData2 = string.Empty;

        private string _searchText = string.Empty;

        [ObservableProperty] private ObservableCollection<EmergencyCase?> _emergencyCaseList = [];

        public bool IsInitialized { get; internal set; }

        public DataViewModel(IContentDialogService contentDialogService)
        {
            _contentDialogService = contentDialogService;

            Task.Run(LoadInformation);
        }

        public async void OnDoubleClick(object parameter)
        {
            var
                item = parameter as EmergencyCase; // MyItemType sollte durch den tatsächlichen Typ deines Items ersetzt werden
            if (item != null)
            {

                var editDataContentDialog = new EditDataContentDialog(_contentDialogService.GetDialogHost(), item.Id);

                _ = await editDataContentDialog.ShowAsync();
                if (EmergencyCaseList != null)
                {

                    EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();
                    var found = EmergencyCaseList.FirstOrDefault(x => x != null && x.Id == item.Id);
                    int i = EmergencyCaseList.IndexOf(found);
                    EmergencyCaseList[i] = emergencyCaseDataBase.Get(item.Id);

                }
            }
        }

        private void LoadInformation()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();
            List<Models.EmergencyCase> emergencyCases = emergencyCaseDataBase.LoadData();


            EmergencyCaseList = new ObservableCollection<EmergencyCase?>(emergencyCases);

            stopwatch.Stop();

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

            Stopwatch stopwatch = Stopwatch.StartNew();
            Helpers.ExcelReader excelReader = new Helpers.ExcelReader();
            int newImported = excelReader.ReadExcelFileData1(openFileDialog.FileName);

            stopwatch.Stop();

            // Loggen der Dauer der Operation
            Log.Information("Die ReadExcel dauerte {Duration} Millisekunden.", stopwatch.ElapsedMilliseconds);


            EmergencyCaseList.Clear();

            EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();

            stopwatch = Stopwatch.StartNew();

            List<Models.EmergencyCase> emergencyCases = emergencyCaseDataBase.LoadData();
            EmergencyCaseList = new ObservableCollection<EmergencyCase?>(emergencyCases);
            // Loggen der Dauer der Operation
            Log.Information("Die LoadData dauerte {Duration} Millisekunden.", stopwatch.ElapsedMilliseconds);


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
            Log.Information("Die LoadData dauerte {Duration} Millisekunden.", stopwatch.ElapsedMilliseconds);


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

            Helpers.ExcelReader excelReader = new Helpers.ExcelReader();
            excelReader.SavePdf(new DirectoryInfo(openFolderDialog.FolderName));
        }
    }
}
