using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DScript.Context;
using DScript.Context.Arguments;
using DScript.Context.Attributes;
using DScript.Context.Variables;
using DScript.Utility;

namespace DScript.Library
{
    public static class IOLibrary
    {
        [ContextRegistration]
        public static void ContextRegistration(IExecutionContext ctx)
        {
            ctx.DefineVariable("io.cwd", new ConstantVariable(new GenericValue<string>(Directory.GetCurrentDirectory())));
        }

        [Command(Name = "io.read_file")]
        public static IValue ReadFile(IExecutionContext ctx, IList<IArgument> arguments)
        {
            var args = CommandUtilities.ManageArguments(ctx, arguments)
                .Exactly(1)
                .Execute()
                .CanConvert<string>(0)
                .Results();

            string result = "";
            using(StreamReader reader = new StreamReader(args[0].GetValue<string>()))
            {
                result = reader.ReadToEnd();
            }

            return new GenericValue<string>(result);
        }
    }
}
