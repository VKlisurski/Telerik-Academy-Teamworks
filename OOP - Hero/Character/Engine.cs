namespace Character
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Linq;
    using System.IO;

    public class Engine
    {
        private static volatile Engine instance = null;

        private readonly int enemySpead = 20;
        private IRenderer renderer;
        private IUserInterface userInterface;
        private List<Character> charackters;
        private Knight playerKnight;
        private int hitPoints = 1;
        private bool isBossAdded = false;

        private Engine(IRenderer renderer, IUserInterface userInterface)
        {
            this.renderer = renderer;
            this.userInterface = userInterface;
            this.charackters = new List<Character>();
        }

        public static Engine EngineInstance(IRenderer renderer, IUserInterface userInterface) // private method that returnn only one instance of the game engine
        {
            if (instance == null) // Singleton
            {
                lock (typeof(Engine)) // check lock
                {
                    instance = new Engine(renderer, userInterface);
                    return instance;
                }
            }
            return null;
        }

        public virtual void AddObject(Character obj)
        {
            if (obj is Knight)
            {
                AddPlayer(obj);
            }
            this.charackters.Add(obj);
        }

        public virtual void AddPlayer(Character obj)
        {
            this.playerKnight = obj as Knight;
        }

        public virtual void MovePlayerLeft()
        {
            if (this.playerKnight.TopLeft.Col > 0)
            {
                this.playerKnight.MoveLeft();
            }

            try
            {
                (new System.Media.SoundPlayer(@"..\..\Step.wav")).Play();
            }
            catch (FileNotFoundException)
            {
                //throw new FileNotFoundException("File was not found!");
            }

        }

        public virtual void MovePlayerRight()
        {
            if (this.playerKnight.TopLeft.Col < (renderer as ConsoleRenderer).RenderContextMatrixCols - playerKnight.GetImage().GetLength(1))
            {
                this.playerKnight.MoveRight();
            }

            try
            {
                (new System.Media.SoundPlayer(@"..\..\Step.wav")).Play();
            }
            catch (FileNotFoundException)
            {
                //throw new FileNotFoundException("File was not found!");
            }

        }

        public virtual void MovePlayerUp()
        {
            if (this.playerKnight.TopLeft.Row > 0)
            {
                this.playerKnight.MoveUp();
            }
            try
            {
                (new System.Media.SoundPlayer(@"..\..\Step.wav")).Play();
            }
            catch (FileNotFoundException)
            {
                //throw new FileNotFoundException("File was not found!");
            }

        }

        public virtual void MovePlayerDown()
        {
            if (this.playerKnight.TopLeft.Row < (renderer as ConsoleRenderer).RenderContextMatrixRows - playerKnight.GetImage().GetLength(0))
            {
                this.playerKnight.MoveDown();
            }

            try
            {
                (new System.Media.SoundPlayer(@"..\..\Step.wav")).Play();
            }
            catch (FileNotFoundException)
            {
                //throw new FileNotFoundException("File was not found!");
            }
        }

        public virtual void PlayerSlash()
        {
            playerKnight.Slash();
        }

        private void PlayerUnslash()
        {
            playerKnight.UnSlash();
        }

        public virtual void MoveCreature(Creature creature, int iterationsCount)
        {
            if (iterationsCount > enemySpead)
            {
                if (playerKnight.TopLeft.Col + 3 < creature.TopLeft.Col)
                {
                    creature.MoveLeft();
                }
                if (playerKnight.TopLeft.Col + 3 > creature.TopLeft.Col)
                {
                    creature.MoveRight();
                }
                if (playerKnight.TopLeft.Row < creature.TopLeft.Row)
                {
                    creature.MoveUp();
                }
                if (playerKnight.TopLeft.Row > creature.TopLeft.Row)
                {
                    creature.MoveDown();
                }
            }
        }

        public void ListenForUserInput()
        {
            userInterface.OnLeftPressed += (sender, eventInfo) =>
            {
                MovePlayerLeft();
            };

            userInterface.OnRightPressed += (sender, eventInfo) =>
            {
                MovePlayerRight();
            };

            userInterface.OnUpPressed += (sender, eventInfo) =>
            {
                MovePlayerUp();
            };

            userInterface.OnDownPressed += (sender, eventInfo) =>
            {
                MovePlayerDown();
            };

            userInterface.OnActionPressed += (sender, eventInfo) =>
            {
                PlayerSlash();
            };
        }

        private void AddBoss()
        {
            Character creature5 = new Boss("Bad boss", new MatrixCoords(20, 50));
            AddObject(creature5);
            isBossAdded = true;

        }

        public void EndGameWin()
        {
            Console.Clear();
            string pathFile = @"..\..\youwin.txt";

            try
            {
                StreamReader reader = new StreamReader(pathFile);
                using (reader)
                {
                    string output = reader.ReadToEnd();
                    Console.WriteLine(output);
                    Thread.Sleep(5000);
                    Environment.Exit(100);
                }
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("End file not found!");
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("End file not found!");
            }
        }

        public void EndGameLoose()
        {
            Console.Clear();
            string pathFile = @"..\..\youloose.txt";

            try
            {
                StreamReader reader = new StreamReader(pathFile);
                using (reader)
                {
                    string output = reader.ReadToEnd();
                    Console.WriteLine(output);
                    Thread.Sleep(5000);
                    Environment.Exit(100);
                }
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("End file not found!");
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("End file not found!");
            }
        }

        public virtual void Run()
        {
            ListenForUserInput();

            int iterationsCount = 0;

            while (true)
            {
                int tempCount = 0;
                iterationsCount++;

                this.renderer.ClearQueue();

                this.userInterface.ProcessInput();

                foreach (var obj in this.charackters)
                {
                    //obj.Update(); // the following code can be upgraded with upgrade

                    var creature = obj as Creature;

                    if (obj is Creature)
                    {
                        if (((creature.TopLeft.Row - creature.VerticalRange) < playerKnight.TopLeft.Row) &&
                            ((creature.TopLeft.Row + creature.VerticalRange) > playerKnight.TopLeft.Row) &&
                            ((creature.TopLeft.Col - creature.HorizontalRange) < playerKnight.TopLeft.Col) &&
                            ((creature.TopLeft.Col + creature.HorizontalRange) > playerKnight.TopLeft.Col)
                            )
                        {
                            MoveCreature(obj as Creature, iterationsCount); // iterationsCount used to slow down the movement of the enemies
                        }
                    }

                    if (playerKnight.IsSlashing && (playerKnight.TopLeft.Col + 3) == obj.TopLeft.Col && (playerKnight.TopLeft.Row) == obj.TopLeft.Row)
                    {
                        creature.Health -= hitPoints + playerKnight.DmgBonus;
                        creature.UpdateHealth(creature.Health);

                        if (creature.Health <= 0)
                        {
                            creature.Die();
                            playerKnight.GainExpirience();

                            if (creature is Boss)
                            {
                                try
                                {
                                    creature.CreatureSqueel();
                                    EndGameWin();
                                }
                                catch (FileNotFoundException)
                                {
                                    //throw new FileNotFoundException("File was not found!");
                                }
                            }
                            else
                            {
                                try
                                {
                                    creature.CreatureSqueel();
                                }
                                catch (FileNotFoundException)
                                {
                                    //throw new FileNotFoundException("File was not found!");
                                }
                            }
                        }
                    }

                    if ((playerKnight.TopLeft.Col + 3) == obj.TopLeft.Col &&
                        (playerKnight.TopLeft.Row) == obj.TopLeft.Row)
                        {
                            playerKnight.Health--;

                            if (playerKnight.Health < 0)
                            {
                                EndGameLoose();
                            }
                        }

                    if (obj.Exist)
                    {
                        this.renderer.EnqueueForRendering(obj);
                    }

                    if (obj.Exist == false)
                    {
                        tempCount++;
                    }
                }

                if ((tempCount == (charackters.Count() - 1)) && (isBossAdded == false))
                {
                    AddBoss();
                    try
                    {
                        (new System.Media.SoundPlayer(@"..\..\Gong.wav")).Play();
                    }
                    catch (FileNotFoundException)
                    {
                        //throw new FileNotFoundException("File was not found!");
                    }
                }

                if (iterationsCount > enemySpead)
                {
                    iterationsCount = 0;
                }

                this.renderer.RenderAll();

                Thread.Sleep(10);

                PlayerUnslash();
            }
        }
    }
}
