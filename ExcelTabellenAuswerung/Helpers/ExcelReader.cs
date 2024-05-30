using ClosedXML.Excel;
using ExcelTabellenAuswerung.DataBase;
using ExcelTabellenAuswerung.Models;
using Serilog;
using System.Diagnostics;

namespace ExcelTabellenAuswerung.Helpers
{
    public class ExcelReader
    {
        public int ReadExcelFileData1(string filePath)
        {
            int newReadRows = 0;

            try
            {
                // Öffnen Sie eine vorhandene Excel-Datei
                using (var workbook = new XLWorkbook(filePath))
                {
                    // Zugriff auf das erste Arbeitsblatt
                    var worksheet = workbook.Worksheet(1);

                    // Zugriff auf eine Zelle
                    var cell = worksheet.Cell("A1");
                    int counter = 1;

                    while (string.IsNullOrWhiteSpace(cell.GetString()) == false)
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        cell = worksheet.Cell("A" + counter);
                        string cellString = cell.GetString();

                        if (cellString != "EINSATZDATUM")
                        {
                            string einsatzDatum = worksheet.Cell("A" + counter).GetString();
                            string funkName = worksheet.Cell("B" + counter).GetString();
                            string grundStichwort = worksheet.Cell("C" + counter).GetString();
                            string einsatzArt = worksheet.Cell("D" + counter).GetString();
                            string diagnose = worksheet.Cell("E" + counter).GetString();
                            string transportZiel = worksheet.Cell("F" + counter).GetString();
                            string protokollNummer = worksheet.Cell("G" + counter).GetString();
                            string icdCode = worksheet.Cell("H" + counter).GetString();
                            string bef2Atmung = worksheet.Cell("I" + counter).GetString();
                            string bef1BlutdruckSys = worksheet.Cell("J" + counter).GetString();
                            string zeittransportBeginn = worksheet.Cell("K" + counter).GetString();
                            string patAlter = worksheet.Cell("L" + counter).GetString();
                            string bef2BlutdruckSys = worksheet.Cell("M" + counter).GetString();
                            string bef2Bewusstlage = worksheet.Cell("N" + counter).GetString();
                            string bef1HerzFrequenz = worksheet.Cell("O" + counter).GetString();
                            string bef2BlutdruckDia = worksheet.Cell("P" + counter).GetString();
                            string bef1SpO2 = worksheet.Cell("Q" + counter).GetString();
                            string bef1Bewusstlage = worksheet.Cell("R" + counter).GetString();
                            string bef2AtemFrequenz = worksheet.Cell("S" + counter).GetString();
                            string bef2HerzFrequenz = worksheet.Cell("T" + counter).GetString();
                            string bef2GCS = worksheet.Cell("U" + counter).GetString();
                            string bef1BlutdruckDia = worksheet.Cell("V" + counter).GetString();
                            string einsatzId = worksheet.Cell("W" + counter).GetString();
                            string zeitAlarm = worksheet.Cell("X" + counter).GetString();
                            string zeitSymptomBeginn = worksheet.Cell("Y" + counter).GetString();
                            string unfallmechanismus = worksheet.Cell("Z" + counter).GetString();
                            string diagnoseGruppe = worksheet.Cell("AA" + counter).GetString();
                            string bef1EtCO2 = worksheet.Cell("AB" + counter).GetString();
                            string bef2EtCO2 = worksheet.Cell("AC" + counter).GetString();
                            string bef1Pysche = worksheet.Cell("AD" + counter).GetString();
                            string diagniseCode = worksheet.Cell("AE" + counter).GetString();
                            string reanimation = worksheet.Cell("AF" + counter).GetString();
                            string beatmung = worksheet.Cell("AG" + counter).GetString();
                            string reaniZeitEOFirstResp = worksheet.Cell("AH" + counter).GetString();
                            string reaniKollapsBeobDurch = worksheet.Cell("AI" + counter).GetString();
                            string reaniBeginnHdmDurch = worksheet.Cell("AJ" + counter).GetString();
                            string transportZielOrt = worksheet.Cell("AK" + counter).GetString();
                            string zeitAnkunftEinsatzort = worksheet.Cell("AL" + counter).GetString();
                            string patientGeschlecht = worksheet.Cell("AM" + counter).GetString();
                            string ambulanteVersorgung = worksheet.Cell("AN" + counter).GetString();
                            string nacaScore = worksheet.Cell("AO" + counter).GetString();
                            string bef1Zucker = worksheet.Cell("AP" + counter).GetString();
                            string unfallHergang = worksheet.Cell("AQ" + counter).GetString();

                            Models.EmergencyCase emergencyCase = new Models.EmergencyCase
                            {
                                EmergencyDate = einsatzDatum,
                                RadioName = funkName,
                                BasicKeyword = grundStichwort,
                                TypeOfUse = einsatzArt,
                                Diagnosis = diagnose,
                                TransportZiel = transportZiel,
                                InternalId = protokollNummer,
                                IcdCode = icdCode,
                                Befund2Atmung = bef2Atmung,
                                Befund1Blutdrucksystolisch = bef1BlutdruckSys,
                                TimeTransportBegin = zeittransportBeginn,
                                PatientAge = patAlter,
                                Befund2Bewusstlage = bef2Bewusstlage,
                                Befund1HerzFrequenz = bef1HerzFrequenz,
                                Befund2BlutdruckDiastolisch = bef2BlutdruckDia,
                                Befund1SpO2 = bef1SpO2,
                                Befund1Bewusstlage = bef1Bewusstlage,
                                Befund2AtmungFrequenz = bef2AtemFrequenz,
                                Befund2HerzFrequenz = bef2HerzFrequenz,
                                Befund2Gcs = bef2GCS,
                                Befund1BlutdruckDiastolisch = bef1BlutdruckDia,
                                EinsatzID = einsatzId,
                                AlarmTime = zeitAlarm,
                                TimeSymptonBegin = zeitSymptomBeginn,
                                UnfallMechanismus = unfallmechanismus,
                                DiagnoseGruppe = diagnoseGruppe,
                                Befund1EtCO2 = bef1EtCO2,
                                Befund2EtCO2 = bef2EtCO2,
                                Befund1Psyche = bef1Pysche,
                                DiagnoseCode = diagniseCode,
                                Reanimation = reanimation,
                                Beatmung = beatmung,
                                ReaniZeitEOFirstResp = reaniZeitEOFirstResp,
                                ReaniKollapsBeobDurch = reaniKollapsBeobDurch,
                                ReaniBeginnHDMDurch = reaniBeginnHdmDurch,
                                TransportZielOrt = transportZielOrt,
                                TimeAnkunftEinsatzOrt = zeitAnkunftEinsatzort,
                                PatientGeschlecht = patientGeschlecht,
                                AmbulanteVersorgung = ambulanteVersorgung,
                                NacaScore = nacaScore,
                                Befund1Zucker = bef1Zucker,
                                UnfallHergang = unfallHergang
                            };

                            EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();

                            Models.EmergencyCase emergencyCaseIsFound = emergencyCaseDataBase.FindOne(x => x.InternalId == emergencyCase.InternalId);
                            if (emergencyCaseIsFound == null)
                            {
                                emergencyCaseDataBase.Save(emergencyCase);
                                newReadRows++;
                            }
                        }
                        stopwatch.Stop();
                        // Loggen der Dauer der Operation
                        Log.Information("Die Zeile einlesen dauerte {Duration} Millisekunden.", stopwatch.ElapsedMilliseconds);

                        counter++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return newReadRows;
        }

        public int ReadExcelFileData2(string filePath)
        {
            int newReadRows = 0;

            // Öffnen Sie eine vorhandene Excel-Datei
            try
            {
                using (var workbook = new XLWorkbook(filePath))
                {
                    // Zugriff auf das erste Arbeitsblatt
                    var worksheet = workbook.Worksheet(1);

                    // Zugriff auf eine Zelle
                    var cell = worksheet.Cell("A1");
                    int counter = 1;

                    while (string.IsNullOrWhiteSpace(cell.GetString()) == false)
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        cell = worksheet.Cell("A" + counter);
                        string cellString = cell.GetString();

                        if (cellString != "EINSATZDATUM")
                        {
                            string einsatzDatum = worksheet.Cell("A" + counter).GetString();
                            string protokollNummer = worksheet.Cell("B" + counter).GetString();
                            string grundStichwort = worksheet.Cell("C" + counter).GetString();
                            string funkName = worksheet.Cell("D" + counter).GetString();
                            string transportZiel = worksheet.Cell("E" + counter).GetString();
                            string ivenaAnmeldeCode = worksheet.Cell("F" + counter).GetString();
                            string ivenaRMC = worksheet.Cell("G" + counter).GetString();
                            string ivenaRMZ = worksheet.Cell("H" + counter).GetString();
                            string zeitTransportBeginn = worksheet.Cell("I" + counter).GetString();
                            string zeitAnkunftPatient = worksheet.Cell("J" + counter).GetString();

                            EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();

                            Models.EmergencyCase emergencyCaseIsFound = emergencyCaseDataBase.FindOne(x => x.InternalId == protokollNummer);

                            if (emergencyCaseIsFound != null)
                            {
                                emergencyCaseIsFound.IvenaAnmaledeCode = ivenaAnmeldeCode;
                                emergencyCaseIsFound.IvenaRmc = ivenaRMC;
                                emergencyCaseIsFound.IvenaRmz = ivenaRMZ;
                                emergencyCaseIsFound.TimeTransportBeginn = zeitTransportBeginn;
                                emergencyCaseIsFound.TimeAnkunftPatient = zeitTransportBeginn;

                                emergencyCaseDataBase.Save(emergencyCaseIsFound);
                                newReadRows++;
                            }
                            else
                            {
                                Log.Warning(protokollNummer + " konnte nicht gefunden werden");
                            }

                        }
                        stopwatch.Stop();
                        // Loggen der Dauer der Operation
                        Log.Information("Die Zeile einlesen dauerte {Duration} Millisekunden.", stopwatch.ElapsedMilliseconds);

                        counter++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return newReadRows;
        }
    }
}
