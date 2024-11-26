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
                using (var workbook = new XLWorkbook(filePath))
                {
                    var worksheet = workbook.Worksheet(1);

                    // Einlesen der Header und Speichern in einem Dictionary
                    var headerRow = worksheet.FirstRowUsed();
                    var columnIndexMap = new Dictionary<string, int>();

                    for (int col = 1; col <= headerRow.LastCellUsed().Address.ColumnNumber; col++)
                    {
                        string header = worksheet.Cell(headerRow.RowNumber(), col).GetString().Trim();
                        columnIndexMap[header] = col; // Speichert den Spaltenindex für jeden Header
                    }

                    // Überprüfen, ob die notwendige Spalte "DIAGNOSE" vorhanden ist
                    if (!columnIndexMap.ContainsKey("DIAGNOSE"))
                    {
                        throw new Exception("Die Excel Tabelle entspricht nicht dem richtigen Format");
                    }

                    int counter = 2; // Start bei der zweiten Zeile, da die erste die Header ist
                    while (true)
                    {
                        var currentRow = worksheet.Row(counter);
                        if (string.IsNullOrWhiteSpace(currentRow.Cell(columnIndexMap["Protokollnummer"]).GetString()))
                        {
                            break; // Beenden, wenn die Diagnose-Spalte leer ist
                        }

                        Stopwatch stopwatch = Stopwatch.StartNew();

                        EmergencyCase emergencyCase = new EmergencyCase
                        {
                            GrundStichwort = currentRow.Cell(columnIndexMap["GRUNDSTICHWORT"]).GetString(),
                            Diagnosis = currentRow.Cell(columnIndexMap["DIAGNOSE"]).GetString(),
                            InternalId = currentRow.Cell(columnIndexMap["Protokollnummer"]).GetString(),
                            Funkname = currentRow.Cell(columnIndexMap["FUNKNAME"]).GetString(),
                            TransportZiel = currentRow.Cell(columnIndexMap["TRANSPORTZIEL"]).GetString(),
                            Befund1SpO2 = currentRow.Cell(columnIndexMap["BEF1_SPO2"]).GetString(),
                            Befund1Zucker = currentRow.Cell(columnIndexMap["BEF1_ZUCKER"]).GetString(),
                            Befund1HerzFrequenz = currentRow.Cell(columnIndexMap["BEF1_HERZ_FREQ"]).GetString(),
                            Befund1Blutdrucksystolisch = currentRow.Cell(columnIndexMap["BEF1_BLUTDRUCK_SYS"]).GetString(),
                            Befund1BlutdruckDiastolisch = currentRow.Cell(columnIndexMap["BEF1_BLUTDRUCK_DIA"]).GetString(),
                            Befund1Bewusstlage = currentRow.Cell(columnIndexMap["BEF1_BEWUSSTLAGE"]).GetString(),
                            Befund1GCS = currentRow.Cell(columnIndexMap["BEF1_GCS"]).GetString(),
                            DiagnoseGruppe = currentRow.Cell(columnIndexMap["DIAGNOSE_GRUPPE"]).GetString(),
                            DiagnoseCode = currentRow.Cell(columnIndexMap["DIAGNOSE_CODE"]).GetString(),
                            NacaScore = currentRow.Cell(columnIndexMap["NACA_SCORE"]).GetString()
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
                    var worksheet = workbook.Worksheet(1);

                    // Einlesen der Header und Speichern in einem Dictionary
                    var headerRow = worksheet.FirstRowUsed();
                    var columnIndexMap = new Dictionary<string, int>();

                    for (int col = 1; col <= headerRow.LastCellUsed().Address.ColumnNumber; col++)
                    {
                        string header = worksheet.Cell(headerRow.RowNumber(), col).GetString().Trim();
                        columnIndexMap[header] = col; // Speichert den Spaltenindex für jeden Header
                    }

                    // Überprüfen, ob die notwendige Spalte "Protokollnummer" vorhanden ist
                    if (!columnIndexMap.ContainsKey("Protokollnummer"))
                    {
                        throw new Exception("Die Excel Tabelle entspricht nicht dem richtigen Format");
                    }


                    int counter = 2; // Start bei der zweiten Zeile, da die erste die Header ist
                    while (true)
                    {

                        var currentRow = worksheet.Row(counter);
                        if (string.IsNullOrWhiteSpace(currentRow.Cell(columnIndexMap["Protokollnummer"]).GetString()))
                        {
                            break; // Beenden, wenn die Diagnose-Spalte leer ist
                        }

                        Stopwatch stopwatch = Stopwatch.StartNew();

                        string protokollNummer = currentRow.Cell(columnIndexMap["Protokollnummer"]).GetString();

                        EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();

                        Models.EmergencyCase emergencyCaseIsFound = emergencyCaseDataBase.FindOne(x => x.InternalId == protokollNummer);

                        if (emergencyCaseIsFound != null)
                        {
                            emergencyCaseIsFound.IvenaAnmaledeCode = currentRow.Cell(columnIndexMap["IVENA_ANMELDECODE"]).GetString();
                            emergencyCaseIsFound.IvenaRmc = currentRow.Cell(columnIndexMap["IVENA_RMC"]).GetString();
                            emergencyCaseIsFound.IvenaRmz = currentRow.Cell(columnIndexMap["IVENA_RMZ"]).GetString();
                            emergencyCaseIsFound.ZeitAnkunftZielklinik = currentRow.Cell(columnIndexMap["ZEIT_ANKUNFT_ZIELKLINIK"]).GetString();
                            emergencyCaseIsFound.UnfallHergang = currentRow.Cell(columnIndexMap["UNFALLHERGANG"]).GetString();
                            emergencyCaseIsFound.PatientGeschlecht = currentRow.Cell(columnIndexMap["PAT_GESCHLECHT"]).GetString();
                            emergencyCaseIsFound.Name = currentRow.Cell(columnIndexMap["PAT_NAME"]).GetString();
                            emergencyCaseIsFound.IcdCode = currentRow.Cell(columnIndexMap["ICD_CODE"]).GetString();
                            emergencyCaseIsFound.EinsatzDatum = currentRow.Cell(columnIndexMap["EINSATZDATUM"]).GetString();
                            //emergencyCaseIsFound.EinsatzOrtStrasseNummer = currentRow.Cell(columnIndexMap["G"]).GetString(),
                            emergencyCaseIsFound.ZeitAnkunftPatient = currentRow.Cell(columnIndexMap["ZEIT_ANKUNFT_PATIENT"]).GetString();
                            emergencyCaseIsFound.ZeitTransportBeginn = currentRow.Cell(columnIndexMap["ZEIT_TRANSPORT_BEGINN"]).GetString();
                            emergencyCaseIsFound.TransportZiel = currentRow.Cell(columnIndexMap["TRANSPORTZIEL"]).GetString();
                            emergencyCaseDataBase.Save(emergencyCaseIsFound);
                            newReadRows++;
                        }
                        else
                        {
                            Log.Warning(protokollNummer + " konnte nicht gefunden werden");
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
