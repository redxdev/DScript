using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Arguments
{
    public class CodeBlockArgument : IArgument
    {
        public ICodeBlock Code
        {
            get;
            set;
        }

        private IValue result = null;

        public IValue GetRawValue()
        {
            return new GenericValue<ICodeBlock>(this.Code);
        }

        public void Execute(IExecutionContext ctx)
        {
            this.result = ctx.Execute(this.Code);
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