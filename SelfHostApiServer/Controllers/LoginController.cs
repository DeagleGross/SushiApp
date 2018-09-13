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
    /// Login on server controller.
    /// </summary>
    public class LoginController : ApiController
    {
        /// <summary>
        /// HTTP POST method for getting info about existing user in database
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="userInfo">User info.</param>
        [HttpPost]
        public IHttpActionResult Post([FromBody]string userInfo)
        {
            if (userInfo == null)
                return Json<int>(404);
            User loggined = JsonConvert.DeserializeObject<User>(userInfo);
            Console.WriteLine("received this on server:\n" + loggined.ToString());

            Console.WriteLine("Trying to find this user in database...");
            User foundInDboUsers = CheckForUser(loggined);

            if (foundInDboUsers != null && foundInDboUsers.Equals(loggined))
            {
                Console.WriteLine("FOUND EQUAL USER IN DATABASE ");   
                return Json<User>(foundInDboUsers);
            }
            else
            {
                Console.WriteLine("NO USER FOUND IN DATABASE");
                // no user was found. Error 404
                return Json<User>(null); 
            }
                
        }

        /// <summary>
        /// Checks if the user exists in the database
        /// </summary>
        /// <returns>The for user.</returns>
        /// <param name="loginned">Loginned.</param>
        static User CheckForUser(User loginned)
        {
            if (loginned == null)
                return null;

            User ret = new User();

            using (SqlConnection cnn = new SqlConnection(GSettings.connectionString))
            {
				try
				{
					cnn.Open();	
				}
				catch
				{
					return null;	
				}

                Console.WriteLine("connection open!");

                string query = "SELECT * FROM dbo.Users " +
                    "WHERE UserName=@UserName AND UserPassword=@UserPassword";

                SqlCommand cmd = new SqlCommand(query, cnn);

                cmd.Parameters.AddWithValue("@UserName", loginned.UserName);
                cmd.Parameters.AddWithValue("@UserPassword", loginned.Password);

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
						if (CheckAddInfo(reader))
							// returns user with defined order info
							ret = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                                           reader.GetString(3), reader.GetString(4), reader.GetString(5),
                                           reader.GetString(6), reader.GetString(7));
						else
							// returns only logged user
							ret = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                    }
                }
                else ret = null;

                Console.WriteLine("connection closed!");
                cnn.Close();
            }

            return ret;
        }

        /// <summary>
        /// if any OrderInfoIsNull -> returns false
        /// </summary>
        /// <returns><c>true</c>, if add info was checked, <c>false</c> otherwise.</returns>
        /// <param name="reader">Reader.</param>
		static bool CheckAddInfo(SqlDataReader reader)
		{
			if (reader.GetString(3) == null || 
			    reader.GetString(4) == null ||
			    reader.GetString(5) == null ||
                reader.GetString(6) == null ||
			    reader.GetString(7) == null)
				    return false;
			return true;
		}
    }
}
