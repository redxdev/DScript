using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Utility.Conversion
{
    public class NoConveter : TypeConverter
    {
        public static readonly NoConveter Instance = new NoConveter();

        private static HashSet<Type> EmptySet = new HashSet<Type>();

        public override ISet<Type> GetConversionsTo()
        {
            return EmptySet;
        }

        public override ISet<Type> GetConversionsFrom()
        {
            return EmptySet;
        }

        public override object ConvertTo(Type t, object obj)
        {
            return null;
        }

        public override object ConvertFrom(Type t, object obj)
        {
            return null;
        }

        public override bool CanConvertFrom(Type t)
        {
            return false;
        }

        public override bool CanConvertTo(Type t)
        {
            return false;
        }
    }
}
