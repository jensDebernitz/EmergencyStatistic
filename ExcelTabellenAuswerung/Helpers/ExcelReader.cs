using ClosedXML.Excel;
using ExcelTabellenAuswerung.DataBase;
using Serilog;
using System.Diagnostics;

namespace ExcelTabellenAuswerung.Helpers
{
    public class ExcelReader
    {
        public int ReadExcelFile(string filePath)
        {
            int newReadRows = 0;

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

                    if (cellString != "InternalId")
                    {
                        string internalId = worksheet.Cell("A" + counter).GetString();
                        string patientName = worksheet.Cell("B" + counter).GetString();
                        string diagnosis = worksheet.Cell("C" + counter).GetString();
                        string dateString = worksheet.Cell("D" + counter).GetString();

                        if (DateTime.TryParse(dateString, out DateTime date) == true)
                        {
                            //do stuff

                            Models.EmergencyCase emergencyCase = new Models.EmergencyCase
                            {
                                InternalId = internalId,                                
                                Diagnosis = diagnosis,                                
                            };

                            EmergencyCaseDataBase emergencyCaseDataBase = new EmergencyCaseDataBase();

                            Models.EmergencyCase emergencyCaseIsFound = emergencyCaseDataBase.FindOne(x => x.InternalId == emergencyCase.InternalId);
                            if (emergencyCaseIsFound == null)
                            {
                                emergencyCaseDataBase.Save(emergencyCase);
                                newReadRows++;
                            }

                        }
                    }
                    stopwatch.Stop();
                    // Loggen der Dauer der Operation
                    Log.Information("Die Zeile einlesen dauerte {Duration} Millisekunden.", stopwatch.ElapsedMilliseconds);

                    counter++;
                }
            }

            return newReadRows;
        }
    }
}
