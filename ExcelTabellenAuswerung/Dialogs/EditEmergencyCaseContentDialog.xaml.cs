using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;
using ExcelTabellenAuswerung.Models;
using LiteDB;
using System.IO;
using Patagames.Pdf.Net;
using static System.Environment;
using MessageBox = System.Windows.MessageBox;
using System.Windows;
using Serilog;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ClosedXML;
using CommunityToolkit.Mvvm.ComponentModel;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Text;
using System.Windows.Media.Converters;
using ExcelTabellenAuswerung.Helpers.Validators;
using Style = System.Windows.Style;

namespace ExcelTabellenAuswerung.Controls;

public class EditEmergencyData : ObservableObject
{
    private string _what = "";
    private string _clinicalRegistration = "";
    private string _clinicalEvaluation = "000000";
    public bool EnableLengthValidation { get; set; } = false;
    public bool EnableNoEmptyValidation { get; set; } = false;
    public int MinLength { get; set; } = -1;
    public int MaxLength { get; set; } = -1;

    public string What
    {
        get { return _what;}
        set { SetProperty(ref _what, value); }
    }
    public string ClinicalRegistration
    {
        get { return _clinicalRegistration; }
        set { SetProperty(ref _clinicalRegistration, value); }
    }
    public string ClinicalEvaluation
    {
        get { return _clinicalEvaluation; }
        set { SetProperty(ref _clinicalEvaluation, value); }
    }
}

/// <summary>
/// Interaktionslogik für EditDataContentDialog.xaml
/// </summary>
///
public partial class EditDataContentDialog : Window
{
    private int _id;
    private Models.EmergencyCase _emergencyCase;
    private EditEmergencyData rmcData;
    public ObservableCollection<EditEmergencyData> _emergencyData { get; } = new();

    public EditDataContentDialog(int id)
    {
        DataBase.EmergencyCaseDataBase emergencyCaseDataBase = new DataBase.EmergencyCaseDataBase();
        DataContext = this;
        _emergencyCase = emergencyCaseDataBase.Get(id);
        _id = id;

        InitializeComponent();

        titleTextBlock.Text = $"Einsatz-Dokumentation & Statistik";

        _emergencyData.Add(new EditEmergencyData()
        {
            What = "IVENA",
            ClinicalRegistration = _emergencyCase.IvenaAnmaledeCode,
            ClinicalEvaluation = _emergencyCase.Review1 != null ? _emergencyCase.Review1.IvenaAnmaledeCode : ""
        });

        _emergencyData.Add(new EditEmergencyData()
        {
            What = "IVENA RMZ",
            ClinicalRegistration = _emergencyCase.IvenaRmz,
            ClinicalEvaluation = _emergencyCase.Review1 != null ? _emergencyCase.Review1.IvenaRmz : ""
        });

        rmcData = new EditEmergencyData()
        {
            What = "IVENA RMC",
            ClinicalRegistration = _emergencyCase.IvenaRmc,
            ClinicalEvaluation = _emergencyCase.Review1 != null && _emergencyCase.Review1.IvenaRmc != null
                ? _emergencyCase.Review1.IvenaRmc
                : "000000",
            EnableLengthValidation = true,
            MinLength = 6,
            MaxLength = 6,
            EnableNoEmptyValidation = true
        };

        _emergencyData.Add(rmcData);
        rmcData.PropertyChanged += Data_PropertyChanged;

        _emergencyData.Add(new EditEmergencyData()
        {
            What = "Blutdruck Diastolisch",
            ClinicalRegistration = _emergencyCase.Befund1BlutdruckDiastolisch,
            ClinicalEvaluation = _emergencyCase.Review1 != null ? _emergencyCase.Review1.BlutdruckDiastolisch : ""
        });

        _emergencyData.Add(new EditEmergencyData()
        {
            What = "Blutdruck Sysstolisch",
            ClinicalRegistration = _emergencyCase.Befund1Blutdrucksystolisch,
            ClinicalEvaluation = _emergencyCase.Review1 != null ? _emergencyCase.Review1.BlutdruckSysstolisch : ""
        });

        _emergencyData.Add(new EditEmergencyData()
        {
            What = "Herz",
            ClinicalRegistration = _emergencyCase.Befund1HerzFrequenz,
            ClinicalEvaluation = _emergencyCase.Review1 != null ? _emergencyCase.Review1.Herz : ""
        });

        _emergencyData.Add(new EditEmergencyData()
        {
            What = "Zucker",
            ClinicalRegistration = _emergencyCase.Befund1Zucker,
            ClinicalEvaluation = _emergencyCase.Review1 != null ? _emergencyCase.Review1.Herz : ""
        });

        _emergencyData.Add(new EditEmergencyData()
        {
            What = "SpO2",
            ClinicalRegistration = _emergencyCase.Befund1SpO2,
            ClinicalEvaluation = _emergencyCase.Review1 != null ? _emergencyCase.Review1.SpO2 : ""
        });

        _emergencyData.Add(new EditEmergencyData()
        {
            What = "Bewusstlage",
            ClinicalRegistration = _emergencyCase.Befund1Bewusstlage,
            ClinicalEvaluation = _emergencyCase.Review1 != null ? _emergencyCase.Review1.Bewusstlage : ""
        });

        _emergencyData.Add(new EditEmergencyData()
        {
            What = "GCS",
            ClinicalRegistration = _emergencyCase.Befund1GCS,
            ClinicalEvaluation = _emergencyCase.Review1 != null ? _emergencyCase.Review1.Gcs : ""
        });

        _emergencyData.Add(new EditEmergencyData()
        {
            What = "Name",
            ClinicalRegistration = _emergencyCase.Name,
            ClinicalEvaluation = _emergencyCase.Review1 != null ? _emergencyCase.Review1.Name : ""
        });

        _emergencyData.Add(new EditEmergencyData()
        {
            What = "Manchester",
            ClinicalRegistration = _emergencyCase.Manchester,
            ClinicalEvaluation = _emergencyCase.Review1 != null ? _emergencyCase.Review1.Manchester : ""
        });

        _emergencyData.Add(new EditEmergencyData()
        {
            What = "Free Space",
            ClinicalRegistration = _emergencyCase.FreeSpace,
            ClinicalEvaluation = _emergencyCase.Review1 != null ? _emergencyCase.Review1.FreeSpace : ""
        });

        Data_PropertyChanged(this, new PropertyChangedEventArgs(String.Empty));
    }


    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        Models.Scaling bewusstsein;
        Models.Scaling atmung;
        Models.Scaling kreislauf;
        Models.Scaling verletzung;
        Models.Scaling neurologie;
        Models.Scaling schmerz;

        checkScaling(out bewusstsein, out atmung, out kreislauf, out verletzung, out neurologie, out schmerz);

        _emergencyCase.ScalingBewusstsein = bewusstsein;
        _emergencyCase.ScalingAtmung = atmung;
        _emergencyCase.ScalingKreislauf = kreislauf;
        _emergencyCase.ScalingVerletzung = verletzung;
        _emergencyCase.ScalingNeurologie = neurologie;
        _emergencyCase.ScalingSchmerz = schmerz;

        DataBase.EmergencyCaseDataBase emergencyCaseDataBase = new DataBase.EmergencyCaseDataBase();

        _emergencyCase.Review1 = new Models.EmegencyCaseReview();

        foreach (EditEmergencyData editEmergencyData in _emergencyData)
        {
            switch (editEmergencyData.What)
            {
                case "IVENA":
                    _emergencyCase.Review1.IvenaAnmaledeCode = editEmergencyData.ClinicalEvaluation;
                    break;
                case "IVENA RMZ":
                    _emergencyCase.Review1.IvenaRmz = editEmergencyData.ClinicalEvaluation;
                    break;
                case "IVENA RMC":
                    _emergencyCase.Review1.IvenaRmc = editEmergencyData.ClinicalEvaluation;
                    break;
                case "Blutdruck Diastolisch":
                    _emergencyCase.Review1.BlutdruckDiastolisch = editEmergencyData.ClinicalEvaluation;
                    break;
                case "Blutdruck Sysstolisch":
                    _emergencyCase.Review1.BlutdruckSysstolisch = editEmergencyData.ClinicalEvaluation;
                    break;
                case "Herz":
                    _emergencyCase.Review1.Herz = editEmergencyData.ClinicalEvaluation;
                    break;
                case "Zucker":
                    _emergencyCase.Review1.Zucker = editEmergencyData.ClinicalEvaluation;
                    break;
                case "SpO2":
                    _emergencyCase.Review1.SpO2 = editEmergencyData.ClinicalEvaluation;
                    break;
                case "Bewusstlage":
                    _emergencyCase.Review1.Bewusstlage = editEmergencyData.ClinicalEvaluation;
                    break;
                case "GCS":
                    _emergencyCase.Review1.Gcs = editEmergencyData.ClinicalEvaluation;
                    break;
                case "Name":
                    _emergencyCase.Review1.Name = editEmergencyData.ClinicalEvaluation;
                    break;
                case "Manchester":
                    _emergencyCase.Review1.Manchester = editEmergencyData.ClinicalEvaluation;
                    break;
                case "Free Space":
                    _emergencyCase.Review1.FreeSpace = editEmergencyData.ClinicalEvaluation;
                    break;
            }
        }

        emergencyCaseDataBase.Save(_emergencyCase);

        Close();
    }

    private void Data_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (rmcData.ClinicalEvaluation is { Length: 6 })
        {
            string tempRmc = rmcData.ClinicalEvaluation;
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

    protected void OnButtonClick()
    {


    }

    private void checkScaling(out Models.Scaling bewusstsein,
        out Models.Scaling atmung,
        out Models.Scaling kreislauf,
        out Models.Scaling verletzung,
        out Models.Scaling neurologie,
        out Models.Scaling schmerz)
    {
        //check RMC Scaling
        string? emergencyRmc = rmcData.ClinicalRegistration;
        string? hospitalRmc = rmcData.ClinicalEvaluation;

        if (string.IsNullOrEmpty(emergencyRmc))
        {
            bewusstsein = Models.Scaling.noMeasure;
            atmung = Models.Scaling.noMeasure;
            kreislauf = Models.Scaling.noMeasure;
            verletzung = Models.Scaling.noMeasure;
            neurologie = Models.Scaling.noMeasure;
            schmerz = Models.Scaling.noMeasure;

            return;
        }

        int emergencyCode1 = Convert.ToInt32(emergencyRmc.Substring(0, 1));
        int hospitalCode1 = Convert.ToInt32(hospitalRmc.Substring(0, 1));

        int emergencyCode2 = Convert.ToInt32(emergencyRmc.Substring(1, 1));
        int hospitalCode2 = Convert.ToInt32(hospitalRmc.Substring(1, 1));

        int emergencyCode3 = Convert.ToInt32(emergencyRmc.Substring(2, 1));
        int hospitalCode3 = Convert.ToInt32(hospitalRmc.Substring(2, 1));

        int emergencyCode4 = Convert.ToInt32(emergencyRmc.Substring(3, 1));
        int hospitalCode4 = Convert.ToInt32(hospitalRmc.Substring(3, 1));

        int emergencyCode5 = Convert.ToInt32(emergencyRmc.Substring(4, 1));
        int hospitalCode5 = Convert.ToInt32(hospitalRmc.Substring(4, 1));

        int emergencyCode6 = Convert.ToInt32(emergencyRmc.Substring(5, 1));
        int hospitalCode6 = Convert.ToInt32(hospitalRmc.Substring(5, 1));

        if (hospitalCode1 > emergencyCode1)
        {
            bewusstsein = Models.Scaling.upScaling;
        }
        else if (hospitalCode1 < emergencyCode1)
        {
            bewusstsein = Models.Scaling.downScaling;
        }
        else
        {
            bewusstsein = Models.Scaling.noScaling;
        }


        if (hospitalCode2 > emergencyCode2)
        {
            atmung = Models.Scaling.upScaling;
        }
        else if (hospitalCode2 < emergencyCode2)
        {
            atmung = Models.Scaling.downScaling;
        }
        else
        {
            atmung = Models.Scaling.noScaling;
        }


        if (hospitalCode3 > emergencyCode3)
        {
            kreislauf = Models.Scaling.upScaling;
        }
        else if (hospitalCode3 < emergencyCode3)
        {
            kreislauf = Models.Scaling.downScaling;
        }
        else
        {
            kreislauf = Models.Scaling.noScaling;
        }


        if (hospitalCode4 > emergencyCode4)
        {
            verletzung = Models.Scaling.upScaling;
        }
        else if (hospitalCode4 < emergencyCode4)
        {
            verletzung = Models.Scaling.downScaling;
        }
        else
        {
            verletzung = Models.Scaling.noScaling;
        }


        if (hospitalCode5 > emergencyCode5)
        {
            neurologie = Models.Scaling.upScaling;
        }
        else if (hospitalCode5 < emergencyCode5)
        {
            neurologie = Models.Scaling.downScaling;
        }
        else
        {
            neurologie = Models.Scaling.noScaling;
        }


        if (hospitalCode6 > emergencyCode6)
        {
            schmerz = Models.Scaling.upScaling;
        }
        else if (hospitalCode6 < emergencyCode6)
        {
            schmerz = Models.Scaling.downScaling;
        }
        else
        {
            schmerz = Models.Scaling.noScaling;
        }
        //check RMZ Scaling
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        using (var db = new LiteDatabase(Helpers.DataBase.DataBaseFileDocuments()))
        {
            var fileStorage = db.FileStorage;
            List<LiteFileInfo<string>> fileInfo =
                fileStorage.Find(x => x.Id.Contains(_emergencyCase.InternalId)).ToList();

            if (fileInfo is { Count: > 0 })
            {
                string filePath = GetFolderPath(SpecialFolder.CommonApplicationData) + "/ExcelTabellenAuswertung/" +
                                  fileInfo[0].Filename;
                try
                {
                    fileInfo[0].SaveAs(filePath);
                }
                catch (Exception exception)
                {
                    Log.Error(exception.Message);
                    throw;
                }

                // Starts Microsoft Edge with the provided URL; NOTE: /C to terminate CMD after the command runs
                System.Diagnostics.Process.Start($"CMD.exe", $"/C start msedge {filePath}");
            }
            else
            {
                MessageBox.Show("Es konnte kein Pdf gefunden werden");
            }
        }
    }

    private void borderBewusstsein_MouseDown(object sender, MouseButtonEventArgs e)
    {
        Border snBorder = sender as Border;
        string changeString = snBorder.Name.Substring(snBorder.Name.Length - 1, 1);

        EditEmergencyData editEmergencyData = _emergencyData.First(x => x.What == "IVENA RMC");
        if (editEmergencyData != null)
        {
            editEmergencyData.ClinicalEvaluation = ReplaceAtIndex(editEmergencyData.ClinicalEvaluation, 0,
                changeString.ToCharArray()[0]);
        }
    }

    private void borderAtmung_MouseDown(object sender, MouseButtonEventArgs e)
    {
        Border snBorder = sender as Border;
        string changeString = snBorder.Name.Substring(snBorder.Name.Length - 1, 1);

        EditEmergencyData editEmergencyData = _emergencyData.First(x => x.What == "IVENA RMC");
        if (editEmergencyData != null)
        {
            editEmergencyData.ClinicalEvaluation = ReplaceAtIndex(editEmergencyData.ClinicalEvaluation, 1,
                changeString.ToCharArray()[0]);
        }
    }

    private void borderKreislauf_MouseDown(object sender, MouseButtonEventArgs e)
    {
        Border snBorder = sender as Border;
        string changeString = snBorder.Name.Substring(snBorder.Name.Length - 1, 1);

        EditEmergencyData editEmergencyData = _emergencyData.First(x => x.What == "IVENA RMC");
        if (editEmergencyData != null)
        {
            editEmergencyData.ClinicalEvaluation = ReplaceAtIndex(editEmergencyData.ClinicalEvaluation, 2,
                changeString.ToCharArray()[0]);
        }
    }

    private void borderVerletzung_MouseDown(object sender, MouseButtonEventArgs e)
    {
        Border snBorder = sender as Border;
        string changeString = snBorder.Name.Substring(snBorder.Name.Length - 1, 1);

        EditEmergencyData editEmergencyData = _emergencyData.First(x => x.What == "IVENA RMC");
        if (editEmergencyData != null)
        {
            editEmergencyData.ClinicalEvaluation = ReplaceAtIndex(editEmergencyData.ClinicalEvaluation, 3,
                changeString.ToCharArray()[0]);
        }
    }

    private void borderNeurologie_MouseDown(object sender, MouseButtonEventArgs e)
    {
        Border snBorder = sender as Border;
        string changeString = snBorder.Name.Substring(snBorder.Name.Length - 1, 1);

        EditEmergencyData editEmergencyData = _emergencyData.First(x => x.What == "IVENA RMC");
        if (editEmergencyData != null)
        {
            editEmergencyData.ClinicalEvaluation = ReplaceAtIndex(editEmergencyData.ClinicalEvaluation, 4,
                changeString.ToCharArray()[0]);
        }
    }

    private void borderSchmerz_MouseDown(object sender, MouseButtonEventArgs e)
    {
        Border snBorder = sender as Border;
        string changeString = snBorder.Name.Substring(snBorder.Name.Length - 1, 1);

        EditEmergencyData editEmergencyData = _emergencyData.First(x => x.What == "IVENA RMC");
        if (editEmergencyData != null)
        {
            editEmergencyData.ClinicalEvaluation = ReplaceAtIndex(editEmergencyData.ClinicalEvaluation, 5,
                changeString.ToCharArray()[0]);
        }
    }

    public static string ReplaceAtIndex(string text, int index, char c)
    {
        if (text.Length < 6)
        {
            text = text.PadRight(6, '1');
        }

        var stringBuilder = new StringBuilder(text);
        stringBuilder[index] = c;
        return stringBuilder.ToString();
    }
}

