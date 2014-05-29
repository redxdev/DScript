using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Arguments
{
    public class ExecutableArgument : AbstractArgument
    {
        public IExecutable Executable
        {
            get;
            set;
        }

        public override IValue GetRawValue()
        {
            return new GenericValue<IExecutable>(this.Executable);
        }

        protected override IValue Execute(IExecutionContext ctx)
        {
            return ctx.Execute(this.Executable);
        }
    }
}
