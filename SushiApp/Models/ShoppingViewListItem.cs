using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;

namespace SushiApp.Models
{
    public class ShoppingListViewItem
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public byte[] ImageBytes { get; set; }

        public ICommand DeleteFromCartCommand { get; private set; }

        /// <summary>
        /// constructor by default
        /// </summary>
        public ShoppingListViewItem()
        {
            DeleteFromCartCommand = new Command<string>(DeleteItem);
        }

        public ShoppingListViewItem(string name, int price, string description, byte[] bitmap)
        {
            Name = name;
            Price = price;
            Description = description;
            ImageBytes = bitmap;
            DeleteFromCartCommand = new Command<string>(DeleteItem);
        }

        public void BeConvertedToShopping(ListViewItem item)
        {
            this.Name = item.Name;
            this.Price = item.Price;
            this.Description = item.Description;
            this.ImageBytes = item.ImageBytes;
        }

        // not responding the delete method ???
        void DeleteItem(string name)
        {
            App.Shopping.Cart.Remove(this);
            Console.WriteLine($"After deleting: AppCart Count = {App.Shopping.Cart.Count}");
            return;
        }

		public override string ToString()
		{
            return $"name={this.Name};price={this.Price}\n" +
                    $"description={this.Description}";
		}
	}
}
