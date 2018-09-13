using System;
using System.Collections.Generic;
using SQLite;
using SushiApp.Models;
using Xamarin.Forms;

namespace SushiApp.Data
{
    public class ItemDatabaseController
    {
        static object locker = new object();

        SQLiteConnection database;

        public ItemDatabaseController()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<Item>();
        }

        public Item GetItem(int id)
        {
            lock (locker)
            {
                if (database.Table<Item>().Count() == 0)
                    return null;
                else
                    return database.Get<Item>(id);
            }
        }

        /// <summary>
        /// returns amount of added items
        /// </summary>
        /// <returns>The list of items.</returns>
        /// <param name="list">List.</param>
        public int SaveListOfItems(List<Item> list)
        {
            lock (locker)
            {
                int amountAdded = 0;
                bool add;

                foreach (Item listItem  in list)
                {
                    add = true;

                    foreach(Item tableItem in database.Table<Item>())
                    {
                        if (tableItem.Equals(listItem))
                        {
                            add = false;
                            break;
                        }
                    }

                    if (add)
                    {
                        amountAdded++;
                        database.Insert(listItem);
                    }
                }
                return amountAdded;
            }
        }

        public int SaveItem(Item item)
        {
            lock (locker)
            {
                if (database.Find<Item>(item.Id).Equals(item) || item.Id <= 0)
                    return 0;
                else
                    return database.Insert(item);
            }   
        }

        public int DeleteItem(int id)
        {
            lock (locker)
            {
                return database.Delete<Item>(id);    
            }
        }

        public string GetItemInfo(int id)
        {
            Item tmp = database.Get<Item>(id);
            return $"Item info: id={tmp.Id};name={tmp.Name}";
        }

        public int GetCountOfItems()
        {
            return database.Table<Item>().Count();
        }

        /// <summary>
        /// returns amount of deleted objects of that type
        /// </summary>
        /// <returns>The all type items.</returns>
        /// <param name="type">Type of parametr of item to be deleted</param>
        public int DeleteAllTypeItems(string type)
        {
            lock (locker)
            {
                int deletedAmount = 0;
                foreach (Item item in database.Table<Item>())
                {
                    if (item.Type == type)
                    {
                        database.Delete<Item>(item.Id);
                        deletedAmount++;
                    }
                }

                return deletedAmount;
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
                int amount = database.Table<Item>().Count();
                database.DeleteAll<Item>();
                return amount;
            }
        }

        public List<Item> GetItemsOfType(string type)
        {
            lock (locker)
            {
                List<Item> items = new List<Item>();
                
                foreach (Item item in database.Table<Item>())
                {
                    if (item.Type == type)
                        items.Add(item);
                }

                return items;
            }
        }
    }
}
