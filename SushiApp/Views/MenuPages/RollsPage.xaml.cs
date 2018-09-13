using System.Collections.Generic;
using System.Collections.Specialized;
using SushiApp.Models;
using Xamarin.Forms;
using System.Linq;

namespace SushiApp.Views.MenuPages
{
    public partial class RollsPage : ContentPage
    {
        private ListViewFormer lstFormer;
        List<ListViewItem> Items;
        private string type;

        public RollsPage()
        {
            type = "Rolls";
            lstFormer = new ListViewFormer();
			App.Shopping.Cart.CollectionChanged += UpdateAddInfo;
            InitList();
            InitializeComponent();
        }

        public RollsPage(string type)
        {
            this.type = type;
            lstFormer = new ListViewFormer();
			App.Shopping.Cart.CollectionChanged += UpdateAddInfo;
            InitList();
            InitializeComponent();
        }

		private void UpdateAddInfo(object sender, NotifyCollectionChangedEventArgs e)
        {
			string dynamicRes;
			if (App.Shopping.Cart.Count == 0)
                dynamicRes = "";
            else
				dynamicRes = "В корзину был добавлен продукт:  " +
                App.Shopping.Cart.Last().Name + ".";
			Resources["AddedInfo"] = dynamicRes;
        }

		void InitList()
		{
			// created a list<listviewitem> of products to be displayed
			Items = lstFormer.GetListForPage(this.type);

			// created a listview with design!!!
			ListView listView = lstFormer.CreateListViewInstance(Items);

			StackLayout forLabel = new StackLayout
			{
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				BackgroundColor = Constants.BackgroundColor,
				Padding = new Thickness(15, 0, 15, 5)
			};

			Label addedLabel = new Label
			{
				BackgroundColor = Constants.BackgroundColor,
				TextColor = Color.White,
				FontAttributes = FontAttributes.Italic,
				HorizontalOptions = LayoutOptions.Center
			};

			ResourceDictionary resDict = new ResourceDictionary();
			// добавляем ресурсы в словарь
			string dynamicRes;

			if (App.Shopping.Cart.Count == 0)
				dynamicRes = "";
			else
				dynamicRes = "В корзину был добавлен продукт:  " +
				App.Shopping.Cart.Last().Name + ".";
			
            resDict.Add("AddedInfo", dynamicRes);

            // устанавливаем словарь ресурсов
            this.Resources = resDict;

            // dynamic resource from shopping cart price
			addedLabel.SetDynamicResource(Label.TextProperty, "AddedInfo");
			forLabel.Children.Add(addedLabel);

			this.Content = new StackLayout { Children = { listView, forLabel } };
		}
    }
}
