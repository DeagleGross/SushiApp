using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Data.SqlClient;
using Newtonsoft.Json;
using SelfHostApiServer.GlobalSettings;
using SelfHostApiServer.Models;

namespace SelfHostApiServer.Controllers
{
	/// <summary>
    /// Load DataBase from server Controller.
    /// </summary>
    public class LoadDBController : ApiController
    {
        private PhotoConverter photoConverter;

        /// <summary>
        /// HTTP POST method for getting info about existing user in database
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="productType">productType to load</param>
        [HttpPost]
        public IHttpActionResult Post([FromBody]string productType)
        {
            List<Item> items;
            Console.WriteLine($"Now ill find {productType} items in database");

            items = GetItems(productType);

            if (items == null)
            {
                Console.WriteLine("Something went wrong");
                return Json<List<Item>>(null);
            }
            else
            {
                Console.WriteLine("Found some list of items");
                return Json<List<Item>>(items);
            }

        }

        /// <summary>
        /// returns list of items found in database of this product type
        /// </summary>
        /// <returns>The items.</returns>
        /// <param name="productType">Product type.</param>
        private List<Item> GetItems(string productType)
        {
            /*
             * SQL ITEMS TABLE ORGANIZATION
             * ItemID 
             * ItemType
             * ItemName
             * ItemPrice
             * ItemDescription
             */ 

            List<Item> items = new List<Item>();
            using (SqlConnection cnn = new SqlConnection(GSettings.connectionString))
            {
                cnn.Open();
                Console.WriteLine("connection opened for loading products of " + productType + "!");

                string query = "SELECT * FROM dbo.Items " +
                    "WHERE ItemType=@ProductType";

                SqlCommand cmd = new SqlCommand(query, cnn);

                cmd.Parameters.AddWithValue("@ProductType", productType);

                var reader = cmd.ExecuteReader();

                // for creating byte[] of images of products to form item object
                photoConverter = new PhotoConverter(productType);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string type = reader.GetString(1);
                        string name = reader.GetString(2);
                        int price = reader.GetInt32(3);
                        string description = reader.GetString(4);

                        // find a pic of this product for user
                        byte[] pic = photoConverter.ConvertImageToBytes(name);

                        // make a picture but its unpleasant for user
                    //    if (pic == null)
                    //        pic = photoConverter.PicNotFound;

                        Item adding = new Item(id, name, price, type, description);
                        adding.Pic = pic;

                        items.Add(adding);
                    }
                }
                else
                    // if smth went absolutely wrong
                    return null;

                Console.WriteLine("connection closed for forming list of items!");
                cnn.Close();
            }

            return items;
        }
    }
}
