using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace con_rogue.Factory
{
    public static class ItemFactory
    {
        private static List<Item> _standardItems;
        private static List<Weapon> _standardWeapons;

        static ItemFactory()
        {
            _standardItems = new List<Item>();
            _standardWeapons = new List<Weapon>();

            _standardWeapons.Add(new Weapon(2001, "Training Stick", "Not much value in a fight.", 5, 3, 7));
            _standardWeapons.Add(new Weapon(2002, "Copper Broadsword", "An apprenticeship's craft.", 10, 7, 10));
            _standardWeapons.Add(new Weapon(2003, "Copper Longsword", "An apprenticeship's craft.", 10, 5, 12));

            _standardItems.Add(new Item(1001, "Floral Dust", "Specks of dust useful in alchemy beginner's alchemy.", 2));

            _standardItems.Add(new Item(3001, "Spider Silk", "Useful in alchemy and threading.", 4));
            _standardItems.Add(new Item(3002, "Spider Eye", "Used for its properties when brewed.", 8));

        }

        public static Item CreateItem(int id)
        {
            // use LINQ to find first item matching passed in ID
            Item standardItem = _standardItems.FirstOrDefault(item => item.ID == id);

            if (standardItem != null)
            {
                if (standardItem is Weapon)
                {
                    return (standardItem as Weapon).Clone();
                }

                return standardItem.Clone();
            }

            return null;
        }

        public static Weapon CreateWeapon(int id)
        {
            // use LINQ to find first item matching passed in ID
            Weapon standardWeapon = _standardWeapons.FirstOrDefault(item => item.ID == id);

            if (standardWeapon != null)
            {
                // Make a copy of standard item and return it
                return standardWeapon.Clone();
            }

            return null;
        }
    }
}
