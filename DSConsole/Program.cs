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
        static void Main(string[] args)
        {
            IExecutionContext context = new ExecutionContext();
            ContextUtilities.RegisterAllAssemblies(context, AppDomain.CurrentDomain);

            Console.WriteLine(string.Format("DScript version {0}.{1} Copyright (c) Sam Bloomberg 2014", context.GetMajorVersion(), context.GetMinorVersion()));

            bool running = true;

            context.RegisterCommand("exit", (ctx, arguments) => { running = false; return null; });

            IValue lastResult = GenericValue<object>.Default;
            context.DefineVariable("console.last_result", new DelegatedVariable()
                {
                    Getter = () => lastResult
                });

            while(running)
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
                    continue;
                }
                catch(Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("Encountered error while parsing input:");
                    Console.WriteLine(e.ToString());
                    continue;
                }

                try
                {
                    lastResult = context.Execute(executable);
                    Console.WriteLine(lastResult.GetValue<string>());
                }
                catch(ArgumentException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("Argument error:");
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch(CommandException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("Command error:");
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch(Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("Encountered error while running executable:");
                    Console.WriteLine(e.ToString());
                    continue;
                }
            }
        }
    }
}
