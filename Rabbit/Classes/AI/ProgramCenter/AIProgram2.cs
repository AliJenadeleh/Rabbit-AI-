using Rabbit.Classes.AI.ActionCenter;
using Rabbit.Classes.BrainFuckEmulator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Rabbit.Classes.AI.ProgramCenter
{
    public class AIProrgam2: IEumulateableProgram
    {
        #region Consts
        private const int EachOperationSize = 5;
        private const string Pattern_Loop_UseLess = @"\[+[.><]*\]+";
        private const string Pattern_Loop_UseLess2 = @"\[\.*\++\.*\]";
        private const string Pattern_Loop_UseLess3 = @"\[\.*\-+\.*\]";
        private readonly int Alpha, MaxSize;
        #endregion

        #region Vars
        private List<char> program;
        //private Regex regex;
        private int MaxValue;
        private string PreFix;
        #endregion

        public AIProrgam2(string Target, int Size, int Alpha, int MaxSize,int MaxValue,int InitInc)
        {
            program = new List<char>();
            this.Alpha = Alpha;
            this.MaxSize = MaxSize;
            this.Size = Size;
            OutputCounter = BeginCounter = EndCounter = 0;
            this.MaxValue = MaxValue + Alpha;
            this.PreFix = "".PadLeft(InitInc,'+');
            IncCounter = Target.Length;
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

        public int ActionCount(char Action)
        {
            switch (Action)
            {
                case StaticsAndDefaults.OperatorBegin:
                    return BeginCounter;
                case StaticsAndDefaults.OperatorEnd:
                    return EndCounter;
                case StaticsAndDefaults.OperatorInc:
                    return IncCounter;
                case StaticsAndDefaults.OperatorDec:
                    return DecCounter;
                case StaticsAndDefaults.OperatorLeft:
                    return LeftCounter;
                case StaticsAndDefaults.OperatorRight:
                    return RightCounter;
                case StaticsAndDefaults.OperatorOutput:
                    return OutputCounter;
                case StaticsAndDefaults.OperatorInput:
                    return InputCounter;
            }
            return -1;
        }

        private void CountersInc(char opt)
        {
            switch (opt)
            {
                case StaticsAndDefaults.OperatorBegin:
                    BeginCounter++;
                    break;
                case StaticsAndDefaults.OperatorEnd:
                    EndCounter++;
                    break;
                case StaticsAndDefaults.OperatorRight:
                    RightCounter++;
                    break;
                case StaticsAndDefaults.OperatorLeft:
                    LeftCounter++;
                    break;
                case StaticsAndDefaults.OperatorInc:
                    IncCounter++;
                    break;
                case StaticsAndDefaults.OperatorDec:
                    DecCounter++;
                    break;
                case StaticsAndDefaults.OperatorOutput:
                    OutputCounter++;
                    break;
                case StaticsAndDefaults.OperatorInput:
                    InputCounter++;
                    break;
            }
        }

        private void CountersDec(char opt)
        {
            switch (opt)
            {
                case StaticsAndDefaults.OperatorBegin:
                    BeginCounter--;
                    break;
                case StaticsAndDefaults.OperatorEnd:
                    EndCounter--;
                    break;
                case StaticsAndDefaults.OperatorRight:
                    RightCounter--;
                    break;
                case StaticsAndDefaults.OperatorLeft:
                    LeftCounter--;
                    break;
                case StaticsAndDefaults.OperatorInc:
                    IncCounter--;
                    break;
                case StaticsAndDefaults.OperatorDec:
                    DecCounter--;
                    break;
                case StaticsAndDefaults.OperatorOutput:
                    OutputCounter--;
                    break;
                case StaticsAndDefaults.OperatorInput:
                    InputCounter--;
                    break;
            }
        }

        public void Add(Cell2 cell)
        {
            program.Add(cell.Tag);
            LastAction = cell;
            CountersInc(cell.Tag);
        }

        public void Change(Cell2 Cell, int Index)
        {

            CountersDec(program[Index]);
            program[Index] = Cell.Tag;
            LastAction = Cell;
            CountersInc(Cell.Tag);
        }

        public void Remove(int Index)
        {
            CountersDec(program[Index]);
            program.RemoveAt(Index);
        }

        public bool CanAddThis(int Index, Cell2 cell)
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
            if (Index > 0)
            return BeginCounter >= EndCounter &&
                    Size > Index &&
                    StaticsAndDefaults.MaxNestLoop > BeginCounter &&
                    program[Index - 1] != StaticsAndDefaults.OperatorBegin &&
                    program[Index - 1] != StaticsAndDefaults.OperatorEnd &&
                    Index < (Size - 3);

            return BeginCounter >= EndCounter &&
                    Size > Index &&
                    StaticsAndDefaults.MaxNestLoop > BeginCounter &&
                    Index < (Size - 3);
        }

        public bool CanAddEnd(int Index)
        {
            return (BeginCounter > EndCounter) &&
                    (Index > 1) &&
                    program[Index - 1] != StaticsAndDefaults.OperatorBegin;
        }

        public bool CanAddLeft(int Index)
        {
            if ((LeftCounter < RightCounter) &&
                (Index > 0 && Index < Size - 1) &&
                    (StaticsAndDefaults.OperatorRight != program[Index - 1]))
            {
                return true;
            }

            return false;
        }

        public bool CanAddRight(int Index)
        {
            if (Index < Size - 1)
                if (Index > 0 && RightCounter < Size / 3 && StaticsAndDefaults.OperatorLeft != program[Index - 1])
                    return true;
            return false;
        }

        public bool CanAddPlus(int Index)
        {
            return (Index > 0 && StaticsAndDefaults.OperatorDec != program[Index - 1]);
        }

        public bool CanAddSub(int Index)
        {
            return 1 < Index &&
                StaticsAndDefaults.OperatorInc != program[Index - 1] ;
        }

        public bool CanAddPrint(int Index)
        {
            //Index > Size / 2  && 
            return (Index > 0 && this[Index - 1] != StaticsAndDefaults.OperatorOutput);
        }

        private bool OkCounters()
        {
            return ((BeginCounter - EndCounter) == 0 && (RightCounter >= LeftCounter));
        }

        private bool OkPatterns()
        {
            string script = GetProgram();
            return !Regex.IsMatch(script, Pattern_Loop_UseLess) &&
                !Regex.IsMatch(script, Pattern_Loop_UseLess2) &&
                !Regex.IsMatch(script,Pattern_Loop_UseLess3);
            /*
            regex = new Regex(Pattern_Loop_UseLess);
            return !regex.IsMatch(GetProgram()) ;
            */
        }

        public bool IsOkToRun()
        {
            return OkCounters() && OkPatterns() && OutputCounter > 0 && BeginCounter > 0;
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

        public string CounterDetails() =>
          $"<:{LeftCounter} >:{RightCounter} , [ : {BeginCounter}  ] : {EndCounter} , + : {IncCounter} - : {DecCounter} , . : {OutputCounter} , :{InputCounter}";        

        #endregion

        #region Fields
        public int BeginCounter { get; private set; }
        public int EndCounter { get; private set; }
        public int RightCounter { get; private set; }
        public int LeftCounter { get; private set; }
        public int IncCounter { get; private set; }
        public int DecCounter { get; private set; }
        public int OutputCounter { get; private set; }
        public int InputCounter { get; private set; }
        public int Count => program.Count;
        public int Size { get; private set; }
        public string GetProgram() => $"{PreFix}{new string(program.ToArray())}";
        public char this[int Index] { get => program[Index]; }
        public Cell2 LastAction { get; private set; }
        #endregion
    }
}
