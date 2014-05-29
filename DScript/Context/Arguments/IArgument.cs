using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Arguments
{
    public interface IArgument
    {
        IValue GetRawValue();

        void Execute(IExecutionContext ctx);

        bool DidExecute();

        IValue GetValue();
    }
}
