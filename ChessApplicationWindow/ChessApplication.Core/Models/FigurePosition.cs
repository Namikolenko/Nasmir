using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApplication.Core.Models
{
    public class FigurePosition
    {
        public Dictionary<string, int> Position { get; set; }

        public FigurePosition(string posLetter, int posNumber)
        {
            Position = new Dictionary<string, int>();
            Position.Add(posLetter, posNumber);
        }
    }
}
