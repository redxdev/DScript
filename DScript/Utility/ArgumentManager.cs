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

        private IValue[] results = null;

        public ArgumentManager(IExecutionContext context, IList<IArgument> args)
        {
            this.context = context;
            this.arguments = args;
        }

        public ArgumentManager Amount(int amount)
        {
            if(this.arguments.Count != amount)
            {
                throw new ArgumentCountException("Expected " + amount + " arguments, got " + this.arguments.Count);
            }

            return this;
        }

        public ArgumentManager AmountBetween(int lower, int upper)
        {
            if(this.arguments.Count < lower || this.arguments.Count > upper)
            {
                throw new ArgumentCountException("Expected between " + lower + " and " + upper + " arguments, got " + this.arguments.Count);
            }

            return this;
        }

        public ArgumentManager AtLeast(int amount)
        {
            if(this.arguments.Count < amount)
            {
                throw new ArgumentCountException("Expected at least " + amount + " arguments, got " + this.arguments.Count);
            }

            return this;
        }

        public ArgumentManager AtMost(int amount)
        {
            if(this.arguments.Count > amount)
            {
                throw new ArgumentCountException("Expected at most " + amount + "arguments, got " + this.arguments.Count);
            }

            return this;
        }

        public ArgumentManager Execute()
        {
            this.results = new IValue[this.arguments.Count];
            for(int i = 0; i < this.arguments.Count; i++)
            {
                this.results[i] = this.arguments[i].GetValue(this.context);
            }

            return this;
        }

        public ArgumentManager CanConvert<T>(int index)
        {
            if(this.results == null)
            {
                throw new ArgumentException("ArgumentManager.Execute must be called before functions acting upon results");
            }

            IValue value = this.results[index];
            if(!value.CanConvert<T>())
            {
                throw new ArgumentTypeException("Cannot convert argument " + index + " from " + value.GetValueType().FullName + " to " + typeof(T).FullName);
            }

            return this;
        }

        public IValue[] Results()
        {
            if (this.results == null)
            {
                throw new ArgumentException("ArgumentManager.Execute must be called before functions acting upon results");
            }

            return this.results;
        }
    }
}
