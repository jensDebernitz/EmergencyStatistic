﻿using System.IO;
using static System.Environment;

namespace ExcelTabellenAuswerung.Helpers
{
    public class DataBase
    {
        public static StringWriter GlobalLogging { get; set; }

        public static string? DataBaseFileData()
        {
            var commonpath = GetFolderPath(SpecialFolder.CommonApplicationData);
            var path = Path.Combine(commonpath, "ExcelTabellenAuswertung\\data.db");
            try
            {
                if (Directory.Exists(commonpath + "\\ExcelTabellenAuswertung") == false)
                {
                    Directory.CreateDirectory(commonpath + "\\ExcelTabellenAuswertung");
                }

                return path;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return null;
        }

        public static string? DataBaseFileSettings()
        {
            var commonpath = GetFolderPath(SpecialFolder.CommonApplicationData);
            var path = Path.Combine(commonpath, "ExcelTabellenAuswertung\\settings.db");
            try
            {
                if (Directory.Exists(commonpath + "\\ExcelTabellenAuswertung") == false)
                {
                    Directory.CreateDirectory(commonpath + "\\ExcelTabellenAuswertung");
                }

                return path;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return null;
        }
    }
}
