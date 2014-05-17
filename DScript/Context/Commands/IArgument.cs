using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Commands
{
    public interface IArgument
    {
        string GetRawValue(IExecutionContext context);

        string GetValue(IExecutionContext context);
    }
}
