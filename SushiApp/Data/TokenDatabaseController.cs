using System;
using SQLite;
using SushiApp.Models;
using Xamarin.Forms;

namespace SushiApp.Data
{
    public class TokenDatabaseController
    {
        static object locker = new object();

        SQLiteConnection database;


        public TokenDatabaseController()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<Token>();
        }

        public Token GetToken(int id)
        {
            lock (locker)
            {
                if (database.Table<Token>().Count() == 0)
                    return null;
                else
                    return database.Get<Token>(id);
            }
        }

        public int SaveToken(Token token)
        {
            lock (locker)
            {
                if (database.Find<Token>(token.Id) == token || token.Id <= 0)
                    return 0;
                else
                    return database.Insert(token);
            }
        }

        public int DeleteToken(int id)
        {
            lock (locker)
            {
                return database.Delete<Token>(id);
            }
        }
    }
}
