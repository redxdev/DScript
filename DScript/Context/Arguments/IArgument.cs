using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Arguments
{
    public interface IArgument
    {
        void BeginExecution(IExecutionContext ctx);

        void EndExecution(IExecutionContext ctx);

        void PreExecute(IExecutionContext ctx);

        IValue GetRawValue();

        IValue GetValue();
    }
}
