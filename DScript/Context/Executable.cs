using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context
{
    public class Executable : IExecutable
    {
        public IEnumerable<ICodeBlock> CodeBlocks
        {
            get;
            set;
        }

        public IValue Execute(IExecutionContext context)
        {
            IValue last = GenericValue<object>.Default;
            foreach(ICodeBlock code in this.CodeBlocks)
            {
                last = context.Execute(code);
            }

            return last;
        }
    }
}
