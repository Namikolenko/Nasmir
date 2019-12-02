using ChessApplication.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApplication.Core.Models
{
    public interface IFigure : IRepository<Figure>
    {
        void Movement();
    }
    public class Figure : IFigure
    {
        public FigurePosition Position { get; set; }
        public int Index { get; set; }

        protected List<Figure> items = new List<Figure>();
        public IEnumerable<Figure> Items => items;

        public void Add(Figure item)
        {
            items.Add(item);
        }

        public void Remove(Figure item)
        {
            items.Remove(item);
        }

        public virtual void Movement()
        {
            //throw new NotImplementedException();
        }

        public Figure(int index, string posLetter, int posNumber)
        {
            Index = index;
            Position = new FigurePosition(posLetter, posNumber);
        }
    }

    public class Rook : Figure
    {
        public Rook(int index, string posLetter, int posNumber) : base(index, posLetter, posNumber)
        {
            Index = index;
            Position = new FigurePosition(posLetter, posNumber);
        }

        public override void Movement()
        {
            //ЛОГИКА ДВИЖЕНИЯ ПЕШКИ
        }
    }

    public class Pawn : Figure
    {
        public Pawn(int index, string posLetter, int posNumber) : base(index, posLetter, posNumber)
        {
            Index = index;
            Position = new FigurePosition(posLetter, posNumber);
        }

        public override void Movement()
        {
            //ЛОГИКА ДВИЖЕНИЯ ПЕШКИ
        }
    }

    public class Knight : Figure
    {
        public Knight(int index, string posLetter, int posNumber) : base(index, posLetter, posNumber)
        {
            Index = index;
            Position = new FigurePosition(posLetter, posNumber);
        }

        public override void Movement()
        {
            //ЛОГИКА ДВИЖЕНИЯ ПЕШКИ
        }
    }

    public class Bishop : Figure
    {
        public Bishop(int index, string posLetter, int posNumber) : base(index, posLetter, posNumber)
        {
            Index = index;
            Position = new FigurePosition(posLetter, posNumber);
        }

        public override void Movement()
        {
            //ЛОГИКА ДВИЖЕНИЯ ПЕШКИ
        }
    }

    public class King : Figure
    {
        public King(int index, string posLetter, int posNumber) : base(index, posLetter, posNumber)
        {
            Index = index;
            Position = new FigurePosition(posLetter, posNumber);
        }

        public override void Movement()
        {
            //ЛОГИКА ДВИЖЕНИЯ ПЕШКИ
        }
    }

    public class Queen : Figure
    {
        public Queen(int index, string posLetter, int posNumber) : base(index, posLetter, posNumber)
        {
            Index = index;
            Position = new FigurePosition(posLetter, posNumber);
        }

        public override void Movement()
        {
            //ЛОГИКА ДВИЖЕНИЯ ПЕШКИ
        }
    }
}
