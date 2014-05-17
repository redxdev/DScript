using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Variables
{
    public class ConstantVariable : IVariable
    {
        private readonly IValue value;

        public ConstantVariable(IValue value)
        {
            this.value = value;
        }

        public IValue Value
        {
            get
            {
                return this.value;
            }

            set
            {
                throw new MemberAccessException("Cannot set Value on a ConstantVariable");
            }
        }
    }
}
