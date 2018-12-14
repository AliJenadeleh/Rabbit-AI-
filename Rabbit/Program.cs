using Rabbit.Classes.AI;
using Rabbit.Classes.BF;
using System;
using System.Diagnostics;

namespace Rabbit
{
    class Program
    {
        private const string version = "0.1";
        private const string appName = "Rabbit";
        static void Main(string[] args)
        {
            const string target = "hi";

            Console.WriteLine($"\nApp Start .\nRabbit version[{version}]\nTarget [{target}]");
            var ws = new Stopwatch();
            ws.Start();
            Console.WriteLine($"Start time : {DateTime.Now}");
            var btm = new BackTrackMind(target.ToLower());
            string p = btm.Start();
            ws.Stop();
            Console.WriteLine($"Target program : {p}");
            //new BrainFuck().Run("++>+++<[>+<]>.");
            /*
            var bf = new BrainFuck();
            bf.Run(">+++++++++++++++++++++>++<[->+<].>.");
            string his = bf.BFPrintHistory();
            Console.WriteLine($"\n BF Print History : \n{his}");
            //*/
            Console.WriteLine($"Program takes {ws.ElapsedMilliseconds} ms");
            Console.WriteLine("Enter any key to continue.");
            Console.ReadKey();
        }
    }
}
