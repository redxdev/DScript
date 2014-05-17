using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Variables
{
    [Serializable]
    public class VariableNotDefinedException : KeyNotFoundException
    {
        public VariableNotDefinedException(string message)
            : base(message)
        {
        }

        public VariableNotDefinedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
