using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.AI.ActionCenter
{
    public class Cell2
    {
        public char Tag { get;  private set; }
        public int Score { get; private set; }

        public Cell2(char Tag='\0',int Score=0)
        {
            this.Tag = Tag;
            this.Score = Score;
        }

        public void Reward(int Value = 1)
        {
            this.Score += Value;
        }
    }
}
