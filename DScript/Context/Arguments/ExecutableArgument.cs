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
            return new GenericValue<ICodeBlock>(this.Code);
        }

        public IValue GetValue(IExecutionContext context)
        {
            return context.Execute(this.Code);
        }
    }
}
