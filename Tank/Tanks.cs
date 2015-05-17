﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Tank
{
    class Tanks
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(80, 30);
            Console.BufferWidth = 80;
            Console.BufferHeight = 30;
            char[] bugsBodies = { '#', '$', '%', '^', '&', '*', '!', '?', '.', ',' };

            Dictionary<string, char[,]> tankPositions = new Dictionary<string, char[,]>();
            tankPositions["up"] = new char[,]{{' ','|',' '}, {'@','#','@'}, {'@','#','@'}};
            tankPositions["right"] = new char[,]{{'@','@',' '}, {'#','#','-'}, {'@','@',' '}};
            tankPositions["down"] = new char[,]{{'@','#','@'}, {'@','#','@'}, {' ','|',' '}};
            tankPositions["left"] = new char[,]{{' ','@','@'}, {'-','#','#'}, {' ','@','@'}};
            
            string[] directions = {"up", "down", "left", "right"};
            List<Bug> bugs = new List<Bug>();
            List<Bullet> bullets = new List<Bullet>();
            Tank tank = new Tank();
            Random bugsGen = new Random();
            int numberOfBugs = bugsGen.Next(15, 20);
            uint score = 0;

            //bugs generator
            for (int i = 0; i < numberOfBugs; i++)
            {
                int x = bugsGen.Next(0, Console.BufferWidth);
                int y = bugsGen.Next(0, Console.BufferHeight - 10);
                string direction = directions[bugsGen.Next(0, directions.Length)];
                char bugsBody = bugsBodies[bugsGen.Next(0, bugsBodies.Length)]; //random bugs' forms
                bugs.Add(new Bug(x, y, direction, bugsBody));   
            }

            while (true)
            {
                Console.Clear();
                foreach (Bug bug in bugs)
                {
                    string newDirection = directions[bugsGen.Next(0, directions.Length)];
                    bug.Move(newDirection);
                }

                tank.Move(tank.Direction);
                
                if (Console.KeyAvailable)
                {
                    //user input
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
                            score++;
                        }
                    }
                }

                DrawTank(tank);
                DrawBullets(bullets);
                DrawBugs(bugs);
                DrawScore(score);

                //collisions
                foreach (Bug bug in bugs)
                {
                    if ((bug.X == tank.X) && (bug.Y == tank.Y))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(1, Console.BufferHeight - 1);
                        Console.WriteLine("Collision detected!");
                        Console.ReadLine();
                        return;
                    }
                }

                Thread.Sleep(200);
            }

        }

        private static void DrawScore(uint score)
        {
            Console.SetCursorPosition(Console.BufferWidth - 20, Console.BufferHeight - 3);
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