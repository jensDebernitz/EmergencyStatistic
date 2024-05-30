namespace ExcelTabellenAuswerung.Models
{
    public class EmegencyCaseReview
    {
        public string IvenaAnmaledeCode { get; set; }
        public string IvenaRmc { get; set; }
        public string IvenaRmz { get; set; }
    }

    public class EmergencyCase
    {
        public int Id { get; set; }
        public string InternalId { get; set; } = "-.-";
        public DateTime EmergencyDate { get; set; }
        public string RadioName { get; set; }
        public string BasicKeyword { get; set; }
        public string TypeOfUse {  get; set; }
        public string Diagnosis { get; set; }
        public string TransportDestination { get; set; }
        public string IcdCode { get; set; }        
        public int Befund1Blutdrucksystolisch { get; set; }
        public int Befund1SpO2 { get; set; }
        public int Befund1Zucker { get; set; }
        public int Befund1EtCO2 { get; set; }
        public int Befund1BlutdruckDiastolisch { get; set; }
        public string Befund1Bewusstlage { get; set; }
        public string Befund1Psyche { get; set; }
        public int Befund1HerzFrequenz { get; set; }
        public int Befund2BlutdruckSystolisch { get; set; }        
        public int Befund2BlutdruckDiastolisch { get; set; }        
        public string Befund2Bewusstlage { get; set; }        
        public int Befund2EtCO2 { get; set; }
        public string Befund2Atmung { get; set; }
        public int Befund2AtmungFrequenz { get; set; }
        public int Befund2HerzFrequenz { get; set; }
        public int Befund2Gcs { get; set; }
        public string DiagnoseCode { get; set; }
        public bool Reanimation { get; set; }
        public bool Beatmung { get; set; }
        public string ReaniZeitEOFirstResp {get; set; }
        public string ReaniKollapsBeobDurch {  get; set; }
        public string ReaniBeginnHDMDurch {  get; set; }
        public string TransportZiel {  get; set; }
        public DateTime TimeTransportBegin { get; set; }
        public DateTime TimeAnkunftEinsatzOrt { get; set; }
        public string PatientGeschlecht { get; set; }
        public bool AmbulanteVersorgung { get; set; }
        public string NacaScore { get; set; }
        public DateTime AlarmTime {  get; set; }
        public DateTime TimeSymptonBegin { get; set; }
        public string UnfallMechanismus { get; set; }
        public string DiagnoseGruppe { get; set; }
        public int PatientAge {  get; set; }
        public int EinsatzID { get; set; }
        public string UnfallHergang {  get; set; }
        public string GrundStichWort { get; set; }
        public string IvenaAnmaledeCode { get; set; }
        public string IvenaRmc {  get; set; }
        public string IvenaRmz { get; set; }
        public string TimeAnkunftPatient { get; set; }

        public EmegencyCaseReview Review1 { get; set; }
        public EmegencyCaseReview Review2 { get; set; }

    }
}
