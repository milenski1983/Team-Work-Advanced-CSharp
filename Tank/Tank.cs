using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank
{
    class Tank
    {
        private int x;
        private int y;
        private string direction;
        private char[,] body;

        public int X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public int Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public string Direction
        {
            get { return this.direction; }
            set { this.direction = value; }
        }

        public char[,] Body
        {
            get { return this.body; }
            set { this.body = value; }
        }
         
        public Tank()
        {
            this.x = Console.BufferWidth / 2 - 1;
            this.y = Console.BufferHeight - 3;
            this.direction = "up";
            this.body = new char[3, 3]
                        {
                            {' ','|',' '},
                            {'@','#','@'}, 
                            {'@','#','@'},                            
                        };
        }
         
        public int Move(string direction)
        {
            switch (direction)
            {
                case "left":
                    if (this.x >= 2)
                    {
                        return this.x--;
                    }
                    else
                    {
                        return this.x;
                    }
                    break;
                case "right":
                    if (this.x < Console.BufferWidth - 2)
                    {
                        return this.x++;
                    }
                    else
                    {
                        return this.x;
                    }
                    break;
                case "up":
                    if (this.y > 1)
                    {
                        return this.y--;
                    }
                    else
                    {
                        return this.y;
                    }
                    break;
                default:
                    if (this.y <= Console.BufferHeight - 4)
                    {
                        return this.y++;
                    }
                    else
                    {
                        return this.y;
                    }
                    break;
            }
        }

        public Bullet OpenFire(string direction)
        {
            return new Bullet(this.x, this.y, direction);
        }
    }
}
