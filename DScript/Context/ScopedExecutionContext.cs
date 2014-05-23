﻿using System;
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
    public class ScopedExecutionContext : IExecutionContext
    {
        public const int MajorVersion = 1;
        public const int MinorVersion = 0;

        private Dictionary<string, IVariable> variables = new Dictionary<string, IVariable>();

        private Dictionary<string, ScriptCommand> commands = new Dictionary<string, ScriptCommand>();

        private IExecutionContext parent = null;

        private IExecutionContext global = null;

        private IExecutable currentExecution = null;

        public ScopedExecutionContext(IExecutionContext parent = null, IExecutionContext global = null)
        {
            this.parent = parent;
            this.global = global ?? this;

            if (this.global == this && this.parent != null)
                throw new ContextException("Cannot create global context with parent");
        }

        public int GetMajorVersion()
        {
            return MajorVersion;
        }

        public int GetMinorVersion()
        {
            return MinorVersion;
        }

        public IExecutionContext GetParentContext()
        {
            return this.parent;
        }

        public IExecutionContext GetGlobalContext()
        {
            return this.global;
        }

        public IExecutionContext CreateChildContext()
        {
            return new ScopedExecutionContext(this, this.global);
        }

        public bool HasVariable(string variable, bool includeParent = true)
        {
            if (this.variables.ContainsKey(variable))
                return true;

            if (includeParent && this.parent != null && this.parent.HasVariable(variable))
                return true;

            return false;
        }

        public bool TryGetVariable(string variable, out IVariable value, bool includeParent = true)
        {
            if (this.variables.TryGetValue(variable, out value))
                return true;

            if (includeParent && this.parent != null && this.parent.TryGetVariable(variable, out value))
                return true;

            return false;
        }

        public IVariable GetVariable(string variable, bool includeParent = true)
        {
            IVariable result = null;
            if(!this.TryGetVariable(variable, out result, includeParent))
            {
                throw new VariableNotDefinedException("Variable \"" + variable + "\" is not defined");
            }

            return result;
        }

        public void DefineVariable(string variable, IVariable value)
        {
            if(this.HasVariable(variable, false))
            {
                throw new VariableAlreadyDefinedException("Variable \"" + variable + "\" has already been defined");
            }

            this.variables[variable] = value;
        }

        public void UndefineVariable(string variable)
        {
            if(!this.HasVariable(variable, false))
            {
                throw new VariableNotDefinedException("Variable \"" + variable + "\" is not defined");
            }

            this.variables.Remove(variable);
        }

        public bool HasCommand(string command, bool includeParent = true)
        {
            if (this.commands.ContainsKey(command))
                return true;

            if (includeParent && this.parent != null && this.parent.HasCommand(command))
                return true;

            return false;
        }

        public bool TryGetCommand(string command, out ScriptCommand value, bool includeParent = true)
        {
            if (this.commands.TryGetValue(command, out value))
                return true;

            if (includeParent && this.parent != null && this.parent.TryGetCommand(command, out value))
                return true;

            return false;
        }

        public ScriptCommand GetCommand(string command, bool includeParent = true)
        {
            ScriptCommand result = null;
            if(!this.TryGetCommand(command, out result, includeParent))
            {
                throw new CommandNotDefinedException("Command \"" + command + "\" is not defined");
            }

            return result;
        }

        public void RegisterCommand(string name, ScriptCommand command)
        {
            if(this.HasCommand(name, false))
            {
                throw new CommandAlreadyDefinedException("Command \"" + command + "\" has already been defined");
            }

            this.commands[name] = command;
        }

        public void RegisterAssembly(Assembly assembly)
        {
            foreach(Type type in assembly.GetTypes())
            {
                this.RegisterType(type);
            }
        }

        public void RegisterType(Type type)
        {
            foreach (MethodInfo methodInfo in type.GetMethods())
            {
                if (!methodInfo.IsStatic)
                    break;

                foreach (CommandAttribute commandAttribute in methodInfo.GetCustomAttributes<CommandAttribute>(false))
                {
                    this.RegisterCommand(commandAttribute.Name, (ScriptCommand)ScriptCommand.CreateDelegate(typeof(ScriptCommand), methodInfo));
                }

                foreach (ContextRegistrationAttribute contextRegAttribute in methodInfo.GetCustomAttributes<ContextRegistrationAttribute>(false))
                {
                    ContextRegistration contextReg = (ContextRegistration)ContextRegistration.CreateDelegate(typeof(ContextRegistration), methodInfo);
                    contextReg(this);
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

        public void BreakExecution(IValue value)
        {
            if(this.currentExecution != null)
            {
                this.currentExecution.BreakExecution(value);
            }
        }

        public IValue Execute(IExecutable executable)
        {
            try
            {
                this.currentExecution = executable;
                return executable.Execute(this);
            }
            finally
            {
                this.currentExecution = null;
            }
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