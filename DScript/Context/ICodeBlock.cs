using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Context.Arguments;

namespace DScript.Context
{
    public interface ICodeBlock
    {
        string Command
        {
            get;
            set;
        }

        IList<IArgument> Arguments
        {
            get;
            set;
        }

        IValue Execute(IExecutionContext context);
    }
}
