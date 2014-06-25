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

            try
            {
                return TypeConverter.Convert<T, V>(this.value);
            }
            catch(ConversionException e)
            {
                if(typeof(V) == typeof(string))
                {
                    return (V)(object)this.value.ToString();
                }
                
                throw;
            }
        }

        public bool CanConvert<V>()
        {
            return TypeConverter.CanConvert<T, V>();
        }

        public Type GetValueType()
        {
            return typeof(T);
        }

        public object GetObject()
        {
            return this.value;
        }

        public override string ToString()
        {
            if (value == null)
                return null;

            return value.ToString();
        }

        public override bool Equals(object obj)
        {
            if(obj is IValue)
            {
                IValue other = obj as IValue;
                if(this.value == null)
                {
                    return other.GetObject() == null;
                }

                return this.value.Equals(other.GetObject());
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.value == null ? 0 : this.value.GetHashCode();
        }
    }
}
