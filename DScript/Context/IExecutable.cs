using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context
{
    public interface IExecutable
    {
        IEnumerable<ICodeBlock> CodeBlocks
        {
            get;
            set;
        }

        void BreakExecution(IValue value);

        void CancelExecution();

        void FaultBreak();

        bool DidBreak();

        bool DidCancel();

        bool DidFault();

        IValue Execute(IExecutionContext context);
    }
}
