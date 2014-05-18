using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Utility.Conversion
{
    [Serializable]
    public class ConversionException : Exception
    {
        public ConversionException()
            : base()
        {
        }

        public ConversionException(string message)
            : base(message)
        {
        }

        public ConversionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
