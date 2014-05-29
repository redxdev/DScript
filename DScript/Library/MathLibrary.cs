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

namespace DScript.Library
{
    public static class MathLibrary
    {
        private static Random random = new Random();

        [ContextRegistration]
        public static void ContextRegistration(IExecutionContext context)
        {
        }

        [Command(Name = "add")]
        public static IValue Add(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .AtLeast(2)
                .CanConvert<double>()
                .Results();

            double result = args[0].GetValue<double>();
            for(int i = 1; i < args.Length; i++)
            {
                result += args[i].GetValue<double>();
            }

            return new GenericValue<double>(result);
        }

        [Command(Name = "sub")]
        public static IValue Sub(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .AtLeast(2)
                .CanConvert<double>()
                .Results();

            double result = args[0].GetValue<double>();
            for (int i = 1; i < args.Length; i++)
            {
                result -= args[i].GetValue<double>();
            }

            return new GenericValue<double>(result);
        }

        [Command(Name = "mul")]
        public static IValue Mul(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .AtLeast(2)
                .CanConvert<double>()
                .Results();

            double result = args[0].GetValue<double>();
            for (int i = 1; i < args.Length; i++)
            {
                result *= args[i].GetValue<double>();
            }

            return new GenericValue<double>(result);
        }

        [Command(Name = "div")]
        public static IValue Div(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Exactly(2)
                .CanConvert<double>()
                .Results();

            double result = args[0].GetValue<double>() / args[1].GetValue<double>();

            return new GenericValue<double>(result);
        }

        [Command(Name = "mod")]
        public static IValue Mod(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Exactly(2)
                .CanConvert<double>()
                .Results();

            double result = args[0].GetValue<double>() % args[1].GetValue<double>();

            return new GenericValue<double>(result);
        }

        [Command(Name = "rand_seed")]
        public static IValue RandSeed(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Between(0, 1)
                .CanConvert<int>()
                .Results();

            if(args.Length == 0)
            {
                random = new Random();
            }
            else
            {
                random = new Random(args[0].GetValue<int>());
            }

            return GenericValue<object>.Default;
        }

        [Command(Name = "rand_int")]
        public static IValue RandInt(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Between(0, 2)
                .Results();

            if (args.Length > 0)
            {
                int min = args.Length == 1 ? 0 : args[0].GetValue<int>();
                int max = args.Length == 1 ? args[0].GetValue<int>() : args[1].GetValue<int>();
                return new GenericValue<int>(random.Next(min, max));
            }
            else
            {
                return new GenericValue<int>(random.Next());
            }
        }
    }
}
