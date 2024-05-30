using System.Collections.ObjectModel;
using ExcelTabellenAuswerung.Controls;
using ExcelTabellenAuswerung.DataBase;
using ExcelTabellenAuswerung.Models;
using Microsoft.Win32;
using Prism.Commands;
using Serilog;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Windows.Threading;
using Wpf.Ui;
using Wpf.Ui.Controls;
using System.ComponentModel;
using DocumentFormat.OpenXml.EMMA;

namespace ExcelTabellenAuswerung.ViewModels.Pages
{
    public partial class DataViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;
        private readonly SynchronizationContext context = SynchronizationContext.Current;
        private readonly IContentDialogService _contentDialogService;

        [ObservableProperty]
        private Visibility _openedFilePathVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private string _openedFilePath = string.Empty;
        private string _searchText = string.Empty;

        [ObservableProperty]
        private ObservableCollection<EmergencyCase> _emergencyCaseList = [];
        private DispatcherTimer _searchTimer;

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public bool IsInitialized { get; internal set; }

        public DataViewModel(IContentDialogService contentDialogService)
        {
            _contentDialogService = contentDialogService;
            EditCommand = new DelegateCommand<object>(EditAction);
            DeleteCommand = new DelegateCommand<object>(DeleteAction);

            Task.Run(() =>
            {
                _ = LoadInformation();
            });
        }

        private EmergencyCase _selectedItem;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EmergencyCase SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }

        private async Task LoadInformation()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();
            List<Models.EmergencyCase> emergencyCases = emergencyCaseDataBase.LoadData();


            EmergencyCaseList = new ObservableCollection<EmergencyCase>(emergencyCases);   

            stopwatch.Stop();

            // Loggen der Dauer der Operation
            Log.Information("Die LoadInformation dauerte {Duration} Millisekunden.", stopwatch.ElapsedMilliseconds);
        }

        private async void EditAction(object parameter)
        {
            var editDataContentDialog = new EditDataContentDialog(_contentDialogService.GetDialogHost(), Convert.ToInt32(parameter));

            _ = await editDataContentDialog.ShowAsync();
            if (EmergencyCaseList != null)
            {
                
                    EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();
                    var found = EmergencyCaseList.FirstOrDefault(x => x.Id == Convert.ToInt32(parameter));
                    int i = EmergencyCaseList.IndexOf(found);
                    EmergencyCaseList[i] = emergencyCaseDataBase.Get(Convert.ToInt32(parameter));
                
            }

        }

        private async void DeleteAction(object parameter)
        {
            var uiMessageBox = new Wpf.Ui.Controls.MessageBox
            {
                Title = "Achtung!",
                IsPrimaryButtonEnabled = true,
                CloseButtonText = "Abbrechen",
                PrimaryButtonText = "Entfernen",
                Content = "Möchten Sie wirklich die Zeile entfernen?",
            };

            Wpf.Ui.Controls.MessageBoxResult result = await uiMessageBox.ShowDialogAsync();

            if (result == Wpf.Ui.Controls.MessageBoxResult.Primary)
            {

                    //for the Database
                    EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();
                    Models.EmergencyCase emergencyCase = emergencyCaseDataBase.Get(Convert.ToInt32(parameter));
                    emergencyCaseDataBase.Erase(emergencyCase);

                    //for the list
                    emergencyCase = EmergencyCaseList.FirstOrDefault(x =>
                    {
                        return x.Id == emergencyCase.Id;
                    });
                    _ = EmergencyCaseList.Remove(item: emergencyCase);
                
            }
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom() { }

        private void InitializeViewModel()
        {

            _isInitialized = true;
        }
        
        [RelayCommand]
        public async Task OnOpenFile()
        {
            OpenedFilePathVisibility = Visibility.Collapsed;

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
                int newImported = excelReader.ReadExcelFile(openFileDialog.FileName);

                stopwatch.Stop();

                // Loggen der Dauer der Operation
                Log.Information("Die ReadExcel dauerte {Duration} Millisekunden.", stopwatch.ElapsedMilliseconds);


                EmergencyCaseList.Clear();

                EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();

                stopwatch = Stopwatch.StartNew();

                List<Models.EmergencyCase> emergencyCases = emergencyCaseDataBase.LoadData();
                EmergencyCaseList = new ObservableCollection<EmergencyCase>(emergencyCases);
                // Loggen der Dauer der Operation
                Log.Information("Die LoadData dauerte {Duration} Millisekunden.", stopwatch.ElapsedMilliseconds);


                OpenedFilePath = newImported.ToString();
                OpenedFilePathVisibility = Visibility.Visible;
            

        }

    }
}
