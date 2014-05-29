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
    public static class ConditionalLibrary
    {
        [Command(Name = "if")]
        public static IValue ifStm(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Between(2, 3)
                .CanConvert<bool>(0)
                .CanConvert<IExecutable>(1)
                .CanConvert<IExecutable>(2)
                .Results();

            if(args[0].GetValue<bool>())
            {
                IExecutionContext localCtx = ctx.CreateChildContext();
                return localCtx.Execute(arguments[1].GetValue().GetValue<IExecutable>());
            }
            else if(args.Length == 3)
            {
                IExecutionContext localCtx = ctx.CreateChildContext();
                return localCtx.Execute(arguments[2].GetValue().GetValue<IExecutable>());
            }

            return GenericValue<object>.Default;
        }

        [Command(Name = "equal")]
        public static IValue Equal(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .AtLeast(2)
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
                .CanConvert<bool>()
                .Results();

            return new GenericValue<bool>(!args[0].GetValue<bool>());
        }

        [Command(Name = "and")]
        public static IValue And(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .AtLeast(2)
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
                .CanConvert<bool>()
                .Results();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].GetValue<bool>())
                    return new GenericValue<bool>(true);
            }

            return new GenericValue<bool>(false);
        }

        [Command(Name = "lt")]
        public static IValue LessThan(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .Exactly(2)
                .CanConvert<double>()
                .Results();

            return new GenericValue<bool>(args[0].GetValue<double>() < args[1].GetValue<double>());
        }

        [Command(Name = "lteq")]
        public static IValue LessThanEqual(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .Exactly(2)
                .CanConvert<double>()
                .Results();

            return new GenericValue<bool>(args[0].GetValue<double>() <= args[1].GetValue<double>());
        }

        [Command(Name = "gt")]
        public static IValue GreaterThan(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .Exactly(2)
                .CanConvert<double>()
                .Results();

            return new GenericValue<bool>(args[0].GetValue<double>() > args[1].GetValue<double>());
        }

        [Command(Name = "gteq")]
        public static IValue GreaterThanEqual(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .Exactly(2)
                .CanConvert<double>()
                .Results();

            return new GenericValue<bool>(args[0].GetValue<double>() >= args[1].GetValue<double>());
        }
    }
}
