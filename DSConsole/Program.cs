using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Context;
using DScript.Context.Arguments;
using DScript.Context.Variables;
using DScript.Language;
using DScript.Utility;

namespace DSConsole
{
    class Program
    {
        public static bool Running
        {
            get;
            set;
        }

        public static IValue LastResult
        {
            get;
            set;
        }

        public static Exception LastException
        {
            get;
            set;
        }

        public static void Main(string[] args)
        {
            Running = true;

            IExecutionContext context = new ScopedExecutionContext();
            ContextUtilities.RegisterAllAssemblies(context, AppDomain.CurrentDomain);

            Console.WriteLine(string.Format("DScript version {0}.{1} Copyright (c) Sam Bloomberg 2014", context.GetMajorVersion(), context.GetMinorVersion()));

            while(Running)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("dscript> ");
                string input = Console.ReadLine();

                IExecutable executable = null;
                try
                {
                    executable = LanguageUtilities.ParseString(input);
                }
                catch(ParseException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine(e.Message);
                    LastException = e;
                    continue;
                }
                catch(Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("Encountered error while parsing input:");
                    Console.WriteLine(e.Message);
                    LastException = e;
                    continue;
                }

                try
                {
                    LastResult = context.Execute(executable);
                    Console.WriteLine(LastResult.GetValue<string>());
                }
                catch(ArgumentException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("Argument error:");
                    Console.WriteLine(e.Message);
                    LastException = e;
                    continue;
                }
                catch(CommandException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("Command error:");
                    Console.WriteLine(e.Message);
                    LastException = e;
                    continue;
                }
                catch(Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("Encountered error while running executable:");
                    Console.WriteLine(e.Message);
                    LastException = e;
                    continue;
                }
            }
        }
    }
}
