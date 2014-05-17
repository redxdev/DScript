using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Context;
using DScript.Utility;

namespace DSConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IExecutionContext context = new ExecutionContext();
            ContextUtilities.RegisterAllAssemblies(context, AppDomain.CurrentDomain);
        }
    }
}
