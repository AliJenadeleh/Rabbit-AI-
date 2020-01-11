using Rabbit.Classes.AI.ActionCenter;
using Rabbit.Classes.AI.ProgramCenter;
using Rabbit.Classes.BF;
using Rabbit.Classes.BrainFuckEmulator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.AI.RL
{
    public class Brain
    {
        private readonly string Target;
        //private List<ActionHistory> HistoryList;
        private ActionList ActionList;
        private AIProrgam Program;
        private BrainFuck BF;
        private int ProgramMaxLength,ProgramLengthInc,ProgramSize;

        private void Thinking()
        {
            int len = (int)Target[0], tmp, sum;
            sum = len;
            for (int i = 1; i < Target.Length; i++)
            {
                tmp = (int)Target[i];
                if (tmp > len)
                    len = tmp;
                sum += tmp;
            }

            sum += Target.Length * 2;
            int avg = sum / Target.Length;


            ProgramMaxLength = sum;
            ProgramLengthInc = avg;
            ProgramSize = len + (avg / 2);

            Program = new AIProrgam(Target, ProgramSize, ProgramLengthInc, ProgramMaxLength);
            ActionList = new ActionList(ProgramSize,ProgramLengthInc / 2,Target);
        }//Thinking();

        public Brain(string Target)
        {
            //HistoryList = new List<ActionHistory>();
            this.Target = Target;
            Thinking();
            BF = new BrainFuck(Verbose:false);
        }

        private void IncSize()
        {
            if (ProgramSize < ProgramMaxLength)
            {
                ProgramSize += ProgramLengthInc;
                Program.IncSize();
            }
        }

        private bool _Start(int Step)
        {
            if (Step < ProgramSize)
            {
                Program.Add(new ActionCell('\0', 0));

                var history = new ActionHistory();
                ActionCell cell;
                while ((cell = ActionList.TakeAnAction(Step, history, Program)) != null)
                {
                    Program.Change(cell, Step);
                    var result = BF.TryRun(Program.GetProgram(), Target);
                    if (result == BFResult.Fail)
                    {
                        ActionList.SetReward(cell, -1);
                        if (_Start(Step + 1))
                            return true;
                    }
                    else if (result == BFResult.RunOk)
                    {
                        ActionList.SetReward(cell, 1);
                        if (_Start(Step + 1))
                            return true;
                    }
                    else if (result == BFResult.ResultMatch)
                        return true;
                }

                Program.Remove(Step);
                if (Step == 0)
                    IncSize();
            }
            return false;
        }//_Start();

        public string Start()
        {
            if (_Start(0))
                return Program.GetProgram();
            return string.Empty;
        }
    }
}
