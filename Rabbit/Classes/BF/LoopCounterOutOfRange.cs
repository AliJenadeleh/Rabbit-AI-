using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.BF
{
    public class LoopCounterOutOfRange:Exception
    {
        public LoopCounterOutOfRange(int counter = -1):base($"Loop counter out of range [{counter}].")
        {
            LoopCounter = counter;
        }

        public int LoopCounter { get; private set; }
    }
}
