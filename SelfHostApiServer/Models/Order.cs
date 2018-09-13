using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SelfHostApiServer.Models
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
			set { phone = value; }
		}

		public string Street
		{
			get { return street; }
			set { street = value; }
		}

		public string House
		{
			get { return house; }
			set { house = value; }
		}

		public string Entrance
		{
			get { return entrance; }
			set { entrance = value; }
		}

		public string Apartment
		{
			get { return apartment; }
			set { apartment = value; }
		}

		public string Commentary
		{
			get { return commentary; }
			set { commentary = value; }
		}

		public bool Saver
		{
			get { return saver; }
			set { saver = value; }
		}

		public List<Item> OrderList
		{
			get { return order; }
			set { order = value; }
		}

		public int Price
		{
			get { return price; }
			set { price = value; }
		}

        /// <summary>
        /// constructor by default
        /// </summary>
        public Order()
        {
        }
        
		public Order(string phone, string street, string house, string entrance, string apartment, string commentary, bool saver)
		{
			this.phone = phone;
			this.street = street;
			this.house = house;
			this.entrance = entrance;
			this.apartment = apartment;
			this.commentary = commentary;
			this.saver = saver;         
		}

        /*
		List<Item> GetItemsFromListView(ObservableCollection<ShoppingListViewItem> shoppings)
		{
			List<Item> items = new List<Item>();

			foreach(ShoppingListViewItem shops in shoppings)
			{
				items.Add(new Item(shops.Name, shops.Price, shops.Description));
			}

			return items;
		}
		*/

		public override string ToString()
		{
			string ret;

			ret =
				"******************************** \n" +
				"******************************** \n" +
				"NEW ORDER::: " + "price=" + price + "rubles \n" +
				"Login: " + username + "; \n" + 
                "Phone: " + phone + "\n" + 
				"Adress: " + street + "\\" + house +
				"\\" + entrance + "\\" + apartment + "\n" +
				"Comments: " + commentary + "\n";

			foreach (Item item in this.order)
			{
				ret += item.ToString() + "\n";
			}

			ret += "******************************** \n";
			ret += "******************************** \n";

			return ret;
		}
	}
}
