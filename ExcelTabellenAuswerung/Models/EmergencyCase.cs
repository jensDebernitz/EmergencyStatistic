namespace ExcelTabellenAuswerung.Models
{
    public class EmegencyCaseReview
    {
        public string? IvenaAnmaledeCode { get; set; }
        public string? IvenaRmc { get; set; }
        public string? IvenaRmz { get; set; }
    }

    public class EmergencyCase
    {
        public int Id { get; set; }
        public string InternalId { get; set; } = "-.-";
        public string? EmergencyDate { get; set; }
        public string? RadioName { get; set; }
        public string? BasicKeyword { get; set; }
        public string? TypeOfUse {  get; set; }
        public string? Diagnosis { get; set; }
        public string? TransportZiel { get; set; }
        public string? IcdCode { get; set; }        
        public string? Befund1Blutdrucksystolisch { get; set; }
        public string? Befund1SpO2 { get; set; }
        public string? Befund1Zucker { get; set; }
        public string? Befund1EtCO2 { get; set; }
        public string? Befund1BlutdruckDiastolisch { get; set; }
        public string? Befund1Bewusstlage { get; set; }
        public string? Befund1Psyche { get; set; }
        public string? Befund1HerzFrequenz { get; set; }
        public string? Befund2BlutdruckSystolisch { get; set; }        
        public string? Befund2BlutdruckDiastolisch { get; set; }        
        public string? Befund2Bewusstlage { get; set; }        
        public string? Befund2EtCO2 { get; set; }
        public string? Befund2Atmung { get; set; }
        public string? Befund2AtmungFrequenz { get; set; }
        public string? Befund2HerzFrequenz { get; set; }
        public string? Befund2Gcs { get; set; }
        public string? DiagnoseCode { get; set; }
        public string? Reanimation { get; set; }
        public string? Beatmung { get; set; }
        public string? ReaniZeitEOFirstResp {get; set; }
        public string? ReaniKollapsBeobDurch {  get; set; }
        public string? ReaniBeginnHDMDurch {  get; set; }
        public string? TransportZielOrt {  get; set; }
        public string? TimeTransportBegin { get; set; }
        public string? TimeAnkunftEinsatzOrt { get; set; }
        public string? PatientGeschlecht { get; set; }
        public string? AmbulanteVersorgung { get; set; }
        public string? NacaScore { get; set; }
        public string? AlarmTime {  get; set; }
        public string? TimeSymptonBegin { get; set; }
        public string? UnfallMechanismus { get; set; }
        public string? DiagnoseGruppe { get; set; }
        public string? PatientAge {  get; set; }
        public string? EinsatzID { get; set; }
        public string? UnfallHergang {  get; set; }
        public string? GrundStichWort { get; set; }
        public string? IvenaAnmaledeCode { get; set; }
        public string? IvenaRmc {  get; set; }
        public string? IvenaRmz { get; set; }
        public string? TimeTransportBeginn {  get; set; }
        public string? TimeAnkunftPatient { get; set; }

        public EmegencyCaseReview? Review1 { get; set; }
        public EmegencyCaseReview? Review2 { get; set; }

    }
}
