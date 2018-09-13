using SushiApp.Data;
using SushiApp.Models;
using SushiApp.Views;
using Xamarin.Forms;

namespace SushiApp
{
    public partial class App : Application
    {
        static ShoppingCart shoppingCart;
        static ItemDatabaseController itemDatabase;
        static TokenDatabaseController tokenDatabase;
        static RestService restService;
        static VersionDatabaseController versionDatabase;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage())
            {
                BarBackgroundColor = Constants.BackgroundColor,
                BarTextColor = Color.White
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static ItemDatabaseController ItemDatabase
        {
            get
            {
                if (itemDatabase == null)
                    itemDatabase = new ItemDatabaseController();

                return itemDatabase;
            }
        }

        public static TokenDatabaseController TokenDatabase
        {
            get
            {
                if (tokenDatabase == null)
                    tokenDatabase = new TokenDatabaseController();

                return tokenDatabase;
            }
        }

        public static RestService RestService
        {
            get
            {
                if (restService == null)
                    restService = new RestService();

                return restService;
            }
        }

        public static VersionDatabaseController VersionDatabase
        {
            get
            {
                if (versionDatabase == null)
                    versionDatabase = new VersionDatabaseController();

                return versionDatabase;
            }
        }

        public static ShoppingCart Shopping
        {
            get
            {
                if (shoppingCart == null)
                    shoppingCart = new ShoppingCart();

                return shoppingCart;
            }
        }
    }
}
