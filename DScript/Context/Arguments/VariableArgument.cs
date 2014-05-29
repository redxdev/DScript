using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Context.Variables;

namespace DScript.Context.Arguments
{
    [Obsolete("Use CodeBlockArgument with the command \"get\" and a single string argument with the name of the variable")]
    public class VariableArgument : AbstractArgument
    {
        public string Variable
        {
            get;
            set;
        }

        public override IValue GetRawValue()
        {
            return new GenericValue<string>(this.Variable);
        }

        protected override IValue Execute(IExecutionContext ctx)
        {
            return ctx.GetVariable(this.Variable).Value;
        }
    }
}
