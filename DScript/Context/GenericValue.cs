using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Utility.Conversion;

namespace DScript.Context
{
    public struct GenericValue<T> : IValue
    {
        public static GenericValue<T> Default = new GenericValue<T>(default(T));

        private T value;

        public GenericValue(T value)
        {
            this.value = value;
        }

        public V GetValue<V>()
        {
            if(this.value == null)
            {
                return default(V);
            }

            if(typeof(T) == typeof(V))
            {
                return (V)(object)this.value;
            }

            return TypeConverter.Convert<T, V>(this.value);
        }

        public bool CanConvert<V>()
        {
            return TypeConverter.CanConvert<T, V>();
        }

        public Type GetValueType()
        {
            return typeof(T);
        }

        public override string ToString()
        {
            if (value == null)
                return null;

            return value.ToString();
        }
    }
}
