using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApplication.Core.Models
{
    struct Square
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public Square(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static Square none = new Square(-1, -1);
        public Square(string cellMove)
        {
            if (cellMove.Length == 2 && cellMove[0] >= 'a' && cellMove[1] <= 'h' && cellMove[0] >= '1' && cellMove[1] <= '8')
            {
                this.x = cellMove[0] - 'a';
                this.y = cellMove[1] - '1';
            }
            else
                this = none;
        }
        public bool OnBoard()
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }
        public static bool operator ==(Square a, Square b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Square a, Square b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public static IEnumerable<Square> YieldSquares()
        {
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    yield return new Square(x, y);
        }
        public string Name { get { return ((char)('a' + x)).ToString() + (y + 1).ToString(); } }
    }
}
