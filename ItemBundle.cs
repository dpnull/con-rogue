using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    public class ItemBundle
    {
        public int ID { get; set; }
        public int Quantity { get; set; }

        public ItemBundle(int id, int quantity)
        {
            ID = id;
            Quantity = quantity;
        }
 
    }
}
