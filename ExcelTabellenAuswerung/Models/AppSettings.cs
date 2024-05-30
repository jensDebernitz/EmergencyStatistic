using Wpf.Ui.Appearance;

namespace ExcelTabellenAuswerung.Models
{
    public class AppSettings
    {
        public int Id { get; set; }
        public ApplicationTheme Theme { get; set; } = ApplicationTheme.Light;
        public string ColumnOrder { get; set; } = "";
    }
}
