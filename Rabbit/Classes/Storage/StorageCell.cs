using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.Storage
{
    [Serializable]
    public class StorageCell
    {
        public string Target { get; set; }
        public string Script { get; set; }
        public long MS { get; set; }
    }
}
