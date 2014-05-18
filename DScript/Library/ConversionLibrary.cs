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
    public static class ConversionLibrary
    {
        public static IValue Convert<T>(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .Exactly(1)
                .CanConvert<T>()
                .Execute()
                .Results();

            return new GenericValue<T>(args[0].GetValue<T>());
        }

        [Command(Name = "to_string")]
        public static IValue ToString(IExecutionContext context, IList<IArgument> arguments)
        {
            return Convert<string>(context, arguments);
        }

        [Command(Name = "to_int")]
        public static IValue ToInt(IExecutionContext context, IList<IArgument> arguments)
        {
            return Convert<int>(context, arguments);
        }

        [Command(Name = "to_float")]
        public static IValue ToFloat(IExecutionContext context, IList<IArgument> arguments)
        {
            return Convert<float>(context, arguments);
        }

        [Command(Name = "to_double")]
        public static IValue ToDouble(IExecutionContext context, IList<IArgument> arguments)
        {
            return Convert<double>(context, arguments);
        }
    }
}
