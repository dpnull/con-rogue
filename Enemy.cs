using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    public class Enemy : Entity
    {
        public int MinDmg { get; set; }
        public int MaxDmg { get; set; }
        public int RewardExp { get; set; }

        public Enemy(string name, int maxHealth, int health, int minDmg, int maxDmg, int rewardExp, int rewardGold) :
            base(name, maxHealth, health, rewardGold)
        {
            MinDmg = minDmg;
            MaxDmg = maxDmg;
            RewardExp = rewardExp;
        }

        public void PrintBattleStats(int x, int y)
        {
            int damage = (MinDmg + MaxDmg) / 2;
            Console.SetCursorPosition(x, y);
            Console.Write($"NAME:       {Name}");
            y++;
            Console.SetCursorPosition(x, y);
            Console.Write($"HEALTH:     {Health}");
            y++;
            Console.SetCursorPosition(x, y);
            Console.Write($"DAMAGE:     {damage}");
        }

        public void DrawEnemy(int x, int y)
        {
            x += 2; y++;
            Console.SetCursorPosition(x, y);
            Console.Write(Name);
            y++;
            Console.SetCursorPosition(x, y);
            Console.Write($"{Health} / {MaxHealth}");
        }
    }
}
