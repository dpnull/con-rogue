using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    public class Weapon : Item
    {
        public int MinDmg { get; set; }
        public int MaxDmg { get; set; }

        public Weapon(int id, string name, string description, int price, int minDmg, int maxDmg)
            : base(id, name, description, price)
        {
            MinDmg = minDmg;
            MaxDmg = maxDmg;
        }

        // 
        public new Weapon Clone()
        {
            return new Weapon(ID, Name, Description, Price, MinDmg, MaxDmg);
        }
    }
}
