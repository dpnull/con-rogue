using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace con_rogue
{
    public class Player : Entity
    {

        public int Experience { get; set; }
        public int Level { get; set; }
        public List<QuestStatus> Quests { get; set; }
        public Item CurrentWeapon { get; set; }

        public Player(string name, int maxHealth, int health, int experience, int level, int gold) :
            base(name, maxHealth, health, gold)
        {
            Experience = experience;
            Level = level;

            Quests = new List<QuestStatus>();
        }

        public bool HasRequiredItems(List<ItemBundle> items)
        {
            foreach(ItemBundle item in items)
            {
                if(Inventory.Count(i => i.ID == item.ID) < item.Quantity)
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
            int index = 0;
            foreach(Item item in Inventory)
            {
                if(item.Type != Item.ItemType.Weapon)
                {
                    continue;
                }
                index++;
                y++;
                Console.SetCursorPosition(x, y);
                Console.WriteLine($"{index}) {item.Name}    [{item.MinDmg} - {item.MaxDmg}]");
            }
        }

        /*
        public void PrintWeapons(int x, int y)
        {
            int index = 0;
            for (int i = 0; i < Inventory.Count; i++)
            {
                if(Inventory[i].Type == Item.ItemType.Weapon)
                {
                    Inventory.OrderByDescending(x => (int)(x.Type)).ToList();
                    index++;
                    y++;
                    Console.SetCursorPosition(x, y);
                    Console.WriteLine("{0}) {1}  [{2} - {3}]", index, Inventory[i].Name, Inventory[i].MinDmg, Inventory[i].MaxDmg);            
                }
            }
        }
        */

        public void PrintMisc(int x, int y)
        {
            int index = 0;
            for (int i = 0; i < GroupedInventory.Count; i++)
            {
                if(GroupedInventory[i].Item.Type == Item.ItemType.Miscellaneous)
                {
                    index++;
                    y++;
                    Console.SetCursorPosition(x, y);
                    Console.WriteLine("{0}) [ x{1} ] {2}   [ ${3} ]", index, GroupedInventory[i].Quantity, GroupedInventory[i].Item.Name, GroupedInventory[i].Item.Price);       
                }
            }
        }

        public void PrintTradeScreen(int x, int y, Action action)
        {
            int index = 0;
            y++;

            Console.SetCursorPosition(x, y);
            Console.WriteLine($"{0} | {1}",action.GetKeybind("weapons"), action.GetKeybind("misc"));
        }

        public void PrintItemOptions(int x, int y, Action action)
        {
            List<Item> filteredList = Inventory.Where(x => x.Type == Item.ItemType.Weapon).ToList();
            string name = filteredList[GameSession.itemChoice].Name;
            string description = filteredList[GameSession.itemChoice].Description;

            y++;
            Console.SetCursorPosition(x, y);
            Console.WriteLine($"- {name} -");
            y++;
            Console.SetCursorPosition(x, y);
            Console.WriteLine($"\"{description}\"");
            y++;
            Console.SetCursorPosition(x, y);
            Console.WriteLine($"{action.GetKeybind("equip")}) Equip");
            y++;
            Console.SetCursorPosition(x, y);
            Console.WriteLine($"{action.GetKeybind("drop")}) Drop");
            y++;
            Console.SetCursorPosition(x, y);
            Console.WriteLine($"{action.GetKeybind("cancel")}) Cancel");
        }

        public void PrintCurrentGold(int x, int y, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.WriteLine($"Gold: {Gold}");
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
