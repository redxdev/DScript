using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Utility
{
    [Serializable]
    public class ArgumentTypeException : ArgumentException
    {
        public ArgumentTypeException()
            : base()
        {
        }

        public ArgumentTypeException(string message)
            : base(message)
        {
        }

        public ArgumentTypeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
