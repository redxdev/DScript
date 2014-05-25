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
                .Exactly(2)
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
                .Exactly(1)
                .Execute()
                .CanConvert<string>(0)
                .Results();

            return ctx.GetVariable(args[0].GetValue<string>()).Value;
        }

        [Command(Name = "define")]
        public static IValue Define(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Between(1, 2)
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
                .Exactly(1)
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
                .Exactly(1)
                .Execute()
                .CanConvert<string>(0)
                .Results();

            return new GenericValue<bool>(ctx.HasVariable(args[0].GetValue<string>()));
        }

        [Command(Name = "block")]
        public static IValue Block(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(1)
                .CanConvert<ICodeBlock, IExecutable>(0)
                .Results();

            return args[0];
        }

        [Command(Name = "build")]
        public static IValue Build(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(1)
                .Execute()
                .CanConvert<string>(0)
                .Results();

            IExecutable executable = LanguageUtilities.ParseString(args[0].GetValue<string>());
            return new GenericValue<IExecutable>(executable);
        }

        [Command(Name = "execute")]
        public static IValue Execute(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(1)
                .Execute()
                .CanConvert<IExecutable>(0)
                .Results();

            IExecutionContext localCtx = ctx.CreateChildContext();

            return localCtx.Execute(args[0].GetValue<IExecutable>(), true);
        }
        private static ScriptCommand CreateCommand(IExecutable executable, IList<string> argumentNames)
        {
            return (ctx, arguments) =>
            {
                var args = CommandUtilities.ManageArguments(ctx, arguments)
                    .Exactly(argumentNames.Count)
                    .Execute()
                    .Results();

                IExecutionContext localCtx = ctx.CreateChildContext();
                for (int i = 0; i < args.Length; i++)
                {
                    localCtx.DefineVariable(argumentNames[i], new BasicVariable() { Value = args[i] });
                }

                return localCtx.Execute(executable, true);
            };
        }

        [Command(Name = "define_func")]
        public static IValue Function(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .AtLeast(2)
                .Execute()
                .CanConvert<string>(0)
                .CanConvert<IExecutable>(1)
                .CanConvert<string>(2, arguments.Count - 1)
                .Results();

            IExecutable executable = args[1].GetValue<IExecutable>();

            List<string> argNames = new List<string>();
            for (int i = 2; i < args.Length; i++)
            {
                argNames.Add(args[i].GetValue<string>());
            }

            ctx.RegisterCommand(args[0].GetValue<string>(), CreateCommand(executable, argNames));

            return new GenericValue<bool>(true);
        }

        [Command(Name = "undefine_func")]
        public static IValue UndefineFunction(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(1)
                .Execute()
                .CanConvert<string>(0)
                .Results();

            ctx.UnregisterCommand(args[0].GetValue<string>());

            return new GenericValue<bool>(true);
        }

        [Command(Name = "is_func_defined")]
        public static IValue IsFunctionDefined(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(1)
                .Execute()
                .CanConvert<string>(0)
                .Results();

            return new GenericValue<bool>(ctx.HasCommand(args[0].GetValue<string>()));
        }

        [Command(Name = "typeof")]
        public static IValue Typeof(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(1)
                .Execute()
                .Results();

            return new GenericValue<Type>(args[0].GetValueType());
        }

        [Command(Name = "break_execution")]
        public static IValue BreakExecution(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Between(0, 1)
                .Execute()
                .Results();

            IValue result = args.Length == 0 ? GenericValue<object>.Default : args[0];

            ctx.BreakExecution(result);
            return result;
        }

        [Command(Name = "cancel_execution")]
        public static IValue CancelExecution(IExecutionContext ctx, IList<IArgument> arguments)
        {
            CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(0)
                .Results();

            ctx.CancelExecution();
            return GenericValue<object>.Default;
        }

        [Command(Name = "fault_execution")]
        public static IValue FaultExecution(IExecutionContext ctx, IList<IArgument> arguments)
        {
            CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(0)
                .Results();

            ctx.FaultExecution();
            return GenericValue<object>.Default;
        }

        [Command(Name ="export_context")]
        public static IValue ExportContext(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(2)
                .CanConvert<IExecutable>(1)
                .Execute(0)
                .CanConvert<IExecutionContext>(0)
                .Results();

            IExecutionContext exportTo = args[0].GetValue<IExecutionContext>();
            if (exportTo == null)
                throw new ContextException("Cannot export to a null context");

            return exportTo.Execute(args[1].GetValue<IExecutable>());
        }

        [Command(Name = "self_context")]
        public static IValue GetSelfContext(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(0)
                .Results();

            return new GenericValue<IExecutionContext>(ctx);
        }

        [Command(Name = "parent_context")]
        public static IValue GetParentContext(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(0)
                .Results();

            return new GenericValue<IExecutionContext>(ctx.GetParentContext());
        }

        [Command(Name = "global_context")]
        public static IValue GetGlobalContext(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(0)
                .Results();

            return new GenericValue<IExecutionContext>(ctx.GetGlobalContext());
        }

        [Command(Name = "define_module")]
        public static IValue DefineModule(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(1)
                .Execute()
                .CanConvert<string>()
                .Results();

            IExecutionContext parent = ctx.GetParentContext() ?? ctx;
            parent.DefineVariable(args[0].GetValue<string>(), new BasicVariable() { Value = new GenericValue<IExecutionContext>(ctx) });
            return new GenericValue<bool>(true);
        }

        [Command(Name = "create_context")]
        public static IValue CreateContext(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(0)
                .Results();

            return new GenericValue<IExecutionContext>(ctx.CreateChildContext());
        }
    }
}
