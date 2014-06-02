using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SpaceObjects;
using System.Threading;

class AllienInvaders
{
    static string playerName;
    static byte level = 1;
    static int shootRate = 1;
    static int bulletMoveRate = 3;
    static int dropBombRate = 3;
    static int dropBombCounter = 0;
    static int createLiveBonusRate = 100;
    static int createLiveBonusCounter = 0;
    static int currentScore;
    static int liveCount = 3;
    static TimeSpan gameDuration;
    static DateTime start = new DateTime();
    static Random rand = new Random();
    static int loopCounter = 0;
    static int playerSpeed = 3;
    static int delay = 100;

    static int playFieldLimit = SpaceObject.PlayFieldLimit;
    const int HEIGHT = 30;
    const int WIDTH = 100;

    // Lists for objects
    static List<Alien> aliens = new List<Alien>();
    static List<Bullet> bullets = new List<Bullet>();
    static List<AlienBomb> alienBombs = new List<AlienBomb>();
    static List<LiveBonus> liveBonus = new List<LiveBonus>();
    static List<BulletSpeedBonus> bulletSpeedBonus = new List<BulletSpeedBonus>();

    static void ChooseLevel()
    {
        switch (level)
        {
            case 1:
                playerSpeed = 3;
                delay = 100;
                shootRate = 1;
                break;
            case 2:
                playerSpeed = 2;
                delay = 75;
                shootRate = 2;
                break;
            case 3:
                playerSpeed = 1;
                delay = 50;
                shootRate = 3;
                break;
            default:
                playerSpeed = 3;
                delay = 100;
                shootRate = 1;
                break;
        }
    }

    static void RefreshLevel()
    {
        aliens.Clear();
        bullets.Clear();
        alienBombs.Clear();
        liveBonus.Clear();
        bulletSpeedBonus.Clear();
    }

    static void GameOver()
    {
        GameOverScreen();

        //SubmitScore(playerName, currentScore);
        playerName = null;
        currentScore = 0;
        liveCount = 3;
        aliens.Clear();
        bullets.Clear();
        alienBombs.Clear();
        liveBonus.Clear();
        bulletSpeedBonus.Clear();
    }

    static void AlienIsHit(Alien alien)
    {
        if (!alien.isEnemy)
        {
            currentScore -= 5;
        }
        else
        {
            currentScore += 10;
        }
    }

    static void PlayerIsHit()
    {
        //Play explosion.wav when a bomb hits the player
        string soundFile = @"..\..\Explosion1.wav";
        System.Media.SoundPlayer player = new System.Media.SoundPlayer(soundFile);
        player.Load();
        player.PlaySync();

        RefreshLevel();

        liveCount--;   
    }
    
    static void CheckPlayerTakesBonus(Player player)
    {}

    static void CheckBonusLiveEnds()
    {}
    
    static ConsoleColor GetRandomColor()
    {
        return (ConsoleColor)rand.Next(1, 15);
    }

    static void ClearBuffer()
    {
        // Read keys until they finish without diplaying
        while (Console.KeyAvailable)
        {
            Console.ReadKey(true);
        }
    }

    static void CreateRandomAlien()
    {
        //Random random = new Random();
        bool isEnemy;
        int randomNumber = rand.Next(11);
        if (randomNumber <= 7)
        {
            isEnemy = true;
        }
        else
        {
            isEnemy = false;
        }

        ConsoleColor color;
        color = GetRandomColor();

        int position = rand.Next(15);

        Alien alien = new Alien(isEnemy, color, position);
        aliens.Add(alien);
    }

    static void CreateAlienBomb()
    {
        int count = aliens.Count;
        if (count > 0)
        {
            int randomNumber = rand.Next(count);
            if (aliens[randomNumber].isEnemy && aliens[randomNumber].CanDropBomb())
            {
                AlienBomb bomb = new AlienBomb(aliens[randomNumber]);
                alienBombs.Add(bomb);
            }
        }
    }

    static void CreateLiveBonus()
    {
        int count = aliens.Count;
        if (count > 0)
        {
            int randomNumber = rand.Next(count);
            if (!aliens[randomNumber].isEnemy && aliens[randomNumber].CanDropBomb())
            {
                LiveBonus bonus = new LiveBonus(aliens[randomNumber]);
                liveBonus.Add(bonus);
            }
        }
    }

    static void MoveAliens()
    {
        aliens.ForEach(alien => alien.Move(1, 0));

        for (int i = 0; i < aliens.Count; i++)
        {
            for (int j = 0; j < bullets.Count; j++)
            {
                if (bullets[j].CheckCollision(aliens[i]))
                {
                    AlienIsHit(aliens[i]);
                    aliens.Remove(aliens[i]);
                    bullets.Remove(bullets[j]);
                }
            }
            if (aliens[i].x >= Alien.PlayFieldLimit)
            {
                aliens.Remove(aliens[i]);
            }
        }
    }

    static void MoveAlienBombs()
    {
        int count = alienBombs.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            if (alienBombs[i].y == HEIGHT - 1)
            {
                alienBombs.RemoveAt(i);
            }
            else
            {
                alienBombs[i].Move(0, 1);
            }
        }
    }

    static void MoveBullets()
    {
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            if (bullets[i].y <= 2)
            {
                bullets.RemoveAt(i);
            }
            else
            {
                bullets[i].Move(0, -1);
            }
        }
    }

    static void MoveLiveBonuses()
    {
        for (int i = liveBonus.Count - 1; i >= 0; i--)
        {
            if (liveBonus[i].y == 0)
            {
                liveBonus.RemoveAt(i);
            }
            else
            {
                liveBonus[i].Move(0, 1);
            }
        }
    }

    static void CheckPlayerIsHit(Player player)
    {
        //int count = alienBombs.Count;
        for (int i = alienBombs.Count - 1; i >= 0; i--)
        {
            if (alienBombs[i].CheckCollision(player))
            {
                PlayerIsHit();

                if (alienBombs.Count > i)
                {
                    alienBombs.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
        }
    }

    static void DisplayCurrentScore()
    {
        int y = HEIGHT / 2 - 4;
        int x = (WIDTH + playFieldLimit) / 2 - 4;
        gameDuration = DateTime.Now - start;
        string[] currentInfo = new string[3] 
                                {String.Format("Current Score: {0}", currentScore), 
                                 String.Format("Lives: {0}", liveCount), 
                                 String.Format("Time: {0}:{1}", gameDuration.Minutes, gameDuration.Seconds)
                                };
        foreach (var item in currentInfo)
        {
            DrawText(x, ++y, item, ConsoleColor.DarkGreen);
        }
    }

    static void DrawText(int x, int y, string text, ConsoleColor textColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = textColor;
        Console.BackgroundColor = backgroundColor;
        Console.Write(text);
        Console.ResetColor();
    }

    static int ChooseFromMenu()
    {

        int optionChoice = 0;                        //Shows the selected row on the menu
        int x = (WIDTH / 2) - 7;                    //Coordinate X for the text position on the screen
        int y = Console.WindowHeight / 3;           //Coordinate Y for the text position on the screen

        string[] menuRows = new string[]
        {
            "Start Game",
            "Change player name",
            "Change level",
            "High score",
            "Exit"
        };

        while (true)
        {
            // Move menu
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);

                //Declare what happend when we press "UpArrow"
                if (pressedKey.Key == ConsoleKey.UpArrow && optionChoice > 0)
                {
                    optionChoice--;
                }

                //Declare what happend when we press "DownArrow"
                else if (pressedKey.Key == ConsoleKey.DownArrow && optionChoice < menuRows.Length - 1)
                {
                    optionChoice++;
                }

                //Declare what happend when we press "Enter" 
                else if (pressedKey.Key == ConsoleKey.Enter)
                {
                    return optionChoice;
                }
            }

            // Clearing the buffer
            ClearBuffer();

            // Clear field
            Console.Clear();


            // Draw field - 'rowSpace' is the space between every row in the menu and 'element' is for each element in array menuRows
            for (int element = 0, rowSpace = 0; element < menuRows.Length; element++, rowSpace += 2)
            {
                if (element == optionChoice)
                {
                    DrawText(x - 1, y + rowSpace, ">" + menuRows[element], ConsoleColor.Red, ConsoleColor.White);
                }
                else
                {
                    DrawText(x, y + rowSpace, menuRows[element]);
                }
            }

            // Slow loop cycle
            Thread.Sleep(250);
        }
    }

    static void StartNewGame()
    {
        start = DateTime.Now;
        Player player = new Player();
        bool killed = false;

        // Game dynamics
        while (!killed)
        {
            #region User Input
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    if (loopCounter % shootRate == 0)
                    {
                        Bullet bullet = new Bullet(player.x + 2, player.y - 1);
                        bullets.Add(bullet);
                    }
                }
                else if (keyPressed.Key == ConsoleKey.LeftArrow)
                {
                    player.Move(-playerSpeed, 0);
                }
                else if (keyPressed.Key == ConsoleKey.RightArrow)
                {
                    player.Move(playerSpeed, 0);
                }
                else if (keyPressed.Key == ConsoleKey.Escape)
                {
                    // Return to menu
                    return;
                }
            }
            ClearBuffer();
            #endregion
            if (loopCounter % 7 == 0)
            {
                CreateRandomAlien();
            }

            if (dropBombCounter == dropBombRate - 1)
            {
                CreateAlienBomb();
            }
            //if (createLiveBonusCounter == createLiveBonusRate - 1)
            //{
            //    CreateLiveBonus();
            //}
            
            // Move and remove
            MoveAliens();
            MoveBullets();
            //MoveLiveBonuses();
            MoveAlienBombs();
            CheckPlayerIsHit(player);
            if (liveCount == 0)
            {
                killed = true;
            }
            CheckPlayerTakesBonus(player);
            CheckBonusLiveEnds();
            
            // Clear screen
            Console.Clear();

            // Draw Screen
            player.Draw();
            bullets.ForEach(bullet => bullet.Draw());
            aliens.ForEach(alien => alien.Draw());
            alienBombs.ForEach(alienBomb => alienBomb.Draw());
            //liveBonus.ForEach(bonus => bonus.Draw());
            DisplayCurrentScore();
            
            // Draw field line
            //for (int i = 0; i < Console.WindowHeight; i++)
            //{
            //    DrawText(playFieldLimit, i, "|", ConsoleColor.DarkGreen);
            //}

            //Loop counters
            loopCounter++;
            dropBombCounter = (dropBombCounter + 1) % dropBombRate;
            createLiveBonusCounter = (createLiveBonusCounter + 1) % createLiveBonusRate;
            
            //Delay
            Thread.Sleep(delay);
        }

        GameOver();
    }

    static string SetPlayerName()
    {
        Console.Clear();

        Console.SetCursorPosition((WIDTH / 3) - 2, (Console.WindowHeight / 3) + 4);
        Console.Write("Enter player name: ");
        playerName = Console.ReadLine();

        if (playerName == string.Empty || playerName == " ")
        {
            Console.Clear();
            Console.SetCursorPosition((WIDTH / 2) - 7, (Console.WindowHeight / 3) + 4);
            Console.WriteLine("Invalid name");
            Thread.Sleep(2000);
            SetPlayerName();
        }
        else if (playerName.Length < 3)
        {
            Console.Clear();
            Console.SetCursorPosition((WIDTH / 3) - 2, (Console.WindowHeight / 3) + 4);
            Console.WriteLine("The name should be at least 3 symbols");
            Thread.Sleep(2000);
            SetPlayerName();
        }

        return playerName;
        
    }

    static void SetLevel()
    {
        Console.Clear();
        try
        {
            Console.SetCursorPosition((WIDTH / 3) + 5, (Console.WindowHeight / 3) + 4);
            Console.Write("Enter start level: ");
            level = byte.Parse(Console.ReadLine());

            ChooseLevel();
        }
        catch (Exception)
        {
            Console.SetCursorPosition((WIDTH / 3) + 1, (Console.WindowHeight / 3) + 4);
            Console.WriteLine("Enter a valid positive number");
            Thread.Sleep(2000);
            SetLevel();
        }
        Console.Clear();
    }

    static void ListHighScore()
    {
        Console.Clear();
        string fileToRead = @"..\..\Rankings.txt";
        StreamReader reader = new StreamReader(fileToRead);

        string line;
        int counter = 1;
        string[] splitLine = new string[3];
        using (reader)
        {
            Console.WriteLine(new string('\n', 5));
            while ((line = reader.ReadLine()) != null && counter <= 5)
            {
                splitLine = line.Split(' ');
                Console.WriteLine("\n");
                Console.Write(new string(' ', (Console.WindowWidth - 20 ) / 2));
                Console.WriteLine("{0}: {1} {2} {3}", counter, splitLine[0], splitLine[1], splitLine[2], splitLine[3]);
                counter++;
            }
        }
        Console.ReadLine();
    }

    static void SubmitScore(string username, int score) // submits the score after game over and sorting the Rankings.txt file
    {
        DateTime dateTime = DateTime.Now;
        string filePath = @"..\..\Rankings.txt";
        StreamReader reader = new StreamReader(filePath);

        // Split dateTime 
        string dateTimeToString = dateTime.ToString();
        string[] dateTimeSplit = dateTimeToString.Split(' ');

        // Input old scores to list
        List<Scores> unSortedScores = new List<Scores>();
        string[] splits = new string[5];
        string currentUsername, currentDate, currentTime, currentPmAm;
        int currentScore;
        using (reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                splits = line.Split(' ');
                currentUsername = splits[0];
                currentScore = int.Parse(splits[1]);
                currentDate = splits[2];
                currentTime = splits[3];
                unSortedScores.Add(new Scores(currentUsername, currentScore, currentDate, currentTime));
            }
        }

        // Input new score to the list with old scores
        unSortedScores.Add(new Scores(username, score, dateTimeSplit[0], dateTimeSplit[1]));

        // Sorting
        List<Scores> sortedScores = unSortedScores.OrderByDescending(a => a.score)
            .ThenBy(b => b.date)
            .ThenBy(c => c.time)
            .ToList();

        // overwrite Rankings.txt after sorting
        StreamWriter writer = new StreamWriter(filePath);
        using (writer)
        {
            foreach (var item in sortedScores)
            {
                writer.WriteLine(item.username + " " + item.score + " " + item.date + " " + item.time);
            }
        }
    }

    static void GameOverScreen()
    {
        Console.Clear();

        using (StreamReader sr = new StreamReader(@"..\..\gameOverScreen.txt"))
        {
            String line = sr.ReadToEnd();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(line);

            Console.ResetColor();
        }

        Thread.Sleep(1500);
    }

    static void ExitScreen()
    {
        using (StreamReader sr = new StreamReader(@"..\..\exitScreen.txt"))
        {
            Console.Clear();

            String line = sr.ReadToEnd();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(line);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, HEIGHT - 1);

            Thread.Sleep(2000);
            
            System.Environment.Exit(0);
        }
    }

    static void StartScreen()
    {
        using (StreamReader sr = new StreamReader(@"..\..\startScreen.txt"))
        {
            String line = sr.ReadToEnd();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(line);

            Console.SetCursorPosition(0, 28);
            for (int i = 0; i < 100; i++)
            {
                Console.Write('|');
                Thread.Sleep(10);
            }
            Console.ResetColor();
        }
    }

    static void Main()
    {
        // set window size
        Console.BufferHeight = Console.WindowHeight = HEIGHT;
        Console.BufferWidth = Console.WindowWidth = WIDTH;
        Console.CursorVisible = false;
        Console.Title = String.Empty;
        StartScreen();      

        try
        {
            playerName = SetPlayerName();
            while (true)
            {
                int choice = ChooseFromMenu(); // Returns int choice 
                switch (choice)
                {
                    case 0: StartNewGame(); break;
                    case 1: SetPlayerName(); break;
                    case 2: SetLevel(); break;
                    case 3: ListHighScore(); break;
                    case 4: ExitScreen(); break;
                    default: break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
class Scores
{
    public string username;
    public int score;
    public string date;
    public string time;

    public Scores(string username, int points, string date, string time)
    {
        this.score = points;
        this.username = username;
        this.date = date;
        this.time = time;
    }
}