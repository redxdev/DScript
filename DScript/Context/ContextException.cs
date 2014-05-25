using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context
{
    [Serializable]
    public class ContextException : Exception
    {
        public ContextException()
            : base()
        {
        }

        public ContextException(string message)
            : base(message)
        {
        }

        public ContextException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    [Serializable]
    public class FaultException : ContextException
    {
        public FaultException()
            : base()
        {
        }

        public FaultException(string message)
            : base(message)
        {
        }

        public FaultException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
