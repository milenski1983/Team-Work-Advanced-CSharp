using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank
{
    class Bullet
    {
        private int x;
        private int y;
        private string direction;
        private char body;

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
        }

        public char Body
        {
            get { return this.body; }
        }

        public Bullet(int x, int y, string direction)
        {
            this.x = x;
            this.y = y;
            this.direction = direction;
            this.body = '+';
        }

        public int Move(string direction)
        {
            switch (direction)
            {
                case "left":
                    return this.x-=2;
                    break;
                case "right":
                    return this.x+=2;
                    break;
                case "up":
                    return this.y-=2;
                    break;
                default:
                    return this.y+=2;
                    break;
            }
        }
    }
}
