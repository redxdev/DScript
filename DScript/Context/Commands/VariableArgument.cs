using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Context.Variables;

namespace DScript.Context.Commands
{
    public class VariableArgument : IArgument
    {
        private string variable;

        public VariableArgument(string variable)
        {
            this.variable = variable;
        }

        public string GetRawValue(IExecutionContext context)
        {
            return this.variable;
        }

        public string GetValue(IExecutionContext context)
        {
            IVariable v = null;
            if(!context.TryGetVariable(this.variable, out v))
            {
                throw new VariableNotDefinedException("Variable \"" + this.variable + "\" is not defined");
            }

            return v.Value;
        }
    }
}
