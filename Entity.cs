using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace con_rogue
{
    // Abstract cannot be instantiated but the child of this class can
    public abstract class Entity
    {
        private string _name;
        private int _maxHealth;
        private int _health;
        private int _gold;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int MaxHealth
        {
            get { return _maxHealth; }
            private set { _maxHealth = value; }
        }
        public int Health
        {
            get { return _health; }
            private set { _health = value; }
        }
        public int Gold
        {
            get { return _gold; }
            private set { _gold = value; }
        }

        public bool IsDead => Health <= 0;

        public List<Item> Inventory { get; set; }
        // Hold items that can be stacked
        public List<GroupedInventoryItem> GroupedInventory { get; set; }

        // 
        public List<Item> Weapons => Inventory.Where(i => i.Type == Item.ItemType.Weapon).ToList();

        // Only a child class can use the constructor
        protected Entity(string name, int maxHealth, int health, int gold)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = health;
            Gold = gold;
            Inventory = new List<Item>();
            GroupedInventory = new List<GroupedInventoryItem>();
        }

        public void TakeDamage(int d)
        {
            Health -= d;

            if (IsDead)
            {
                Health = 0;
            }

        }

        public void Heal(int h)
        {
            Health += h;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
        }

        public void SetHealth(int h)
        {
            Health = h;
        }

        public void AddGold(int g)
        {
            Gold += g;
        }

        public void RemoveGold(int g)
        {
            Gold -= g;
        }

        public void PrintItemSellOptions(int x, int y, Action action, bool isSelling)
        {
            // TODO: fix out of bounds array when 0 is inputted
            // List<Item> filteredList = Inventory.Where(x => x.Type == Item.ItemType.Weapon).ToList();
            if (GameSession.itemChoice < GroupedInventory.Count())
            {
                string name = GroupedInventory[GameSession.itemChoice].Item.Name;
                string description = GroupedInventory[GameSession.itemChoice].Item.Description;
                int value = GroupedInventory[GameSession.itemChoice].Item.Price;

                y++;
                Console.SetCursorPosition(x, y);
                Console.Write($"- {name} -");
                y++;
                Console.SetCursorPosition(x, y);
                Console.Write($"\"{description}\"");
                y++;
                Console.SetCursorPosition(x, y);
                Console.Write($"Value: {value}");
                if (isSelling)
                {
                    y++;
                    Console.SetCursorPosition(x, y);
                    Console.Write($"{action.GetKeybind("sell")}) Sell");
                } else
                {
                    y++;
                    Console.SetCursorPosition(x, y);
                    Console.Write($"{action.GetKeybind("buy")}) Buy");
                }
                y++;
                Console.SetCursorPosition(x, y);
                Console.Write($"{action.GetKeybind("cancel")}) Cancel");
            }
        }

        public void PrintItems(int x, int y, int windowWidth, ConsoleColor color, bool focused)
        {
            int index = 0;
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y + 1);
            Console.Write("[[NAME]]                             [[PRICE]]");
            y++;

            for(int i = 0; i < GroupedInventory.Count; i++)
            {
                if (GroupedInventory[i].Item!=null)
                {
                    if (!GroupedInventory[i].Item.IsUnique)
                    {
                        index++;
                        y++;
                        Console.SetCursorPosition(x, y);
                        Console.Write($"{index}) {GroupedInventory[i].Item.Name} Quantity:{GroupedInventory[i].Quantity}");
                        Console.SetCursorPosition(x + windowWidth - 9, y);
                        Console.Write($"{GroupedInventory[i].Item.Price}");
                    }
                    else
                    {
                        index++;
                        y++;
                        Console.SetCursorPosition(x, y);
                        Console.Write($"{index}) {GroupedInventory[i].Item.Name}");
                        Console.SetCursorPosition(x + windowWidth - 9, y);
                        Console.Write($"{GroupedInventory[i].Item.Price}");
                    }
                    // Very messy, needs to be redone
                    if(GameSession.itemChoice >= 0 && GameSession.itemChoice < Inventory.Count)
                    {
                        if (Inventory[GameSession.itemChoice] == Inventory[i] && focused)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(x, y);

                            if (!GroupedInventory[i].Item.IsUnique)
                            {
                                Console.Write($"{index}) {GroupedInventory[i].Item.Name} Quantity:{GroupedInventory[i].Quantity}");
                            }
                            else
                            {
                                Console.Write($"{index}) {GroupedInventory[i].Item.Name}");
                            }
                            Console.SetCursorPosition(x + windowWidth - 9, y);
                            Console.Write($"{GroupedInventory[i].Item.Price}");
                            Console.ForegroundColor = color;
                        }
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void AddItemToInventory(Item item)
        {
            Inventory.Add(item);

            // if item is unique, create a stack with single item inside of it
            if (item.IsUnique)
            {
                GroupedInventory.Add(new GroupedInventoryItem(item, 1));
            }
            // if it isn't, loop through every item using id to see if passed id is equal then skip. if not, create the stack and add the item to stack inventory
            else
            {
                if (!GroupedInventory.Any(gi => gi.Item.ID == item.ID))
                {
                    GroupedInventory.Add(new GroupedInventoryItem(item, 0));
                }

                GroupedInventory.First(gi => gi.Item.ID == item.ID).Quantity++;

            }
        }


        public void RemoveItemFromInventory(Item item)
        {
            Inventory.Remove(item);

            // Check if item is unique
            // If unique, return the exact matching item
            // If not unique, return first or default grouped item with same item id in it
            GroupedInventoryItem stackedItemToRemove = item.IsUnique ?
                GroupedInventory.FirstOrDefault(gi => gi.Item == item) :
                GroupedInventory.FirstOrDefault(gi => gi.Item.ID == item.ID);

            if (stackedItemToRemove != null)
            {
                if (stackedItemToRemove.Quantity == 1)
                {
                    GroupedInventory.Remove(stackedItemToRemove);
                }
                else
                {
                    stackedItemToRemove.Quantity--;
                }
            }
        }

    }
}
