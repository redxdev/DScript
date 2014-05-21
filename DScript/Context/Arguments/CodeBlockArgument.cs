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

        public IValue GetRawValue(IExecutionContext context)
        {
            return new GenericValue<ICodeBlock>(this.Code);
        }

        public IValue GetValue(IExecutionContext context)
        {
            IExecutionContext localCtx = context.CreateChildContext();
            return localCtx.Execute(this.Code);
        }
    }
}