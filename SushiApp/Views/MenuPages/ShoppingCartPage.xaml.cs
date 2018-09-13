using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using SushiApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace SushiApp.Views.MenuPages
{
    public partial class ShoppingCartPage : ContentPage
    {
        private ShoppingListViewFormer lstFormer;

        public ShoppingCartPage()
        {
            lstFormer = new ShoppingListViewFormer();
            App.Shopping.Cart.CollectionChanged += UpdatePrice;
            InitList();
            InitializeComponent();
        }

        /// <summary>
        /// Updates the resource so value changes in listview
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void UpdatePrice(object sender, NotifyCollectionChangedEventArgs e)
        {
			string dynamicUpdater = "Общая стоимость покупки: " +
                            App.Shopping.Price.ToString() + " рублей";
			Resources["PriceCart"] = dynamicUpdater;
        }

        void InitList()
        {
            this.Content = formPageWithControls();
        }

        StackLayout formPageWithControls()
        {
            StackLayout page = new StackLayout 
			{ 
			    Orientation = StackOrientation.Vertical
			};

            // Items is the list to be displayed
            // created a listview with design!!!
            ListView listView = lstFormer.ShoppingItems;

            StackLayout priceInfo = new StackLayout 
            { 
				Orientation = StackOrientation.Vertical,
				BackgroundColor = Constants.BackgroundColor,
				Padding = new Thickness (15,0,15,5)
            };         

			double width = (double)UIKit.UIScreen.MainScreen.Bounds.Width;
			double height = priceInfo.Height;

			Label priceLabel = new Label
			{
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				WidthRequest = width,
				HeightRequest = height,
				TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
				BackgroundColor = Constants.BackgroundColor
            };

			ResourceDictionary resDict = new ResourceDictionary();
			// добавляем ресурсы в словарь
			string dynamicRes = "Общая стоимость покупки: " +
				            App.Shopping.Price.ToString() + " рублей";
			resDict.Add("PriceCart", dynamicRes);

            // устанавливаем словарь ресурсов
            this.Resources = resDict;

            // dynamic resource from shopping cart price
			priceLabel.SetDynamicResource(Label.TextProperty, "PriceCart");         
            priceInfo.Children.Add(priceLabel);
            
            Button buttonDelivery = new Button
            {
				TextColor = Color.Black,
                Text = "Оформить заказ",
                FontAttributes = FontAttributes.Bold,
				FontSize = 20,
				BackgroundColor = Color.White,
                BorderColor = Color.Black,
                BorderWidth = 3,
				CornerRadius = 5,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

			priceInfo.Children.Add(buttonDelivery);

            buttonDelivery.Clicked += GoToFinalDelivery;
            
            page.Children.Add(listView);
			page.Children.Add(priceInfo);
            //page.Children.Add(buttonDelivery);

            return page;
        }

        /// <summary>
        /// Goes to final delivery page
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void GoToFinalDelivery(object sender, EventArgs e)
        {
			if (App.Shopping.Cart.Count == 0 || App.Shopping.Price == 0)
			{
				DisplayAlert("Оформление заказа", 
				             "Корзина пуста! Пожалуйста, выберите продукты :)", 
				             "Конечно!");            
                return;
			}

            Navigation.PushAsync(new FinalDeliveryPage());
        }
    }
}
