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
    public static class ListLibrary
    {
        [Command(Name = "list.create")]
        public static IValue Create(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .Execute()
                .Results();

            IList<IValue> list = new List<IValue>();
            foreach(IValue arg in args)
            {
                list.Add(arg);
            }

            return new GenericValue<IList<IValue>>(list);
        }

        [Command(Name = "list.count")]
        public static IValue Count(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .Exactly(1)
                .Execute()
                .CanConvert<IList<IValue>>()
                .Results();

            return new GenericValue<int>(args[0].GetValue<IList<IValue>>().Count);
        }

        [Command(Name = "list.index")]
        public static IValue Index(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .Exactly(2)
                .Execute()
                .CanConvert<IList<IValue>>(0)
                .CanConvert<int>(1)
                .Results();

            return args[0].GetValue<IList<IValue>>()[args[1].GetValue<int>()];
        }

        [Command(Name = "list.set")]
        public static IValue SetIndex(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .Exactly(3)
                .Execute()
                .CanConvert<IList<IValue>>(0)
                .CanConvert<int>(1)
                .Results();

            args[0].GetValue<IList<IValue>>()[args[1].GetValue<int>()] = args[2];
            return new GenericValue<bool>(true);
        }

        [Command(Name = "list.add_range")]
        public static IValue AddRange(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .Exactly(2)
                .Execute()
                .CanConvert<IList<IValue>>()
                .Results();

            IList<IValue> list = args[0].GetValue<IList<IValue>>();
            foreach(IValue value in args[1].GetValue<IList<IValue>>())
            {
                list.Add(value);
            }

            return new GenericValue<bool>(true);
        }

        [Command(Name = "list.add")]
        public static IValue Add(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .AtLeast(2)
                .Execute()
                .CanConvert<IList<IValue>>(0)
                .Results();

            IList<IValue> list = args[0].GetValue<IList<IValue>>();
            for(int i = 1; i < args.Length; i++)
            {
                list.Add(args[i]);
            }

            return new GenericValue<bool>(true);
        }

        [Command(Name = "list.remove_at")]
        public static IValue RemoveAt(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .Exactly(2)
                .Execute()
                .CanConvert<IList<IValue>>(0)
                .CanConvert<int>(1)
                .Results();

            args[0].GetValue<IList<IValue>>().RemoveAt(args[1].GetValue<int>());
            return new GenericValue<bool>(true);
        }

        [Command(Name = "list.remove")]
        public static IValue Remove(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .Exactly(2)
                .Execute()
                .CanConvert<IList<IValue>>(0)
                .Results();

            return new GenericValue<bool>(args[0].GetValue<IList<IValue>>().Remove(args[1]));
        }

        [Command(Name = "list.clear")]
        public static IValue Clear(IExecutionContext context, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(context, arguments)
                .Exactly(1)
                .Execute()
                .CanConvert<IList<IValue>>()
                .Results();

            args[0].GetValue<IList<IValue>>().Clear();
            return new GenericValue<bool>(true);
        }
    }
}
