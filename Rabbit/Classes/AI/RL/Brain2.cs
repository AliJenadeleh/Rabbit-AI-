using Rabbit.Classes.AI.ActionCenter;
using Rabbit.Classes.AI.ProgramCenter;
using Rabbit.Classes.BrainFuckEmulator;
using Rabbit.Classes.BrainFuckEmulator.Exceptions;
using Rabbit.Classes.Storage;
using System;
using System.Diagnostics;
using System.Linq;

namespace Rabbit.Classes.AI.RL
{
    public class Brain2
    {
        private readonly string Target;
        private readonly int TargetLength;
        private int ProgramMaxLength;
        private int ProgramInitLegth;
        private int ProgramMinLength;
        private int ProgramLengthInc;
        private int ProgramLength;
        private int InitPlusScore;
        private int ErrorPosition;

        private List2 Actions;
        //private History2 History;
        private AIProrgam2 Program;
        private Emulator emulator;

        #region Initial
        private void InitVars()
        {
            int Max = (int)Target[0];
            int Min = Max;
            int sum = Max,tmp;
            for(int i=1;i<TargetLength;i++)
            {
                tmp = (int)Target[i] + 2;
                sum += tmp;
                if (tmp > Max)
                    Max = tmp;
                else if (tmp < Min)
                    Min = tmp;
            }
            int avg = sum / TargetLength;

            ProgramMaxLength = sum;
            ProgramLengthInc = avg / 2;
            ProgramLength = ProgramInitLegth = avg + ProgramLengthInc;
            ProgramMinLength = Min;
            InitPlusScore = Max;
        }// InitVars();

        private void InitActionList()
        {
            Actions = new List2();

            Actions.Add(new Cell2(StaticsAndDefaults.OperatorInc, InitPlusScore));
            Actions.Add(new Cell2(StaticsAndDefaults.OperatorDec, ProgramInitLegth / 2));
            Actions.Add(new Cell2(StaticsAndDefaults.OperatorOutput, TargetLength));
            //Actions.Add(new Cell2(StaticsAndDefaults.OperatorInput, 0));
            Actions.Add(new Cell2(StaticsAndDefaults.OperatorBegin,TargetLength * 2));
            Actions.Add(new Cell2(StaticsAndDefaults.OperatorRight, TargetLength * 2));
            Actions.Add(new Cell2(StaticsAndDefaults.OperatorLeft, TargetLength));
            Actions.Add(new Cell2(StaticsAndDefaults.OperatorEnd, TargetLength));
        }//InitActionList();
        /*
        private void InitHistory()
        {
            //History = new History2();
        }
        */
        private void InitProgram()
        {
            Program = new AIProrgam2(Target, ProgramInitLegth, ProgramLengthInc, ProgramMaxLength);
        }

        private void InitEmulator()
        {
            emulator = new Emulator(Verbose: false,ShowOutput:false);
        }
        #endregion

    
        public Brain2(string Target)
        {
            this.Target = Target;
            this.TargetLength = Target.Length;

            InitVars();
            InitActionList();
            //InitHistory();
            InitProgram();
            InitEmulator();
        }//Brain2()

        #region MainPart


        private Cell2 TakeAnAction(History2 history, int Position)
        {
            // add thinking 

            var query = from a in Actions.Actions
                        where !history.Exists(a) &&
                        Program.CanAddThis(Position,a)
                        orderby a.Score descending
                        select a;

            /*
            for(int i=0;i<query.Count();i++)
            {
                cell = query.Skip(i).FirstOrDefault<Cell2>();

                if (cell != null && 
                    Program.CanAddThis(Position, cell))
                    return cell;
            }// for

            */

            return query.FirstOrDefault<Cell2>();
        }//TakeAnAction();

        private bool _Start(int Position)
        {
            if(Position <= ProgramLength)
            {
                var cell = new Cell2();
                var history = new History2();
                //string script;
                ErrorPosition = -1;
                Program.Add(cell);

                while((cell = TakeAnAction(history,Position))!= null)
                {
                    ErrorPosition = -1;
                    Program.Change(cell, Position);
                    history.Add(cell);

                    if (Program.IsOkToRun())
                    {

                        //script = Program.GetProgram();

                        //Console.WriteLine($"Try to Emulate {Position} : {script}");

                        //Console.WriteLine($"<:{Program.LeftCounter} | >:{Program.RightCounter}");
                        var result = emulator.TryToEmulate(Program, Target);


                        if (result == EResult.Match)
                            return true;
                        else if(result == EResult.NotMatch)
                        {
                            Actions.Reward(cell, 1, Program.ActionCount(cell.Tag));
                            if (_Start(Position + 1))
                            {
                                return true;
                            }// if(_start)
                        }else if (result == EResult.Error)
                        {

                            if(emulator.LocalErrorType == ErrorType.MemoryOutOfRange)
                            {
                                ErrorPosition = emulator.ProgramIndex;
                                if (ErrorPosition < Position) return false;
                            }else if (emulator.LocalErrorType == ErrorType.LoopRunOutOfRange)
                            {
                                ErrorPosition = emulator.ProgramIndex + 1;
                                if (ErrorPosition < Position) return false;
                            }


                            // detect error pattern
                            if (_Start(Position + 1))
                            {
                                return true;
                            }// if(_start)
                        }
                        else if (result == EResult.MatchWithError)
                        {
                            return true;
                        }

                    }//if
                    else
                    {
                        if(_Start(Position + 1))
                        {
                            return true;
                        }// if(_start)

                    }// else

                    if (ErrorPosition != -1 && ErrorPosition <  Position)
                            return false;
                            
                }//while

                Program.Remove(Position);

            }//pos < PLen
            

            /*
            if(Position == 0)
            {
                // inc size
            }
            */

            return false;
        }// _Start();

        public string Start()
        {
            // develop take an action
            // calc rewards
            // 
            var sw = new Stopwatch();
            sw.Start();
            if(_Start(0))
            {
                sw.Stop();
                var cell = new StorageCell() {Script = Program.GetProgram(),Target = Target ,MS = sw.ElapsedMilliseconds};
                var stg = new StorageDB();
                stg.Add(cell);
                stg.Save();
                Console.WriteLine("Result saved.");

                return Program.GetProgram();
            }

            return string.Empty;
        }//Start();

        #endregion
    }
}
