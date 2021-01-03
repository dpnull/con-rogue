using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

namespace con_rogue
{
    public class GUI
    {
        public int Width;
        public int Height;

        private bool travelWindow = false;
        private bool inventoryWindow = false;
        private bool mainWindow = true;
        private bool battleWindow = false;

        // Lazy bools for inventory
        private bool miscTab = false;
        private bool weaponsTab = false;
        private bool itemSelected = false;

        public GUI()
        {
            Width = Console.WindowWidth;
            Height = Console.WindowHeight;
        }

        public GUI(int width, int height)
        {
            // Unsure about this yet
            Width = width;
            Height = height;
        }

        public void Render(GameSession gameSession)
        {
            Console.Clear();

            Width = Console.WindowWidth;
            Height = Console.WindowHeight;

            // variables for window sizes
            int statsH = 3;
            int statsY = 0;
            int locH = 3;
            int locY = statsY + statsH;
            int invH = 6;
            int invY = locY;
            int trvlH = 10;
            int trvlY = invY + locY;


            if (gameSession.InCombat)
            {
                battleWindow = true;
                BattleActionManagement(gameSession.battleAction);
                DrawBattleWindow(gameSession.battleAction, gameSession.CurrentPlayer, gameSession.CurrentEnemy);
                DrawMessageLog(gameSession.messageLog, 0, Height / 2, Width, (Height / 2));
            }
            else if (!gameSession.InCombat)
            {
                // Draw main screen which consists of:
                // Player Display, Current Location, Action Menu, Message Log
                // When Inventory is displayed: hide Current Location
                // When Travel is displayed: hide all but Action Menu, Player Stats, Location Menu, (maybe Message Log Too)
                DrawPlayerStats(gameSession.CurrentPlayer, statsH);

                if (!inventoryWindow)
                {
                    DrawPlayerLocation(gameSession.CurrentLocation, locY, locH);
                }
                else
                {
                    DrawInventory(gameSession.CurrentPlayer, gameSession.action, invY, invH);
                }

                if (travelWindow)
                {
                    DrawAvailableLocations(gameSession.CurrentWorld, trvlY, trvlH);
                }

                // Message log
                DrawMessageLog(gameSession.messageLog, 0, Height - 10, Width - 30, 8);

                // Auto hiding of actions and drawing them onto the screen
                ActionManagement(gameSession.action, gameSession.CurrentLocation);
                DrawActions(gameSession.action);
            }

            // Battle window. Only initiated when there's an existing enemy.
            // When Battle window is displayed: Hide all but message log and expand message log (or alternatively create new message log)




            // When inventory is open, hide location [TODO]


            /*
            MainWindowAutoVisiblity();
            // Draw MessageLog
            DrawMessageLog(gameSession.messageLog);
            if (gameSession.HasEnemy)
            {
                battleWindow = true;
                BattleActionManagement(gameSession.battleAction);
                DrawBattleWindow(gameSession.battleAction, gameSession.CurrentPlayer);
            }
            if (!battleWindow)
            {
                ActionManagement(gameSession.action, gameSession.CurrentLocation);
                DrawActions(gameSession.action);


                if (!travelWindow)
                {
                    // Draw Player Stats
                    DrawPlayerStats(gameSession.CurrentPlayer, 1);
                    // Draw Player Location
                    DrawPlayerLocation(gameSession.CurrentLocation, 1);
                    // Draw Player Quests
                    // DrawQuests(gameSession.CurrentPlayer);

                }
                // Draw Action Window   
                if (travelWindow)
                {
                    DrawAvailableLocations(gameSession.CurrentWorld, gameSession.CurrentWorld.GetLocationCount());
                }
                if (inventoryWindow)
                {
                    // Draw Player Inventory
                    DrawInventory(gameSession.CurrentPlayer);
                }
            }
            */

        }

        // Draw player stats
        public void DrawPlayerStats(Player player, int height)
        {
            string stats = string.Format(" Health: {0} | Exp: {1} | Level: {2} | Gold: {3} | Weapon: {4}",
                player.Health, player.Experience, player.Level, player.Gold, player.CurrentWeapon.Name);
            string header = string.Format(" STATS ", player.Name);

            DrawBox(0, 0, Width - 30, height, header, false, false);
            Console.SetCursorPosition(1, 1);
            Console.Write(stats);
        }
        // Draw the current location of player
        public void DrawPlayerLocation(Location location, int y, int height)
        {
            int locWidth = Width - 30;
            int locHeight = 3;
            string header = string.Format(" {0} ", location.Name.ToUpper());
            DrawBox(0, y, locWidth, locHeight, header, false, false);
            Console.SetCursorPosition(1, 4);
            Console.WriteLine(" {0}", location.Description);
        }

        public void BattleActionManagement(Action action)
        {
            if (battleWindow)
            {
                action.MakeVisible("attack");
                action.MakeVisible("defend");
                action.MakeVisible("retreat");
            }
            else
            {
                action.MakeHidden("attack");
                action.MakeHidden("defend");
                action.MakeHidden("retreat");
            }
        }

        public void ActionManagement(Action action, Location loc)
        {
            // Main window actions
            action.AutoVisibility("inventory", !GetInventoryWindowState() && !GetTravelWindowState());
            action.AutoVisibility("travel", !GetInventoryWindowState() && !GetTravelWindowState());
            // Inventory window actions
            if (!GetTravelWindowState()) // Travel window must be off
            {
                action.AutoVisibility("exit", GetInventoryWindowState());
                action.AutoVisibility("misc_tab", GetInventoryWindowState());
                action.AutoVisibility("weapons_tab", GetInventoryWindowState());
            }
            // Travel window actions
            if (!GetInventoryWindowState()) // Inventory must be off
            {
                action.AutoVisibility("exit", GetTravelWindowState());
                action.AutoVisibility("travel", !GetTravelWindowState());
            }
            // Location exclusive actions
            if(loc.X == 1)
            {
                action.AutoVisibility("blacksmith", !GetInventoryWindowState() && !GetTravelWindowState());
                action.AutoVisibility("market", !GetInventoryWindowState() && !GetTravelWindowState());
            }
            if(loc.X == 2)
            {
                action.AutoVisibility("hunt", !GetInventoryWindowState() && !GetTravelWindowState());
                action.AutoVisibility("gather", !GetInventoryWindowState() && !GetTravelWindowState());
            }
        
        }

        public void DrawActions(Action action)
        {
            DrawBox(Width - 30, 0, 30, Height - 2, " ACTIONS ", true, false);
            action.PrintActions(Width - 29, 0);
        }

        public void DrawInventory(Player player, Action action, int y, int height)
        {
            // can probably be improved upon
            DrawBox(0, y, Width - 30, height, " INVENTORY ", false, false);
            if (miscTab)
            {
                player.PrintMisc(2, y);
            }
            if (weaponsTab)
            {
                player.PrintWeapons(2, y);
            }
            if (itemSelected)
            {
                DrawBox(0, y + height, 32, 7, null, false, false);
                player.PrintItemOptions(2, y + height, action);
            }
        }

        // Draw all available locations at that moment
        public void DrawAvailableLocations(World world, int y, int height)
        {
            DrawBox(0, y, Width - 30, height, "AVAILABLE LOCATIONS", false, false);
            world.PrintGuiLocations(1, y);


            // GameSession.menuChoice = MultipleChoice(true, world, xpos+1, ypos+1);
        }

        public void DrawMessageLog(MessageLog messageLog, int x, int y, int w, int h)
        {

            DrawBox(x, y, w, h, " MESSAGES ", true, false);
            messageLog.Draw(x + 1, y + 1);
        }

        public void DrawBattleWindow(Action action, Player player, Enemy enemy)
        {
            int xpos = 0;
            int ypos = 0;
            DrawBox(xpos, ypos, Width, Height / 2, " BATTLE ", true, true);
            DrawBox(xpos + 1, ypos + 1, Width / 2 - 9, (Height / 2) - 2, "PLAYER", false, true);
            player.PrintBattleStats((Width / 8), (Height / 4) - 1);
            DrawBox((Width / 2) + 8, ypos + 1, Width / 2 - 9, (Height / 2) - 2, "ENEMY", false, true);
            enemy.PrintBattleStats(Width - (Width / 4) - 7, (Height / 4) - 1);
            action.PrintActions(Width / 2 - 5, 2);

            // enemy.DrawEnemy(xpos, ypos);

        }

        public bool GetBattleWindowState()
        {
            return battleWindow;
        }

        public void OpenBattleWindow()
        {
            battleWindow = true;
        }

        public void CloseBattleWindow()
        {
            battleWindow = false;
        }

        public void MainWindowAutoVisiblity()
        {
            if (travelWindow == true || inventoryWindow == true)
            {
                mainWindow = false;
            }
            else if (!travelWindow || !inventoryWindow)
            {
                mainWindow = true;
            }
        }
        // Make the travel window visible and turn off other windows
        public void OpenTravelWindow()
        {
            travelWindow = true;
        }
        // Make the travel window invisible and enable other windows back again
        public void CloseTravelWindow()
        {
            travelWindow = false;
        }
        // Check whether travel window is visible or not
        public bool GetTravelWindowState()
        {
            return travelWindow;
        }
        public void OpenInventoryWindow()
        {
            inventoryWindow = true;
        }
        public void CloseInventoryWindow()
        {
            inventoryWindow = false;
        }
        public bool GetInventoryWindowState()
        {
            return inventoryWindow;
        }
        public void OpenItemSelected()
        {
            itemSelected = true;
        }
        public void CloseItemSelected()
        {
            itemSelected = false;
        }
        public bool GetItemSelectedState()
        {
            return itemSelected;
        }
        public void OpenMainWindow()
        {
            mainWindow = true;
        }
        public void CloseMainWindow()
        {
            mainWindow = false;
        }
        public bool GetMainWindowState()
        {
            return mainWindow;
        }

        public void WeaponsVisibility(bool isVisible)
        {
            weaponsTab = isVisible;
            miscTab = false;
        }


        public void MiscVisibility(bool isVisible)
        {
            miscTab = isVisible;
            weaponsTab = false;
        }

        public void DrawQuests(Player player)
        {
            DrawBox(0, 11, Width - 30, 10, "QUESTS ", false, false);
            player.PrintQuests(2, 11);
        }

        // Obsolete until I figure out how to filter and order only locations where Y is 0

        /*
        public int MultipleChoice(bool canCancel, World world, int x, int y)
        {
            const int optionsPerLine = 3;
            const int spacingPerLine = 10;

            int currentSelection = 0;

            ConsoleKey key = GameSession.cki;

            Console.CursorVisible = false;

            do
            {

                for (int i = 0; i < world.locations.Count; i++)
                {
                    Console.SetCursorPosition(x + (i % optionsPerLine) * spacingPerLine, y + i / optionsPerLine);

                    if (i == currentSelection)
                        Console.ForegroundColor = ConsoleColor.Red;

                    if(world.locations[i].Y == 0)
                    {
                        Console.Write(world.locations[i].Name);
                    }
                    
                    Console.ResetColor();
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        {
                            if (currentSelection % optionsPerLine > 0)
                                currentSelection--;
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            if (currentSelection % optionsPerLine < optionsPerLine - 1)
                                currentSelection++;
                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            if (currentSelection >= optionsPerLine)
                                currentSelection -= optionsPerLine;
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            if (currentSelection + optionsPerLine < world.locations.Count)
                                currentSelection += optionsPerLine;
                            break;
                        }
                    case ConsoleKey.X:
                        {
                            if (canCancel)
                            {
                                return -1;
                            }        
                            break;
                        }
                }
            } while (key != ConsoleKey.Enter);

            Console.CursorVisible = true;
            return currentSelection;
        }
        */

        public int DrawBox(int _x, int _y, int _width, int _height, string _title, bool _bold, bool _centered_title)
        {
            int result = 0;
            int x = _x;
            int y = _y;
            char h = '═';
            char v = '║';
            char tl = '╔';
            char bl = '╚';
            char tr = '╗';
            char br = '╝';

            if (!_bold)
            {
                h = '─';
                v = '│';
                tl = '┌';
                bl = '└';
                tr = '┐';
                br = '┘';
            }

            //Top
            for (x = _x; x < (_x + _width); x++)
            {
                try
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(h);
                }
                catch { result = -1; }
            }
            //Bottom
            y = _y + _height - 1;
            for (x = _x; x < (_x + _width); x++)
            {
                try
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(h);
                }
                catch { result = -1; }
            }
            //Left
            x = _x;
            for (y = _y; y < (_y + _height); y++)
            {
                try
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(v);
                }
                catch { result = -1; }
            }
            //Right
            x = _x + _width - 1;
            for (y = _y; y < (_y + _height - 1); y++)
            {
                try
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(v);
                }
                catch { result = -1; }
            }

            try
            {
                //Bottom-Left
                Console.SetCursorPosition(_x, _y + _height - 1);
                Console.Write(bl);
                //Bottom-Right
                Console.SetCursorPosition(_x + _width - 1, _y + _height - 1);
                Console.Write(br);
                //Top-Right
                Console.SetCursorPosition(_x + _width - 1, _y);
                Console.Write(tr);
                //Top-Left
                Console.SetCursorPosition(_x, _y);
                Console.Write(tl);
            }
            catch { result = -1; }


            if (_centered_title)
            {
                Console.SetCursorPosition((_x + _width / 2) - (_title.Length / 2), _y);
                Console.Write(_title);
            }
            else
            {
                Console.SetCursorPosition(_x + 1, _y);
                Console.Write(_title);
            }



            return result;

        }


    }
}
