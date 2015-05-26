using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Diagnostics;
namespace Tank
{
    class Tanks
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetWindowSize(80, 30);
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;

            //tanks positions
            Dictionary<string, char[,]> tankPositions = new Dictionary<string, char[,]>();
            tankPositions["up"] = new char[,] { { ' ', '|', ' ' }, { '@', '#', '@' }, { '@', '#', '@' } };
            tankPositions["right"] = new char[,] { { '@', '@', ' ' }, { '#', '#', '-' }, { '@', '@', ' ' } };
            tankPositions["down"] = new char[,] { { '@', '#', '@' }, { '@', '#', '@' }, { ' ', '|', ' ' } };
            tankPositions["left"] = new char[,] { { ' ', '@', '@' }, { '-', '#', '#' }, { ' ', '@', '@' } };

            string[] directions = { "up", "down", "left", "right" };
            List<Bug> bugs = new List<Bug>();
            char[] bugsBodies = { '\u00F6', '\u022D', '\u0298', '\u0398', '\u047E', '\u058E', '\u06DD', '\u070F', '\u06E9', '\u07F7', '\u080E', '\u0994', '\u263B', '\u263A', '\u00A9', '\u2117', '\u00AE' };
            List<Bullet> bullets = new List<Bullet>();
            Tank tank = new Tank();
            Random randomGen = new Random(); //random generator for bugs
            int numberOfBugs = randomGen.Next(15, 25);
            uint score = 0;
            bool gameOver = false;
            bool Invisible = true;
            int sleepTime = 180;
            List<Mine> mines = new List<Mine>();
            string direction = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            //bugs generator
            for (int i = 0; i < numberOfBugs; i++)
            {
                direction = directions[randomGen.Next(0, directions.Length)];
                //random bugs' forms
                bugs.Add(new Bug(randomGen.Next(0, Console.BufferWidth), randomGen.Next(0, Console.BufferHeight - 10), direction, bugsBodies[randomGen.Next(0, bugsBodies.Length)]));
                //x                                        //y                          //direction   //bugs' forms
            }
            if (score % 2 == 0)
            {
                bugs.Add(new Bug(randomGen.Next(0, Console.BufferWidth), randomGen.Next(0, Console.BufferHeight - 10), direction, bugsBodies[randomGen.Next(0, bugsBodies.Length)]));
            }
            //Mines generator
            for (int i = 0; i < 10; i++)
            {
                mines.Add(new Mine(randomGen.Next(1, Console.BufferWidth - 1), randomGen.Next(1, Console.BufferHeight - 1)));
            }

            while (true)
            {
                Console.Clear();
                foreach (Bug bug in bugs)
                {
                    string newDirection = directions[randomGen.Next(0, directions.Length)];
                    bug.Move(newDirection);
                }

                tank.Move(tank.Direction);

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();

                    //tank direction
                    if (key.Key == ConsoleKey.UpArrow)
                    {
                        tank.Direction = directions[0];
                    }
                    else if (key.Key == ConsoleKey.RightArrow)
                    {
                        tank.Direction = directions[3];
                    }
                    else if (key.Key == ConsoleKey.DownArrow)
                    {
                        tank.Direction = directions[1];
                    }
                    else if (key.Key == ConsoleKey.LeftArrow)
                    {
                        tank.Direction = directions[2];
                    }
                    tank.Body = tankPositions[tank.Direction];

                    //tank opens fire
                    if (key.Key == ConsoleKey.Spacebar)
                    {
                        bullets.Add(tank.OpenFire(tank.Direction));
                    }

                    if (key.Key == ConsoleKey.Escape)
                    {
                        return;
                    }

                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(false);
                    }
                }

                //move existing bullets
                foreach (Bullet bullet in bullets)
                {
                    bullet.Move(bullet.Direction);
                }

                //remove bullets that exit the console window
                for (int i = bullets.Count - 1; i >= 0; i--)
                {
                    if (bullets[i].X < 1 || bullets[i].X >= Console.BufferWidth - 1 || bullets[i].Y < 1 || bullets[i].Y >= Console.BufferHeight - 1)
                    {
                        bullets.Remove(bullets[i]);
                    }
                }

                //destroy hitted bugs
                foreach (Bullet bullet in bullets)
                {
                    for (int i = bugs.Count - 1; i >= 0; i--)
                    {
                        if (bullet.X == bugs[i].X && bullet.Y == bugs[i].Y)
                        {
                            bugs.RemoveAt(i);
                            Console.Beep();
                            score++;
                            sleepTime -= 5;
                        }
                    }
                }

                DrawMines(mines);
                DrawTank(tank);
                DrawBugs(bugs);
                DrawBullets(bullets);
                DrawScore(score);

                // Invisible
                if (sw.ElapsedMilliseconds > 10000)
                {
                    Invisible = false;
                } 
                //collisions
                if (!Invisible)
                {
                    foreach (Bug bug in bugs)
                    {
                        if ((bug.X - 1) <= tank.X && tank.X <= (bug.X + 1) && (bug.Y - 1 <= tank.Y && tank.Y <= bug.Y + 1))
                        {
                            Console.Beep(1250, 500);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("  Collision detected!    ");
                            gameOver = true;
                            break;
                        }
                    }

                    foreach (Mine mine in mines)
                    {
                        if ((mine.X - 1) <= tank.X && tank.X <= (mine.X + 1) && (mine.Y - 1 <= tank.Y && tank.Y <= mine.Y + 1))
                        {
                            Console.Beep(1250, 500);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("  Collision detected!    ");
                            gameOver = true;
                            break;
                        }
                    }
                }
                try
                {
                    Thread.Sleep(sleepTime);
                }
                catch (ArgumentOutOfRangeException)
                {
                    Thread.Sleep(1);
                }
                
                if (gameOver)
                {
                    Console.Write("Your score is: {0} ,", score);
                    int bestScore;
                    using (StreamReader reader = new StreamReader("bestscore.txt"))
                    {
                        bestScore = int.Parse(reader.ReadLine());
                    }
                    Console.Write("the best score is: {0}. ", bestScore);
                    Console.WriteLine(score >= bestScore ? "Super!!!" : "Try harder to beat the record!");
                    if (score > bestScore)
                    {
                        using (StreamWriter writer = new StreamWriter("bestscore.txt"))
                        {
                            writer.Write(score);
                        }
                    }

                    Console.Write("Game over! Press 1) to try again or Escape to exit.");
                    ConsoleKeyInfo choice;

                    do
	                {
	                    choice = Console.ReadKey();
	                } while (choice.Key != ConsoleKey.D1 && choice.Key != ConsoleKey.Escape);
                    
                    switch (choice.Key)
                    {
                        case ConsoleKey.D1:
                            Main();
                            break;
                        case ConsoleKey.Escape:
                            return;
                            break;
                        default:
                            break;
                    }
                }
            }

        }

        private static void DrawMines(List<Mine> mines)
        {
            foreach (Mine mine in mines)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition(mine.X, mine.Y);
                Console.Write(mine.Body);
            }
        }

        private static void DrawScore(uint score)
        {
            Console.SetCursorPosition(Console.BufferWidth - 20, Console.BufferHeight - 1);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Score: {0}", score);
        }

        private static void DrawBullets(List<Bullet> bullets)
        {
            foreach (Bullet bullet in bullets)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(bullet.X, bullet.Y);
                Console.WriteLine(bullet.Body);
            }
        }

        private static void DrawBugs(List<Bug> bugs)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (Bug bug in bugs)
            {
                Console.SetCursorPosition(bug.X, bug.Y);
                Console.Write(bug.Body);
            }
        }

        private static void DrawTank(Tank tank)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < tank.Body.GetLength(0); i++)
            {
                for (int j = 0; j < tank.Body.GetLength(1); j++)
                {
                    Console.SetCursorPosition(tank.X - 1 + j, tank.Y - 1 + i);
                    Console.Write(tank.Body[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
