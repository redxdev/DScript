using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context
{
    public class Executable : IExecutable
    {
        public IEnumerable<ICodeBlock> CodeBlocks
        {
            get;
            set;
        }

        public IEnumerable<ICodeBlock> GetCodeBlocks()
        {
            return this.CodeBlocks;
        }

        public void Execute(IExecutionContext context)
        {
            foreach(ICodeBlock code in this.CodeBlocks)
            {
                context.Execute(code);
            }
        }
    }
}
