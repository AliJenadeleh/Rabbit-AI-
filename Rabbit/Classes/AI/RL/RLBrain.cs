using Rabbit.Classes.BF;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.AI.RL
{
    public class RLBrain
    {
        private readonly string target;
        private char[] Program;
        private int ProgramSize;//, CommandsLength;
        private readonly int ProgramMaxSize
                            , ProgramSizeStep
                            , MaxLoopSize
                            , Treshold
                            , MemorySize;
        //private readonly string commands; // scored commands
        private BrainFuck bf;

        public RLBrain(string Target="hi")
        {
            target = Target;
            Treshold = 25;
            ProgramSize = 25;
            ProgramSizeStep = 25;
            ProgramMaxSize = 500;
            //MaxLoopSize = 100;
            MemorySize = (target.Length * 10) + Treshold;
            MaxLoopSize = 100;
            //commands = "<>+-.[]"; 
            // without input
            //commands = "<>+-.,[]"; // with input
            Program = new char[ProgramSize];
            bf = new BrainFuck();
        }

        public bool Start()
        {
            // Action List
            // Select Action



            return false;
        }
    }
}
