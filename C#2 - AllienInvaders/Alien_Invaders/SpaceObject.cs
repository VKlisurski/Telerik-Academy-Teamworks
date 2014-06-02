using System;

namespace SpaceObjects
{
    public class SpaceObject
    {

        public const int PlayFieldLimit = 60;
        public int length;
        public int height;
        public string symbol;
        public ConsoleColor color;
        public int x;
        public int y;

        public SpaceObject()
        {
            this.length = 1;
            this.height = 1;
            this.symbol = String.Empty;
            this.color = ConsoleColor.Blue;
            this.x = 0;
            this.y = 0;
        }

        public void Draw()
        {
            Console.ForegroundColor = this.color;
            int currentY = this.y;
            Console.SetCursorPosition(x, y);
            for (int i = 0; i < symbol.Length; i++)
            {
                if (symbol[i] == '\n')
                {
                    Console.SetCursorPosition(x, ++currentY);
                }
                else
                {
                    Console.Write(this.symbol[i]);
                }
            }
        }

        public void Move(int xDelta, int yDelta)
        {
            if (this.x + this.length - 1 + xDelta < PlayFieldLimit && this.x + xDelta >= 0)
            {
                this.x += xDelta;
            }
            if (this.y + this.height - 1 + yDelta < Console.WindowHeight && this.y + yDelta >= 0)
            {
                this.y += yDelta;
            }
        }
         
        public bool CheckCollision(SpaceObject obj)
        {
            return false;
        }

        public bool IsOnPlayGround()
        {
            return true;
        }
    }

    public class Player : SpaceObject
    {
        public Player()
        {
            this.length = 5;
            this.height = 2;
            this.symbol = " _^_\n/___\\";
            this.color = ConsoleColor.White;
            this.x = PlayFieldLimit / 2;
            this.y = Console.WindowHeight - 2;
        }
    }
    public class Bullet: SpaceObject
    {
        public Bullet(int xPos, int yPos)
        {
            this.x = xPos;
            this.y = yPos;
            this.symbol = "|";
            this.color = ConsoleColor.White;
        }

        public bool CheckCollision(Alien alien)
        {
            return
            (alien.x == this.x && alien.y == this.y) ||
            ((alien.x + 1 == this.x) && alien.y + 1 == this.y) ||
            ((alien.x + 2 == this.x) && alien.y + 1 == this.y) ||
            ((alien.x + 3 == this.x) && alien.y + 1 == this.y) ||
            ((alien.x + 4 == this.x) && alien.y + 1 == this.y) ||
            ((alien.x == this.x) && alien.y + 2 == this.y);
        }
    }

    public class Alien: SpaceObject
    {
        //TODO: Different length depending on if it's enemy
        public bool isEnemy;

        public Alien(bool enemy = true, ConsoleColor col = ConsoleColor.DarkRed, int yPos = 0)
        {
            this.height = 3;
            this.length = 5;
            this.isEnemy = enemy;
            this.color = col;
            this.y = yPos;
            if (enemy)
            {
                this.symbol = "\\\n ===>\n/";
            }
            else
            {
                this.symbol = "\\\n )))>\n/";
                this.color = ConsoleColor.White;
            }

            this.x = - this.length;   
        }

        public void Move(int xDelta, int yDelta)
        {
            if (this.x + xDelta < PlayFieldLimit + 1)
            {
                this.x += xDelta;
            }
            if (this.y + yDelta + this.height < Console.WindowHeight && this.y + yDelta > 0)
            {
                this.y += yDelta;
            }
        }

        public void Draw()
        {
            Console.ForegroundColor = this.color;

            if (this.x < 0)
            {
                Console.SetCursorPosition(0, y + 1);
                int startIndex = 6 - (length - 1 + x); // 6 is the index of the '>' symbol
                for (int i = startIndex; i <= 6; i++)
                {
                    Console.Write(this.symbol[i]);
                }
            }
            else if (this.x == PlayFieldLimit)
            {
                
            }
            else if(this.x > PlayFieldLimit - this.length)
            {
                int currentY = this.y;
                Console.SetCursorPosition(x, y);
                int startIndex = 6 - (length - 1 + x - PlayFieldLimit);
                for (int i = 0; i < symbol.Length; i++)
                {
                    if (i >= startIndex && i <= 6)
                    {
                        continue;
                    }
                    if (symbol[i] == '\n')
                    {
                        Console.SetCursorPosition(x, ++currentY);
                    }
                    else
                    {
                        Console.Write(this.symbol[i]);
                    }
                }
            }
            else
            {
                int currentY = this.y;
                Console.SetCursorPosition(x, y);
                for (int i = 0; i < symbol.Length; i++)
                {
                    if (symbol[i] == '\n')
                    {
                        Console.SetCursorPosition(x, ++currentY);
                    }
                    else
                    {
                        Console.Write(this.symbol[i]);
                    }
                }
            }
        }

        public bool IsOnPlayGround()
        {
            return this.y <= PlayFieldLimit;
        }
        public bool CanDropBomb()
        {

            return x >= 0 && y + length < PlayFieldLimit;
        }

    }

    public class AlienBomb: SpaceObject
    {
        public AlienBomb(Alien alien)
        {
            this.x = alien.x;
            this.y = alien.y + height;
            this.color = alien.color;
            this.symbol = "*";
        }

        public bool CheckCollision(Player player)
        {
            return
            (player.x + 1 == this.x && player.y == this.y) ||
            (player.x + 2 == this.x && player.y == this.y) ||
            (player.x + 3 == this.x && player.y == this.y) ||
            (player.x == this.x && player.y + 1 == this.y) ||
            (player.x + 4 == this.x && player.y + 1 == this.y);
        }
    }

    public class BulletSpeedBonus: SpaceObject
    {
        public BulletSpeedBonus(Alien alien)
        {
            this.x = alien.x;
            this.y = alien.y + height;
            this.color = alien.color;
            this.symbol = "+";
        }

        public bool CheckCollision(Player player)
        {
            return
            (player.x + 1 == this.x && player.y == this.y) ||
            (player.x + 2 == this.x && player.y == this.y) ||
            (player.x + 3 == this.x && player.y == this.y) ||
            (player.x == this.x && player.y + 1 == this.y) ||
            (player.x + 4 == this.x && player.y + 1 == this.y);
        }
    }

    public class LiveBonus : SpaceObject
    {
        public LiveBonus(Alien alien)
        {
            this.x = alien.x;
            this.y = alien.y + height;
            this.color = alien.color;
            this.symbol = "l";
        }

        public bool CheckCollision(Player player)
        {
            return
            (player.x + 1 == this.x && player.y == this.y) ||
            (player.x + 2 == this.x && player.y == this.y) ||
            (player.x + 3 == this.x && player.y == this.y) ||
            (player.x == this.x && player.y + 1 == this.y) ||
            (player.x + 4 == this.x && player.y + 1 == this.y);
        }
    }
}