using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    public class GroupedInventoryItem
    {
        public Item Item { get; set; }
        public int Quantity { get; set; }

        public GroupedInventoryItem(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}
