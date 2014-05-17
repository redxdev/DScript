﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScript.Context.Variables
{
    public class DelegatedVariable
    {
        public DelegatedVariable()
        {
            this.Getter = null;
            this.Setter = null;
        }

        public delegate string GetterFunction();

        public delegate void SetterFunction(string value);

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

        public string Value
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
