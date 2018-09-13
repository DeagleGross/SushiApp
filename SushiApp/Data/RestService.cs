using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SushiApp.Models;
using SushiApp.Helpers;
using System.IO;
using RestSharp;
using System.Collections.Generic;
using Plugin.Connectivity;

namespace SushiApp.Data
{
    public class RestService
    {
        RestClient client;

        public RestService()
        {
            client = new RestClient()
            {
                BaseUrl = new Uri($"http://{Constants.serverSettings.IP}" +
                                        $":{Constants.serverSettings.Port}" + "/api")
            };
        }
      
        /// <summary>
        /// Gets the value of bool if internet connection is working
        /// </summary>
        /// <value><c>true</c> if check connection; otherwise, <c>false</c>.</value>
		bool CheckConnection
		{
			get
			{
				if (CrossConnectivity.Current.IsConnected == true)
					return true;
				else
					return false;
			}
		}

        /// <summary>
        /// sends User to server to LOGIN
        /// </summary>
        /// <returns>The login.</returns>
        /// <param name="loginned">Loginned.</param>
        public async Task<string> Login(User loginned)
        {
			if (!CheckConnection)
			{
				return "Нет соединения с интернетом.";
			}

            RestRequest request = new RestRequest("login", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddBody(SerializationHelper.SerializeToJson(loginned));

            var response = await client.ExecuteTaskAsync(request);
           
			User serverResponse = JsonConvert.DeserializeObject<User>(response.Content);	         

            // Error 404 means no user was found to login
            if (serverResponse == null)
                return "Не было найдено такого юзера. Зарегистрируйтесь, если у Вас нет аккаунта.";
            else
            {
				if (serverResponse.PhoneNum != null)
					Constants.MainUser = serverResponse;
				else
					Constants.MainUser = loginned;
				
                return $"Добро пожаловать в SushiApp, {loginned.UserName}!";
            }
        }

        /// <summary>
        /// Sends User to server for registration
        /// </summary>
        /// <returns>The register.</returns>
        /// <param name="registered">Registered.</param>
        public async Task<string> Register(User registered)
        {
			if (!CheckConnection)
            {
                return "Нет соединения с интернетом.";
            }

            RestRequest request = new RestRequest("registration", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddBody(SerializationHelper.SerializeToJson(registered));

            var response = await client.ExecuteTaskAsync(request);

            User serverResponse = JsonConvert.DeserializeObject<User>(response.Content);

            // if server is not responding anything
			if (serverResponse == null)
            {
                return "Технические неполадки. Перейдите на страницу \"?\".";
            }

            // Error 15023 means this user already exists
			if (serverResponse.Id == -15023)
                return "Пользователь с таким именем и паролем уже существует. Пожалуйста, придумайте другую комбинацию.";
            else if (serverResponse.Id == -404)
                return "Пользователь не может иметь такое имя и пароль. Попробуйте снова.";
            else
            {
				Constants.MainUser = serverResponse;
                return $"Добро пожаловать в SushiApp, {registered.UserName}";
            }
        }

        /// <summary>
        /// Checks the data type version.
        /// </summary>
        /// <returns>The data type version.</returns>
        /// <param name="type">Type.</param>
        /// <param name="version">Version.</param>
        public async Task<string> CheckDataTypeVersion(string type, string version)
        {
			if (!CheckConnection)
            {
                return "-404";
            }

            DataVersion data = new DataVersion(type, version,1);
            RestRequest request = new RestRequest("versionchecker", Method.POST) 
                    { RequestFormat = DataFormat.Json };
            request.AddBody(SerializationHelper.SerializeToJson(data));

            var response = await client.ExecuteTaskAsync(request);

			if (response.StatusCode == HttpStatusCode.InternalServerError)
				return "-404";

            string serverResponse = JsonConvert.DeserializeObject<string>(response.Content);

            // error 404 means no type was found; error 0 means wtf???
            if (serverResponse == "404" || serverResponse == "0")
                return "0"; // ERROR TYPE
            else
                return serverResponse;
        }

        /// <summary>
        /// Loads the DB of page products.
        /// </summary>
        /// <returns>The DB of page products.</returns>
        /// <param name="type">Type.</param>
        public async Task<int> LoadDBofPageProducts(string type)
        {
			if (!CheckConnection)
            {
                return 404;
            }

            RestRequest request = new RestRequest("loaddb", Method.POST) 
                    { RequestFormat = DataFormat.Json };
            request.AddBody(type);

            var response = await client.ExecuteTaskAsync(request);

            List<Item> serverResponse = JsonConvert.DeserializeObject<List<Item>>(response.Content);

            // ERROR 404 NOT FOUND products
            if (serverResponse == null)
                return 404;
            else
            {
                int amountofdeleted = App.ItemDatabase.DeleteAllTypeItems(type);
                return App.ItemDatabase.SaveListOfItems(serverResponse);
            }
        }

        /// <summary>
        /// Updates the DB of page products.
        /// </summary>
        /// <returns>The DB of page products.</returns>
        /// <param name="type">Type.</param>
        public async Task<int> UpdateDBofPageProducts(string type)
        {
			if (!CheckConnection)
            {
                return 404;
            }

            RestRequest request = new RestRequest("loaddb", Method.POST)
            { RequestFormat = DataFormat.Json };
            request.AddBody(type);

            var response = await client.ExecuteTaskAsync(request);

            List<Item> serverResponse = JsonConvert.DeserializeObject<List<Item>>(response.Content);

            // ERROR 404 NOT FOUND products
            if (serverResponse == null)
                return 404;
            else
            {
                return App.ItemDatabase.SaveListOfItems(serverResponse);
            }
        }

		public async Task<string> SendOrder(Order order)
		{
			if (!CheckConnection)
            {
                return "Нет соединения с интернетом.";
            }

			RestRequest request = new RestRequest("order", Method.POST)
			{ RequestFormat = DataFormat.Json };
			request.AddBody(SerializationHelper.SerializeToJson(order));

			var response = await client.ExecuteTaskAsync(request);

			if (response.StatusCode == 0)
				return "Технические неполадки. Пожалуйста, перейдите на страницу \"?\" в главном меню.";

			return JsonConvert.DeserializeObject<string>(response.Content);
		}
    }
}