using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Context;
using DScript.Context.Arguments;

namespace DScript.Utility
{
    public class ArgumentManager
    {
        private IExecutionContext context = null;

        private IList<IArgument> arguments = null;

        private IValue[] values = null;

        private LinkedList<Action> preExecutionActions = new LinkedList<Action>();

        private LinkedList<Action> executionActions = new LinkedList<Action>();

        private LinkedList<Action> postExecutionActions = new LinkedList<Action>();

        public ArgumentManager(IExecutionContext context, IList<IArgument> arguments)
        {
            this.context = context;
            this.arguments = arguments;

            values = new IValue[this.arguments.Count];
            for (int i = 0; i < this.arguments.Count; i++)
            {
                values[i] = this.arguments[i].GetRawValue(this.context);
            }
        }

        public IValue[] Results()
        {
            foreach(Action action in this.preExecutionActions)
            {
                action.Invoke();
            }

            foreach(Action action in this.executionActions)
            {
                action.Invoke();
            }

            foreach(Action action in this.postExecutionActions)
            {
                action.Invoke();
            }

            return this.values;
        }

        public ArgumentManager Exactly(int amount)
        {
            this.preExecutionActions.AddLast(() =>
            {
                if(this.arguments.Count != amount)
                {
                    throw new ArgumentCountException("Expected " + amount + " arguments, got " + this.arguments.Count);
                }
            });

            return this;
        }

        public ArgumentManager Between(int lower, int upper)
        {
            this.preExecutionActions.AddLast(() =>
                {
                    if (this.arguments.Count < lower || this.arguments.Count > upper)
                    {
                        throw new ArgumentCountException("Expected between " + lower + " and " + upper + " arguments, got " + this.arguments.Count);
                    }
                });

            return this;
        }

        public ArgumentManager AtLeast(int amount)
        {
            this.preExecutionActions.AddLast(() =>
                {
                    if (this.arguments.Count < amount)
                    {
                        throw new ArgumentCountException("Expected at least " + amount + " arguments, got " + this.arguments.Count);
                    }
                });

            return this;
        }

        public ArgumentManager AtMost(int amount)
        {
            this.preExecutionActions.AddLast(() =>
                {
                    if (this.arguments.Count > amount)
                    {
                        throw new ArgumentCountException("Expected at most " + amount + "arguments, got " + this.arguments.Count);
                    }
                });

            return this;
        }

        public ArgumentManager CanConvert<T1>(int index, bool raw = false)
        {
            LinkedList<Action> list = raw ? this.preExecutionActions : this.postExecutionActions;
            list.AddLast(() =>
                {
                    IValue value = this.values[index];
                    if (!value.CanConvert<T1>())
                    {
                        throw new ArgumentTypeException("Cannot convert argument " + index + " from " + value.GetValueType().FullName + " to " + typeof(T1).FullName);
                    }
                });

            return this;
        }

        public ArgumentManager CanConvert<T1, T2>(int index, bool raw = false)
        {
            LinkedList<Action> list = raw ? this.preExecutionActions : this.postExecutionActions;
            list.AddLast(() =>
            {
                IValue value = this.values[index];
                if (!value.CanConvert<T1>() && !value.CanConvert<T2>())
                {
                    throw new ArgumentTypeException("Cannot convert argument " + index + " from " + value.GetValueType().FullName + " to " + typeof(T1).FullName + " or " + typeof(T2).FullName);
                }
            });

            return this;
        }

        public ArgumentManager CanConvert<T1>(int start, int end, bool raw = false)
        {
            LinkedList<Action> list = raw ? this.preExecutionActions : this.postExecutionActions;
            list.AddLast(() =>
            {
                for (int i = start; i < end; i++)
                {
                    IValue value = this.values[i];
                    if (!value.CanConvert<T1>())
                    {
                        throw new ArgumentTypeException("Cannot convert argument " + i + " from " + value.GetValueType().FullName + " to " + typeof(T1).FullName);
                    }
                }
            });

            return this;
        }

        public ArgumentManager CanConvert<T1, T2>(int start, int end, bool raw = false)
        {
            LinkedList<Action> list = raw ? this.preExecutionActions : this.postExecutionActions;
            list.AddLast(() =>
            {
                for (int i = start; i < end; i++)
                {
                    IValue value = this.values[i];
                    if (!value.CanConvert<T1>() && !value.CanConvert<T2>())
                    {
                        throw new ArgumentTypeException("Cannot convert argument " + i + " from " + value.GetValueType().FullName + " to " + typeof(T1).FullName + " or " + typeof(T2).FullName);
                    }
                }
            });

            return this;
        }

        public ArgumentManager CanConvert<T1>(bool raw = false)
        {
            return this.CanConvert<T1>(0, this.arguments.Count);
        }

        public ArgumentManager CanConvert<T1, T2>(bool raw = false)
        {
            return this.CanConvert<T1, T2>(0, this.arguments.Count);
        }

        public ArgumentManager Execute()
        {
            return this.Execute(0, this.arguments.Count);
        }

        public ArgumentManager Execute(int index)
        {
            this.executionActions.AddLast(() =>
                {
                    this.values[index] = this.arguments[index].GetValue(this.context);
                });

            return this;
        }

        public ArgumentManager Execute(int start, int end)
        {
            this.executionActions.AddLast(() =>
                {
                    for(int i = start; i < end; i++)
                    {
                        this.values[i] = this.arguments[i].GetValue(this.context);
                    }
                });

            return this;
        }
    }
}
