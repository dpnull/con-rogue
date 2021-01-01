using System;
using System.Collections.Generic;
using System.Text;
using con_rogue.Factory;
using System.Linq;

// TODO: FIX ITEM/WEAPON FACTORY

namespace con_rogue
{
    public class GameSession
    {
        // Property to hold player object
        private bool gameOver = false;
        public Player CurrentPlayer { get; set; }

        private Location _currentLocation;
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                GiveQuestsAtLocation();
                CompleteQuestsAtLocation();

            }
        }

        public World CurrentWorld { get; set; }

        public Action action { get; set; }
        public Action battleAction { get; set; }

        public Enemy CurrentEnemy { get; set; }

        public MessageLog messageLog { get; set; }

        public bool HasEnemy => CurrentEnemy != null;
        public bool InCombat = false;

        private GUI gui;
        public static ConsoleKeyInfo Input;
        public static int menuChoice;


        public GameSession()
        {
            // Create message log
            messageLog = new MessageLog();

            // Populate player object
            CurrentPlayer = new Player();

            // Create new GUI dimensions
            gui = new GUI(100, 30);

            // Create new WorldFactory
            WorldFactory worldFactory = new WorldFactory();

            // Put it into CurrentWorld property
            CurrentWorld = worldFactory.CreateWorld();
            CurrentLocation = CurrentWorld.GetLocation(0, 0);

            // Create new ActionFactory
            ActionFactory actionFactory = new ActionFactory();
            action = actionFactory.CreateAction();
            battleAction = actionFactory.CreateBattleAction();


            CurrentPlayer.WeaponInventory.Add(ItemFactory.CreateWeapon(2001));
            CurrentPlayer.ItemInventory.Add(ItemFactory.CreateItem(1001));

            CurrentPlayer.CurrentWeapon = CurrentPlayer.WeaponInventory.First();
        }

        public void Update(ConsoleKeyInfo input)
        {
            // Open Inventory
            if (input.KeyChar == action.GetKeybind("inventory") && !gui.GetTravelWindowState())
            {
                gui.OpenInventoryWindow();
            }
            // Inventory window logic
            if (gui.GetInventoryWindowState())
            {
                // Open Misc Tab
                if (input.KeyChar == action.GetKeybind("misc_tab"))
                {
                    gui.MiscVisibility(true);
                }
                // Open Weapons Tab
                if (input.KeyChar == action.GetKeybind("weapons_tab"))
                {
                    gui.WeaponsVisibility(true);
                }
                // Close Inventory
                if (input.KeyChar == action.GetKeybind("exit"))
                {
                    gui.CloseInventoryWindow();
                }
            }

            // Open Travel Menu
            if (input.KeyChar == action.GetKeybind("travel"))
            {
                gui.OpenTravelWindow();
            }
            // Close Travel Menu
            if (input.KeyChar == action.GetKeybind("exit") && gui.GetTravelWindowState())
            {
                gui.CloseTravelWindow();
            }
            // Commence X Travel
            if (char.IsDigit(input.KeyChar) && gui.GetTravelWindowState())
            {
                int choice = 0;
                choice = int.Parse(input.KeyChar.ToString()) - 1;
                for (int i = 0; i < CurrentWorld.locations.Count; i++)
                {
                    if (choice == CurrentWorld.locations[i].X && CurrentWorld.locations[i].Y == 0)
                    {
                        CurrentLocation = CurrentWorld.GetLocation(choice, 0);
                        messageLog.Add($"Travelling to {CurrentWorld.GetLocation(choice, 0).Name}...");
                        gui.CloseTravelWindow();
                    }
                }

            }
            // Battle
            if (InCombat)
            {
                if (input.KeyChar == battleAction.GetKeybind("attack") && battleAction.GetActionState("attack"))
                {
                    Attack();
                }
                if(input.KeyChar == action.GetKeybind("exit"))
                {
                    InCombat = false;
                }
            }

            // Location specific
            if(CurrentLocation.X == 1)
            {
                if (input.KeyChar == action.GetKeybind("blacksmith") && action.GetActionState("blacksmith"))
                {
                    CurrentLocation = CurrentWorld.GetLocation(1, 1);
                }
                if (input.KeyChar == action.GetKeybind("market") && action.GetActionState("market"))
                {
                    CurrentLocation = CurrentWorld.GetLocation(1, 2);
                }
            }

            if (CurrentLocation.X == 2)
            {
                if(input.KeyChar == action.GetKeybind("hunt") && action.GetActionState("hunt"))
                {
                    GetEnemyAtLocation();
                    InCombat = true;
                }
            }

            // Redraw
            gui.Render(this);
        }
        public void Attack()
        {
            if (CurrentPlayer.CurrentWeapon == null)
            {
                messageLog.Add("You must wield a weapon to attack!");
                return;
            }

            // If player has a weapon, get raw damage to enemy
            int damageDealt = RNG.Generator(CurrentPlayer.CurrentWeapon.MinDmg, CurrentPlayer.CurrentWeapon.MaxDmg);

            if (damageDealt == 0)
            {
                messageLog.Add("You have missed " + CurrentEnemy.Name + " !");

            }
            else
            {
                CurrentEnemy.Health -= damageDealt;
                messageLog.Add("You hit " + CurrentEnemy.Name + " for " + damageDealt + " damage!");
            }

            if (CurrentEnemy.Health <= 0)
            {
                KillReward();
                CurrentEnemy.Health = 0;
            }
            else
            {
                Defend();
            }
        }

        public void KillReward()
        {
            messageLog.Add("You have defeated " + CurrentEnemy.Name + " !");
            messageLog.Add("You gain " + CurrentEnemy.RewardExp + " experienece");
            messageLog.Add("You gain " + CurrentEnemy.RewardGold + " gold");
            messageLog.Add("Press [x] to exit...");
            foreach (ItemBundle itemBundle in CurrentEnemy.Inventory)
            {
                Item item = ItemFactory.CreateItem(itemBundle.ID);
                CurrentPlayer.AddItemToInventory(item);
                messageLog.Add("You obtain " + itemBundle.Quantity + " " + item.Name);
            }
            CurrentPlayer.Experience += CurrentEnemy.RewardExp;
            CurrentPlayer.Gold += CurrentEnemy.RewardGold;
        }

        public void Defend()
        {
            int damageReceived = RNG.Generator(CurrentEnemy.MinDmg, CurrentEnemy.MaxDmg);

            if (damageReceived == 0)
            {
                messageLog.Add(CurrentEnemy.Name + " misses!");
            }
            else
            {
                CurrentPlayer.Health -= damageReceived;
                messageLog.Add(CurrentEnemy.Name + " hits you for " + damageReceived + " damage!");
            }

            if (CurrentPlayer.Health <= 0)
            {
                Death();
            }
        }

        public void Death()
        {
            bool death = true;
            while (death)
            {
                messageLog.Add("You died...");
                Console.ReadKey();
            }

        }

        public void GiveQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailable)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));

                    messageLog.Add("");
                    messageLog.Add($"You receive the {quest.Name} quest!");
                    messageLog.Add($"Objective: {quest.Description}");
                }
            }
        }

        public void CompleteQuestsAtLocation()
        {
            foreach(Quest quest in CurrentLocation.QuestsAvailable)
            {
                // Get the first quest where the ID matches and has not yet been completed
                QuestStatus questToComplete = CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID && !q.IsCompleted);

                if(questToComplete != null)
                {
                    if (CurrentPlayer.HasRequiredItems(quest.ItemsToComplete))
                    {
                        foreach (ItemBundle itemBundle in quest.ItemsToComplete)
                        {
                            for (int i = 0; i < CurrentPlayer.ItemInventory.Count; i++)
                            {
                                CurrentPlayer.ItemInventory.Remove(CurrentPlayer.ItemInventory.First(item => item.ID == itemBundle.ID));
                            }
                        }

                        messageLog.Add($"{quest.Name} has been completed!");
                        messageLog.Add($"You receive {quest.RewardExp} experience and {quest.RewardGold} gold.");

                        foreach (ItemBundle itemBundle in quest.RewardItems)
                        {
                            Item rewardItem = ItemFactory.CreateWeapon(itemBundle.ID);

                            CurrentPlayer.AddItemToInventory(rewardItem);
                            messageLog.Add($"You receive {rewardItem.Name}.");
                        }

                        questToComplete.IsCompleted = true;
                    }
                }

            }
        }

        public void GetEnemyAtLocation()
        {
            CurrentEnemy = CurrentLocation.GetEnemy();
            messageLog.Add("-------------------------------------------");
            messageLog.Add($"You have encountered {CurrentEnemy.Name}!");

        }
    }
}
