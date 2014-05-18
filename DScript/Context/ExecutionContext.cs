using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DScript.Context.Variables;
using DScript.Context.Arguments;
using DScript.Context.Attributes;

namespace DScript.Context
{
    public class ExecutionContext : IExecutionContext
    {
        public const int MajorVersion = 1;
        public const int MinorVersion = 0;

        private Dictionary<string, IVariable> variables = new Dictionary<string, IVariable>();

        private Dictionary<string, ScriptCommand> commands = new Dictionary<string, ScriptCommand>();

        public ExecutionContext()
        {
        }

        public int GetMajorVersion()
        {
            return MajorVersion;
        }

        public int GetMinorVersion()
        {
            return MinorVersion;
        }

        public bool HasVariable(string variable)
        {
            return this.variables.ContainsKey(variable);
        }

        public bool TryGetVariable(string variable, out IVariable value)
        {
            return this.variables.TryGetValue(variable, out value);
        }

        public IVariable GetVariable(string variable)
        {
            IVariable result = null;
            if(!this.TryGetVariable(variable, out result))
            {
                throw new VariableNotDefinedException("Variable \"" + variable + "\" is not defined");
            }

            return result;
        }

        public void DefineVariable(string variable, IVariable value)
        {
            if(this.HasVariable(variable))
            {
                throw new VariableAlreadyDefinedException("Variable \"" + variable + "\" has already been defined");
            }

            this.variables[variable] = value;
        }

        public void UndefineVariable(string variable)
        {
            if(!this.HasVariable(variable))
            {
                throw new VariableNotDefinedException("Variable \"" + variable + "\" is not defined");
            }

            this.variables.Remove(variable);
        }

        public bool HasCommand(string command)
        {
            return this.commands.ContainsKey(command);
        }

        public bool TryGetCommand(string command, out ScriptCommand value)
        {
            return this.commands.TryGetValue(command, out value);
        }

        public ScriptCommand GetCommand(string command)
        {
            ScriptCommand result = null;
            if(!this.TryGetCommand(command, out result))
            {
                throw new CommandNotDefinedException("Command \"" + command + "\" is not defined");
            }

            return result;
        }

        public void RegisterCommand(string name, ScriptCommand command)
        {
            if(this.HasCommand(name))
            {
                throw new CommandAlreadyDefinedException("Command \"" + command + "\" has already been defined");
            }

            this.commands[name] = command;
        }

        public void RegisterAssembly(Assembly assembly)
        {
            foreach(Type type in assembly.GetTypes())
            {
                foreach(MethodInfo methodInfo in type.GetMethods())
                {
                    if (!methodInfo.IsStatic)
                        return;

                    foreach(CommandAttribute commandAttribute in methodInfo.GetCustomAttributes<CommandAttribute>(false))
                    {
                        this.RegisterCommand(commandAttribute.Name, (ScriptCommand)ScriptCommand.CreateDelegate(typeof(ScriptCommand), methodInfo));
                    }

                    foreach(ContextRegistrationAttribute contextRegAttribute in methodInfo.GetCustomAttributes<ContextRegistrationAttribute>(false))
                    {
                        ContextRegistration contextReg = (ContextRegistration)ContextRegistration.CreateDelegate(typeof(ContextRegistration), methodInfo);
                        contextReg(this);
                    }
                }
            }
        }

        public void UnregisterCommand(string name)
        {
            if(!this.HasCommand(name))
            {
                throw new CommandNotDefinedException("Command \"" + name + "\" is not defined");
            }

            this.commands.Remove(name);
        }

        public IValue Execute(IExecutable executable)
        {
            return executable.Execute(this);
        }

        public IValue Execute(ICodeBlock code)
        {
            return code.Execute(this);
        }

        public IValue Execute(string command, IList<IArgument> arguments)
        {
            IValue result = this.GetCommand(command)(this, arguments);
            if (result == null)
                result = GenericValue<object>.Default;
            return result;
        }
    }
}
