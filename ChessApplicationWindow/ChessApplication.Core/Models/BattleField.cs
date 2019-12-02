using ChessApplication.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessApplication.Core.Models
{
    public interface IBattlefield : IRepository<Figure>
    {

    }
    public class BattleField : IBattlefield
    {
        private List<Figure> items = new List<Figure>();
        public IEnumerable<Figure> Items => items;

        public void Add(Figure item)
        {
            items.Add(item);
        }

        public void Remove(Figure item)
        {
            items.Remove(item);
        }
    }
}
