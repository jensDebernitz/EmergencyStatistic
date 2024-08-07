using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using ExcelTabellenAuswerung.DataBase;
using ExcelTabellenAuswerung.Models;
using LiteDB;
using Serilog;
using System.Diagnostics;
using System.IO;
using System.Windows;

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
                    var cell = worksheet.Cell("B1");
                    string diagnoseHeader = cell.GetString();

                    if (diagnoseHeader.ToUpper() != "DIAGNOSE")
                    {
                        throw new Exception("Die Excel Tabelle entspricht nicht dem richtigem format");
                    }


                    cell = worksheet.Cell("A1");
                    int counter = 1;

                    while (string.IsNullOrWhiteSpace(cell.GetString()) == false)
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        cell = worksheet.Cell("E" + counter);
                        string cellString = cell.GetString();

                        if (cellString != "Protokollnummer")
                        {
                            string grundStichwort = worksheet.Cell("A" + counter).GetString();
                            string diagnose = worksheet.Cell("B" + counter).GetString();
                            string name = worksheet.Cell("C" + counter).GetString();
                            string icdCode = worksheet.Cell("D" + counter).GetString();
                            string protokollNummer = worksheet.Cell("E" + counter).GetString();
                            string einsatzDatum = worksheet.Cell("F" + counter).GetString();
                            string einsatzOrtStrasseNummer = worksheet.Cell("G" + counter).GetString();
                            string funkName = worksheet.Cell("H" + counter).GetString();
                            string transportZiel = worksheet.Cell("I" + counter).GetString();
                            string zeitAnkunftPatient =  worksheet.Cell("J" + counter).GetString();
                            string zeitTransportBeginn =  worksheet.Cell("K" + counter).GetString();
                            string bef1SpO2 = worksheet.Cell("L" + counter).GetString();
                            string bef1Zucker = worksheet.Cell("M" + counter).GetString();
                            string bef1HerzFrequenz = worksheet.Cell("N" + counter).GetString();
                            string bef1BlutdruckSys = worksheet.Cell("O" + counter).GetString();
                            string bef1BlutdruckDia = worksheet.Cell("P" + counter).GetString();
                            string bef1Bewusstlage = worksheet.Cell("Q" + counter).GetString();
                            string befund1Gcs = worksheet.Cell("R" + counter).GetString();
                            string diagnoseGruppe = worksheet.Cell("S" + counter).GetString();
                            string diagniseCode = worksheet.Cell("T" + counter).GetString();
                            string nacaScore = worksheet.Cell("U" + counter).GetString();

                            EmergencyCase emergencyCase = new EmergencyCase
                            {
                                GrundStichwort = grundStichwort,
                                Diagnosis = diagnose,
                                Name = name,
                                IcdCode = icdCode,
                                InternalId = protokollNummer,
                                EinsatzDatum = einsatzDatum,
                                EinsatzOrtStrasseNummer = einsatzOrtStrasseNummer,
                                Funkname = funkName,
                                TransportZiel = transportZiel,
                                ZeitAnkunftPatient = zeitAnkunftPatient,
                                ZeitTransportBeginn = zeitTransportBeginn,
                                Befund1SpO2 = bef1SpO2,
                                Befund1Zucker = bef1Zucker,
                                Befund1HerzFrequenz = bef1HerzFrequenz,
                                Befund1Blutdrucksystolisch = bef1BlutdruckSys,
                                Befund1BlutdruckDiastolisch = bef1BlutdruckDia,
                                Befund1Bewusstlage = bef1Bewusstlage,
                                Befund1GCS = befund1Gcs,
                                DiagnoseGruppe = diagnoseGruppe,
                                DiagnoseCode = diagniseCode,
                                NacaScore = nacaScore
                            };

                            EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();

                            Models.EmergencyCase emergencyCaseIsFound = emergencyCaseDataBase.FindOne(x => x.InternalId == emergencyCase.InternalId);
                            if (emergencyCaseIsFound == null)
                            {
                                emergencyCaseDataBase.Save(emergencyCase);
                                newReadRows++;
                            }
                            else
                            {
                                Log.Information($"Die Internal Id wurde schon vergeben {emergencyCase.InternalId}");
                            }
                        }
                        stopwatch.Stop();
                        // Loggen der Dauer der Operation
                       // Log.Information("Die Zeile einlesen dauerte {Duration} Millisekunden.", stopwatch.ElapsedMilliseconds);

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

                    var cell = worksheet.Cell("B1");
                    string diagnoseHeader = cell.GetString();

                    if (diagnoseHeader.ToUpper() != "PROTOKOLLNUMMER")
                    {
                        throw new Exception("Die Excel Tabelle entspricht nicht dem richtigem format");
                    }

                    cell = worksheet.Cell("A1");
                    int counter = 1;

                    while (string.IsNullOrWhiteSpace(cell.GetString()) == false)
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        cell = worksheet.Cell("B" + counter);
                        string cellString = cell.GetString();

                        if (cellString != "Protokollnummer")
                        {
                            string protokollNummer = worksheet.Cell("B" + counter).GetString();
                            string ivenaAnmeldeCode = worksheet.Cell("I" + counter).GetString();
                            string ivenaRMC = worksheet.Cell("J" + counter).GetString();
                            string ivenaRMZ = worksheet.Cell("L" + counter).GetString();
                            string zeitAnkunftZielklinik = worksheet.Cell("E" + counter).GetString();
                            string unfallHergang = worksheet.Cell("F" + counter).GetString();
                            string patientGeschlecht = worksheet.Cell("G" + counter).GetString();

                            EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();

                            Models.EmergencyCase emergencyCaseIsFound = emergencyCaseDataBase.FindOne(x => x.InternalId == protokollNummer);

                            if (emergencyCaseIsFound != null)
                            {
                                emergencyCaseIsFound.IvenaAnmaledeCode = ivenaAnmeldeCode;
                                emergencyCaseIsFound.IvenaRmc = ivenaRMC;
                                emergencyCaseIsFound.IvenaRmz = ivenaRMZ;
                                emergencyCaseIsFound.ZeitAnkunftZielklinik = zeitAnkunftZielklinik;
                                emergencyCaseIsFound.UnfallHergang = unfallHergang;
                                emergencyCaseIsFound.PatientGeschlecht = patientGeschlecht;

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

        public void SavePdf(DirectoryInfo folder)
        {
            List<FileInfo> files = folder.GetFiles("*.pdf", SearchOption.AllDirectories).ToList();

            foreach (FileInfo fileInfo in files)
            {
                string fileName = fileInfo.Name.Replace(".pdf", "");
                fileName = fileName.ToLower().Replace("naprot_", "");

                using (var db = new LiteDatabase(Helpers.DataBase.DataBaseFileDocuments()))
                {
                    // Zugriff auf den FileStorage-Bereich der Datenbank
                    var fileStorage = db.FileStorage;

                    // Speichern der PDF-Datei in der Datenbank
                    using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
                    {
                        fileStorage.Upload(fileName, fileInfo.FullName, fileStream);
                    }

                    Log.Information("PDF gespeichert.");
                    File.Delete(fileInfo.FullName);
                }

               
            }
        }
    }
}
