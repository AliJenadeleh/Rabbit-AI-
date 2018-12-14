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

        public BrainFuck(int NestLoop=5,int MaxLoopRun=255)
        {
            this.nestLoop = NestLoop;
            this.maxLoopRun = MaxLoopRun;
            nestLoopCounter = -1;
        }

        /*
        public BrainFuck(string Program)
        {
            Init(Program);
        }
        */

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
                        throw new LoopNestOutOfRange();
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
                    /// todo input not completed
                    // not yet
                    break;
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
        
        public string BFPrintHistory()
        {
            return PrintHistory.ToString();
        }
    }
}
