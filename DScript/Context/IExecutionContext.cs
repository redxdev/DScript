using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Context.Variables;

namespace DScript.Context
{
    public interface IExecutionContext
    {
        bool HasVariable(string variable);

        bool TryGetVariable(string variable, out IVariable value);

        IVariable GetVariable(string variable);

        void SetVariable(string variable, IVariable value);

        void UnsetVariable(string variable);
    }
}
