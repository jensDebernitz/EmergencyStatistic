using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Controls;
using System.Text.RegularExpressions;

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


            textIvena.Text = _emergencyCase.IvenaAnmaledeCode;
            textIvenaRMC.Text = _emergencyCase.IvenaRmc;
            textIvenaRMZ.Text = _emergencyCase.IvenaRmz;

            if (_emergencyCase.Review1 != null)
            {
                textBoxIvena.Text = _emergencyCase.Review1.IvenaAnmaledeCode;
                textBoxIvenaRMC.Text = _emergencyCase.Review1.IvenaRmc;
                textBoxIvenaRMZ.Text = _emergencyCase.Review1.IvenaRmz;
            }
        }


        protected override void OnButtonClick(ContentDialogButton button)
        {
            DataBase.EmergencyCaseDataBase emergencyCaseDataBase = new DataBase.EmergencyCaseDataBase();

            _emergencyCase.Review1 = new Models.EmegencyCaseReview()
            {
                IvenaAnmaledeCode = textBoxIvena.Text,
                IvenaRmc = textBoxIvenaRMC.Text,
                IvenaRmz = textBoxIvenaRMZ.Text
            };

            emergencyCaseDataBase.Save(_emergencyCase);

            base.OnButtonClick(button);
        }

        
    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    private void textBoxIvenaRMC_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(textBoxIvenaRMC.Text.Length == 6)
            {
                string tempRmc = textBoxIvenaRMC.Text;
                string subString = tempRmc.Substring(0, 1);
                switch (subString)
                {
                    case "1":
                        borderBewusstsein1.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderBewusstsein2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderBewusstsein3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderBewusstsein4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderBewusstsein5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "2":
                        borderBewusstsein1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderBewusstsein2.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderBewusstsein3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderBewusstsein4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderBewusstsein5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "3":
                        borderBewusstsein1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderBewusstsein2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderBewusstsein3.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderBewusstsein4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderBewusstsein5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "4":
                        borderBewusstsein1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderBewusstsein2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderBewusstsein3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderBewusstsein4.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderBewusstsein5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "5":
                        borderBewusstsein1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderBewusstsein2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderBewusstsein3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderBewusstsein4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderBewusstsein5.BorderBrush = new SolidColorBrush(Colors.Black);
                        break;
                }

                subString = tempRmc.Substring(1, 1);
                switch (subString)
                {
                    case "1":
                        borderAtmung1.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderAtmung2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderAtmung3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderAtmung4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderAtmung5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "2": 
                        borderAtmung1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderAtmung2.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderAtmung3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderAtmung4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderAtmung5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "3": 
                        borderAtmung1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderAtmung2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderAtmung3.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderAtmung4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderAtmung5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "4": 
                        borderAtmung1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderAtmung2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderAtmung3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderAtmung4.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderAtmung5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "5": 
                        borderAtmung1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderAtmung2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderAtmung3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderAtmung4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderAtmung5.BorderBrush = new SolidColorBrush(Colors.Black);
                        break;
                }

                subString = tempRmc.Substring(2, 1);
                switch (subString)
                {
                    case "1":
                        borderKreislauf1.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderKreislauf2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderKreislauf3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderKreislauf4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderKreislauf5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "2": 
                        borderKreislauf1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderKreislauf2.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderKreislauf3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderKreislauf4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderKreislauf5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "3": 
                        borderKreislauf1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderKreislauf2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderKreislauf3.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderKreislauf4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderKreislauf5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "4": 
                        borderKreislauf1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderKreislauf2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderKreislauf3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderKreislauf4.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderKreislauf5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "5": 
                        borderKreislauf1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderKreislauf2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderKreislauf3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderKreislauf4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderKreislauf5.BorderBrush = new SolidColorBrush(Colors.Black);
                        break;
                }

                subString = tempRmc.Substring(3, 1);
                switch (subString)
                {
                    case "1":
                        borderVerletzung1.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderVerletzung2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderVerletzung3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderVerletzung4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderVerletzung5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "2": 
                        borderVerletzung1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderVerletzung2.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderVerletzung3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderVerletzung4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderVerletzung5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "3": 
                        borderVerletzung1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderVerletzung2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderVerletzung3.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderVerletzung4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderVerletzung5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "4": 
                        borderVerletzung1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderVerletzung2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderVerletzung3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderVerletzung4.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderVerletzung5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "5": 
                        borderVerletzung1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderVerletzung2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderVerletzung3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderVerletzung4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderVerletzung5.BorderBrush = new SolidColorBrush(Colors.Black);
                        break;
                }

                subString = tempRmc.Substring(4, 1);
                switch (subString)
                {
                    case "1":
                        borderNeurologie1.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderNeurologie2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderNeurologie3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderNeurologie4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderNeurologie5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "2": 
                        borderNeurologie1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderNeurologie2.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderNeurologie3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderNeurologie4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderNeurologie5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "3": 
                        borderNeurologie1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderNeurologie2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderNeurologie3.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderNeurologie4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderNeurologie5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "4": 
                        borderNeurologie1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderNeurologie2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderNeurologie3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderNeurologie4.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderNeurologie5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "5": 
                        borderNeurologie1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderNeurologie2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderNeurologie3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderNeurologie4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderNeurologie5.BorderBrush = new SolidColorBrush(Colors.Black);
                        break;
                }


                subString = tempRmc.Substring(5, 1);
                switch (subString)
                {
                    case "1":
                        borderSchmerz1.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderSchmerz2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderSchmerz3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderSchmerz4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderSchmerz5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "2": 
                        borderSchmerz1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderSchmerz2.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderSchmerz3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderSchmerz4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderSchmerz5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "3": 
                        borderSchmerz1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderSchmerz2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderSchmerz3.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderSchmerz4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderSchmerz5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "4": 
                        borderSchmerz1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderSchmerz2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderSchmerz3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderSchmerz4.BorderBrush = new SolidColorBrush(Colors.Black);
                        borderSchmerz5.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        break;
                    case "5": 
                        borderSchmerz1.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderSchmerz2.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderSchmerz3.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderSchmerz4.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        borderSchmerz5.BorderBrush = new SolidColorBrush(Colors.Black);
                        break;
                }
            }
        }
    }
}
