using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApplication.Core.Models
{
    public interface ISession
    {
        void Setup();
    }
    public class Session : ISession
    {
        List<IFigure> figures1 = new List<IFigure>();
        public void Setup()
        {
            //figures1.Add(new Rook(1, "A", 1));
        }
    }
}
