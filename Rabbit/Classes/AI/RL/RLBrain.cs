using Rabbit.Classes.BF;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.AI.RL
{
    public class RLBrain
    {
        readonly string Target;
        int ProgramSize, ProgramSizeStep;

        public RLBrain(string Target)
        {
            this.Target = Target;
            ProgramSize = 75;
            ProgramSizeStep = 15;
        }

        public bool _Start()
        {


            return false;
        }

        public string Start()
        {



            return string.Empty;
        }

    }
}
