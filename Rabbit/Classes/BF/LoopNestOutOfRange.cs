using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.BF
{
    public class LoopNestOutOfRange : Exception
    {
        public LoopNestOutOfRange():base("Nest loop out of range.")
        {
            
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
