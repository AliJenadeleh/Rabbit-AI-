using Rabbit.Classes.BrainFuckEmulator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.BF
{
    class BrainFuck
    {
        private int memorySize, inx, pos; // inx = Memory Index , pos = Program Position
        private int nestLoop, maxLoop, maxLoopRun,loopinx,nestLoopCounter;
        private byte[] Memory;
        private int[] loopCounter;
        private string program;
        private StringBuilder PrintHistory;
        private readonly bool Verbose;


        #region PublicConst
        public const char OperatorBegin = StaticsAndDefaults.OperatorBegin;
        public const char OperatorEnd   = StaticsAndDefaults.OperatorEnd;
        public const char OperatorAdd   = StaticsAndDefaults.OperatorInc;
        public const char OperatorSub   = StaticsAndDefaults.OperatorDec;
        public const char OperatorLeft  = StaticsAndDefaults.OperatorLeft;
        public const char OperatorRight = StaticsAndDefaults.OperatorRight;
        public const char OperatorPrint = StaticsAndDefaults.OperatorOutput;
        public const char OperatorInput = StaticsAndDefaults.OperatorInput;

        public const int OperatorBeginCode = 101;
        public const int OperatorEndCode = 102;
        public const int OperatorAddCode = 103;
        public const int OperatorSubCode = 104;
        public const int OperatorLeftCode = 105;
        public const int OperatorRightCode = 106;
        public const int OperatorPrintCode = 107;
        public const int OperatorInputCode = 108;

        public const int OperatorInputInit  = 0900;
        public const int OperatorLeftInit   = 1010;
        public const int OperatorRightInit  = 1030;
        public const int OperatorEndInit    = 1040;
        public const int OperatorPrintInit  = 1050;
        public const int OperatorSubInit    = 1060;
        public const int OperatorBeginInit  = 1070;
        public const int OperatorAddInit    = 1080;

        public const int DefaultNestLoop = StaticsAndDefaults.MaxNestLoop;
        #endregion

        private void Init(string Program)
        {
            pos = inx = 0;
            loopinx = -1;

                PrintHistory = new StringBuilder();

            program = Program;
            memorySize = program.Length * 10;
            maxLoop = memorySize / 3;
            Memory = new byte[memorySize];
            loopCounter = new int[maxLoop];
        }

        public BrainFuck(int NestLoop=DefaultNestLoop,int MaxLoopRun=255 ,bool Verbose = true)
        {
            this.nestLoop = NestLoop;
            this.maxLoopRun = MaxLoopRun;
            nestLoopCounter = -1;
            this.Verbose = Verbose;
        }
        
        private void MoveIndex(int value)
        {
            if (inx + value > memorySize || inx + value < 0)
                throw new OutOfMemoryException();
            inx += value;
        }

        private void Output()
        {
            char ch = (char)Memory[inx];
            PrintHistory.Append(ch);

            if(Verbose)
                Console.WriteLine($"char : {ch} , byte : {Memory[inx]}");
        }

        private void BeginWhile()
        {
            if(Memory[inx] == 0)
            {
                int indent = 1;
                
                do
                {
                    pos++;
                    if (program[pos] == '[')
                        indent++;
                    else if (program[pos] == ']')
                        indent--;
                } while (indent > 0);
                loopCounter[loopinx + 1] = 0;
            }
            else
            {
                Memory[inx]--;

                nestLoopCounter ++;
                if(nestLoopCounter > nestLoop)
                        throw new LoopNestOutOfRange(nestLoopCounter);
                loopinx++;
                if (loopCounter[loopinx] > maxLoopRun)
                    throw new LoopCounterOutOfRange(loopCounter[loopinx]);
                loopCounter[loopinx]++;
            }
        }

        private void EndWhile()
        {
            
                int indent = 1;
                do
                {
                    pos--;
                    if (program[pos] == ']')
                        indent++;
                    else if (program[pos] == '[')
                        indent--;
                } while (indent > 0);
            pos--;
            nestLoopCounter--;
            loopinx--;
        }

        private void Compile(char ins)
        {
            switch (ins)
            {
                case '>':
                    MoveIndex(1);
                    break;
                case '<':
                    MoveIndex(-1);
                    break;
                case '+':
                    Memory[inx]++;
                    break;
                case '-':
                    Memory[inx]--;
                    break;
                case '.':
                    Output();
                    break;
                case ',':
                    throw new Exception("Not Completed yet...");
                    /// todo input not completed
                    // not yet
                    //break;
                case '[':
                    BeginWhile();
                    break;
                case ']':
                    EndWhile();
                    break;
            }
        }

        private void _Run()
        {
            while (pos < program.Length)
            {
                Compile(program[pos]);
                pos++;
            }
        }

        public void Run(string Program)
        {
            Init(Program);
            _Run();
        }

        public BFResult TryRun(string Program,string Target)
        {
            
            try
            {
                Run(Program);
                Console.WriteLine($"Run ok : {program}");
                //return HasTargetInResult(Target);
                if (HasTargetInResult(Target))
                    return BFResult.ResultMatch;
                else
                    return BFResult.RunOk;
            }
            catch (Exception ex)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Run time error {program} | {ex.Message}");
                Console.ForegroundColor = color;
            }
            return BFResult.Fail;
            //return false;
        }
        
        public string BFPrintHistory()
        {
            return PrintHistory.ToString();
        }

        public bool HasTargetInResult(string Target)
        {
            return BFPrintHistory().IndexOf(Target) >= 0;
        }

        public string Actions => 
                              "+-.[]<>";//without input
                                        //"+-.,[]<>";//original

        public int Position => pos;
    }
}
