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

        private bool cancel = false;

        public void BreakExecution(IValue returnValue)
        {
            this.returnValue = returnValue;
        }

        public void CancelExecution()
        {
            this.cancel = true;
        }

        public bool DidBreak()
        {
            return this.returnValue != null;
        }

        public bool DidCancel()
        {
            return this.cancel;
        }

        public IValue Execute(IExecutionContext context)
        {
            this.returnValue = null;
            this.cancel = false;

            IValue last = GenericValue<object>.Default;
            foreach(ICodeBlock code in this.CodeBlocks)
            {
                last = context.Execute(code);

                if (returnValue != null)
                    return returnValue;

                if (this.cancel)
                    break;
            }

            return last;
        }
    }
}
