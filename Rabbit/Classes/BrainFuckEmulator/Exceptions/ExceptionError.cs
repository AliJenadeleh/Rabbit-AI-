using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.BrainFuckEmulator.Exceptions
{
    public enum ErrorType
    {
        UnknownError = 0,
        LoopRunOutOfRange = 1,
        NestloopOutOfRange = 2,
        MemoryOutOfRange = 4
    }
}
