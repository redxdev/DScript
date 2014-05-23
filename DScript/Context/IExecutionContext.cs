using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DScript.Context.Variables;
using DScript.Context.Arguments;

namespace DScript.Context
{
    public delegate IValue ScriptCommand(IExecutionContext context, IList<IArgument> arguments);

    public delegate void ContextRegistration(IExecutionContext context);

    /// <summary>
    /// The script execution context, which contains variables and commands.
    /// </summary>
    public interface IExecutionContext
    {
        int GetMajorVersion();

        int GetMinorVersion();

        IExecutionContext GetParentContext();

        IExecutionContext GetGlobalContext();

        IExecutionContext CreateChildContext();

        bool HasVariable(string variable, bool includeParent = true);

        bool TryGetVariable(string variable, out IVariable value, bool includeParent = true);

        IVariable GetVariable(string variable, bool includeParent = true);

        void DefineVariable(string variable, IVariable value);

        void UndefineVariable(string variable);

        bool HasCommand(string command, bool includeParent = true);

        bool TryGetCommand(string command, out ScriptCommand value, bool includeParent = true);

        ScriptCommand GetCommand(string command, bool includeParent = true);

        void RegisterCommand(string name, ScriptCommand command);

        void RegisterAssembly(Assembly assembly);

        void RegisterType(Type type);

        void UnregisterCommand(string name);

        void BreakExecution(IValue value);

        IValue Execute(IExecutable executable);

        IValue Execute(ICodeBlock code);

        IValue Execute(string command, IList<IArgument> arguments);
    }
}
