using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context
{
    public interface ICodeBlock
    {
        IValue Execute(IExecutionContext context);
    }
}
