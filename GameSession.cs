using System;
using System.Collections.Generic;
using System.Text;
using con_rogue.Factory;
using System.Linq;


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

                CurrentTrader = CurrentLocation.TraderHere;
            }
        }

        public World CurrentWorld { get; set; }

        public Action action { get; set; }
        public Action battleAction { get; set; }

        public Enemy CurrentEnemy { get; set; }

        public static Trader CurrentTrader { get; set; }

        public MessageLog messageLog { get; set; }

        public bool HasEnemy => CurrentEnemy != null;
        public static bool HasTrader => CurrentTrader != null;
        public bool InCombat = false;

        public static bool CAN_OPEN_INVENTORY = true;
        public static bool CAN_TRADE = false;
        public static bool CAN_TRAVEL = false;

        private GUI gui;
        public static ConsoleKeyInfo Input;
        public static int menuChoice;
        public static int itemChoice;


        public GameSession()
        {
            // Create message log
            messageLog = new MessageLog();

            // Populate player object
            CurrentPlayer = new Player("Dom", 500, 500, 0, 1, 0);

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


            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(2001));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(2002));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(2003));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateItem(1001));

            CurrentPlayer.CurrentWeapon = ItemFactory.CreateItem(2001);
        }

        public void Update(ConsoleKeyInfo input)
        {
            // Open Inventory
            if (input.KeyChar == action.GetKeybind("inventory") && CAN_OPEN_INVENTORY)
            {
                gui.OpenInventoryWindow();
            }
            // Trade
            if (input.KeyChar == action.GetKeybind("trade") && CAN_TRADE)
            {
                gui.OpenTradeWindow();
            }
            // Trade window logic
            if (gui.GetTradeWindowState())
            {
                if (input.KeyChar == action.GetKeybind("trader_side"))
                {
                    gui.ChooseTraderTradeSide();
                }
                if (input.KeyChar == action.GetKeybind("player_side"))
                {
                    gui.ChoosePlayerTradeSide();
                }
                if (gui.GetPlayerTradeSide())
                {
                    SelectedItemAction(CurrentPlayer, input);
                }
                if (gui.GetTraderTradeSide())
                {
                    SelectedItemAction(CurrentLocation.TraderHere, input);
                }
                if (gui.GetItemSelectedState())
                {
                    if (input.KeyChar == action.GetKeybind("cancel"))
                    {
                        gui.CloseItemSelected();
                    }
                    if(input.KeyChar == action.GetKeybind("sell"))
                    {
                        SellSelectedItem();
                    }
                    if(input.KeyChar == action.GetKeybind("buy"))
                    {
                        BuySelectedItem();
                    }
                }

                // Exit
                if(input.KeyChar == action.GetKeybind("exit"))
                {
                    gui.CloseTradeWindow();
                }

            }
            // Inventory window logic
            if (gui.GetInventoryWindowState())
            {
                if (!gui.GetItemSelectedState())
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
                else
                {
                    // Equip selected weapon
                    if (input.KeyChar == action.GetKeybind("equip"))
                    {
                        // Filter and order every weapon type item
                        List<Item> filteredList = CurrentPlayer.Inventory.Where(x => x.Type == Item.ItemType.Weapon).ToList();
                        CurrentPlayer.CurrentWeapon = filteredList[itemChoice];
                        gui.CloseItemSelected();
                    }
                    // Close specific item option menu
                    if (input.KeyChar == action.GetKeybind("cancel"))
                    {
                        gui.CloseItemSelected();
                    }
                }
                SelectedItemAction(CurrentPlayer, input);
            }

            // Open Travel Menu
            if (input.KeyChar == action.GetKeybind("travel") && CAN_TRAVEL)
            {
                gui.OpenTravelWindow();
            }
            // Close Travel Menu
            if (input.KeyChar == action.GetKeybind("exit") && CAN_TRAVEL)
            {
                gui.CloseTravelWindow();
            }
            // Commence X Travel
            if (char.IsDigit(input.KeyChar) && gui.GetTravelWindowState()) // GetTravelWindow state shouldn't be here
            if (char.IsDigit(input.KeyChar) && CAN_TRAVEL)
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

            // Location specific 
            if (CurrentLocation.X == 1)
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
                if (input.KeyChar == action.GetKeybind("hunt") && action.GetActionState("hunt") && InCombat != true)
                {
                    GetEnemyAtLocation();
                    gui.OpenBattleWindow();
                    InCombat = true;
                }
            }

            // Battle system
            if (InCombat)
            {
                if (input.KeyChar == battleAction.GetKeybind("attack") && battleAction.GetActionState("attack") && CurrentEnemy.Health > 0)
                {
                    Attack();
                }
                if (input.KeyChar == action.GetKeybind("exit") && CurrentEnemy.Health <= 0)
                {
                    if (CurrentEnemy.Health <= 0)
                    {
                        InCombat = false;
                        gui.CloseBattleWindow();
                    }
                }

            }

            // Redraw
            gui.Render(this);
        }

        public void SelectedItemAction(Entity entity, ConsoleKeyInfo input)
        {
            if (char.IsDigit(input.KeyChar))
            {
                itemChoice = 0;
                itemChoice = int.Parse(input.KeyChar.ToString()) - 1;

                for (int i = 0; i < entity.GroupedInventory.Count; i++)
                {
                    if (itemChoice == i)
                    {
                        gui.OpenItemSelected();
                    }
                }
            }
        }

        public void SellSelectedItem()
        {
            var toSell = CurrentPlayer.GroupedInventory[itemChoice];
            CurrentPlayer.AddGold(toSell.Item.Price);
            CurrentLocation.TraderHere.AddItemToInventory(toSell.Item);
            CurrentPlayer.RemoveItemFromInventory(toSell);
        }

        public void BuySelectedItem()
        {
            var toBuy = CurrentLocation.TraderHere.GroupedInventory[itemChoice];

            CurrentPlayer.AddItemToInventory(toBuy.Item);
            CurrentLocation.TraderHere.RemoveItemFromInventory(toBuy);
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
                CurrentEnemy.TakeDamage(damageDealt);
                messageLog.Add("You hit " + CurrentEnemy.Name + " for " + damageDealt + " damage!");
            }

            if (CurrentEnemy.Health <= 0)
            {
                KillReward();
                CurrentEnemy.SetHealth(0);
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
            messageLog.Add("You gain " + CurrentEnemy.Gold + " gold");
            messageLog.Add("Press [x] to exit...");
            foreach (Item item in CurrentEnemy.Inventory)
            {
                CurrentPlayer.AddItemToInventory(item);
                messageLog.Add("You obtain " + item.Name);
            }
            CurrentPlayer.Experience += CurrentEnemy.RewardExp;
            CurrentPlayer.AddGold(CurrentEnemy.Gold);
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
                CurrentPlayer.TakeDamage(damageReceived);
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
            foreach (Quest quest in CurrentLocation.QuestsAvailable)
            {
                // Get the first quest where the ID matches and has not yet been completed
                QuestStatus questToComplete = CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID && !q.IsCompleted);

                if (questToComplete != null)
                {
                    if (CurrentPlayer.HasRequiredItems(quest.ItemsToComplete))
                    {
                        foreach (ItemBundle itemBundle in quest.ItemsToComplete)
                        {
                            for (int i = 0; i < itemBundle.Quantity; i++)
                            {
                                CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.GroupedInventory.First(item => item.Item.ID == itemBundle.ID));
                            }
                        }

                        messageLog.Add($"{quest.Name} has been completed!");
                        messageLog.Add($"You receive {quest.RewardExp} experience and {quest.RewardGold} gold.");
                        CurrentPlayer.AddGold(quest.RewardGold);
                        CurrentPlayer.Experience += quest.RewardExp;

                        foreach (ItemBundle itemBundle in quest.RewardItems)
                        {
                            Item rewardItem = ItemFactory.CreateItem(itemBundle.ID);

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
