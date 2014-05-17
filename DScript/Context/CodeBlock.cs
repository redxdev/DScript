using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Context.Arguments;

namespace DScript.Context
{
    public class CodeBlock : ICodeBlock
    {
        public string Command
        {
            get;
            set;
        }

        public IList<IArgument> Arguments
        {
            get;
            set;
        }

        public IValue Execute(IExecutionContext context)
        {
            return context.Execute(this.Command, this.Arguments);
        }
    }
}
