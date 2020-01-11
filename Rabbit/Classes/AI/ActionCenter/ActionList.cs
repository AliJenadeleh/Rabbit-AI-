using System;
using System.Text;
using System.Linq;
using Rabbit.Classes.BF;
using System.Collections.Generic;
using Rabbit.Classes.AI.ProgramCenter;

namespace Rabbit.Classes.AI.ActionCenter
{
    public class ActionList
    {
        #region Vars
        private List<ActionCell> Actions;
        #endregion

        #region PublicFields
        public int Count { get => Actions.Count; }
        #endregion

        #region PrivateMethods

        private void Init(int ProgramStartLength,int Alpha, string Target)
        {
            Actions.Add(new ActionCell(BrainFuck.OperatorAdd, ProgramStartLength));
            Actions.Add(new ActionCell(BrainFuck.OperatorSub, Alpha));
            Actions.Add(new ActionCell(BrainFuck.OperatorBegin, Target.Length));
            Actions.Add(new ActionCell(BrainFuck.OperatorEnd, Target.Length));
            Actions.Add(new ActionCell(BrainFuck.OperatorLeft, Target.Length));
            Actions.Add(new ActionCell(BrainFuck.OperatorRight, Target.Length));
            Actions.Add(new ActionCell(BrainFuck.OperatorPrint, Target.Length));
            //Actions.Add(new ActionCell(BrainFuck.OperatorInput, 0));
        }

        public void SetReward(ActionCell cell,int Reward = -1)
        {
            for (int i = 0; i < Actions.Count; i++)
                if (Actions[i].ActionTag == cell.ActionTag)
                    Actions[i].SetReward(Reward);
            /*
            var c = Actions.Where(a => a.ActionTag == cell.ActionTag).FirstOrDefault();
            c.SetReward(Reward);
            */
        }

        private bool IsOkAction(int Position, ActionHistory History, AIProrgam Program,ActionCell cell)
        {
            if (!History.HasCell(cell))
            {
                switch (cell.ActionTag)
                {
                    case BrainFuck.OperatorAdd:
                        return Program.CanAddPlus(Position);
                    case BrainFuck.OperatorSub:
                        return Program.CanAddSub(Position);
                    case BrainFuck.OperatorBegin:
                        return Program.CanAddBegin(Position);
                    case BrainFuck.OperatorEnd:
                        return Program.CanAddEnd(Position);
                    case BrainFuck.OperatorLeft:
                        return Program.CanAddLeft(Position);
                    case BrainFuck.OperatorRight:
                        return Program.CanAddRight(Position);
                    case BrainFuck.OperatorPrint:
                        return Program.CanAddPrint(Position);
                    case BrainFuck.OperatorInput:
                        throw new Exception("Operation Input Not Completed yet.");
                        //return CheckAdd(Position, History, Program, cell);
                }//switch
            }// if history

            return false;
        }
        #endregion

        #region PublicMethods

        public ActionList(int ProgramStartLength,int Alpha,string Target)
        {
            Actions = new List<ActionCell>();
            Init(ProgramStartLength,Alpha,Target);
        }

        public ActionCell TakeAnAction(int Position,ActionHistory History,AIProrgam Program)
        {
            ActionCell cell = null;
            var q = Actions.OrderByDescending(a => a.Reward);
            for(int i = 0; i < q.Count(); i++)
            {
                cell = q.Skip(i).FirstOrDefault();
                if (IsOkAction(Position, History, Program, cell))
                {
                    //SetReward(cell);
                    History.Add(cell);
                    return cell;
                }
            }

            return null ;
        }

        #endregion
    }
}
