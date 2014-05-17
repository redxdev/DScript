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

        public IValue GetRawValue(IExecutionContext context)
        {
            return this.value;
        }

        public IValue GetValue(IExecutionContext context)
        {
            return this.value;
        }
    }
}
