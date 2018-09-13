using System;
using System.Drawing;

// SERVER CLASS -----------------------------------------------------------------

namespace SelfHostApiServer.Models
{
    public class Item : IEquatable<Item>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }

        // image fields 
        public byte[] Pic { get; set; }

        public Item(){}

        public Item(int id, string name, int price,  string type, string description)
        {
            Id = id;
            Name = name;
            Price = price;
            Type = type;
            Description = description;
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

		public override string ToString()
		{
			return $"Name {Name}, price {Price}, descr: {Description}";
		}
	}
}
