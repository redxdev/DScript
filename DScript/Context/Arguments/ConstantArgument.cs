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

        public IValue GetRawValue()
        {
            return this.value;
        }

        public void Execute(IExecutionContext ctx)
        {
        }

        public bool DidExecute()
        {
            return true;
        }

        public IValue GetValue()
        {
            return this.value;
        }
    }
}
