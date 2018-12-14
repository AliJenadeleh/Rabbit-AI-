using Rabbit.Classes.BF;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.AI
{
    class BackTrackMind
    {
        private char[] Program;
        private readonly string target;
        private int ProgramSize,CommandsLength;
        private readonly int ProgramMaxSize
                            ,ProgramSizeStep
                            ,MaxLoopSize
                            ,Treshold 
                            ,MemorySize;
        private readonly string commands;
        private BrainFuck bf;

        public BackTrackMind(string Target)
        {
            target = Target;
            ProgramSize = 25;
            ProgramMaxSize = 500;
            ProgramSizeStep = 25;
            MaxLoopSize = 100;
            Treshold = 50;
            MemorySize = (target.Length * 10) + Treshold;
            MaxLoopSize = 100;
            commands = "+><-[]."; // an other ordering
            CommandsLength = commands.Length;
            //commands = "<>+-.[]"; 
            // without input
            //commands = "<>+-.,[]"; // with input
            Program = new char[ProgramSize];
            bf = new BrainFuck();
        }

        private bool OkCommand(char cmd,int step)
        {
            if (0 == step && "],.<".IndexOf(cmd) >= 0)
                return false;
            else if (0 < step)
            { 
                if (']' == cmd && Program[step - 1] == '[')
                    return false;
                else if ('>' == cmd && Program[step - 1] == '<')
                    return false;
                else if ('<' == cmd && Program[step - 1] == '>')
                    return false;                        
                else if ('.' == cmd && Program[step - 1] == '.')
                    return false;                        
                else if ('+' == cmd && Program[step - 1] == '-')
                    return false;                        
                else if ('-' == cmd && Program[step - 1] == '+')
                    return false;
            }
            return true;
        }

        private bool _Start(int step)
        {
            if (step > (ProgramSize-1))
                return false;
            char ins;
            
            for(int i =0;i<CommandsLength;i++)
            {
                ins = commands[i];
                if (OkCommand(ins,step))
                {
                    Program[step] = ins;
                    //bool test = false;
                    string program = new string(Program);
                    //var bf = new BrainFuck();//instance as general one
                    try
                    {
                        bf.Run(program);
                        if (bf.BFPrintHistory().IndexOf(target) > 0)
                            return true;
                    }
                    catch
                    {

                    }
                    Console.WriteLine($"Generated {program}");
                    if (_Start(step + 1))
                        return true;
                }
            }
            Program[step] = '\0';
            return false;
        }

        public string Start()
        {
            while (!_Start(0))
            {
                ProgramSize += ProgramSizeStep;
                Program = new char[ProgramSize];
            }
            return new string(Program);
        }
    }
}
