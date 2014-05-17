using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Variables
{
    [Serializable]
    public class VariableAlreadyDefinedException : Exception
    {
        public VariableAlreadyDefinedException()
            : base()
        {
        }

        public VariableAlreadyDefinedException(string message)
            : base(message)
        {
        }

        public VariableAlreadyDefinedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
