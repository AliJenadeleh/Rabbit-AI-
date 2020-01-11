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
        private const int MUL = 10;
        private readonly string Target;
        private readonly int TargetLength;
        private readonly bool Verbose;
        private int ProgramMaxLength;
        private int ProgramInitLegth;
        private int ProgramMinLength;
        private int ProgramAvgLength;
        private int ProgramLengthInc;
        private int ProgramLength;
        private int InitPlusScore;
        private int ErrorPosition;
        private Random rnd = new Random();

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
            ProgramAvgLength = avg;
            ProgramMaxLength = sum;
            ProgramLengthInc = avg / 2;
            //ProgramLength = ProgramInitLegth = sum + Target.Length;
            //ProgramLength = ProgramInitLegth = avg + ProgramLengthInc;
            ProgramLength = 150 - ProgramLengthInc;//  ProgramInitLegth = avg / 2;
            ProgramInitLegth = 150 - ProgramLengthInc; // avg / 2;
            ProgramMinLength = Min;
            InitPlusScore = Max;
        }// InitVars();

        private void InitActionList()
        {
            Actions = new List2();

            Actions.Add(new Cell2(StaticsAndDefaults.OperatorBegin, InitPlusScore   * MUL));
            Actions.Add(new Cell2(StaticsAndDefaults.OperatorEnd, (TargetLength / 2) * MUL));
            Actions.Add(new Cell2(StaticsAndDefaults.OperatorRight, TargetLength * MUL ));
            Actions.Add(new Cell2(StaticsAndDefaults.OperatorLeft, (TargetLength ) * MUL ));
            Actions.Add(new Cell2(StaticsAndDefaults.OperatorInc, (ProgramInitLegth / 2) * MUL));
            Actions.Add(new Cell2(StaticsAndDefaults.OperatorDec, (ProgramInitLegth / 4) * MUL));
            Actions.Add(new Cell2(StaticsAndDefaults.OperatorOutput, TargetLength * MUL));
            //Actions.Add(new Cell2(StaticsAndDefaults.OperatorInput, 0));
            //Actions.ReOrder();
        }//InitActionList();
     
        private void InitProgram()
        {
            Program = new AIProrgam2(Target, ProgramInitLegth, ProgramLengthInc, ProgramMaxLength,InitPlusScore,ProgramAvgLength);
        }

        private void InitEmulator()
        {
            emulator = new Emulator(Verbose: false,ShowOutput:false,MaxLoopRun:TargetLength * ProgramLengthInc);
        }
        #endregion

    
        public Brain2(string Target,bool Verbose = false)
        {
            this.Verbose = Verbose;
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

            //if (history.Count > 0 || Position == 0)
            //{

                var query = from a in Actions.Actions
                            where !history.Exists(a) &&
                            Program.CanAddThis(Position, a)
                            orderby a.Score descending
                            select a;

                return query.FirstOrDefault<Cell2>();
            //}
            //else
            //{
            //    var tmplist = Actions.Clone();
            //    Cell2 tag;
            //    do
            //    {
            //        if (tmplist.Count == 1) return tmplist[0];

            //        int inx = rnd.Next(0, tmplist.Count);
            //        tag = tmplist[inx];
            //        tmplist.Remove(inx);
            //    } while (!Program.CanAddThis(Position, tag));
            //    return tag;
            //}
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
                            int any = emulator.AnyInOutPut(Target);
                            if(any > 0)
                            {
                                Actions.Reward(Program,1);
                            }
                            else
                            {
                                Actions.Reward(Program, -1);
                            }
                            if (_Start(Position + 1))
                            {
                                return true;
                            }// if(_start)
                        }else if (result == EResult.Error)
                        {
                            Actions.Reward(Program, -1);

                            if (emulator.LocalErrorType == ErrorType.MemoryOutOfRange)
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
                var cell = new StorageCell() {Script = Program.GetProgram(),Target = Target ,MS = sw.ElapsedMilliseconds,BrainVersion=2};
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
