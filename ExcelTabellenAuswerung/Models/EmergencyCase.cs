using LiteDB;
using Wpf.Ui.Controls;

namespace ExcelTabellenAuswerung.Models
{
    public class EmegencyCaseReview
    {
        public string? IvenaAnmaledeCode { get; set; }
        public string? IvenaRmc { get; set; }
        public string? IvenaRmz { get; set; }
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
        public string InternalId { get; set; } = "-.-";

        public Scaling ScalingBewusstsein { get; set; } = Scaling.noMeasure;
        public Scaling ScalingAtmung { get; set; } = Scaling.noMeasure;
        public Scaling ScalingKreislauf { get; set; } = Scaling.noMeasure;
        public Scaling ScalingVerletzung { get; set; } = Scaling.noMeasure;
        public Scaling ScalingNeurologie { get; set; } = Scaling.noMeasure;
        public Scaling ScalingSchmerz { get; set; } = Scaling.noMeasure;

        public string? GrundStichwort { get; set; }
        public string? Diagnosis { get; set; }
        public string? IcdCode { get; set; }
        public string? EinsatzDatum { get; set; }
        public string? EinsatzOrtStrasseNummer { get; set; }
        public string? Funkname { get; set; }
        public string? TransportZiel { get; set; }
        public string? ZeitAnkunftPatient { get; set; }
        public string? ZeitTransportBeginn { get; set; }
        public string? Befund1Zucker { get; set; }
        public string? Befund1HerzFrequenz { get; set; }
        public string? Befund1Blutdrucksystolisch { get; set; }
        public string? Befund1BlutdruckDiastolisch { get; set; }
        public string? Befund1Bewusstlage { get; set; }
        public string? Befund1GCS { get; set; }
        public string? DiagnoseGruppe { get; set; }
        public string? DiagnoseCode { get; set; }
        public string? NacaScore { get; set; }
        public string? ZeitAnkunftZielklinik { get; set; }
        public string? UnfallHergang { get; set; }
        public string? PatientGeschlecht { get; set; }
        public string? IvenaAnmaledeCode { get; set; }
        public string? IvenaRmc { get; set; }
        public string? IvenaRmz { get; set; }

        public EmegencyCaseReview? Review1 { get; set; }
        public EmegencyCaseReview? Review2 { get; set; }

        [BsonIgnore]
        public SymbolRegular IsScaling
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
                    return SymbolGlyph.Parse("Warning28");
                }

                if(Review1 == null || string.IsNullOrEmpty( Review1.IvenaAnmaledeCode))
                {
                    return SymbolGlyph.Parse("ChatEmpty28");
                }

                return SymbolGlyph.Parse("CheckmarkCircle24");
            }
        }

    }
}
