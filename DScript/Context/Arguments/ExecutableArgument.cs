using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Arguments
{
    public class ExecutableArgument : IArgument
    {
        public IExecutable Executable
        {
            get;
            set;
        }

        public IValue GetRawValue(IExecutionContext context)
        {
            return new GenericValue<IExecutable>(this.Executable);
        }

        public IValue GetValue(IExecutionContext context)
        {
            IExecutionContext localCtx = context.CreateChildContext();
            return localCtx.Execute(this.Executable);
        }
    }
}
