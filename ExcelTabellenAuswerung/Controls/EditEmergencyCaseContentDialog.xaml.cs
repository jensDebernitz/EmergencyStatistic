using System.Windows.Controls;
using Wpf.Ui.Controls;


namespace ExcelTabellenAuswerung.Controls
{
    /// <summary>
    /// Interaktionslogik für EditDataContentDialog.xaml
    /// </summary>
    public partial class EditDataContentDialog : ContentDialog
    {
        private int _id;
        private Models.EmergencyCase _emergencyCase;
        public EditDataContentDialog(ContentPresenter? contentPresenter, int id) : base(contentPresenter)
        {
            DataBase.EmergencyCaseDataBase emergencyCaseDataBase = new DataBase.EmergencyCaseDataBase();
            _emergencyCase = emergencyCaseDataBase.Get(id);
            _id = id;
            InitializeComponent();
        }


        protected override void OnButtonClick(ContentDialogButton button)
        {
            DataBase.EmergencyCaseDataBase emergencyCaseDataBase = new DataBase.EmergencyCaseDataBase();
            
            emergencyCaseDataBase.Save(_emergencyCase);

            base.OnButtonClick(button);
        }
    }
}
