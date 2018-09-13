using System;
using System.Collections.Generic;
using SushiApp.Data;
using SushiApp.Views.MenuPages;
using Xamarin.Forms;
using SushiApp.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SushiApp.Views
{
    public partial class MainMenuPage : ContentPage
    {
        DataVersion data;

        public MainMenuPage()
        {
            this.BackgroundColor = Constants.BackgroundColor;
            InitializeComponent();
			//InitVersionDB();
			InitForAdmin();
        }

		void InitForAdmin()
		{
			if (Constants.MainUser.UserName == "AdminSushi")
            {
                ButtonDeleteItemDB.IsVisible = true;
                ButtonDeleteVersionDB.IsVisible = true;
            }
		}

        /// <summary>
        /// in case we need to change mobiledatabase
        /// </summary>
        void InitVersionDB()
        {
            Item returnedItem = App.ItemDatabase.GetItem(1);
            int returned = App.VersionDatabase.SaveVersion(new DataVersion("Rolls", "1", 1));
            data = App.VersionDatabase.GetVersionByID(1);
        }

        void ShoppingCartBrowse(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ShoppingCartPage());
        }

        async void RollsBrowse(object sender, EventArgs e)
        {
            if (await WorkWithDatabase("Rolls"))
                await Navigation.PushAsync(new RollsPage("Rolls"));
        }

        async void HotRollsBrowse(object sender, EventArgs e)
        {
            if (await WorkWithDatabase("HotRolls"))
                await Navigation.PushAsync(new HotRollsPage());
        }

        async void SushiBrowse(object sender, EventArgs e)
        {
            if (await WorkWithDatabase("Sushi"))
                await Navigation.PushAsync(new SushiPage());
        }

        async void FireBrowse(object sender, EventArgs e)
        {
            if (await WorkWithDatabase("Fire"))
                await Navigation.PushAsync(new FirePage());
        }

        async void SetsBrowse(object sender, EventArgs e)
        {
            if (await WorkWithDatabase("Sets"))
                await Navigation.PushAsync(new SetsPage());
        }

        async void SoucesBrowse(object sender, EventArgs e)
        {
            if (await WorkWithDatabase("Souces"))
                await Navigation.PushAsync(new SoucesPage());
        }

        async void DrinksBrowse(object sender, EventArgs e)
        {
            if (await WorkWithDatabase("Drinks"))
                await Navigation.PushAsync(new DrinksPage());
        }

        async void DeliveryBrowse(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DeliveryPage());
        }

		void GoToTroublePage(object sender, EventArgs e)
		{
			Navigation.PushAsync(new TroublePage());
		}

        // for testing --------------------------------------------------------

        async void ClearVersionDatabase(object sender, EventArgs e)
        {
            int amount = App.VersionDatabase.DeleteWholeDB();
            await DisplayAlert("Versions", $"deleted dataversions {amount}", "ok");
        }

        async void ClearItemDatabase(object sender, EventArgs e)
        {
            int amount = App.ItemDatabase.DeleteWholeDB();
            await DisplayAlert("Items", $"deleted products {amount}", "ok");   
        }

        // for testing --------------------------------------------------------


        /// <summary>
        /// Checks version of mobile db and then updates\loads\nothing
        /// </summary>
        /// <param name="typename">string var of type</param>
        async Task<bool> WorkWithDatabase(string typename)
        {
            // getting version from mobile db
            string version = App.VersionDatabase.GetTypeVersion(typename);

            if (version == null) // no version of this type-product was found
            {
                await DisplayAlert("Небольшая ошибочка", "Не найдено загруженных товаров. Сейчас загрузим :)", "Конечно!");
                // here try to load database too, cause its a trouble. BUT NO CRASH!!!
                bool loadCompletedOk = await LoadFullDatabase(typename); // loads fine without images but well fix it soon

                // here we need to load actual version of type from server if load was ok
                if (loadCompletedOk)
                    await SetupDataVersion(typename);

                return loadCompletedOk;
            }

            string response = await App.RestService.CheckDataTypeVersion(typename, version);
            switch (response)
            {
                case "0": // Error occured
                    await DisplayAlert("Небольшая ошибочка", "Сейчас загрузим актуальную базу товаров :)", "Ждем-с");
                    // here try to load database
                    bool loadCompletedOk = await LoadFullDatabase(typename); 

                    // here we need to load actual version of type from server if load was ok
                    if (loadCompletedOk)
                        await SetupDataVersion(typename);

                    return loadCompletedOk;
				case "-404":
					await DisplayAlert("Ошибка","Вероятно, есть неполадки на сервере. Перейдите на страницу \"?\" для дополнительной информации","Ок");
					return false;
                default:
                    if (version != response) // check for update is needed or not
                    {
                        await DisplayAlert("База товаров", "Версия товаров не последняя. Сейчас загрузим :)", "ОК");
                        // here try to update в
                        bool updateCompletedOk = await UpdateDatabase(typename);

                        // here we need to change version of type from server if update was ok
                        if (updateCompletedOk)
                            await ChangeDataVersion(typename);

                        return updateCompletedOk;
                    }
                    else
                    {
                        //await DisplayAlert("База товаров", "Версия товаров последняя. Заказывайте, что хотите :)", "Поехали");
                        return true;
                    }    
            }
        }
              
        /// <summary>
        /// Loads the full data to mobile db
        /// </summary>
        /// <param name="type">Type.</param>
        async Task<bool> LoadFullDatabase(string type)
        {
            int response = await App.RestService.LoadDBofPageProducts(type);
                
            if (response == 404)
                {
				await DisplayAlert("Еще не готовы :(","База продуктов этого вида еще не готова для доставки. Приносим наши извинения","Вернуться в меню");
                    return false;
                }

                //for testing NEED TO BE COMMENTED BEFORE PUBLISHING if needed
                int amountofproductsinwholedb = App.ItemDatabase.GetCountOfItems();
                Console.WriteLine($"Products loaded={response}\\now={amountofproductsinwholedb}");
            return true;
        }

        /// <summary>
        /// Checks for missing products and updates the mobile db
        /// </summary>
        /// <param name="type">Type.</param>
        async Task<bool> UpdateDatabase(string type)
        {
            int response = await App.RestService.UpdateDBofPageProducts(type);

            if (response == 404)
            {
				await DisplayAlert("Еще не готовы :(", "База продуктов этого вида еще не готова для доставки. Приносим наши извинения.", "Вернуться в меню");
                return false;
            }

    			//for testing NEED TO BE COMMENTED BEFORE PUBLISHING
                int amountofproductsinwholedb = App.ItemDatabase.GetCountOfItems();
                Console.WriteLine($"Products loaded={response}\\now={amountofproductsinwholedb}");
            return true;
        }

        /// <summary>
        /// Setups the actual data version to mobile db
        /// </summary>
        /// <param name="typename">Typename.</param>
        async Task SetupDataVersion(string typename)
        {
            string gotFromServer = await App.RestService.CheckDataTypeVersion(typename, "_load");
            if (!App.VersionDatabase.ChangeDataVersion(typename, gotFromServer))
                App.VersionDatabase.SetupDataVersion(typename, gotFromServer);
        }

        /// <summary>
        /// Changes the actual data version to mobile db
        /// </summary>
        /// <param name="typename">Typename.</param>
        async Task ChangeDataVersion(string typename)
        {
            string gotFromServer = await App.RestService.CheckDataTypeVersion(typename, "_load");
            bool changed = App.VersionDatabase.ChangeDataVersion(typename, gotFromServer);
        }
    }
}
