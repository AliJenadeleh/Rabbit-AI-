using Rabbit.Classes.AI.ActionCenter;
using Rabbit.Classes.BF;
using Rabbit.Classes.BrainFuckEmulator;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Rabbit.Classes.AI.ProgramCenter
{
    public class AIProrgam
    {
        #region Consts
        private const int EachOperationSize = 5;
        private const string Pattern_Loop_UseLess = @"\[+[.><]*\]+";
        private readonly int Alpha, MaxSize;
        #endregion

        #region Vars
        private List<char> program;
        private Regex regex;
        #endregion

        public AIProrgam(string Target,int Size,int Alpha,int MaxSize)
        {
            program = new List<char>();
            this.Alpha = Alpha;
            this.MaxSize = MaxSize;
            this.Size = Size;
            BeginCounter = EndCounter = 0;
        }

        #region Methods

        public void IncSize(int Bias = 0)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("***************Size Change*********************");
            Console.WriteLine($"Current Size : {Size}");
            Size += Alpha;
            Console.WriteLine($"New Size : {Size}");
            Console.WriteLine("***************Size Change*********************");
            Console.ForegroundColor = color;
        }

        private void CheckCountersInc(char opt)
        {
            switch (opt) {
                case BrainFuck.OperatorBegin:
                    BeginCounter++;
                    break;
                case BrainFuck.OperatorEnd:
                    EndCounter++;
                    break;
                case BrainFuck.OperatorRight:
                    RightCounter++;
                    break;
                case BrainFuck.OperatorLeft:
                    LeftCounter++;
                    break;
                case BrainFuck.OperatorAdd:
                    AddCounter++;
                    break;
                case BrainFuck.OperatorSub:
                    SubCounter++;
                    break;
                case BrainFuck.OperatorPrint:
                    PrintCounter++;
                    break;
                case BrainFuck.OperatorInput:
                    InputCounter++;
                    break;
            }   
        }

        private void CheckCountersDec(char opt)
        {
            switch (opt)
            {
                case BrainFuck.OperatorBegin:
                    BeginCounter--;
                    break;
                case BrainFuck.OperatorEnd:
                    EndCounter--;
                    break;
                case BrainFuck.OperatorRight:
                    RightCounter--;
                    break;
                case BrainFuck.OperatorLeft:
                    LeftCounter--;
                    break;
                case BrainFuck.OperatorAdd:
                    AddCounter--;
                    break;
                case BrainFuck.OperatorSub:
                    SubCounter--;
                    break;
                case BrainFuck.OperatorPrint:
                    PrintCounter--;
                    break;
                case BrainFuck.OperatorInput:
                    InputCounter--;
                    break;
            }
        }

        public void Add(ActionCell cell)
        {
            program.Add(cell.ActionTag);
            LastAction = cell;
            CheckCountersInc(cell.ActionTag);
        }

        public void Change(ActionCell Cell,int Index)
        {

            CheckCountersDec(program[Index]);
            program[Index] = Cell.ActionTag;
            LastAction = Cell;
            CheckCountersInc(Cell.ActionTag);
        }

        public void Remove(int Index)
        {
            CheckCountersDec(program[Index]);
            program.RemoveAt(Index);
        }

        public bool CanAddThis(int Index,Cell2 cell)
        {
            switch (cell.Tag)
            {
                case StaticsAndDefaults.OperatorBegin:
                    return CanAddBegin(Index);
                case StaticsAndDefaults.OperatorEnd:
                    return CanAddEnd(Index);
                case StaticsAndDefaults.OperatorInc:
                    return CanAddPlus(Index);
                case StaticsAndDefaults.OperatorDec:
                    return CanAddSub(Index);
                case StaticsAndDefaults.OperatorLeft:
                    return CanAddLeft(Index);
                case StaticsAndDefaults.OperatorRight:
                    return CanAddRight(Index);
                case StaticsAndDefaults.OperatorOutput:
                    return CanAddPrint(Index);
                case StaticsAndDefaults.OperatorInput:
                     //CanAddI(Index);
                default:
                    return false;
            }
        }

        public bool CanAddBegin(int Index)
        {
            return  BeginCounter >= EndCounter &&
                    Size > Index &&
                    BrainFuck.DefaultNestLoop > BeginCounter &&
                    Index < (Size - 3);
        }

        public bool CanAddEnd(int Index)
        {
            return (BeginCounter > EndCounter) &&
                    (Index > 1) &&
                    program[Index - 1] != BrainFuck.OperatorBegin;
        }

        public bool CanAddLeft(int Index)
        {
            if (Index > 0 && Index < Size)
                if (RightCounter > LeftCounter)
                    if (program[Index - 1] != BrainFuck.OperatorRight)
                        return true;

            return false;
        }

        public bool CanAddRight(int Index)
        {
            if (Index < Size - 1)
                if (Index > 0 && BrainFuck.OperatorLeft != program[Index - 1])
                    return true;
            return false;
        }

        public bool CanAddPlus(int Index)
        {
            return !((Index > 0 && BrainFuck.OperatorSub != program[Index - 1]) && Index >= Size - 1);
        }

        public bool CanAddSub(int Index)
        {
            return 1 < Index &&
                BrainFuck.OperatorAdd != program[Index - 1] &&
                Index < Size - 1;
        }

        public bool CanAddPrint(int Index)
        {
            return (Index > 0);
        }

        private bool OkCounters()
        {
            return ((BeginCounter - EndCounter) == 0);
        }

        private bool OkPatterns()
        {
            regex = new Regex(Pattern_Loop_UseLess);
            return ! regex.IsMatch(GetProgram());
        }

        public bool IsOkToRun()
        {
             return OkCounters() && OkPatterns();
        }

        public void Verbose()
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("***************VS*********************");
            Console.WriteLine(new string(program.ToArray()));
            Console.WriteLine($"Begin:{BeginCounter} | End:{EndCounter} | Size : {Size}");
            Console.WriteLine("***************VE*********************");
            Console.ForegroundColor = color;
        }
        #endregion

        #region Fields
        public int BeginCounter { get; private set; }
        public int EndCounter { get; private set; }
        public int RightCounter { get; private set; }
        public int LeftCounter { get; private set; }
        public int AddCounter { get; private set; }
        public int SubCounter { get; private set; }
        public int PrintCounter { get; private set; }
        public int InputCounter { get; private set; }
        public int Count => program.Count;
        public int Size { get; private set; }
        public string GetProgram() => new string(program.ToArray());
        public char this[int Index]{get => program[Index];}
        public ActionCell LastAction { get; private set; }
        #endregion
    }
}
