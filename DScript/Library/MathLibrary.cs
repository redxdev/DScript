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
        [ContextRegistration]
        public static void ContextRegistration(IExecutionContext context)
        {
            context.DefineVariable("math.pi", new ConstantVariable(new GenericValue<double>(Math.PI)));
            context.DefineVariable("math.e", new ConstantVariable(new GenericValue<double>(Math.E)));
        }

        [Command(Name = "equal")]
        public static IValue Equal(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .AtLeast(2)
                .Execute()
                .Results();

            IValue first = args[0];
            for (int i = 1; i < args.Length; i++)
            {
                if (!first.Equals(args[i]))
                    return new GenericValue<bool>(false);
            }

            return new GenericValue<bool>(true);
        }

        [Command(Name = "not")]
        public static IValue Not(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .Exactly(1)
                .Execute()
                .CanConvert<bool>()
                .Results();

            return new GenericValue<bool>(!args[0].GetValue<bool>());
        }

        [Command(Name = "and")]
        public static IValue And(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .AtLeast(2)
                .Execute()
                .CanConvert<bool>()
                .Results();

            for (int i = 0; i < args.Length; i++)
            {
                if (!args[i].GetValue<bool>())
                    return new GenericValue<bool>(false);
            }

            return new GenericValue<bool>(true);
        }

        [Command(Name = "or")]
        public static IValue Or(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .AtLeast(2)
                .Execute()
                .CanConvert<bool>()
                .Results();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].GetValue<bool>())
                    return new GenericValue<bool>(true);
            }

            return new GenericValue<bool>(false);
        }

        [Command(Name = "add")]
        public static IValue Add(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .AtLeast(2)
                .Execute()
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
            var args = CommandUtilities.ManageArguments(context, arguments)
                .AtLeast(2)
                .Execute()
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
            var args = CommandUtilities.ManageArguments(context, arguments)
                .AtLeast(2)
                .Execute()
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
            var args = CommandUtilities.ManageArguments(context, arguments)
                .Exactly(2)
                .Execute()
                .CanConvert<double>()
                .Results();

            double result = args[0].GetValue<double>() / args[1].GetValue<double>();

            return new GenericValue<double>(result);
        }
    }
}
