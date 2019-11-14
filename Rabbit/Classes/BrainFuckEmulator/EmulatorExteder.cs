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
        private static bool HasTarget(string Source,string Target)
        {
            return Source.IndexOf(Target) >= 0;
        }
        public static EResult TryToEmulate(this Emulator emu,AIProrgam2 Program,string Target)
        {
            string script = Program.GetProgram();
            string src;
            try
            {
                Console.WriteLine(script);
                emu.Start(script);
                src = emu.ToString();
                if (HasTarget(src, Target))
                {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Match Found");
                    Console.WriteLine(src);
                    Console.ForegroundColor = color;
                    return EResult.Match;
                }
                else
                {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(src);
                    Console.ForegroundColor = color;

                    return EResult.NotMatch;
                }
            }
            catch(Exception ex)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Program.CounterDetails());
                Console.WriteLine($"Error on TryToEmulate {script}");
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = color;
                src = emu.ToString();
                if (HasTarget(src, Target))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Match Found with error");
                    Console.WriteLine(src);
                    Console.ForegroundColor = color;
                    return EResult.MatchWithError;
                }

                return EResult.Error;
            }
            
        }
    }
}
