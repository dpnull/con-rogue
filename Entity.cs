using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace con_rogue
{
    // Abstract cannot be instantiated but the child of this class can
    public abstract class Entity
    {
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Gold { get; set; }

        public List<Item> Inventory { get; set; }
        public List<GroupedInventoryItem> GroupedInventory { get; set; }

        public List<Item> Weapons => Inventory.Where(i => i.Type == Item.ItemType.Weapon).ToList();

        // Only a child class can use the constructor
        protected Entity()
        {
            Inventory = new List<Item>();
            GroupedInventory = new List<GroupedInventoryItem>();
        }

        public void AddItemToInventory(Item item)
        {
            Inventory.Add(item);
        }

        public void RemoveItemFromInventory(Item item)
        {
            Inventory.Remove(item);
        }
    }
}
