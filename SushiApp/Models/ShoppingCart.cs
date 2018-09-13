using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SushiApp.Models
{


	public class ShoppingCart 
    {
        ObservableCollection<ShoppingListViewItem> cart;
        int price;

        /// <summary>
        /// Property for getting the value of cart price
        /// </summary>
        /// <value>The price.</value>
        public int Price
        {
            get
            {
                price = GetPriceValue();
                return price;
            }
        }

        /// <summary>
        /// PUBLIC Property for getting the cart
        /// </summary>
        /// <value>The cart.</value>
        public ObservableCollection<ShoppingListViewItem> Cart 
        {
            get
            {
                if (cart == null)
                    cart = new ObservableCollection<ShoppingListViewItem>();

                return cart;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ShoppingCart class with price = 0.
        /// </summary>
        public ShoppingCart()
        {
            price = 0;
        }

        /// <summary>
        /// returns value of price of whole cart
        /// </summary>
        /// <returns>The price value.</returns>
        int GetPriceValue()
        {
            price = 0;

            foreach (ShoppingListViewItem item in cart)
            {
                price += item.Price;    
            }

            return price;
        }

		public void ClearAll()
		{
			this.price = 0;
			cart.Clear();
		}
    }
}
