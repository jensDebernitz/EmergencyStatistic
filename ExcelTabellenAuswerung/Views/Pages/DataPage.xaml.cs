using ExcelTabellenAuswerung.DataBase;
using ExcelTabellenAuswerung.ViewModels.Pages;
using Serilog;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Xml.Serialization;
using Wpf.Ui.Controls;

namespace ExcelTabellenAuswerung.Views.Pages
{
    public partial class DataPage : INavigableView<DataViewModel>, IDisposable
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
            LoadColumnState();
        }

        private void LoadColumnState()
        {
            SettingsDataBase settingsDataBase = new SettingsDataBase();
            Models.AppSettings appSettings = settingsDataBase.LoadAppSettings();
            bool deserializationSucceeded = true;
            List<String> columnOrder = null;

            try
            {               
                columnOrder = DeserializeFromXML<List<string>>(appSettings.ColumnOrder);
            }
            catch (Exception ex)
            {
                deserializationSucceeded = false;
            }

            if (deserializationSucceeded && columnOrder != null && columnOrder.Count > 0)
            {
                int newIndex = 0;
                foreach (var colName in columnOrder)
                {

                    int oldIndex = 0;
                    for (int i = 0; i < EmergencyGridView.Columns.Count; i++)
                    {
                        if (EmergencyGridView.Columns[i].Header.ToString().Equals(colName))
                        {
                            oldIndex = i;
                            break;
                        }

                    }
                    EmergencyGridView.Columns.Move(oldIndex, newIndex++);
                }
            }
        }

        void SaveColumnState()
        {
            List<String> columnOrder = new List<string>();
            foreach (var column in EmergencyGridView.Columns)
            {
                columnOrder.Add(column.Header.ToString());
                Log.Information(column.Header.ToString());
            }

            string xml = SerializeToXML(columnOrder);

            SettingsDataBase settingsDataBase = new SettingsDataBase();
            Models.AppSettings appSettings = settingsDataBase.LoadAppSettings();
            appSettings.ColumnOrder = xml;
            settingsDataBase.SaveAppSettings(appSettings);
        }

        public void Dispose()
        {
            SaveColumnState();
        }

        public static T DeserializeFromXML<T>(string inString)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            TextReader textReader = (TextReader)new StringReader(inString);
            T retVal = (T)deserializer.Deserialize(textReader);
            textReader.Close();
            return retVal;
        }

        public static string SerializeToXML<T>(T t, XmlSerializerNamespaces inNameSpaces = null)
        {
            XmlSerializerNamespaces ns = inNameSpaces;
            if (ns == null)
            {
                ns = new XmlSerializerNamespaces();
                ns.Add("", "");
            }
            XmlSerializer serializer = new XmlSerializer(t.GetType());
            TextWriter textWriter = (TextWriter)new StringWriter();
            serializer.Serialize(textWriter, t, ns);
            return textWriter.ToString();
        }

      

        private void ScrollableListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.OnDoubleClick(ScrollableListView.SelectedItem);
        }
    }
}
