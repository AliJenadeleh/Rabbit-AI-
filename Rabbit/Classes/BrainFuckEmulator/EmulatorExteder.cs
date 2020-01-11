using Rabbit.Classes.AI.ProgramCenter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.BrainFuckEmulator
{
    public enum EResult{
        Match=0,
        NotMatch=1,
        Error=2,
        MatchWithError=4
    }
    public static class EmulatorExteder
    {
        private static bool HasTarget(string Source,string Target,bool IgnoreCase)
        {
            if(IgnoreCase)
                return Source.ToLower().IndexOf(Target.ToLower()) >= 0; 
            return Source.IndexOf(Target) >= 0;
        }
        public static void PrintSelected(string Source,string Target,bool IgnoreCase){
            string src,target;
            if(IgnoreCase){
                src = Source.ToLower();
                target = Target.ToLower();
            }else{
                src = Source.ToLower();
                target = Target.ToLower();
            }
                int inx = src.IndexOf(target);
                Console.WriteLine(src.Substring(0,inx  + 1));
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(src.Substring(inx,target.Length));
                Console.ForegroundColor = color;
                Console.WriteLine(src.Substring(inx + target.Length,src.Length - (inx + target.Length)));
        }
        public static EResult TryToEmulate(this Emulator emu, IEumulateableProgram Program, string Target,bool Verbose = false,bool IgnoreCase=true)
        {
            string script = Program.GetProgram();
            string src;
            try
            {
                Console.WriteLine(script);
                emu.Start(script);
                src = emu.ToString();
                if (HasTarget(src, Target,IgnoreCase))
                {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Match Found. Target: {Target}");
                    //Console.WriteLine(src);
                    PrintSelected(src,Target,IgnoreCase);
                    Console.ForegroundColor = color;
                    return EResult.Match;
                }
                else
                {
                    if(Verbose){
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(src);
                    Console.ForegroundColor = color;
                    }
                    return EResult.NotMatch;
                }
            }
            catch(Exception ex)
            {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                if (Verbose)
                {
                    Console.WriteLine(Program.CounterDetails());
                    Console.WriteLine($"Error on TryToEmulate {Target} -> {script}");
                    Console.WriteLine(ex.Message);

                }
                src = emu.ToString();
                if (HasTarget(src, Target,IgnoreCase))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Match Found with error. Target : {Target}");
                    //Console.WriteLine(src);
                    PrintSelected(src,Target,IgnoreCase);
                    Console.ReadKey();
                    Console.WriteLine("Press any key to continue.");
                    return EResult.MatchWithError;
                }

                Console.ForegroundColor = color;
                return EResult.Error;
            }
            
        }
    }
}
