using Rabbit.Classes.AI.ProgramCenter;
using Rabbit.Classes.BrainFuckEmulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rabbit.Classes.AI.ActionCenter
{
    public class List2
    {
        private List<Cell2> actions;

        public List2()
        {
            actions = new List<Cell2>();
        }

        public bool Exists(Cell2 cell)
        {
            for (int i = 0; i < actions.Count; i++)
                if (actions[i].Tag == cell.Tag)
                    return true;

            return false;
        }

        public int IndexOf(Cell2 cell)
        {
            for (int i = 0; i < actions.Count; i++)
                if (actions[i].Tag == cell.Tag)
                    return i;

            return -1;
        }

        public int IndexOf(char Tag)
        {
            for (int i = 0; i < actions.Count; i++)
                if (actions[i].Tag == Tag)
                    return i;

            return -1;
        }

        public bool Add(Cell2 cell)
        {
            if (Exists(cell))
                return false;

            actions.Add(cell);
            return true;
        }

        public Cell2 GetCell(int Index)
        {
            return actions[Index];
        }

        public void Reward(Cell2 cell,int reward)
        {
            int inx = IndexOf(cell);
            actions[inx].Reward(reward);
        }

        public void Reward(Cell2 cell, int reward,int count)
        {
            int inx = IndexOf(cell);
            actions[inx].Reward(reward * count);
        }

        public void Reward(char Tag, int reward)
        {
            int inx = IndexOf(Tag);
            actions[inx].Reward(reward);
        }


        public void Reward(AIProrgam Program,int reward)
        {
            Reward(StaticsAndDefaults.OperatorInc, Program.AddCounter * reward);
            Reward(StaticsAndDefaults.OperatorDec, Program.SubCounter * reward);
            Reward(StaticsAndDefaults.OperatorBegin, Program.BeginCounter * reward);
            Reward(StaticsAndDefaults.OperatorEnd, Program.EndCounter * reward);
            Reward(StaticsAndDefaults.OperatorOutput, Program.PrintCounter * reward);
            //Reward(StaticsAndDefaults.OperatorInput, Program.InputCounter * reward);
            Reward(StaticsAndDefaults.OperatorLeft, Program.LeftCounter * reward);
            Reward(StaticsAndDefaults.OperatorRight, Program.RightCounter * reward);
        }

        public IEnumerator<Cell2> GetEnumerator()
        {
            for (int i = 0; i < actions.Count; i++)
                yield return actions[i];
            yield break;
        }

        public int Count => actions.Count;
        public IEnumerable<Cell2> OrderedActions => actions.OrderByDescending(a => a.Score);
        public IEnumerable<Cell2> Actions => this.actions;
    }
}
