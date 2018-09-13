using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;
using Newtonsoft.Json;
using SelfHostApiServer.GlobalSettings;
using SelfHostApiServer.Models;

namespace SelfHostApiServer.Controllers
{
	public class OrderController : ApiController
    {
		/// <summary>
        /// HTTP POST method for getting info about existing user in database
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="productType">productType to load</param>
        [HttpPost]
        public IHttpActionResult Post([FromBody]string orderItems)
        {
			string str = "Заказ готов. ";
			Order order = JsonConvert.DeserializeObject<Order>(orderItems);

            // if user wanted to save settings than do it
			if (order.Saver)
			{
				if (SaveUserInfo(order))
					str += "Ваши настройки сохранены для будущих заказов. ";
			}

			Console.WriteLine(order.ToString());
			str += "В ближайшее время с Вами свяжется оператор!";
			return Json<string>(str);
        }

        
        /// <summary>
        /// Saves the OrderInfo user has inputed
        /// </summary>
        /// <returns><c>true</c>, if user info was saved, <c>false</c> otherwise.</returns>
        /// <param name="order">Order.</param>
		bool SaveUserInfo(Order order)
		{
			using (SqlConnection cnn = new SqlConnection(GSettings.connectionString))
            {
                cnn.Open();
                Console.WriteLine("connection opened for updating user info!");

				string query = "UPDATE dbo.Users" +
					" SET PhoneNumber=@phone, Street=@street, House=@house, Entrance=@entrance, Apartment=@apartment" +
					" WHERE UserName=@UserName AND UserPassword=@UserPassword";

                SqlCommand cmd = new SqlCommand(query, cnn);

				cmd.Parameters.AddWithValue("@UserName", order.UserName);
				cmd.Parameters.AddWithValue("@UserPassword", order.UserPassword);

				cmd.Parameters.AddWithValue("@phone", order.Phone);
				cmd.Parameters.AddWithValue("@street", order.Street);
				cmd.Parameters.AddWithValue("@house", order.House);
				cmd.Parameters.AddWithValue("@entrance", order.Entrance);
				cmd.Parameters.AddWithValue("@apartment", order.Apartment);
                            
				var affected = cmd.ExecuteNonQuery();

				if (affected == 1)
				{
					cnn.Close();
					return true;
				}
					

                Console.WriteLine("connection closed for updating user info!");
                cnn.Close();
            }

			return false;
		}

    }
}
