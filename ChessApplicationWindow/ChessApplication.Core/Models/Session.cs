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
        public User UserWhite { get; set; }
        public User UserBlack { get; set; }
        public BattleField Battlefield { get; set; }
        
        public void Setup()
        {
            //figures1.Add(new Rook(1, "A", 1));
        }
    }
}
