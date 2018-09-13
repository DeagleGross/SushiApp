using System;
using SQLite;
using Xamarin.Forms;

// MOBILE CLASS --------------------------------------------------------------- 

namespace SushiApp.Models
{
    public class Item : IEquatable<Item>
    {
        [AutoIncrement][PrimaryKey][NotNull]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }

        public byte[] Pic { get; set; }
        // ------------------------------------------------------ image fields

        public Item(){}

        public Item(int id, string name, int price, string type, string description)
        {
            Id = id;
            Name = name;
            Price = price;
            Type = type;
            Description = description;
        }

		public Item(string name, int price, string description)
		{
			this.Name = name;
			this.Price = price;
			this.Description = description;
		}

        public bool Equals(Item other)
        {
            if (this.Name == other.Name &&
                this.Price == other.Price &&
                this.Type == other.Type &&
                this.Description == other.Description
               )
                return true;
            else
                return false;
        }
    }
}
