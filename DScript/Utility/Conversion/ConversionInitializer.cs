using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DScript.Utility.Conversion.Attributes;

namespace DScript.Utility.Conversion
{
    internal class ConversionInitializer
    {
        public ConversionInitializer(Dictionary<Type, TypeConverter> converters)
        {
            foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(Type type in assembly.GetTypes())
                {
                    TypeConverter instance = null;
                    foreach(ConverterAttribute attribute in type.GetCustomAttributes<ConverterAttribute>(false))
                    {
                        if(instance == null)
                        {
                            instance = (TypeConverter)Activator.CreateInstance(type);
                        }

                        converters.Add(attribute.ForType, instance);
                    }
                }
            }
        }
    }
}
