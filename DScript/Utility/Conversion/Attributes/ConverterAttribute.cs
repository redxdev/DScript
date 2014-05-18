using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Utility.Conversion.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    class ConverterAttribute : Attribute
    {
        public Type ForType
        {
            get;
            set;
        }
    }
}
