using MaterialDesignThemes.Wpf;

namespace ExcelTabellenAuswerung.Models
{
    public class AppSettings
    {
        public int Id { get; set; }
        public BaseTheme Theme { get; set; } = BaseTheme.Dark;
        public string ColumnOrder { get; set; } = "";
    }
}
