using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Commands
{
    public class ConstantArgument : IArgument
    {
        private readonly string value;

        public ConstantArgument(string value)
        {
            this.value = value;
        }

        public string GetRawValue(IExecutionContext context)
        {
            return this.value;
        }

        public string GetValue(IExecutionContext context)
        {
            return this.value;
        }
    }
}
