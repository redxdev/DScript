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
    [Module(Name = "dictionary")]
    class DictionaryLibrary
    {
        [Command(Name = "new")]
        public static IValue CreateNew(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Exactly(0)
                .Results();

            IExecutionContext dict = ctx.CreateChildContext();

            dict.DefineVariable("+object", new ConstantVariable(new GenericValue<Dictionary<IValue, IValue>>(new Dictionary<IValue, IValue>())));
            dict.RegisterCommand("add", Add);
            dict.RegisterCommand("get", Get);
            dict.RegisterCommand("clear", Clear);
            dict.RegisterCommand("containsKey", ContainsKey);
            dict.RegisterCommand("containsValue", ContainsValue);

            return new GenericValue<IExecutionContext>(dict);
        }

        private static Dictionary<IValue, IValue> GetDictionary(IExecutionContext ctx)
        {
            return ctx.GetVariable("+object", false).Value.GetValue<Dictionary<IValue, IValue>>();
        }

        public static IValue Add(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Exactly(2)
                .Results();

            GetDictionary(ctx).Add(args[0], args[1]);

            return new GenericValue<bool>(true);
        }

        public static IValue Get(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Exactly(1)
                .Results();

            return GetDictionary(ctx)[args[0]];
        }

        public static IValue Clear(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Exactly(0)
                .Results();

            GetDictionary(ctx).Clear();

            return new GenericValue<bool>(true);
        }

        public static IValue ContainsKey(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Exactly(1)
                .Results();

            return new GenericValue<bool>(GetDictionary(ctx).ContainsKey(args[0]));
        }

        public static IValue ContainsValue(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Exactly(1)
                .Results();

            return new GenericValue<bool>(GetDictionary(ctx).ContainsValue(args[0]));
        }
    }
}
