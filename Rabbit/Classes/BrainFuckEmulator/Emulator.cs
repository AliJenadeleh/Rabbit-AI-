using Rabbit.Classes.BrainFuckEmulator.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.BrainFuckEmulator
{
    public class Emulator
    {
        #region vars
        private byte[] Memory;
        private int[] LoopCounter;

        private StringBuilder Log; // Output log

        private string Program;
        private int PInx;  //Program Index
        private int MInx;  // Memory Index
        private int PLen;  // Program Length
        private ErrorType errorType;

        private const int MemorySize = 255;

        //private readonly int MaxNestLoop; // Max nested loop
        private readonly int MaxLoopRun; // Max repate of each loop

        private readonly bool Verbose,ShowOutput;
        #endregion

        #region Privates

        private int CharCounter(char Target)
        {
            int Counter = 0;

            for (int i = 0; i < PLen; i++)
                if (Program[i] == Target)
                    Counter++;

            return Counter;
        }

        private int OperatorIndex(int Pos,char Operator)
        { // Get The Index Of Same Operator in program
            int Inx = -1;

            for (int i = 0; i <= Pos; i++)
                if (Program[i] == Operator)
                    Inx++;

            return Inx;
        }

        private void Init(string Program)
        {
             // Set program
            this.Program = Program;

            // set Program length
            this.PLen = Program.Length;

            // Set Pointers
            PInx = 0;
            MInx = 0;

            //  Create output log
            Log = new StringBuilder();

            // Create Loop Counters
            int lc = CharCounter(StaticsAndDefaults.OperatorBegin);//'['
            LoopCounter = new int[lc];

            // Init Memory
            Memory = new byte[MemorySize];
        }

        private void DoInc()
        {
            Memory[MInx]++; // Inc The current Memory
            if (Verbose)
            {
                Console.WriteLine($"Memory[{MInx}] : {Memory[MInx]}");
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = color;
            }
        }

        private void DoDec()
        {
            Memory[MInx]--; // Dec the current Memory
            if (Verbose)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Memory[{MInx}] : {Memory[MInx]}");
                Console.ForegroundColor = color;
            }
        }

        private void MoveRight()
        {
            if (MInx + 1 <= Memory.Length)
                MInx++;
            else
            {
                errorType = ErrorType.MemoryOutOfRange;
                throw new BFOutOfMemoryException(PInx, Program[PInx]);
            }

            if (Verbose)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Memory Index : [{MInx}]");
                Console.ForegroundColor = color;
            }
        }

        private void MoveLeft()
        {
            if (MInx - 1 >= 0)
                MInx--;
            else
            {
                errorType = ErrorType.MemoryOutOfRange;
                throw new BFOutOfMemoryException(PInx, Program[PInx]);
            }

            if (Verbose)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Memory Index : [{MInx}]");
                Console.ForegroundColor = color;
            }
        }

        private void Output()
        {
            byte value = Memory[MInx];
            char dis = (char)value;
            Log.Append(dis);

            if (ShowOutput)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{value}]:[{dis}]");
                Console.ForegroundColor = color;
            }
        }

        private void Input()
        {
            errorType = ErrorType.UnknownError;
            throw new Exception("Not completed yet");
        }

        private void Begin()
        {
            int inx = OperatorIndex(PInx, StaticsAndDefaults.OperatorBegin);
            if (LoopCounter[inx] > StaticsAndDefaults.MaxLoopRun)
            {
                errorType = ErrorType.LoopRunOutOfRange;
                throw new BFEOutOfLoopRunRange(LoopCounter[inx]);
            }

            if (Memory[MInx] > 0)
            {
                DoDec();
                LoopCounter[inx]++;
            }
            else
            {
                int indent = 1;
                do
                {
                    PInx++;
                    if (Program[PInx] == StaticsAndDefaults.OperatorBegin)
                        indent++;
                    else if (Program[PInx] == StaticsAndDefaults.OperatorEnd)
                        indent--;
                } while (indent > 0);

                LoopCounter[inx] = 0;
            }
            
            
        }//Begin();

        private void End()
        {
            // goto begin
            int indent = 1;
            do
            {
                PInx--;
                if (Program[PInx] == StaticsAndDefaults.OperatorBegin)
                    indent--;
                else if (Program[PInx] == StaticsAndDefaults.OperatorEnd)
                    indent++;
            } while (indent > 0);

            if (Verbose)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Goin to {PInx}");
                Console.ForegroundColor = color;
            }


            PInx--; // Because the Program Index will inc in Start Loop
        }// End();

        private void Emulate(char Operator)
        {
            switch (Operator)
            {
                case StaticsAndDefaults.OperatorInc:
                    DoInc();
                    break;
                case StaticsAndDefaults.OperatorDec:
                    DoDec();
                    break;
                case StaticsAndDefaults.OperatorLeft:
                    MoveLeft();
                    break;
                case StaticsAndDefaults.OperatorRight:
                    MoveRight();
                    break;
                case StaticsAndDefaults.OperatorOutput:
                    Output();
                    break;
                case StaticsAndDefaults.OperatorInput:
                    Input();
                    break;
                case StaticsAndDefaults.OperatorBegin:
                    Begin();
                    break;
                case StaticsAndDefaults.OperatorEnd:
                    End();
                    break;
            }
        }// Emulate();

        private void StartEmulate()
        {
            while(PInx < PLen)
            {
                char p = Program[PInx];
                if (Verbose)
                {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Program Index [{PInx}] / [{p}]");
                    Console.ForegroundColor = color;
                }

                Emulate(p);
                PInx++;
            }
        }//StartEmulate()

        #endregion

        public Emulator(//int MaxNestLoop = StaticsAndDefaults.MaxNestLoop,
                        int MaxLoopRun = StaticsAndDefaults.MaxLoopRun,
                        bool Verbose = StaticsAndDefaults.DefaultVerbose,
                        bool ShowOutput = StaticsAndDefaults.DefaultShowOutput)
        {
            this.MaxLoopRun = MaxLoopRun;
            //this.MaxNestLoop = MaxNestLoop;
            this.Verbose = Verbose;
            this.ShowOutput = ShowOutput;
        }//Emulator


        #region Public

        public int AnyInOutPut(string Target)
        {
            int result = 0;
            string tmp = this.ToString();
            for (int i = 0; i < Target.Length; i++)
                if (tmp.IndexOf(Target[i]) >= 0)
                    result++;
            return result;
        }

        public void Start(string Program)
        {
            Init(Program);
            StartEmulate();
        }//Start()

        #endregion

        #region Overrides
        public override string ToString()
        {
            if (Log != null)
                return Log.ToString();

            return string.Empty;
        }
        #endregion

        #region Fields
        public ErrorType LocalErrorType => errorType;
        public int ProgramIndex => PInx;
        public int MemoryIndex => MInx;
        public byte[] LocalMemory { get => Memory; }
        #endregion
    }//Emulator
}