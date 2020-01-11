using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.BrainFuckEmulator
{
    public interface IEumulateableProgram
    {
        string GetProgram();
        string CounterDetails();
    }
}
