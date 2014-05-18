using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Utility.Conversion.Attributes;

namespace DScript.Utility.Conversion
{
    [Converter(ForType = typeof(int))]
    public class IntConverter : BaseConverter
    {
        public IntConverter()
        {
            this.AddRelationship<int, string>((obj) => int.Parse(obj), (obj) => obj.ToString());
            this.AddRelationship<int, float>((obj) => (int)obj, (obj) => (float)obj);
            this.AddRelationship<int, double>((obj) => (int)obj, (obj) => (double)obj);
        }
    }

    [Converter(ForType = typeof(float))]
    public class FloatConverter : BaseConverter
    {
        public FloatConverter()
        {
            this.AddRelationship<float, string>((obj) => float.Parse(obj), (obj) => obj.ToString());
            this.AddRelationship<float, double>((obj) => (float)obj, (obj) => (double)obj);
        }
    }

    [Converter(ForType = typeof(double))]
    public class DoubleConverter : BaseConverter
    {
        public DoubleConverter()
        {
            this.AddRelationship<double, string>((obj) => double.Parse(obj), (obj) => obj.ToString());
        }
    }
}
