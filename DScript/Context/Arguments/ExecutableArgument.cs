using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Arguments
{
    public class ExecutableArgument : IArgument
    {
        public ICodeBlock Code
        {
            get;
            set;
        }

        public IValue GetRawValue(IExecutionContext context)
        {
            return new GenericValue<string>(this.Code.ToString());
        }

        public IValue GetValue(IExecutionContext context)
        {
            return context.Execute(this.Code);
        }
    }
}
