using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Context;
using DScript.Context.Arguments;
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
            context.RegisterCommand("echo", (ctx, arguments) =>
            {
                string[] str = new string[arguments.Count];
                for (int i = 0; i < arguments.Count; i++)
                {
                    str[i] = arguments[i].GetValue(ctx).ToString();
                }

                string result = string.Join(" ", str);

                return new GenericValue<string>(result);
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

                IValue result = null;
                try
                {
                    result = context.Execute(executable);
                    Console.WriteLine(result);
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
