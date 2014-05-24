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
    public static class LoopLibrary
    {
        [Command(Name = "for")]
        public static IValue For(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(4)
                .CanConvert<IExecutable>()
                .Results();

            IExecutionContext localCtx = ctx.CreateChildContext();
            IExecutable initializer = args[0].GetValue<IExecutable>();
            IExecutable check = args[1].GetValue<IExecutable>();
            IExecutable increment = args[2].GetValue<IExecutable>();
            IExecutable execute = args[3].GetValue<IExecutable>();

            localCtx.Execute(initializer);

            IValue lastValue = GenericValue<object>.Default;

            while(localCtx.Execute(check).GetValue<bool>())
            {
                lastValue = localCtx.Execute(execute, true);

                if (execute.DidBreak())
                    break;

                localCtx.Execute(increment);
            }

            return lastValue;
        }

        [Command(Name = "while")]
        public static IValue While(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(2)
                .CanConvert<IExecutable>()
                .Results();

            IExecutionContext localCtx = ctx.CreateChildContext();
            IExecutable check = args[0].GetValue<IExecutable>();
            IExecutable execute = args[1].GetValue<IExecutable>();

            IValue lastValue = GenericValue<object>.Default;

            while(localCtx.Execute(check).GetValue<bool>())
            {
                lastValue = localCtx.Execute(execute, true);

                if (execute.DidBreak())
                    break;
            }

            return lastValue;
        }

        [Command(Name = "do")]
        public static IValue Do(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(2)
                .CanConvert<IExecutable>()
                .Results();

            IExecutionContext localCtx = ctx.CreateChildContext();
            IExecutable check = args[1].GetValue<IExecutable>();
            IExecutable execute = args[0].GetValue<IExecutable>();

            IValue lastValue = GenericValue<object>.Default;

            do
            {
                lastValue = localCtx.Execute(execute);

                if(execute.DidBreak())
                    break;
            } while(localCtx.Execute(check).GetValue<bool>());

            return lastValue;
        }
    }
}
