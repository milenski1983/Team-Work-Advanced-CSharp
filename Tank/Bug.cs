using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank
{
    class Bug
    {
        private int x;
        private int y;
        private char body;

        public int X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public int Y
        {
            get { return this.y; }
            set { this.y = value;}
        }
        
        public char Body
        {
            get { return this.body; }
        }

        public Bug(int x, int y, string direction, char body)
        {
            this.x = x;
            this.y = y;
            this.body = body;
        }

        public int Move(string direction)
        {
            switch (direction)
            {
                case "left":
                    if (this.x >= 1)
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
                    if (this.y >= 2)
                    {
                        return this.y--;
                    }
                    else
                    {
                        return this.y;
                    }
                    break;
                default:
                    if (this.y <= Console.BufferHeight - 2)
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
    }
}
