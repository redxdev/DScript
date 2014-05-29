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

        private IValue result = null;

        public IValue GetRawValue()
        {
            return new GenericValue<IExecutable>(this.Executable);
        }

        public void Execute(IExecutionContext ctx)
        {
            this.result = ctx.Execute(this.Executable);
        }

        public bool DidExecute()
        {
            return this.result != null;
        }

        public IValue GetValue()
        {
            return this.result;
        }
    }
}
