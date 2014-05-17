using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Arguments
{
    public interface IArgument
    {
        IValue GetRawValue(IExecutionContext context);

        IValue GetValue(IExecutionContext context);
    }
}
