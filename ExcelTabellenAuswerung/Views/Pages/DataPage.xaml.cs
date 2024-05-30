using ExcelTabellenAuswerung.DataBase;
using ExcelTabellenAuswerung.ViewModels.Pages;
using Serilog;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace ExcelTabellenAuswerung.Views.Pages
{
    public partial class DataPage : INavigableView<DataViewModel>
    {
        public DataViewModel ViewModel { get; }
        bool _shown;

        public DataPage(DataViewModel viewModel)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
            stopwatch.Stop();

            // Loggen der Dauer der Operation
            Log.Information("Die DataPage(DataViewModel viewModel) dauerte {Duration} Millisekunden.", stopwatch.ElapsedMilliseconds);
        }

        private bool _isScrolling;

        private void OnFixedScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_isScrolling)
                return;

            _isScrolling = true;
            SyncScroll(FixedListView, ScrollableListView, e);
            _isScrolling = false;
        }

        private void OnScrollableScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_isScrolling)
                return;

            _isScrolling = true;
            SyncScroll(ScrollableListView, FixedListView, e);
            _isScrolling = false;
        }

        private void SyncScroll(Wpf.Ui.Controls.ListView source, Wpf.Ui.Controls.ListView target, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange != 0)
            {
                ScrollViewer sourceScrollViewer = GetScrollViewer(source);
                ScrollViewer targetScrollViewer = GetScrollViewer(target);

                if (sourceScrollViewer != null && targetScrollViewer != null)
                {
                    targetScrollViewer.ScrollToVerticalOffset(sourceScrollViewer.VerticalOffset);
                }
            }
        }

        private ScrollViewer? GetScrollViewer(DependencyObject o)
        {
            if (o is ScrollViewer)
                return o as ScrollViewer;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
            {
                var child = VisualTreeHelper.GetChild(o, i);
                var result = GetScrollViewer(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (_shown)
                return;

            _shown = true;
            ViewModel.IsInitialized = true;
            SetDisplayIndex();
        }

        private void SetDisplayIndex()
        {
            //SettingsDataBase settingsDataBase = new SettingsDataBase();
            //Models.AppSettings appSettings = settingsDataBase.LoadAppSettings();

            //if (appSettings != null)
            //{
            //    string columnOrder = appSettings.ColumnOrder;

            //    if (string.IsNullOrEmpty(columnOrder) == false)
            //    {
            //        string[] columns = columnOrder.Split(';');
            //        foreach (string column in columns)
            //        {
            //            string[] columnValues = column.Split(",");

            //            foreach (DataGridColumn gridColumn in dataGridEmergencyCases.Columns)
            //            {
            //                if (gridColumn.Header.ToString() == columnValues[0])
            //                {
            //                    gridColumn.DisplayIndex = Convert.ToInt32(columnValues[1]);
            //                }
            //            }
            //        }
            //    }
            //}
        }

        private void DataGrid_ColumnDisplayIndexChanged(object sender, DataGridColumnEventArgs e)
        {
            //string columnOrder = "";

            //foreach (DataGridColumn column in dataGridEmergencyCases.Columns)
            //{
            //    columnOrder += column.Header.ToString() + "," + column.DisplayIndex + ";";
            //}

            //SettingsDataBase settingsDataBase = new SettingsDataBase();
            //Models.AppSettings appSettings = settingsDataBase.LoadAppSettings();
            //appSettings.ColumnOrder = columnOrder;
            //settingsDataBase.SaveAppSettings(appSettings);
        }

        private void ScrollableListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.OnDoubleClick(ScrollableListView.SelectedItem);
        }
    }
}
