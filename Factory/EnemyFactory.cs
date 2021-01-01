using System;
using System.Collections.Generic;
using System.Text;


namespace con_rogue.Factory
{
    public static class EnemyFactory
    {
        public static Enemy CreateEnemy(int enemyID)
        {
            switch (enemyID)
            {
                case 1:
                    Enemy spider = new Enemy("Spider", 30, 30, 5, 10, RNG.Generator(3, 6), RNG.Generator(5, 8));

                    AddLootItem(spider, 3001, 50);
                    AddLootItem(spider, 3002, 50);

                    return spider;


                default:
                    // add throw exception
                    break;

            }
            return null;
        }

        public static void AddLootItem(Enemy enemy, int itemID, int percentage)
        {
            if (RNG.Generator(1, 100) <= percentage)
            {
                enemy.Inventory.Add(new ItemBundle(itemID, 1));
            }
        }
    }

}
