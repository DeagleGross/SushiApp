using System;
using System.Data.SqlClient;
using System.Web.Http;
using Newtonsoft.Json;
using SelfHostApiServer.GlobalSettings;
using SelfHostApiServer.Models;

namespace SelfHostApiServer.Controllers
{
    public class VersionCheckerController : ApiController
    {
		/// <summary>
        /// Checks the vesrion of data 
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="data">Data.</param>
        [HttpPost]
        public IHttpActionResult Post([FromBody] string data)
        {
            if (data == null)
                return Json<int>(0);
            DataVersion converted = JsonConvert.DeserializeObject<DataVersion>(data);
            Console.WriteLine("received this on server for checking database version:\n" + converted.ToString());

            Console.WriteLine("Trying to find the version...");

            string fromDB = GetVersionOfThatType(converted.TypeName);

            if (fromDB == converted.Version)
            {
                // version is ok and nothing to be changed
                Console.WriteLine("Version was ok. returning it so its true.");
                return Json<string>(converted.Version);
            }
            else if (fromDB == "404")
            {
                // version is error
                Console.WriteLine("Some error occured");
                return Json<int>(404);
            }
            else
            {
                // version is just another. so we will return it anyway
                Console.WriteLine("Version is not the same. Returning it and getting false but its ok.");
                return Json<string>(fromDB);
            }
        }

        /// <summary>
        /// returns string with actual version of data from db
        /// </summary>
        /// <returns>The version of that type.</returns>
        /// <param name="type">Type.</param>
        static string GetVersionOfThatType(string type)
        {
            string version = "404";
            using (SqlConnection cnn = new SqlConnection(GSettings.connectionString))
            {
                cnn.Open();
                Console.WriteLine("connection opened for checking type version!");

                string query = "SELECT VersionVersion FROM dbo.Versions " +
                    "WHERE VersionType=@VersionNumer";

                SqlCommand cmd = new SqlCommand(query, cnn);

                cmd.Parameters.AddWithValue("@VersionNumer", type);

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        version = reader.GetString(0);
                    }
                }

                Console.WriteLine("connection closed for checking version!");
                cnn.Close();
            }

            return version;
        }
    }
}
