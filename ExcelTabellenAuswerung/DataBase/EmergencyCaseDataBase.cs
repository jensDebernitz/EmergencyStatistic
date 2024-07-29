using LiteDB;
using System.Linq.Expressions;
using ExcelTabellenAuswerung.Models;

namespace ExcelTabellenAuswerung.DataBase
{
    public class EmergencyCaseDataBase
    {
        public bool SaveData(IEnumerable<Models.EmergencyCase> data)
        {
            if (data == null)
            {
                return false;
            }

            using (var db = new LiteDatabase(Helpers.DataBase.DataBaseFileData()))
            {
                var col = db.GetCollection<Models.EmergencyCase>(typeof(Models.EmergencyCase).Name);
                col.Insert(data);
            }

            return true;
        }

        public List<Models.EmergencyCase> LoadData()
        {
            using (var db = new LiteDatabase(Helpers.DataBase.DataBaseFileData()))
            {
                var col = db.GetCollection<Models.EmergencyCase>(typeof(Models.EmergencyCase).Name);

                var p =  db.GetCollection<Models.EmergencyCase>(typeof(Models.EmergencyCase).Name).FindAll();
                return p.ToList();
            }
        }

        public Models.EmergencyCase Get(int id)
        {
            using (var db = new LiteDatabase(Helpers.DataBase.DataBaseFileData()))
            {
                var col = db.GetCollection<Models.EmergencyCase>(typeof(Models.EmergencyCase).Name);
                return col.FindOne(x => x.Id == id);
            }
        }

        public Models.EmergencyCase FindOne(Expression<Func<Models.EmergencyCase, bool>> predicate)
        {
            using (var db = new LiteDatabase(Helpers.DataBase.DataBaseFileData()))
            {
                var col = db.GetCollection<Models.EmergencyCase>(typeof(Models.EmergencyCase).Name);
                return col.FindOne(predicate);
            }
        }




        public void Save(Models.EmergencyCase data)
        {
            using (var db = new LiteDatabase(Helpers.DataBase.DataBaseFileData()))
            {
                var col = db.GetCollection<Models.EmergencyCase>(typeof(Models.EmergencyCase).Name);
                if (data.Id == 0)
                {
                    col.Insert(data);
                }
                else
                {
                    col.Update(data);
                }
            }
        }

        public void Erase(Models.EmergencyCase data)
        {
            using (var db = new LiteDatabase(Helpers.DataBase.DataBaseFileData()))
            {
                var col = db.GetCollection<Models.EmergencyCase>(typeof(Models.EmergencyCase).Name);
                col.Delete(data.Id);
            }
        }
    }
}
