using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Utility.Conversion
{
    /// <summary>
    /// Similar to System.ComponentModel.TypeConverter, except that it can handle intermediate types. That is,
    /// if type A can convert to type B and type B can convert to type C, but A cannot convert to C, then
    /// Converter will automatically figure out how to convert from type A to type B to type C.
    /// </summary>
    public abstract class TypeConverter
    {
        private static Dictionary<Type, TypeConverter> converters = new Dictionary<Type, TypeConverter>();

        private static ConversionInitializer intializer = new ConversionInitializer(converters);

        public static TypeConverter GetConverter(Type t)
        {
            TypeConverter converter = null;
            if(!converters.TryGetValue(t, out converter))
            {
                return NoConveter.Instance;
            }

            return converter;
        }

        public static bool CanConvert(Type fromType, Type toType)
        {
            if (fromType == toType || fromType.IsSubclassOf(toType))
                return true;

            TypeConverter from = null;
            TypeConverter to = null;
            converters.TryGetValue(fromType, out from);
            converters.TryGetValue(toType, out to);

            if (from != null && from.CanConvertTo(toType))
                return true;

            if (to != null && to.CanConvertFrom(fromType))
                return true;

            if (from == null || to == null)
            {
                return false;
            }

            ISet<Type> fromConversions = from.GetConversionsTo();
            ISet<Type> toConversions = to.GetConversionsFrom();

            return fromConversions.Union(toConversions).Count() > 0;
        }

        public static bool CanConvert<From, To>()
        {
            return CanConvert(typeof(From), typeof(To));
        }

        public static To Convert<From, To>(From obj)
        {
            return (To)Convert(typeof(From), typeof(To), obj);
        }

        public static object Convert(Type fromType, Type toType, object obj)
        {
            if (fromType == toType || fromType.IsSubclassOf(toType))
                return obj;

            TypeConverter from = null;
            TypeConverter to = null;
            converters.TryGetValue(fromType, out from);
            converters.TryGetValue(toType, out to);

            if (from != null && from.CanConvertTo(toType))
            {
                return from.ConvertTo(toType, obj);
            }

            if (to != null && to.CanConvertFrom(fromType))
            {
                return to.ConvertFrom(fromType, obj);
            }

            if(from == null || to == null)
            {
                throw new ConversionException("No conversion possible between " + fromType.FullName + " and " + toType.FullName);
            }

            ISet<Type> fromConversions = from.GetConversionsTo();
            ISet<Type> toConversions = to.GetConversionsFrom();

            LinkedList<Type> canConvert = new LinkedList<Type>(fromConversions.Union(toConversions));
            foreach (Type iType in canConvert)
            {
                try
                {
                    object intermediate = Convert(fromType, iType, obj);
                    return Convert(iType, toType, intermediate);
                }
                catch (Exception)
                {
                }
            }

            throw new ConversionException("No conversion possible between " + fromType.FullName + " and " + toType.FullName);
        }

        public abstract ISet<Type> GetConversionsTo();

        public abstract ISet<Type> GetConversionsFrom();

        public abstract object ConvertTo(Type t, object obj);

        public abstract object ConvertFrom(Type t, object obj);

        public virtual bool CanConvertFrom(Type t)
        {
            return GetConversionsFrom().Contains(t);
        }

        public virtual bool CanConvertTo(Type t)
        {
            return GetConversionsTo().Contains(t);
        }
    }
}
