using System;
using System.Collections.Generic;
using SushiApp.Models;
using Xamarin.Forms;

namespace SushiApp.Views
{
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Lbl_Username.TextColor = Constants.MainTextColor;
            Lbl_Password.TextColor = Constants.MainTextColor;
            Lbl_RepeatPassword.TextColor = Constants.MainTextColor;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;
            Btn_Register.TextColor = Constants.MainTextColor;

            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => Entry_RepeatPassword.Focus();
        }

        async void RegisterUser(object sender, EventArgs e)
        {
            User user = new User(Entry_Username.Text, Entry_Password.Text);
            string message;
            if (CheckStatusOfInput(Entry_Username.Text, Entry_Password.Text, Entry_RepeatPassword.Text, out message))
            {
                var response = await App.RestService.Register(user);
                await DisplayAlert("Регистрация", response, "Ок");

                if (Constants.MainUser == null)
                    return;

                // going to main menu page
                Application.Current.MainPage = new NavigationPage(new MainMenuPage())
                {
                    BarBackgroundColor = Constants.BackgroundColor,
                    BarTextColor = Color.White,
                };
            }  
            else
                await DisplayAlert("Регистрация", message, "Ок");
        }

		void GoToTroublePage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TroublePage());
        }

        /// <summary>
        /// returns true if input is ok for registration. Otherwise returns false
        /// </summary>
        /// <returns><c>true</c>, if status of input was checked, <c>false</c> otherwise.</returns>
        /// <param name="username">Username.</param>
        /// <param name="pass">Pass.</param>
        /// <param name="r_pass">R pass.</param>
        /// <param name="message">Message.</param>
        bool CheckStatusOfInput(string username, string pass, string r_pass, out string message)
        {
            if (username == null || pass == null || r_pass == null)
            {
                message = "Вы не заполнили все поля. Повторите ввод.";
                return false;
            }

            if (pass == r_pass)
            {
                message = "Идет регистрация";
                return true;
            }
            else
            {
                message = "Пароли не совпадают. Введите пароли снова.";
                return false;
            }
        }
    }
}
