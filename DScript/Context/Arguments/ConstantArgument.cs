using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Arguments
{
    public class ConstantArgument : IArgument
    {
        private readonly IValue value;

        public ConstantArgument(IValue value)
        {
            this.value = value;
        }

        public void BeginExecution(IExecutionContext ctx)
        {
        }

        public void EndExecution(IExecutionContext ctx)
        {
        }

        public void PreExecute(IExecutionContext ctx)
        {
        }

        public IValue GetRawValue()
        {
            return this.value;
        }

        public IValue GetValue()
        {
            return this.value;
        }
    }
}
