using System;
using Xamarin.Forms;

namespace SushiApp.Models
{
    public static class Constants
    {
        public static User MainUser;

        public static ServerSettings serverSettings 
            = new ServerSettings("localhost", "12345", new ServerController("api","main"));

        public static bool isDev = true;
        public static Color BackgroundColor = Color.FromHex("#d13a80");
        public static Color MainTextColor = Color.White;

        public static int LoginIconHeight = 120;
    }
}
