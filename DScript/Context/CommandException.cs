using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context
{
    [Serializable]
    public class CommandException : ContextException
    {
        public CommandException()
            : base()
        {
        }

        public CommandException(string message)
            : base(message)
        {
        }

        public CommandException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
