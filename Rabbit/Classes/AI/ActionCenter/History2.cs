using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.AI.ActionCenter
{
     public class History2
    {
        List<Cell2> history;

        public History2()
        {
            history = new List<Cell2>();
        }

        public bool Exists(Cell2 cell)
        {
            for (int i = 0; i < history.Count; i++)
                if (history[i].Tag == cell.Tag)
                    return true;
            return false;
        }

        private int IndexOf(Cell2 cell)
        {
            for (int i = 0; i < history.Count; i++)
                if (history[i].Tag == cell.Tag)
                    return i;
            return -1;
        }

        public void Remove(Cell2 cell)
        {
            int Inx = IndexOf(cell);
            if(Inx > -1)
                history.RemoveAt(Inx);
        }

        public void Add(Cell2 cell)
        {
            if (!Exists(cell))
                history.Add(cell);
        }

        public void Clear() => history.Clear();

        public int Count => history.Count;

        public Cell2 LastAction()
        {
            if(history.Count > 0)
                return history[history.Count - 1];
            return null;
        }
        // add
        // find
    }
}
