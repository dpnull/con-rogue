using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    // Abstract cannot be instantiated but the child of this class can
    public abstract class Entity
    {
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Gold { get; set; }


    }
}
