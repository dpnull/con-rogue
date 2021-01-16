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

        public static MessageLog messageLog { get; set; }

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

            CurrentPlayer.CurrentWeapon = CurrentPlayer.Inventory.Where(i => i.Type == Item.ItemType.Weapon).FirstOrDefault();
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
                // Switch between player and trader window
                if (input.KeyChar == action.GetKeybind("trader_side") || input.Key == ConsoleKey.RightArrow)
                {
                    gui.ChooseTraderTradeSide();
                    itemChoice = 0;
                }
                if (input.KeyChar == action.GetKeybind("player_side") || input.Key == ConsoleKey.LeftArrow)
                {
                    gui.ChoosePlayerTradeSide();
                    itemChoice = 0;
                }
                // Move up and down on the list
                if (input.Key == ConsoleKey.UpArrow && itemChoice > 0)
                {
                    itemChoice--;
                }
                if (input.Key == ConsoleKey.DownArrow)
                {
                    if (gui.GetTraderTradeSide() && itemChoice < CurrentTrader.Inventory.Count - 1)
                    {
                        itemChoice++;
                    }
                    else if (gui.GetPlayerTradeSide() && itemChoice < CurrentPlayer.Inventory.Count - 1)
                    {
                        itemChoice++;
                    }
                    
                }
                // Select item from player and trader window
                if (gui.GetPlayerTradeSide())
                {
                    SelectedItemAction(CurrentPlayer, input);
                }
                if (gui.GetTraderTradeSide())
                {
                    SelectedItemAction(CurrentLocation.TraderHere, input);
                }
                // Selected item actions
                if (gui.GetItemSelectedState())
                {
                    // Visible on both windows
                    if (input.KeyChar == action.GetKeybind("cancel"))
                    {
                        gui.CloseItemSelected();
                    }
                    // Visible on player window
                    if(input.KeyChar == action.GetKeybind("sell") && gui.GetPlayerTradeSide())
                    {
                        SellSelectedItem();
                    }
                    // Visible on trader window
                    if(input.KeyChar == action.GetKeybind("buy") && gui.GetTraderTradeSide())
                    {
                        BuySelectedItem();
                    }
                }

                // Exit
                if (input.KeyChar == action.GetKeybind("exit") && !gui.GetItemSelectedState())
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
                        EquipWeapon();
                        gui.CloseItemSelected();
                    }
                    // Remove item from inventory
                    if(input.KeyChar == action.GetKeybind("drop"))
                    {
                        DropItem();
                    }
                    // Close specific item option menu
                    if (input.KeyChar == action.GetKeybind("cancel"))
                    {
                        gui.CloseItemSelected();
                    }
                }
                SelectedItemAction(CurrentPlayer, input);
            }

            if (CAN_TRAVEL)
            {
                // Open Travel Menu
                if (input.KeyChar == action.GetKeybind("travel"))
                {
                    gui.OpenTravelWindow();
                }
                // Close Travel Menu
                if (input.KeyChar == action.GetKeybind("exit"))
                {
                    gui.CloseTravelWindow();
                }
                // Commence X Travel
                if (char.IsDigit(input.KeyChar) && gui.GetTravelWindowState()) // GetTravelWindow state shouldn't be here
                    if (char.IsDigit(input.KeyChar))
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
            }


            // Location specific 
            if (CAN_TRAVEL)
            {
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
            }


            // Battle system
            if (InCombat)
            {
                if (input.KeyChar == battleAction.GetKeybind("attack") && battleAction.GetActionState("attack") && CurrentEnemy.Health > 0)
                {
                    CurrentPlayer.UseCurrentWeaponOn(CurrentEnemy);
                    CurrentEnemy.UseCurrentWeaponOn(CurrentPlayer);
                    KillReward();
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
            if(itemChoice >= 0 && itemChoice < entity.Inventory.Count) {
                if (input.Key == ConsoleKey.Enter)
                {
                    gui.OpenItemSelected();
                }
                else if (char.IsDigit(input.KeyChar))
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

        }

        public void EquipWeapon()
        {
            // Filter and order every weapon type item
            List<Item> filteredList = CurrentPlayer.Inventory.Where(x => x.Type == Item.ItemType.Weapon).ToList();
            CurrentPlayer.CurrentWeapon = filteredList[itemChoice];
        }

        public void DropItem()
        {
            if (itemChoice >= 0 && itemChoice < CurrentPlayer.Inventory.Count)
            {
                var toDrop = CurrentPlayer.Inventory[itemChoice];
                CurrentPlayer.RemoveItemFromInventory(toDrop);
            }
               
        }

        public void SellSelectedItem()
        {
            if(itemChoice >= 0 && itemChoice < CurrentPlayer.Inventory.Count)
            {
                var toSell = CurrentPlayer.Inventory[itemChoice];
                CurrentPlayer.AddGold(toSell.Price);
                CurrentLocation.TraderHere.AddItemToInventory(toSell);
                CurrentPlayer.RemoveItemFromInventory(toSell);
            }
            else
            {
                gui.CloseItemSelected();
            }
        }

        public void BuySelectedItem()
        {
            if (itemChoice <= CurrentTrader.Inventory.Count)
            {
                var toBuy = CurrentTrader.Inventory[itemChoice];

                if(CurrentPlayer.Gold < toBuy.Price)
                {
                    messageLog.Add($"You don't have enough gold to buy {toBuy.Name}!");
                }
                else
                {
                    CurrentPlayer.AddItemToInventory(toBuy);
                    CurrentLocation.TraderHere.RemoveItemFromInventory(toBuy);
                }
            }
            else
            {
                gui.CloseItemSelected();
            }

        }

        public void Attack()
        {

        }

        public void KillReward()
        {
            if(CurrentEnemy.Health <= 0)
            {
                messageLog.Add($"You defeat {CurrentEnemy.Name.ToLower()}");
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
                                CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.Inventory.First(item => item.ID == itemBundle.ID));
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

/*
[ Character Info / Health / Weapon ] [ ACTIONS ]
[










*/