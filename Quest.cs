using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    public class Quest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ItemBundle> ItemsToComplete { get; set; }

        public int RewardExp { get; set; }
        public int RewardGold { get; set; }

        public List<ItemBundle> RewardItems { get; set; }

        public Quest(int id, string name, string description, List<ItemBundle> itemsToComplete, int rewardExp, int rewardGold, List<ItemBundle> rewardItems)
        {
            ID = id;
            Name = name;
            Description = description;
            ItemsToComplete = itemsToComplete;
            RewardExp = rewardExp;
            RewardGold = rewardGold;
            RewardItems = rewardItems;

        }
    }
}
