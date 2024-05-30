using LiteDB;
using Wpf.Ui.Appearance;

namespace ExcelTabellenAuswerung.DataBase
{
    public class SettingsDataBase
    {
        public void SaveTheme(ApplicationTheme theme)
        {
            using (var db = new LiteDatabase(Helpers.DataBase.DataBaseFileSettings()))
            {
                var col = db.GetCollection<Models.AppSettings>(typeof(Models.AppSettings).Name);

                //search when exist
                var item = col.FindOne(x => x.Id == 1);
                if (item != null)
                {
                    item.Theme = theme;
                    col.Update(item);
                }
                else
                {
                    Models.AppSettings config = new Models.AppSettings
                    {
                        Theme = theme
                    };
                    col.Insert(config);
                }
            }
        }

        public ApplicationTheme LoadTheme()
        {
            using (var db = new LiteDatabase(Helpers.DataBase.DataBaseFileSettings()))
            {
                var col = db.GetCollection<Models.AppSettings>(typeof(Models.AppSettings).Name);

                //search when exist
                var item = col.FindOne(x => x.Id == 1);

                if (item != null)
                {
                    return item.Theme;
                }
            }
            return ApplicationTheme.Light;
        }

        public void SaveAppSettings(Models.AppSettings appSettings)
        {
            using (var db = new LiteDatabase(Helpers.DataBase.DataBaseFileSettings()))
            {
                var col = db.GetCollection<Models.AppSettings>(typeof(Models.AppSettings).Name);

                //search when exist
                var item = col.FindOne(x => x.Id == 1);
                if (item != null)
                {
                    col.Update(appSettings);
                }
                else
                {
                    col.Insert(appSettings);
                }
            }
        }

        public Models.AppSettings LoadAppSettings()
        {
            using (var db = new LiteDatabase(Helpers.DataBase.DataBaseFileSettings()))
            {
                var col = db.GetCollection<Models.AppSettings>(typeof(Models.AppSettings).Name);

                //search when exist
                var item = col.FindOne(x => x.Id == 1);

                if (item != null)
                {
                    return item;
                }
            }
            return CreateNewAppSettings();
        }

        public static Models.AppSettings CreateNewAppSettings()
        {
            return new Models.AppSettings();
        }
    }


}
