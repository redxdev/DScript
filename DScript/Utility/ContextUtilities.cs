using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DScript.Context;
using DScript.Context.Variables;

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

        public static IExecutionContext CreateEmptyModule(IExecutionContext ctx, string name)
        {
            IExecutionContext module = ctx.CreateChildContext();
            ctx.DefineVariable(name, new ConstantVariable(new GenericValue<IExecutionContext>(module)));
            return module;
        }
    }
}
