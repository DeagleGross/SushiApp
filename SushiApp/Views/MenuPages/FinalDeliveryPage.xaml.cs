using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SushiApp.Models;
using Xamarin.Forms;

namespace SushiApp.Views.MenuPages
{
    public partial class FinalDeliveryPage : ContentPage
    {
		public FinalDeliveryPage()
		{
			InitializeComponent();
			this.BackgroundColor = Constants.BackgroundColor;
			PriceState.Text = $"Цена заказа: {App.Shopping.Price.ToString()} рублей";
			LoadOrderInfo();
		}      

		async void SendOrder(object sender, EventArgs e)
		{
			if (CheckInputState())
			{
				string phoneNum = GlueNumber();
				string street = Street.Text;
				string house = House.Text;
				string entrance = Entrance.Text;
				string apartment = Apartment.Text;
				string commentary = Commentary.Text;
				bool save = SwitchToggler.IsToggled;

				Order order = new Order
					(phoneNum, street, house, entrance, apartment, commentary, save);
                                
				var response = await App.RestService.SendOrder(order);

				if (response == "Технические неполадки. Пожалуйста, перейдите на страницу \"?\" в главном меню.")
				{
					await DisplayAlert("Заказ", $"{response}", "Ок");
					return;
				}
				
				await DisplayAlert("Заказ", $"{response}", "Ок");


				await CloseCart();
			}
			else
				return;
		}

		async Task CloseCart()
		{
			App.Shopping.ClearAll();

			Application.Current.MainPage = new NavigationPage(new MainMenuPage())
            {
                BarBackgroundColor = Constants.BackgroundColor,
                BarTextColor = Color.White,
            };

			await Application.Current.MainPage.Navigation.PushAsync(new DeliveryPage());
		}

		/// <summary>
        /// Loads the order info.
        /// </summary>
        void LoadOrderInfo()
        {
            if (Constants.MainUser.PhoneNum == null ||
                Constants.MainUser.Street == null ||
                Constants.MainUser.House == null ||
                Constants.MainUser.Entrance == null ||
                Constants.MainUser.Apartment == null
               )
                return;

            string[] phone = BackPhoneGluerer(Constants.MainUser.PhoneNum);
            PhoneQ.Text = phone[0];
            PhoneW.Text = phone[1];
            PhoneE.Text = phone[2];
            PhoneR.Text = phone[3];
            PhoneT.Text = phone[4];

            Street.Text = Constants.MainUser.Street;
            House.Text = Constants.MainUser.House;
            Entrance.Text = Constants.MainUser.Entrance;
            Apartment.Text = Constants.MainUser.Apartment;
        }

        /// <summary>
        /// Divides the phone number so it can be easily shown
        /// </summary>
        /// <returns>The phone gluerer.</returns>
        /// <param name="phone">Phone.</param>
        string[] BackPhoneGluerer(string phone)
        {
            string[] tmp = new string[5];
            char[] arr;

            if (phone[0] == '+')
            {
                arr = new char[] { phone[0], phone[1] };
                tmp[0] = new string(arr);

                arr = new char[] { phone[2], phone[3], phone[4] };
                tmp[1] = new string(arr);

                arr = new char[] { phone[5], phone[6], phone[7] };
                tmp[2] = new string(arr);

                arr = new char[] { phone[8], phone[9] };
                tmp[3] = new string(arr);

                arr = new char[] { phone[10], phone[11] };
                tmp[4] = new string(arr);
            }
            else
            {
                arr = new char[] { phone[0] };
                tmp[0] = new string(arr);

                arr = new char[] { phone[1], phone[2], phone[3] };
                tmp[1] = new string(arr);

                arr = new char[] { phone[4], phone[5], phone[6] };
                tmp[2] = new string(arr);

                arr = new char[] { phone[7], phone[8] };
                tmp[3] = new string(arr);

                arr = new char[] { phone[9], phone[10] };
                tmp[4] = new string(arr);
            }

            return tmp;
        }

        /// <summary>
        /// returns phone number from all the fields in one string
        /// </summary>
        /// <returns>The number.</returns>
		string GlueNumber()
		{
			return PhoneQ.Text + PhoneW.Text + PhoneE.Text + PhoneR.Text + PhoneT.Text;
		}

		bool CheckInputState()
		{
			if (PhoneQ.Text == null ||
			    PhoneW.Text == null ||
			    PhoneE.Text == null ||
			    PhoneR.Text == null ||
			    PhoneT.Text == null)
			{
				DisplayAlert("Информация о заказе", "Вы не заполнили телефонный номер", "Ок");
                return false;
			}

			if (Street.Text == null ||
			    House.Text == null ||
			    Entrance.Text == null ||
			    Apartment.Text == null)
			{
				DisplayAlert("Информация о заказе","Вы не заполнили обязательные поля по доставке","Ок");
				return false;
			}				    

			if (!CheckPhoneNumberValid())
			{
				DisplayAlert("Информация о заказе", "Введенный вами телефон не может существовать", "Ок");
				return false;
			}
                     
			return true;
		}

		bool CheckPhoneNumberValid()
		{
			// first check the first letters of phone number
			if (PhoneQ.Text.Length > 2)
				return false;
			if (CheckLengthAndInput(PhoneW.Text, 3) == false ||
				CheckLengthAndInput(PhoneE.Text, 3) == false ||
				CheckLengthAndInput(PhoneR.Text, 2) == false ||
				CheckLengthAndInput(PhoneT.Text, 2) == false)
				    return false;
			return true;
		}

		bool CheckLengthAndInput(string txt, int length)
		{
			if (ContainsOnlyDigits(txt) && txt.Length == length)
				return true;
			return false;
		}

		bool ContainsOnlyDigits(string phone)
		{
			foreach(char symb in phone)
			{
				if (!(symb <= '9' && symb >= '0'))
					return false;
			}
			return true;
		}
    }
}
