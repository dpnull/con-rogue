using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    public class Trader : Entity
    {
        public Trader(string name) : base(name, 100, 100, 10000)
        {
        }

        public void PrintItems(int x, int y, int windowWidth)
        {
            int index = 0;
            Console.SetCursorPosition(x, y + 1);
            Console.WriteLine("[[NAME]]                                 [[PRICE]]");
            y++;
            foreach (GroupedInventoryItem item in GroupedInventory)
            {
                if (item.Item.Type != Item.ItemType.Weapon)
                {
                    index++;
                    y++;
                    Console.SetCursorPosition(x, y);
                    Console.WriteLine($"{index}) {item.Item.Name} Quantity:{item.Quantity}");
                    Console.SetCursorPosition(x + windowWidth - 9, y);
                    Console.WriteLine($"{item.Item.Price}");
                } else
                {
                    index++;
                    y++;
                    Console.SetCursorPosition(x, y);
                    Console.WriteLine($"{index}) {item.Item.Name}");
                    Console.SetCursorPosition(x + windowWidth - 9, y);
                    Console.WriteLine($"{item.Item.Price}");
                }

            }
        }
    }
}
