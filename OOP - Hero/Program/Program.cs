namespace Program
{
    using System;
    using System.Collections.Generic;  // for delays
    using System.IO;  // for first screen delay
    using System.Threading;
    using Character;

    public class Program
    {
        private const int WORLDROWS = 45;
        private const int WORLDCOLS = 100;
        private const int LENGTHOFLONGESTPLAYERBODY = 3;
        private const int DISTANCEKNIGHT = 10; // distance from the knight in the begin
        private const int DISTANCECREATURE = 3; // distance between the creatures in the begin
        private static List<int> mapCoordinate = new List<int>(); // keeps initial coordinates of all units: row = odd, col = even; 

        public static void Main()
        {
            Menu.Load(false);
        }

        public static void StartEngine()
        {
            SetUpConsoleDimentions();

            FirstScreen();

            var renderer = new ConsoleRenderer(WORLDROWS, WORLDCOLS);
            var userInterface = new KeyboardInterface();

            Engine gameEngine = Engine.EngineInstance(renderer, userInterface); // singleton
            //Engine gameengine2 = Engine.EngineInstance(renderer, userInterface); // every next time EngineInstance is called it returns null

            Initialize(gameEngine);
                        
            gameEngine.Run();
        }

        private static void FirstScreen()
        {
            int centerWidth = Console.WindowWidth / 2;
            int centerHeight = Console.WindowHeight / 2;
            Console.ForegroundColor = ConsoleColor.Red;

            StreamReader reader = new StreamReader(@"..\..\FirstScreen.txt");

            try
            {
                (new System.Media.SoundPlayer(@"..\..\IntroGame.wav")).Play();
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("File was not found!");
            }

            string line = reader.ReadLine();

            int lineNumber = 1;
            while (line != null)
            {
                Console.SetCursorPosition(0, centerHeight - 12 + lineNumber);
                Console.WriteLine(line);
                lineNumber++;
                line = reader.ReadLine();
            }

            Thread.Sleep(1000);

            Console.ForegroundColor = ConsoleColor.White;

            Console.ReadKey();
            Console.Clear();
            new System.Media.SoundPlayer(@"..\..\IntroGame.wav").Stop();
        }

        private static void SetUpConsoleDimentions()
        {
            Console.SetBufferSize(WORLDCOLS, WORLDROWS);

            Console.SetWindowPosition(0, 0);

            Console.SetWindowSize(WORLDCOLS, WORLDROWS + LENGTHOFLONGESTPLAYERBODY);
            Console.WindowHeight = WORLDROWS + LENGTHOFLONGESTPLAYERBODY;
            Console.WindowWidth = WORLDCOLS + 30;
        }

        private static void Initialize(Engine engine)
        {
            // int startRow = 5;
            // int startCol = 5;

            Character playerKnight = new Knight("Goliath", "Bla bla bla", new MatrixCoords(RowIni(), ColIni()));
            Character creature1 = new Orc("Generic orc", new MatrixCoords(RowIni(), ColIni()));
            Character creature2 = new Ninja("Generic ninja", new MatrixCoords(RowIni(), ColIni()));
            Character creature3 = new Bat("Generic bat", new MatrixCoords(RowIni(), ColIni()));
            Character creature4 = new Rat("Generic rat", new MatrixCoords(RowIni(), ColIni()));
            //Character creature5 = new Boss("Bad boss", new MatrixCoords(RowIni(), ColIni()));

            engine.AddObject(playerKnight);
            engine.AddObject(creature1);
            engine.AddObject(creature2);
            engine.AddObject(creature3);
            engine.AddObject(creature4);
            
            //engine.AddObject(creature5);
        }

        private static int RowIni()  // random generated row method
        {
            int row = 0;
            bool placeWrong = false;
            Random randomGenerator = new Random();
            do
            {
                placeWrong = false;
                row = randomGenerator.Next(0, WORLDROWS - 5);
                if (mapCoordinate.Count > 0)
                {
                    if ((int)Math.Abs(mapCoordinate[0] - row) < DISTANCEKNIGHT)
                    {
                        placeWrong = true;
                    }

                    for (int i = 2; i < mapCoordinate.Count; i = i + 2)
                    {
                        if ((int)Math.Abs(mapCoordinate[i] - row) < DISTANCECREATURE)
                        {
                            placeWrong = true;
                        }
                    }
                }
            }
            while (placeWrong);
            mapCoordinate.Add(row);
            return row;
        }

        private static int ColIni() // random generated col method
        {
            int col = 0;
            bool placeWrong = false;
            Random randomGenerator = new Random();
            do
            {
                placeWrong = false;
                col = randomGenerator.Next(0, WORLDCOLS - 5);
                if (mapCoordinate.Count > 1)
                {
                    if ((int)Math.Abs(mapCoordinate[1] - col) < DISTANCEKNIGHT)
                    {
                        placeWrong = true;
                    }

                    for (int i = 3; i < mapCoordinate.Count; i = i + 2)
                    {
                        if ((int)Math.Abs(mapCoordinate[i] - col) < DISTANCECREATURE)
                        {
                            placeWrong = true;
                        }
                    }
                }
            }
            while (placeWrong);
            mapCoordinate.Add(col);
            return col;
        }
    }
}