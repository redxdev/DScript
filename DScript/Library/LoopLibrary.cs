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
        public static IValue For(IExecutionContext ctx, IList<IArgument> arguments)
        {


            return new GenericValue<bool>(true);
        }
    }
}
