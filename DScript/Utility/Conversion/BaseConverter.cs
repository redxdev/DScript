using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Utility.Conversion
{
    public abstract class BaseConverter : TypeConverter
    {
        protected Dictionary<Type, Delegate> conversionsTo = new Dictionary<Type, Delegate>();

        protected Dictionary<Type, Delegate> conversionsFrom = new Dictionary<Type, Delegate>();

        protected void AddRelationship<My,T>(Func<T,My> convertFrom, Func<My,T> convertTo)
        {
            conversionsTo.Add(typeof(T), convertTo);
            conversionsFrom.Add(typeof(T), convertFrom);
        }

        public override ISet<Type> GetConversionsFrom()
        {
            return new HashSet<Type>(conversionsFrom.Keys);
        }

        public override ISet<Type> GetConversionsTo()
        {
            return new HashSet<Type>(conversionsTo.Keys);
        }

        public override object ConvertFrom(Type t, object obj)
        {
            Delegate conversion = null;
            if(!conversionsFrom.TryGetValue(t, out conversion))
            {
                throw new ConversionException("Unable to convert from " + t.FullName);
            }

            try
            {
                return conversion.DynamicInvoke(obj);
            }
            catch(Exception e)
            {
                throw new ConversionException("Unable to convert from " + t.FullName, e);
            }
        }

        public override object ConvertTo(Type t, object obj)
        {
            Delegate conversion = null;
            if (!conversionsTo.TryGetValue(t, out conversion))
            {
                throw new ConversionException("Unable to convert to " + t.FullName);
            }

            try
            {
                return conversion.DynamicInvoke(obj);
            }
            catch(Exception e)
            {
                throw new ConversionException("Unable to convert to " + t.FullName, e);
            }
        }
    }
}
