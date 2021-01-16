using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    public class Enemy : Entity
    {
        public int RewardExp { get; set; }

        public Enemy(string name, int maxHealth, int health, int rewardExp, int rewardGold) :
            base(name, maxHealth, health, rewardGold)
        {
            RewardExp = rewardExp;
        }

        public void PrintBattleStats(int x, int y, int width)
        {
            // [==========]
            // [========  ]

            string bar = "[";

            double percent = (double)Health / MaxHealth;
            int complete = Convert.ToInt32(percent * width);
            int incomplete = width - complete;




            for(int i = 0; i < complete; i++)
            {
                bar += "=";
            }

            for (int i = complete; i < width; i++)
            {
                bar += "-";
            }

            bar += "]";

            Console.SetCursorPosition(x, y);
            Console.Write($"-------- {Name} --------");
            y++;
            Console.SetCursorPosition(x, y);
            Console.Write(bar);
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
