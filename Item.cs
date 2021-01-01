using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    public class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        public Item(int id, string name, string description, int price)
        {
            ID = id;
            Name = name;
            Price = price;
            Description = description;
        }

        // Instantiate new game item 
        public Item Clone()
        {
            return new Item(ID, Name, Description, Price);
        }
    }
}
