using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Context;
using DScript.Context.Arguments;
using DScript.Context.Attributes;
using DScript.Context.Variables;
using DScript.Utility;

namespace DSConsole
{
    [Module(Name = "console")]
    public static class ConsoleLibrary
    {
        [Command(Name = "exit")]
        public static IValue Exit(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Exactly(0)
                .Results();

            Program.Running = false;

            return GenericValue<object>.Default;
        }

        [Command(Name = "print")]
        public static IValue Print(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Exactly(1)
                .Results();

            Console.WriteLine(args[0].GetValue<string>());

            return GenericValue<object>.Default;
        }

        [Command(Name = "readline")]
        public static IValue ReadLine(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Exactly(0)
                .Results();

            return new GenericValue<string>(Console.ReadLine());
        }

        [Command(Name = "result")]
        public static IValue Result(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Exactly(0)
                .Results();

            return Program.LastResult;
        }

        [Command(Name = "exception")]
        public static IValue Exception(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Exactly(0)
                .Results();

            return new GenericValue<string>(Program.LastException.ToString());
        }
    }
}
