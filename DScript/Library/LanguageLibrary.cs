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

namespace DScript.Libary
{
    public static class LanguageLibrary
    {
        [ContextRegistration]
        public static void ContextRegistration(IExecutionContext ctx)
        {
            ctx.DefineVariable("null", new ConstantVariable(GenericValue<object>.Default));
            ctx.DefineVariable("true", new ConstantVariable(new GenericValue<bool>(true)));
            ctx.DefineVariable("false", new ConstantVariable(new GenericValue<bool>(false)));
        }

        [Command(Name = "nullcmd")]
        public static IValue NullCommand(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Execute()
                .Results();

            return GenericValue<object>.Default;
        }

        [Command(Name = "set")]
        public static IValue Set(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Amount(2)
                .Execute()
                .CanConvert<string>(0)
                .Results();

            ctx.GetVariable(args[0].GetValue<string>()).Value = args[1];

            return new GenericValue<bool>(true);
        }

        [Command(Name = "get")]
        public static IValue Get(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Amount(1)
                .Execute()
                .CanConvert<string>(0)
                .Results();

            return ctx.GetVariable(args[0].GetValue<string>()).Value;
        }

        [Command(Name = "define")]
        public static IValue Define(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .AmountBetween(1, 2)
                .Execute()
                .CanConvert<string>(0)
                .Results();

            switch(args.Length)
            {
                case 1:
                    ctx.DefineVariable(args[0].GetValue<string>(), new BasicVariable() { Value = GenericValue<object>.Default });
                    break;

                case 2:
                    ctx.DefineVariable(args[0].GetValue<string>(), new BasicVariable() { Value = args[1] });
                    break;
            }

            return new GenericValue<bool>(true);
        }

        [Command(Name = "undefine")]
        public static IValue Undefine(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Amount(1)
                .Execute()
                .CanConvert<string>(0)
                .Results();

            ctx.UndefineVariable(args[0].GetValue<string>());

            return new GenericValue<bool>(true);
        }

        [Command(Name = "is_defined")]
        public static IValue IsDefined(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Amount(1)
                .Execute()
                .CanConvert<string>(0)
                .Results();

            return new GenericValue<bool>(ctx.HasVariable(args[0].GetValue<string>()));
        }
    }
}
