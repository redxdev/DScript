﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context
{
    [Serializable]
    public class CommandNotDefinedException : CommandException
    {
        public CommandNotDefinedException()
            : base()
        {
        }

        public CommandNotDefinedException(string message)
            : base(message)
        {
        }

        public CommandNotDefinedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
