using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    public class Item
    {
        public enum ItemType
        {
            Miscellaneous,
            Weapon
        }
        public ItemType Type { get; }
        public int ID { get; }
        public string Name { get; }
        public string Description { get; }
        public int Price { get; }
        public bool IsUnique { get; }
        public int MinDmg { get; }
        public int MaxDmg { get; }

        // Item is not unique by default
        public Item(ItemType type, int id, string name, string description, int price, bool isUnique = false, int minDmg = 0, int maxDmg = 0)
        {
            Type = type;
            ID = id;
            Name = name;
            Price = price;
            Description = description;
            IsUnique = isUnique;
            MinDmg = minDmg;
            MaxDmg = maxDmg;
            
        }

        // Instantiate new game item 
        public Item Clone()
        {
            return new Item(Type, ID, Name, Description, Price, IsUnique, MinDmg, MaxDmg);
        }
    }
}
