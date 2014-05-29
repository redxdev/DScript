using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Arguments
{
    public abstract class AbstractArgument : IArgument
    {
        private Stack<IValue> resultStack = new Stack<IValue>();

        private bool isPreExecuted = false;

        protected abstract IValue Execute(IExecutionContext ctx);

        public void BeginExecution(IExecutionContext ctx)
        {
            if(!this.isPreExecuted)
            {
                this.resultStack.Push(this.Execute(ctx));
            }
            else
            {
                this.isPreExecuted = false;
            }
        }

        public void EndExecution(IExecutionContext ctx)
        {
            if(this.isPreExecuted)
            {
                this.isPreExecuted = false;
                resultStack.Pop();
            }

            resultStack.Pop();
        }

        public void PreExecute(IExecutionContext ctx)
        {
            this.isPreExecuted = true;
            this.resultStack.Push(this.Execute(ctx));
        }

        public abstract IValue GetRawValue();

        public IValue GetValue()
        {
            return resultStack.Peek();
        }
    }
}
