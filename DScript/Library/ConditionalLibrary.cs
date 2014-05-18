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
                .Execute(0)
                .CanConvert<bool>(0)
                .Results();

            if(args[0].GetValue<bool>())
            {
                return arguments[1].GetValue(ctx);
            }
            else if(args.Length == 3)
            {
                return arguments[2].GetValue(ctx);
            }

            return GenericValue<object>.Default;
        }
    }
}
