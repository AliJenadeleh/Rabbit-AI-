using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.BrainFuckEmulator.Exceptions
{
    public class BFOutOfMemoryException:OutOfMemoryException
    {
        public BFOutOfMemoryException(int Position,char Operator):base($"Out of memory exception on [{Position}]/[{Operator}]")
        {
            
        }
    }
}
