
using Rabbit.Classes.AI.RL;
using Rabbit.Classes.AILess;
using Rabbit.Classes.BF;
using Rabbit.Classes.BrainFuckEmulator;
using System;
using System.Diagnostics;
using System.IO;

namespace Rabbit
{
    class Program
    {
        private const string version = "0.2";
        private const string appName = "Rabbit";
        private const string target = "ali";//"hi";

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
                var rlb = new Brain2(target);
                //var rlb = new Brain2(target,true );//With Verbose
            //return rlb.ShadowStart();
            return rlb.Start();
        }

        private static void Run()
        {
            

            //var ws = new Stopwatch();
            //ws.Start();
            string p =
                //AILessMethod(); // BackTruckMethod
                AIMethod(); // RL Method
            //ws.Stop();


            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Target program : {p}");
            //BFTest(); // "Default BF Programm"
            //Console.WriteLine($"Program takes {ws.ElapsedMilliseconds} ms");
            Console.ForegroundColor = color;
            File.AppendAllText("tmp.txt",Environment.NewLine + $"{p} /  {target}");
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
