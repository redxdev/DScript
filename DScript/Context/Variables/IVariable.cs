using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Variables
{
    public interface IVariable
    {
        IValue Value
        {
            get;
            set;
        }
    }
}
