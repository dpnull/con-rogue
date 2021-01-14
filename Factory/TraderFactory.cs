using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace con_rogue.Factory
{
    public static class TraderFactory
    {
        private static readonly List<Trader> _traders = new List<Trader>();

        static TraderFactory()
        {
            Trader oldLady = new Trader("Old Lady");
            oldLady.AddItemToInventory(ItemFactory.CreateItem(2004));
            oldLady.AddItemToInventory(ItemFactory.CreateItem(2003));
            oldLady.AddItemToInventory(ItemFactory.CreateItem(1000));

            AddTrader(oldLady);
        }

        public static Trader GetTraderByName(string name)
        {
            return _traders.FirstOrDefault(t => t.Name == name);
        }

        private static void AddTrader(Trader trader)
        {
            if(_traders.Any(t => t.Name == trader.Name))
            {
                throw new ArgumentException("Trader with that name already exists!");
            }

            _traders.Add(trader);
        }
    }
}
