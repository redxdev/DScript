using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Context.Variables;

namespace DScript.Context.Arguments
{
    public class VariableArgument : IArgument
    {
        public string Variable
        {
            get;
            set;
        }

        public IValue GetRawValue(IExecutionContext context)
        {
            return new GenericValue<string>(this.Variable);
        }

        public IValue GetValue(IExecutionContext context)
        {
            return context.GetVariable(this.Variable).Value;
        }
    }
}
