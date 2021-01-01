using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace con_rogue
{
    public class Player
    {

        public string Name { get; set; }
        public int Health { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }
        public int Gold { get; set; }

        public List<Item> ItemInventory { get; set; }
        public List<Weapon> WeaponInventory { get; set; }

        public List<QuestStatus> Quests { get; set; }

        public Weapon CurrentWeapon { get; set; }

        public Player()
        {
            Name = "Dom";
            Health = 100;
            Experience = 0;
            Level = 1;
            Gold = 25;

            ItemInventory = new List<Item>();
            WeaponInventory = new List<Weapon>();
            Quests = new List<QuestStatus>();


        }

        public Player(string name, int health, int experience, int level, int gold)
        {
            Name = name;
            Health = health;
            Experience = experience;
            Level = level;
            Gold = gold;
        }

        // Remove item from player's inventory
        public void RemoveItemFromInventory(Item item)
        {
            ItemInventory.Remove(item);
        }

        // 
        public bool HasRequiredItems(List<ItemBundle> items)
        {
            foreach(ItemBundle item in items)
            {
                if(ItemInventory.Count(i => i.ID == item.ID) < item.Quantity)
                {
                    return false;
                }
            }

            return true;
        }

        public void PrintBattleStats(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine($"HEALTH:     {Health}");
            y++;
            Console.SetCursorPosition(x, y);
            Console.WriteLine($"WEAPON:     {CurrentWeapon.Name}");       
            y++;
            Console.SetCursorPosition(x, y);
            Console.WriteLine($"DAMAGE:     {CurrentWeapon.MinDmg} - {CurrentWeapon.MaxDmg}");
        }

        public void PrintQuests(int x, int y)
        {
            for (int i = 0; i < Quests.Count; i++)
            {
                y++;
                Console.SetCursorPosition(x, y);
                Console.WriteLine("{0}", Quests[i].PlayerQuest.Name);
            }
        }

        public void PrintWeapons(int x, int y)
        {
            for (int i = 0; i < WeaponInventory.Count; i++)
            {
                y++;
                Console.SetCursorPosition(x, y);
                Console.WriteLine("{0}) {1}  [{2} - {3}]", i + 1, WeaponInventory[i].Name, WeaponInventory[i].MinDmg, WeaponInventory[i].MaxDmg);
            }
        }

        public void PrintMisc(int x, int y)
        {
            for (int i = 0; i < ItemInventory.Count; i++)
            {
                y++;
                Console.SetCursorPosition(x, y);
                Console.WriteLine("{0}) {1}   [ ${2} ]", i, ItemInventory[i].Name, ItemInventory[i].Price);
            }
        }

        public void AddWeaponToInventory(Weapon weapon)
        {
            WeaponInventory.Add(weapon);
        }

        public void AddItemToInventory(Item item)
        {
            ItemInventory.Add(item);
        }
    }
}
