using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DScript.Context;

namespace DScript.Utility
{
    public static class ContextUtilities
    {
        public static void RegisterAllAssemblies(IExecutionContext context, AppDomain domain)
        {
            foreach(Assembly assembly in domain.GetAssemblies())
            {
                context.RegisterAssembly(assembly);
            }
        }
    }
}
