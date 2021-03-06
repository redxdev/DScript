﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DScript.Context;
using DScript.Context.Arguments;
using DScript.Context.Attributes;
using DScript.Context.Variables;
using DScript.Utility;

namespace DScript.Library
{
    public static class UtilityLibrary
    {
        [Command(Name = "echo")]
        public static IValue Echo(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Exactly(1)
                .Results();

            return args[0];
        }

        [Command(Name = "concat")]
        public static IValue Concat(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .AtLeast(2)
                .CanConvert<string>()
                .Results();

            string[] str = new string[args.Length];
            for(int i = 0; i < args.Length; i++)
            {
                str[i] = args[i].GetValue<string>();
            }

            string result = string.Join("", str);
            return new GenericValue<string>(result);
        }

        [Command(Name = "time")]
        public static IValue Time(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Exactly(1)
                .CanConvert<IExecutable>()
                .Results();

            IExecutable execute = args[0].GetValue<IExecutable>();
            IExecutionContext localCtx = ctx.CreateChildContext();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            execute.Execute(localCtx);
            stopwatch.Stop();

            return new GenericValue<double>(stopwatch.Elapsed.TotalSeconds);
        }



        private static string trace_recurse(IExecutionContext ctx, ICodeBlock code, string currIndent)
        {
            string result = currIndent + code.Command;
            string indent = currIndent + "  ";

            foreach (IArgument arg in code.Arguments)
            {
                result += Environment.NewLine + indent + arg.GetType().Name + "[RAW]" + Environment.NewLine;
                if (arg is CodeBlockArgument)
                {
                    result += trace_recurse(ctx, (arg as CodeBlockArgument).Code, indent + "  ");
                }
                else if (arg is ExecutableArgument)
                {
                    result += trace_recurse(ctx, (arg as ExecutableArgument).Executable, indent + "  ");
                }
                else
                {
                    IValue value = arg.GetRawValue();

                    if (typeof(ICodeBlock).IsAssignableFrom(value.GetValueType()))
                    {
                        result += indent + "  ICodeBlock" + Environment.NewLine;
                        result += trace_recurse(ctx, value.GetValue<ICodeBlock>(), indent + "    ");
                    }
                    else if (typeof(IExecutable).IsAssignableFrom(value.GetValueType()))
                    {
                        result += trace_recurse(ctx, value.GetValue<IExecutable>(), indent + "  ");
                    }
                    else
                    {
                        result += indent + "  " + value.ToString();
                    }
                }
            }

            return result;
        }

        private static string trace_recurse(IExecutionContext ctx, IExecutable executable, string currIndent)
        {
            string result = currIndent + "IExecutable";
            string indent = currIndent + "  ";

            foreach (ICodeBlock code in executable.CodeBlocks)
            {
                result += Environment.NewLine + trace_recurse(ctx, code, indent);
            }

            return result;
        }

        [Command(Name = "trace")]
        public static IValue Trace(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(arguments)
                .Exactly(1)
                .CanConvert<IExecutable>()
                .Results();

            return new GenericValue<string>(trace_recurse(ctx, args[0].GetValue<IExecutable>(), ""));
        }
    }
}
