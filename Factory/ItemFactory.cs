using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace con_rogue.Factory
{
    public static class ItemFactory
    {
        private static List<Item> _standardItems = new List<Item>();

        static ItemFactory()
        {
            NewMisc(1000, "Floral Dust", "Specks of dust useful in alchemy beginner's alchemy.", 2);
            NewMisc(1001, "Spider silk", "Silky smooth web", 3);
            NewMisc(1002, "Spider Eye", "Used for its properties when brewed.", 8);
            NewMisc(1003, "Boar skin", "Local standard for clothes quality.", 10);

            CreateWeapon(2001, "Training Stick", "Not much value in a fight.", 5, 3, 7);
            CreateWeapon(2002, "Copper Broadsword", "An apprenticeship's craft.", 10, 7, 10);
            CreateWeapon(2003, "Copper Longsword", "An apprenticeship's craft.", 10, 5, 12);
            CreateWeapon(2004, "Iron Longsword", "Poor king's knight's weapon.", 25, 9, 16);

            CreateWeapon(2501, "Spider Attack", "", 0, 5, 15);
            CreateWeapon(2502, "Boar Attack", "", 0, 5, 10);
            

        }

        public static Item CreateItem(int ID)
        {
            return _standardItems.FirstOrDefault(item => item.ID == ID)?.Clone();
        }

        public static void NewMisc(int id, string name, string description, int price)
        {
            _standardItems.Add(new Item(Item.ItemType.Miscellaneous, id, name, description, price));
        }

        // Below function is not a good practice (placeholder currently)
        // Composition over inheritance
        public static void CreateWeapon(int id, string name, string description, int price, int minDmg, int maxDmg)
        {
            Item weapon = new Item(Item.ItemType.Weapon, id, name, description, price, true, minDmg, maxDmg);
            weapon.Action = new AttackWithWeapon(weapon, minDmg, maxDmg);
            _standardItems.Add(weapon);
            
        }
    }
}
