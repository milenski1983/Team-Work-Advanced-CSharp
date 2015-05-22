using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Tank
{
    class Tanks
    {
        static void Main()
        {
            Console.SetWindowSize(80, 30);
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;

            //tanks positions
            Dictionary<string, char[,]> tankPositions = new Dictionary<string, char[,]>();
            tankPositions["up"] = new char[,]{{' ','|',' '}, {'@','#','@'}, {'@','#','@'}};
            tankPositions["right"] = new char[,]{{'@','@',' '}, {'#','#','-'}, {'@','@',' '}};
            tankPositions["down"] = new char[,]{{'@','#','@'}, {'@','#','@'}, {' ','|',' '}};
            tankPositions["left"] = new char[,]{{' ','@','@'}, {'-','#','#'}, {' ','@','@'}};
            
            string[] directions = {"up", "down", "left", "right"};
            List<Bug> bugs = new List<Bug>();
            char[] bugsBodies = { '#', '$', '%', '^', '&', '*', '!', '?', '.', ',' };
            List<Bullet> bullets = new List<Bullet>();
            Tank tank = new Tank();
            Random bugsGen = new Random(); //random generator for bugs
            int numberOfBugs = bugsGen.Next(15,25);
            uint score = 0;
            bool gameOver = false;
            DateTime now = new DateTime();
            Stopwatch sw = new Stopwatch();
            TimeSpan timeElapsed = new TimeSpan();
            int sleepTime = 180;
            List<Mine> mines = new List<Mine>();
            int x = 0;
            int y = 0;
            string direction = null;
            char bugsBody = '*';

            //bugs generator
            for (int i = 0; i < numberOfBugs; i++)
            {
                x = bugsGen.Next(0, Console.BufferWidth);
                y = bugsGen.Next(0, Console.BufferHeight - 10);
                 direction = directions[bugsGen.Next(0, directions.Length)];
                 bugsBody = bugsBodies[bugsGen.Next(0, bugsBodies.Length)]; //random bugs' forms
                bugs.Add(new Bug(x, y, direction, bugsBody));   
                
            }
            if (score % 2 == 0)
            {
                bugs.Add(new Bug(x, y, direction, bugsBody));
            }
            //Mines generator
            for (int i = 0; i < 10; i++)
            {
                mines.Add(new Mine(bugsGen.Next(1,Console.BufferWidth - 1), bugsGen.Next(1,Console.BufferHeight - 1)));
            }

            while (true)
            {
                //sw.Start();
                now = DateTime.Now;
                Console.Clear();
                foreach (Bug bug in bugs)
                {
                    string newDirection = directions[bugsGen.Next(0, directions.Length)];
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
                        }
                    }
                }
                DrawMines(mines);
                DrawTank(tank);
                DrawBugs(bugs);
                DrawBullets(bullets);
                DrawScore(score);

                //collisions
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

                //sw.Stop();
             //   timeElapsed.Add(DateTime.Now - now);
                if (score>7)
                {
                    sleepTime = 140;
                }
                if (score>13)
                {
                    sleepTime = 100;
                }

                try
                {
                    Thread.Sleep(sleepTime);
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.Write("Time's up!");
                    Thread.Sleep(2000);
                    return;
                }
                

                if (gameOver)
                {
                    Console.Write("Game over! Press 1) to try again or Escape to exit.");
                Loop:
                    ConsoleKeyInfo choice = Console.ReadKey(true);
                    
                    switch (choice.Key)
                    {
                        case ConsoleKey.D1:
                            Main();
                            break;
                        case ConsoleKey.Escape:
                            return;
                            break;
                        default:
                            goto Loop; 
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
