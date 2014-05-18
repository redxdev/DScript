using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DScript.Context
{
    public struct GenericValue<T> : IValue
    {
        public static GenericValue<T> Default = new GenericValue<T>(default(T));

        private string value;

        public GenericValue(T value)
        {
            this.value = value == null ? null : value.ToString();
        }

        public V GetValue<V>()
        {
            if(this.value == null)
            {
                return default(V);
            }

            if(typeof(string) == typeof(V))
            {
                return (V)(object)this.value;
            }

            var converter = TypeDescriptor.GetConverter(typeof(V));
            if (converter != null && converter.CanConvertFrom(typeof(string)))
            {
                return (V)converter.ConvertFrom(this.value);
            }

            return default(V);
        }

        public bool CanConvert<V>()
        {
            if (typeof(V) == typeof(string))
                return true;

            var converter = TypeDescriptor.GetConverter(typeof(V));
            return converter != null && converter.CanConvertFrom(typeof(string));
        }

        public Type GetValueType()
        {
            return typeof(string);
        }

        public override string ToString()
        {
            if (value == null)
                return null;

            return value.ToString();
        }
    }
}
