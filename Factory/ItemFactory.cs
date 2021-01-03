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

            NewWeapon(2001, "Training Stick", "Not much value in a fight.", 5, 3, 7);
            NewWeapon(2002, "Copper Broadsword", "An apprenticeship's craft.", 10, 7, 10);
            NewWeapon(2003, "Copper Longsword", "An apprenticeship's craft.", 10, 5, 12);

        }

        public static Item CreateItem(int ID)
        {
            return _standardItems.FirstOrDefault(item => item.ID == ID)?.Clone();
        }

        public static void NewMisc(int id, string name, string description, int price)
        {
            _standardItems.Add(new Item(Item.ItemType.Miscellaneous, id, name, description, price));
        }

        public static void NewWeapon(int id, string name, string description, int price, int minDmg, int maxDmg)
        {
            _standardItems.Add(new Item(Item.ItemType.Weapon, id, name, description, price, true, minDmg, maxDmg));
        }
    }
}
