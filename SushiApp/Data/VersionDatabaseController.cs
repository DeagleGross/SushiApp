using System;
using SQLite;
using SushiApp.Models;
using Xamarin.Forms;

namespace SushiApp.Data
{
    public class VersionDatabaseController
    {
        static object locker = new object();

        SQLiteConnection database;

        public VersionDatabaseController()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<DataVersion>();
        }

        public int SaveVersion(DataVersion data)
        {
            lock (locker)
            {
                if (database.Find<DataVersion>(data.Id) == data || data.Id <= 0)
                    return 0;
                else
                    return database.Insert(data);
            }
        }

        public DataVersion GetVersionByID(int id)
        {
            lock (locker)
            {
                if (database.Table<DataVersion>().Count() == 0 || database.Table<DataVersion>().Count() < id)
                    return null;
                else
                    return database.Get<DataVersion>(id);
            }
        }

        /// <summary>
        /// returns string value for version of TypeData
        /// </summary>
        /// <returns>The type version.</returns>
        /// <param name="type">Type.</param>
        public string GetTypeVersion(string type)
        {
            lock (locker)
            {
                foreach(DataVersion data in database.Table<DataVersion>())
                {                    
                    // find DataVersion object with needed type and returns its version
                    if (data.TypeName == type)
                        return data.Version;
                }

                // if nothing was found
                return null;
            }
        }

        public bool ChangeDataVersion(string type, string version)
        {
			bool changedOk = false;

            lock (locker)
            {                
                foreach(DataVersion data in database.Table<DataVersion>())
                {
                    // find DataVersion object with needed type and returns its version
                    if (data.TypeName == type)
                    {
						int dataId = data.Id;
						database.Delete(data);                  
						database.Insert(new DataVersion(type,version,dataId));
						changedOk = true;
						/*
                        data.Version = version;
						changedOk = true;
						*/
                    }
                }

				if (changedOk) // if dataversion changed okay 
					return true;
				else // if nothing was found
                    return false;
            }
        }

        public void SetupDataVersion(string type, string version)
        {
            lock (locker)
            {
                int count = database.Table<DataVersion>().Count();
                database.Insert(new DataVersion(type, version, count + 1));
                return;
            }
        }

        /// <summary>
        /// returns count of whole db and deletes it
        /// </summary>
        /// <returns>The whole db.</returns>
        public int DeleteWholeDB()
        {
            lock (locker)
            {
                int amount = database.Table<DataVersion>().Count();
                database.DeleteAll<DataVersion>();
                return amount;
            }
        }
    }
}
