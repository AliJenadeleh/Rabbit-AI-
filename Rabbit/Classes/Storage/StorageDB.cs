using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Rabbit.Classes.Storage
{
    public class StorageDB
    {
        private const string DBName = "db.bf";
        List<StorageCell> list;

        private void Load()
        {
            var fs = new FileStream(DBName, FileMode.Open, FileAccess.Read);
            var bf = new BinaryFormatter();
            list = (List<StorageCell>) bf.Deserialize(fs);
            fs.Close();
        }

        public StorageDB()
        {
            if (File.Exists(DBName))
            {
                Load();
            }else
                list = new List<StorageCell>();
        }

        public void Add(StorageCell cell)
        {
            list.Add(cell);
        }

        public void Save()
        {
            if (File.Exists(DBName))
                File.Delete(DBName);
            var fs = new FileStream(DBName, FileMode.Create, FileAccess.Write);
            var bf = new BinaryFormatter();
            bf.Serialize(fs, list);
            fs.Close();
        }


        public int Count => list.Count;
    }
}
