using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Utility
{
    [Serializable]
    public class ArgumentCountException : ArgumentException
    {
        public ArgumentCountException()
            : base()
        {
        }

        public ArgumentCountException(string message)
            : base(message)
        {
        }

        public ArgumentCountException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
