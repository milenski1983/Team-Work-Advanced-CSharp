using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank
{
    class Mine
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
            set { this.y = value; }
        }

        public char Body
        {
            get { return this.body; }
        }

        public Mine(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.body = 'x';
        }
    }
}
