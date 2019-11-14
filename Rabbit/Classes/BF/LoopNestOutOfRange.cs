    using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.BF
{
    public class LoopNestOutOfRange : Exception
    {
        public int Counter { get; private set; }
        public LoopNestOutOfRange(int Counter) : base($"Nest loop out of range [{Counter}]")
        {
            this.Counter = Counter;
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
