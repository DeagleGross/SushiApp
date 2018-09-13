using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SushiApp.Models;
using Xamarin.Forms;

namespace SushiApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Lbl_Username.TextColor = Constants.MainTextColor;
            Lbl_Password.TextColor = Constants.MainTextColor;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;
            Btn_Signin.TextColor = Constants.MainTextColor;
            Btn_Register.TextColor = Constants.MainTextColor;

            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
        }

        async void SignInProcedure(object sender, EventArgs e)
        {
            User user = new User(Entry_Username.Text, Entry_Password.Text);
            if (user.CheckAuthorizationInfo())
            {
                var tryLogin = await App.RestService.Login(user);
				await DisplayAlert("Авторизация", $"{tryLogin}", "OK");

                // if login operation has failed
                if (Constants.MainUser == null)
                    return;

				Console.WriteLine(Constants.MainUser.ToString());
                Application.Current.MainPage = new NavigationPage(new MainMenuPage())
                {
                    BarBackgroundColor = Constants.BackgroundColor,
                    BarTextColor = Color.White,
                };
            }
            else
            {
                await DisplayAlert("Ошибка авторизации", "Вы ввели не все данные", "Ок");
            }
        }

        void GoToRegistrationPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegistrationPage());
        }

		void GoToTroublePage(object sender, EventArgs e)
		{
			Navigation.PushAsync(new TroublePage());
		}
    }
}
