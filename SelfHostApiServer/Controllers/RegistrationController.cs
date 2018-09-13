using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using Newtonsoft.Json;
using SelfHostApiServer.GlobalSettings;
using SelfHostApiServer.Models;

namespace SelfHostApiServer.Controllers
{
    public class RegistrationController : ApiController
    {
        /// <summary>
        /// HTTP POST method for registering the user in the database
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="registrationUser">Registration user.</param>
        [HttpPost]
        public IHttpActionResult Post([FromBody]string registrationUser)
        {
            if (registrationUser == null)
                return Json<User>(null);
            User registered = JsonConvert.DeserializeObject<User>(registrationUser);
            Console.WriteLine("received this on server for registration:\n" + registered.ToString());

            Console.WriteLine("Trying to find this user in database...");
            if (CheckForUser(registered))
            {
                // user is ok to be registered
                int NewUserId = RegisterUserInDatabase(registered);

				if (NewUserId == 404)
				{
				    Console.WriteLine("UserError 404.");
				    return Json<User>(new User(-404, "error", "error"));
				}
                else 
                    Console.WriteLine($"User successfully registered. His ID = {NewUserId}");
				return Json<User>(registered);
            }
            else
            {
                //the same user was already found. Error 15023
                Console.WriteLine("UserCredits were found in dbo. Registration-Error 15023");
				return Json<User>(new User(-15023,"error", "error"));
            }
        }

        /// <summary>
        /// returns false if user with same credits was found. Otherwise returns true 
        /// </summary>
        /// <returns><c>true</c>, if for user was checked, <c>false</c> otherwise.</returns>
        /// <param name="registrated">Registrated.</param>
        bool CheckForUser(User registrated)
        {
            User ret = new User();
            using (SqlConnection cnn = new SqlConnection(GSettings.connectionString))
            {
                cnn.Open();
                Console.WriteLine("connection opened for finding registrationUser!");

                string query = "SELECT * FROM dbo.Users " +
                    "WHERE UserName=@UserName AND UserPassword=@UserPassword";

                SqlCommand cmd = new SqlCommand(query, cnn);

                cmd.Parameters.AddWithValue("@UserName", registrated.UserName);
                cmd.Parameters.AddWithValue("@UserPassword", registrated.Password);

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ret = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                    }
                }

                Console.WriteLine("connection closed for finding registrationUser!");
                cnn.Close();
            }

            if (ret.Equals(registrated))
                return false;
            else
                return true;
        }


        /// <summary>
        /// returns the id of user to be saved in database
        /// </summary>
        /// <returns>The user in database.</returns>
        /// <param name="user">User.</param>
        int RegisterUserInDatabase(User user)
        {
            int count = GetUsersCount();
            if (count == 0)
                return 404;
            int newId = count + 1;

            RegisterUser(newId, user);

            return newId;
        }

        // ERROR DOWN HERE CNN IS NOT OPENNING - WHY/???????

        /// <summary>
        /// Gets the users count.
        /// </summary>
        /// <returns>The users count.</returns>
        int GetUsersCount()
        {
            int count = 0;
            using (SqlConnection cnn = new SqlConnection(GSettings.connectionString))
            {
                cnn.Open();
                Console.WriteLine("Opened connection to get count of rows in dbo.Users!");

                string query = "SELECT COUNT(UserID) FROM dbo.Users";
                SqlCommand cmd = new SqlCommand(query, cnn);

                //var reader = cmd.ExecuteReader();
                //count = reader.GetInt32(0);
                count = (Int32)cmd.ExecuteScalar();

                cnn.Close();
                Console.WriteLine("Closed connection for getting number of rows in dbo.Users!");
            }

            return count;
        }

        /// <summary>
        /// inserts new user in dbo.Users
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="user">User.</param>
        void RegisterUser(int id, User user)
        {
            using (SqlConnection cnn = new SqlConnection(GSettings.connectionString))
            {
                cnn.Open();
                Console.WriteLine("Opened connection for inserting into dbo.Users!");

				string cmdStr = "INSERT INTO dbo.Users (UserID, UserName, UserPassword) " +
                                "VALUES (@UserID, @UserName, @UserPassword)";

                SqlCommand cmd = cnn.CreateCommand();

                cmd.CommandText = cmdStr;

                cmd.Parameters.AddWithValue("@UserID", id);
                cmd.Parameters.AddWithValue("@UserName", user.UserName);
                cmd.Parameters.AddWithValue("@UserPassword", user.Password);

                cmd.ExecuteNonQuery();

                cnn.Close();
                Console.WriteLine("Closed connection for inserting into dbo.Users!");
            }
        } 
    }
}
