using Rabbit.Classes.AI.RL;
using Rabbit.Classes.AILess;
using Rabbit.Classes.BF;
using System;
using System.Diagnostics;

namespace Rabbit
{
    class Program
    {
        private const string version = "0.1";
        private const string appName = "Rabbit";
        private const string target = "hi";

        private static string AILessMethod()
        {
            Console.WriteLine($"Start time : {DateTime.Now}");
            var btm = new BackTrackMind(target.ToLower());
            return btm.Start();
        }

        private static void BFTest(string BfProgram = ">+++++++++++++++++++++>++<[->+<].>.")
        {
            //new BrainFuck().Run("++>+++<[>+<]>.");
            var bf = new BrainFuck();
            bf.Run(BfProgram);
            string his = bf.BFPrintHistory();
            Console.WriteLine($"\n BF Print History : \n{his}");
        }

        private static string AIMethod()
        {
            Console.WriteLine("RL-Brain start....");
            var rlb = new RLBrain(target);
            return rlb.Start();

            // Create AI Brain
            // Create NN To detect fails
            // Create Action cells
            // 
        }

        private static void Run()
        {
            var ws = new Stopwatch();
            ws.Start();
            string p =
                //AILessMethod(); // BackTruckMethod
                AILessMethod(); // RL Method
            ws.Stop();

            Console.WriteLine($"Target program : {p}");
            //BFTest(); // "Default BF Programm"
            Console.WriteLine($"Program takes {ws.ElapsedMilliseconds} ms");
        }

        static void Main(string[] args)
        {

            Console.WriteLine($"\nApp Start .\nRabbit version[{version}]\nTarget [{target}]");
                Run();
            Console.WriteLine("Enter any key to exit.");
            Console.ReadKey();
        }
    }
}
