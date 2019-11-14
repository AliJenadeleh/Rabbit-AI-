using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.BrainFuckEmulator.Exceptions
{
    public class BFEOutOfLoopRunRange : Exception
    {
        public BFEOutOfLoopRunRange(int LoopRun):base($"Loop Run[{LoopRun}] OutOfLoopRun")
        {

        }
    }
}
