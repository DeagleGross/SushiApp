using System;
using System.Data.SqlClient;
using SelfHostApiServer.Models;

namespace SelfHostApiServer.GlobalSettings
{
    public static class GSettings
    {
        /// <summary>
        /// The connection string.
        /// </summary>
        public static string connectionString = @"Data Source=localhost;Initial Catalog=SushiApp;" +
                               "User ID=sa;Password=Deaglegross123";

        /// <summary>
        /// The project folder path on server
        /// </summary>
        //public static string ProjectFolder = "Hackintosh/Users/dgmachack/Projects/CourseProject/SelfHostApiServer/";

        /// <summary>
        /// The adress of connection
        /// </summary>
        public static Address Adress = new Address("localhost", "12345");
    }
}
