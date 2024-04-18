using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Labirint
{
    internal class Player
    {
        public int X { get; set; }
        public int Y { get; set; }


        public Player(int primaryX, int primaryY)
        {
            X = primaryX;
            Y = primaryY;
        }

        public void Draw()
        {
            SetCursorPosition(X, Y);
            Write("0");
        }
        


    }
}