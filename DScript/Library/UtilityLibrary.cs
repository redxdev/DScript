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
    public static class UtilityLibrary
    {
        [Command(Name = "echo")]
        public static IValue Echo(IExecutionContext ctx, IList<IArgument> arguments)
        {
            string[] str = new string[arguments.Count];
            for (int i = 0; i < arguments.Count; i++)
            {
                str[i] = arguments[i].GetValue(ctx).ToString();
            }

            string result = string.Join(" ", str);

            return new GenericValue<string>(result);
        }
    }
}
