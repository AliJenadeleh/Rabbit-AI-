using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Rabbit.Classes.AI.ActionCenter
{
    public class ActionHistory
    {
        List<ActionCell> history;

        public ActionHistory()
        {
            history = new List<ActionCell>();
        }

        public bool HasCell(ActionCell cell)
        {
            for (int i = 0; i < history.Count; i++)
                if (history[i].ActionTag == cell.ActionTag)
                    return true;
            return false;
        }

        private int IndexOf(ActionCell cell)
        {
            for (int i = 0; i < history.Count; i++)
                if (history[i].ActionTag == cell.ActionTag)
                    return i;
            return -1;
        }

        public void Remove(ActionCell cell)
        {
            int Inx = IndexOf(cell);
            if(Inx > -1)
                history.RemoveAt(Inx);
        }

        public void Add(ActionCell cell)
        {
            if (!HasCell(cell))
                history.Add(cell);
        }

        public void Clear() => history.Clear();

        public ActionCell LastAction()
        {
            if(history.Count > 0)
                return history[history.Count - 1];
            return null;
        }
        // add
        // find
    }
}
