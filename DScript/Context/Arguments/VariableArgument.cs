using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Context.Variables;

namespace DScript.Context.Arguments
{
    [Obsolete("Use CodeBlockArgument with the command \"get\" and a single string argument with the name of the variable")]
    public class VariableArgument : IArgument
    {
        public string Variable
        {
            get;
            set;
        }

        private IValue result = null;

        public IValue GetRawValue()
        {
            return new GenericValue<string>(this.Variable);
        }

        public void Execute(IExecutionContext ctx)
        {
            this.result = ctx.GetVariable(this.Variable).Value;
        }

        public bool DidExecute()
        {
            return this.result != null;
        }

        public void Reset()
        {
            this.result = null;
        }

        public IValue GetValue()
        {
            return this.result;
        }
    }
}
