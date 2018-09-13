using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;

namespace SushiApp.Models
{
    public class ListViewItem
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public byte[] ImageBytes { get; set; }

        public ICommand AddToCartCommand { get; private set; }

        /// <summary>
        /// constructor by default
        /// </summary>
        public ListViewItem(){}

        public ListViewItem(string name, int price, string description, byte[] bitmap)
        {
            Name = name;
            Price = price;
            Description = description;
            ImageBytes = bitmap;
            AddToCartCommand = new Command<string>(AddItem);
        }

        void AddItem(string name)
        {
            ShoppingListViewItem convertor = new ShoppingListViewItem();
            convertor.BeConvertedToShopping(this);
            App.Shopping.Cart.Add(convertor);
            Console.WriteLine($"added. AppCart Count = {App.Shopping.Cart.Count}");
            return;
        }

        public override string ToString()
        {
            return $"name={this.Name};price={this.Price}\n" +
                    $"description={this.Description}";
        }
    }
}
