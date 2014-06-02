namespace Program
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    public class MenuItem
    {
        private int x, y;
        private string name;
        private bool selected;
        /* properties */
        public int X
        {
            get { return this.x; }
        }

        public int Y
        {
            get { return this.y; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public bool Selected
        {
            get { return this.selected; }
            set { this.selected = value; }
        }

        public MenuItem(int x, int y, string name, bool selected)   // constructor
        {
            this.x = x;
            this.y = y;
            this.name = name;
            this.selected = selected;
        }
    }

    public class Menu
    {
        public static void Load(bool paused)
        {
            List<MenuItem> menuItems = new List<MenuItem>()
                                {   
                                    new MenuItem(54, 6, "New Game", false),                                    
                                    new MenuItem(54, 8, "High Scores", false),
                                    new MenuItem(54, 10, "Quit", false)    
                                };

            if (paused)
            {
                menuItems.Insert(0, new MenuItem(54, 4, "Resume", false));
            }

            int selectedItem = 0;
            menuItems[selectedItem].Selected = true;
            PrintLogo();
            Console.CursorVisible = false;
            Console.SetCursorPosition(45, 19);
            PrintMenu(menuItems);

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);

                    if (pressedKey.Key == ConsoleKey.UpArrow)
                    {
                        if (selectedItem > 0)
                        {
                            menuItems[selectedItem - 1].Selected = true;
                            menuItems[selectedItem].Selected = false;
                            selectedItem--;
                        }
                    }
                    else if (pressedKey.Key == ConsoleKey.DownArrow)
                    {
                        if (selectedItem < menuItems.Count - 1)
                        {
                            menuItems[selectedItem + 1].Selected = true;
                            menuItems[selectedItem].Selected = false;
                            selectedItem++;
                        }
                    }
                    else if (pressedKey.Key == ConsoleKey.Escape && paused)
                    {
                        break;  // return to game
                    }
                    else if (pressedKey.Key == ConsoleKey.Enter)
                    {
                        if (menuItems[selectedItem].Name == "Resume")
                        {
                            break;
                        }

                        if (menuItems[selectedItem].Name == "New Game")
                        {
                            Console.Clear();
                            Program.StartEngine();
                        }
                        
                        if (menuItems[selectedItem].Name == "High Scores")
                        {
                            // TODO: ScoreSystem
                            PrintLogo();
                        }

                        if (menuItems[selectedItem].Name == "Quit")
                        {
                            Exit();
                        }
                    }

                    Console.CursorVisible = false;
                    Console.SetCursorPosition(45, 19);
                    Console.Clear();
                    PrintLogo();
                    PrintMenu(menuItems);
                }
            }
        }

        public static void PrintLogo()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            try
            {
                using (StreamReader reader = new StreamReader(@"..\..\GameLogo.txt"))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
            catch (FileNotFoundException)
            {
                Console.SetCursorPosition(20, 10);
                Console.WriteLine("Mighty Hero");
            }
        }

        private static void PrintMenu(List<MenuItem> items)
        {
            foreach (var item in items)
            {
                if (item.Selected)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.SetCursorPosition(item.X, item.Y);
                Console.WriteLine(item.Name);
            }
        }

        private static void Exit()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(20, 8);
            Console.WriteLine("Developed by Team J. K. Rowling");
            Console.SetCursorPosition(20, 10);
            Console.WriteLine("Telerik Academy 2013/2014");
            Thread.Sleep(3000);
            Environment.Exit(0);
        }

        private static void InvalidInput()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(26, 14);
            Console.WriteLine("Invalid Dimantions");
            Thread.Sleep(1500);
            Console.Clear();
            Menu.Load(false);
        }
    }
}
