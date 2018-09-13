using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SushiApp.Models
{
    public class Order
    {
		string username;
		string userpassword;
		string phone;
		string street;
		string house;
		string entrance;
		string apartment;
		string commentary;
		bool saver;
		List<Item> order;
		int price;

		public string UserName
        {
            get { return username; }
            set { username = value; }
        }

        public string UserPassword
        {
            get { return userpassword; }
            set { userpassword = value; }
        }

		public string Phone
        {
            get { return phone; }
        }

        public string Street
        {
            get { return street; }
        }

        public string House
        {
            get { return house; }
        }

        public string Entrance
        {
            get { return entrance; }
        }

        public string Apartment
        {
            get { return apartment; }
        }

        public string Commentary
        {
            get { return commentary; }
        }

        public bool Saver
        {
            get { return saver; }
        }

        public List<Item> OrderList
        {
            get { return order; }
        }

        public int Price
        {
            get { return price; }
        }

        /// <summary>
        /// constructor by default
        /// </summary>
        public Order()
        {
        }

		public Order(string phone, string street, string house, string entrance, string apartment, string commentary, bool saver)
		{         
            this.username = Constants.MainUser.UserName;
            this.userpassword = Constants.MainUser.Password;
			this.phone = phone;
			this.street = street;
			this.house = house;
			this.entrance = entrance;
			this.apartment = apartment;
			this.commentary = commentary;
			this.saver = saver;
			order = GetItemsFromListView(App.Shopping.Cart);
			price = App.Shopping.Price;
		}

        /// <summary>
        /// Gets the items from list view.
        /// </summary>
        /// <returns>The items from list view.</returns>
        /// <param name="shoppings">Shoppings.</param>
		List<Item> GetItemsFromListView(ObservableCollection<ShoppingListViewItem> shoppings)
		{
			List<Item> items = new List<Item>();

			foreach(ShoppingListViewItem shops in shoppings)
			{
				items.Add(new Item(shops.Name, shops.Price, shops.Description));
			}

			return items;
		}
	}
}
