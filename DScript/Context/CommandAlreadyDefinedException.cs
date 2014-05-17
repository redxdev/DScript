using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context
{
    [Serializable]
    public class CommandAlreadyDefinedException : Exception
    {
        public CommandAlreadyDefinedException()
            : base()
        {
        }

        public CommandAlreadyDefinedException(string message)
            : base(message)
        {
        }

        public CommandAlreadyDefinedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
