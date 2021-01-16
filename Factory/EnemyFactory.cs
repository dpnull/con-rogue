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
                    Enemy spider = new Enemy("Spider", 30, 30, 5, 10);

                    AttackWeapon(spider, 2501);

                    AddLootItem(spider, 1001, 50);
                    AddLootItem(spider, 1002, 50);

                    return spider;

                case 2:
                    Enemy boar = new Enemy("Boar", 50, 50, 3, 6);

                    AttackWeapon(boar, 2502);

                    AddLootItem(boar, 1003, 100);

                    return boar;

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
                enemy.AddItemToInventory(ItemFactory.CreateItem(itemID));
            }
        }

        private static void AttackWeapon(Enemy enemy, int id)
        {
            enemy.CurrentWeapon = ItemFactory.CreateItem(id);
        }
    }

}
