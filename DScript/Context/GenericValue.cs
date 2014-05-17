using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context
{
    public struct GenericValue<T> : IValue
    {
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

            return (V)Convert.ChangeType(this.value, typeof(V));
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}
