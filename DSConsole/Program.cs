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
            IExecutionContext context = new ScopedExecutionContext();
            ContextUtilities.RegisterAllAssemblies(context, AppDomain.CurrentDomain);

            Console.WriteLine(string.Format("DScript version {0}.{1} Copyright (c) Sam Bloomberg 2014", context.GetMajorVersion(), context.GetMinorVersion()));

            bool running = true;

            context.RegisterCommand("exit", (ctx, arguments) => { running = false; return null; });
            context.RegisterCommand("print", (ctx, arguments) => { Console.WriteLine(arguments[0].GetValue(ctx).GetValue<string>()); return GenericValue<object>.Default; });

            IValue lastResult = GenericValue<object>.Default;
            context.DefineVariable("console.last_result", new DelegatedVariable()
                {
                    Getter = () => lastResult
                });

            Exception lastException = null;
            context.DefineVariable("console.last_trace", new DelegatedVariable()
                {
                    Getter = () => new GenericValue<string>(lastException.ToString())
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
                    lastException = e;
                    continue;
                }
                catch(Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("Encountered error while parsing input:");
                    Console.WriteLine(e.Message);
                    lastException = e;
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
                    lastException = e;
                    continue;
                }
                catch(CommandException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("Command error:");
                    Console.WriteLine(e.Message);
                    lastException = e;
                    continue;
                }
                catch(Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("Encountered error while running executable:");
                    Console.WriteLine(e.Message);
                    lastException = e;
                    continue;
                }
            }
        }
    }
}
