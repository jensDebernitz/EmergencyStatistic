using LiteDB;
using MaterialDesignThemes.Wpf;

namespace ExcelTabellenAuswerung.Models
{
    public class EmegencyCaseReview
    {
        public string? IvenaAnmaledeCode { get; set; }
        public string? IvenaRmc { get; set; }
        public string? IvenaRmz { get; set; }
        public string? BlutdruckDiastolisch { get; set; }
        public string? BlutdruckSysstolisch { get; set; }
        public string? Herz { get; set; }
        public string? Zucker { get; set; }
        public string? SpO2 { get; set; }
        public string? Bewusstlage { get; set; }
        public string? Gcs { get; set; }
        public string? Name { get; set; }
        public string? Manchester { get; set; }
        public string? FreeSpace { get; set; }
    }

    public enum Scaling
    {
        noMeasure,
        upScaling,
        downScaling,
        noScaling
    }

    public class EmergencyCase
    {
        public int Id { get; set; }
        //OK 25.10.2024
        public string InternalId { get; set; } = "-.-";

        public Scaling ScalingBewusstsein { get; set; } = Scaling.noMeasure;
        public Scaling ScalingAtmung { get; set; } = Scaling.noMeasure;
        public Scaling ScalingKreislauf { get; set; } = Scaling.noMeasure;
        public Scaling ScalingVerletzung { get; set; } = Scaling.noMeasure;
        public Scaling ScalingNeurologie { get; set; } = Scaling.noMeasure;
        public Scaling ScalingSchmerz { get; set; } = Scaling.noMeasure;

        //OK 25.10.2024
        public string? GrundStichwort { get; set; }
        //OK 25.10.2024
        public string? Diagnosis { get; set; }
        //OK 25.10.2024
        public string? IcdCode { get; set; }
        //OK 25.10.2024
        public string? EinsatzDatum { get; set; }
        //OK 25.10.2024
        public string? Funkname { get; set; }
        //OK 25.10.2024
        public string? TransportZiel { get; set; }
        //OK 25.10.2024
        public string? ZeitAnkunftPatient { get; set; }
        //OK 25.10.2024
        public string? ZeitTransportBeginn { get; set; }
        //OK 25.10.2024
        public string? Befund1Zucker { get; set; }
        //OK 25.10.2024
        public string? Befund1HerzFrequenz { get; set; }
        //OK 25.10.2024
        public string? Befund1Blutdrucksystolisch { get; set; }
        //OK 25.10.2024
        public string? Befund1BlutdruckDiastolisch { get; set; }
        //OK 25.10.2024
        public string? Befund1Bewusstlage { get; set; }
        //OK 25.10.2024
        public string? Befund1GCS { get; set; }
        //OK 25.10.2024
        public string? DiagnoseGruppe { get; set; }
        //OK 25.10.2024
        public string? DiagnoseCode { get; set; }
        //OK 25.10.2024
        public string? NacaScore { get; set; }
        //OK 25.10.2024
        public string? ZeitAnkunftZielklinik { get; set; }
        //OK 25.10.2024
        public string? UnfallHergang { get; set; }
        //OK 25.10.2024
        public string? PatientGeschlecht { get; set; }
        //OK 25.10.2024
        public string? IvenaAnmaledeCode { get; set; }
        //OK 25.10.2024
        public string? IvenaRmc { get; set; }
        //OK 25.10.2024
        public string? IvenaRmz { get; set; }
        //OK 25.10.2024
        public string? Name { get; set; }
        public string? Manchester { get; set; }
        public string? FreeSpace { get; set; }
        //OK 25.10.2024
        public string? Befund1SpO2 { get; set; }

        public EmegencyCaseReview? Review1 { get; set; }
        public EmegencyCaseReview? Review2 { get; set; }

        [BsonIgnore]
        public PackIconKind IsScaling
        {
            get
            {
                bool isScalingBewusstsein = ScalingBewusstsein == Scaling.upScaling || ScalingBewusstsein == Scaling.downScaling;
                bool isScalingAtmung = ScalingAtmung == Scaling.upScaling || ScalingAtmung == Scaling.downScaling;
                bool isScalingKreislauf = ScalingKreislauf == Scaling.upScaling || ScalingKreislauf == Scaling.downScaling;
                bool isScalingVerletzung = ScalingVerletzung == Scaling.upScaling || ScalingVerletzung == Scaling.downScaling;
                bool isScalingNeurologie = ScalingNeurologie == Scaling.upScaling || ScalingNeurologie == Scaling.downScaling;
                bool isScalingSchmerz = ScalingSchmerz == Scaling.upScaling || ScalingSchmerz == Scaling.downScaling;

                if (isScalingBewusstsein || isScalingAtmung || isScalingKreislauf || isScalingVerletzung || isScalingNeurologie || isScalingSchmerz)
                {
                    return PackIconKind.Alert;
                }

                if(Review1 == null || string.IsNullOrEmpty( Review1.IvenaRmc))
                {
                    return PackIconKind.ChatQuestion;
                }

                return PackIconKind.CheckboxMarkedCircle;
            }
        }      
        
        [BsonIgnore]
        public string IsScalingAsString
        {
            get
            {
                bool isScalingBewusstsein = ScalingBewusstsein == Scaling.upScaling || ScalingBewusstsein == Scaling.downScaling;
                bool isScalingAtmung = ScalingAtmung == Scaling.upScaling || ScalingAtmung == Scaling.downScaling;
                bool isScalingKreislauf = ScalingKreislauf == Scaling.upScaling || ScalingKreislauf == Scaling.downScaling;
                bool isScalingVerletzung = ScalingVerletzung == Scaling.upScaling || ScalingVerletzung == Scaling.downScaling;
                bool isScalingNeurologie = ScalingNeurologie == Scaling.upScaling || ScalingNeurologie == Scaling.downScaling;
                bool isScalingSchmerz = ScalingSchmerz == Scaling.upScaling || ScalingSchmerz == Scaling.downScaling;

                if (isScalingBewusstsein || isScalingAtmung || isScalingKreislauf || isScalingVerletzung || isScalingNeurologie || isScalingSchmerz)
                {
                    return "Skallierung";
                }

                if(Review1 == null || string.IsNullOrEmpty( Review1.IvenaRmc))
                {
                    return "Nicht ausgefüllt";
                }

                return "OK";
            }
        }

    }
}
