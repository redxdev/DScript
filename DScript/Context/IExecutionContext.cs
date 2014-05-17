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
        bool HasVariable(string variable);

        bool TryGetVariable(string variable, out IVariable value);

        IVariable GetVariable(string variable);

        void DefineVariable(string variable, IVariable value);

        void UndefineVariable(string variable);

        bool HasCommand(string command);

        bool TryGetCommand(string command, out ScriptCommand value);

        ScriptCommand GetCommand(string command);

        void RegisterCommand(string name, ScriptCommand command);

        void RegisterAssembly(Assembly assembly);

        void UnregisterCommand(string name);

        void Execute(IExecutable executable);

        IValue Execute(ICodeBlock code);

        IValue Execute(string command, IList<IArgument> arguments);
    }
}
