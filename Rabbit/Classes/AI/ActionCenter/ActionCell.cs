using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.AI.ActionCenter
{
    public class ActionCell
    {
        public char ActionTag { get; private set; }
        public int Reward { get; private set; }

        public ActionCell(char ActionTag,int Reward)
        {
            this.ActionTag = ActionTag;
            this.Reward = Reward;
        }

        public void SetReward(int Value = 1)=>Reward += Value;
    }
}
