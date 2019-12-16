using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApplication.Core.Models
{
    enum Figure
    {
        none,
        whiteKing = 'K',
        whiteQueen = 'Q',
        whiteRook = 'R',
        whiteBishop = 'B',
        whiteKnight = 'N',
        whitePawn = 'P',

        blackKing = 'k',
        blackQueen = 'q',
        blackRook = 'r',
        blackBishop = 'b',
        blackKnight = 'n',
        blackPawn = 'p'
    }
    static class FigureMethods
    {
        public static Color GetColor(this Figure figure)
        {
            if (figure == Figure.none)
                return Color.none;
            return (figure == Figure.whiteKing ||
                figure == Figure.whiteQueen ||
                figure == Figure.whiteRook ||
                figure == Figure.whitePawn ||
                figure == Figure.whiteKnight ||
                figure == Figure.whiteBishop) ?
                Color.white : Color.black;
        }
    }
}
