using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Variables
{
    public class DelegatedVariable : IVariable
    {
        public DelegatedVariable()
        {
            this.Getter = null;
            this.Setter = null;
        }

        public delegate IValue GetterFunction();

        public delegate void SetterFunction(IValue value);

        public GetterFunction Getter
        {
            get;
            set;
        }

        public SetterFunction Setter
        {
            get;
            set;
        }

        public IValue Value
        {
            get
            {
                if(this.Getter == null)
                {
                    throw new MemberAccessException("No Getter set for DelegatedVariable");
                }

                return this.Getter();
            }

            set
            {
                if(this.Setter == null)
                {
                    throw new MemberAccessException("No Setter set for DelegatedVariable");
                }

                this.Setter(value);
            }
        }
    }
}
