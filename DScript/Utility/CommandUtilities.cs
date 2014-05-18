using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Context;
using DScript.Context.Arguments;

namespace DScript.Utility
{
    public static class CommandUtilities
    {
        public static ArgumentManager ManageArguments(IExecutionContext context, IList<IArgument> args)
        {
            return new ArgumentManager(context, args);
        }
    }
}
