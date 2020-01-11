using Rabbit.Classes.AI.ActionCenter;
using Rabbit.Classes.AI.ProgramCenter;
using Rabbit.Classes.BrainFuckEmulator;
using Rabbit.Classes.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Rabbit.Classes.AI.RL
{
    public class Brain3
    {
        private readonly string Target;
        private List2 Actions;        
        private AIProgram3 Program;
        private Emulator emulator;
        private int InitLength, IncLength, MaxLength;
        private void InitOperators()
        {
            Actions = new List2();
            Actions.Add(new Cell2(StaticsAndDefaults.OperatorInc, Target.Length));
            Actions.Add(new Cell2(StaticsAndDefaults.OperatorOutput, Target.Length));
            Actions.Add(new Cell2(StaticsAndDefaults.OperatorDec, Target.Length / 2));
            //Actions.Add(new Cell2(StaticsAndDefaults.OperatorDec, Target.Length));
        }

        private void InitValues()
        {
            int sum = Target.Length;
            foreach (char c in Target)
                sum += (int)c;

            MaxLength = sum;
            IncLength = sum / Target.Length;
            InitLength = MaxLength;
        }

        public Brain3(string Target)
        {
            this.Target = Target;
            InitValues();
            InitOperators();
            emulator = new Emulator(Verbose: false,ShowOutput:false);
            Program = new AIProgram3(Target, InitLength, IncLength);
        }
        
        private Cell2 TakeAction(List2 Actions)
        {
            if (Actions.Count > 0)
            {
                var cell = Actions[0];
                Actions.Remove(0);
                return cell;
            }
            return null;
        }

        private bool _Start(int Position)
        {
            if(Position <= InitLength)
            {
                var actions = Actions.Clone();
                Cell2 cell = new Cell2();
                Program.Add(cell);
                while ((cell = TakeAction(actions)) != null
                    && Program.CanAddThis(Position, cell))
                {

                    Program.Change(cell, Position);

                    if (Position >= InitLength)
                    {
                        if (Program.IsOkToRun() && Program.OutputCounter == Target.Length)
                        {
                            var result = emulator.TryToEmulate(Program, Target);

                            switch (result)
                            {
                                case EResult.Match:
                                    Console.WriteLine("Match Found :)");
                                    return true;
                                case EResult.MatchWithError:
                                    Console.WriteLine("Match Found (with issue) :)");
                                    return true;
                                case EResult.NotMatch:
                                    Console.WriteLine("Match Not Found :(  [go next]");
                                    break;
                                case EResult.Error:
                                    Console.WriteLine("Error happend :(  [go next]");
                                    break;
                            }
                            Console.WriteLine(emulator.ToString());
                        }

                    }
                    else
                    {
                        bool result = _Start(Position + 1);
                        if (result)
                            return result;
                    }

                }//while

            }
            Program.Remove(Position);
            return false;
        }// ----------------------------------------- ()

        public string Start()
        {
            var sw = new Stopwatch();
            sw.Start();
            if (_Start(0))
            {
                sw.Stop();
                var cell = new StorageCell() { Script = Program.GetProgram(), Target = Target, MS = sw.ElapsedMilliseconds,BrainVersion=3 };
                var stg = new StorageDB();
                stg.Add(cell);
                stg.Save();
                Console.WriteLine("Result saved.");

                return Program.GetProgram();
            }

            return string.Empty;
        }
    }
}
