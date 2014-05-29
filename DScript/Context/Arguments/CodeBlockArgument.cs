using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Arguments
{
    public class CodeBlockArgument : AbstractArgument
    {
        public ICodeBlock Code
        {
            get;
            set;
        }

        public override IValue GetRawValue()
        {
            return new GenericValue<ICodeBlock>(this.Code);
        }

        protected override IValue Execute(IExecutionContext ctx)
        {
            return ctx.Execute(this.Code);
        }
    }
}