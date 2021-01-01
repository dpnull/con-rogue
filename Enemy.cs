using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    public class Enemy
    {
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int MinDmg { get; set; }
        public int MaxDmg { get; set; }
        public int RewardExp { get; set; }
        public int RewardGold { get; set; }
        public List<ItemBundle> Inventory { get; set; }

        public Enemy(string name, int maxHealth, int health, int minDmg, int maxDmg, int rewardExp, int rewardGold)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = health;
            MinDmg = minDmg;
            MaxDmg = maxDmg;
            RewardExp = rewardExp;
            RewardGold = rewardGold;

            Inventory = new List<ItemBundle>();
        }

        public void PrintBattleStats(int x, int y)
        {
            int damage = (MinDmg + MaxDmg) / 2;
            Console.SetCursorPosition(x, y);
            Console.WriteLine($"NAME:       {Name}");
            y++;
            Console.SetCursorPosition(x, y);
            Console.WriteLine($"HEALTH:     {Health}");
            y++;
            Console.SetCursorPosition(x, y);
            Console.WriteLine($"DAMAGE:     {damage}");
        }

        public void DrawEnemy(int x, int y)
        {
            x += 2; y++;
            Console.SetCursorPosition(x, y);
            Console.WriteLine(Name);
            y++;
            Console.SetCursorPosition(x, y);
            Console.WriteLine($"{Health} / {MaxHealth}");
        }
    }
}
