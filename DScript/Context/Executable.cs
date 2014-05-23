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

        private IValue returnValue = null;

        public void BreakExecution(IValue returnValue)
        {
            this.returnValue = returnValue;
        }

        public IValue Execute(IExecutionContext context)
        {
            IValue last = GenericValue<object>.Default;
            foreach(ICodeBlock code in this.CodeBlocks)
            {
                last = context.Execute(code);

                if (returnValue != null)
                    return returnValue;
            }

            return last;
        }
    }
}
