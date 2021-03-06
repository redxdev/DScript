﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Context;
using DScript.Utility.Conversion.Attributes;

namespace DScript.Utility.Conversion
{
    [Converter(ForType = typeof(int))]
    public class IntConverter : BaseConverter
    {
        public IntConverter()
        {
            this.AddRelationship<int, string>((obj) => int.Parse(obj), (obj) => obj.ToString());
            this.AddRelationship<int, float>((obj) => (int)obj, (obj) => (float)obj);
            this.AddRelationship<int, double>((obj) => (int)obj, (obj) => (double)obj);
            this.AddRelationship<int, bool>((obj) => obj ? 1 : 0, (obj) => obj != 0 ? true : false);
        }
    }

    [Converter(ForType = typeof(float))]
    public class FloatConverter : BaseConverter
    {
        public FloatConverter()
        {
            this.AddRelationship<float, string>((obj) => float.Parse(obj), (obj) => obj.ToString());
            this.AddRelationship<float, double>((obj) => (float)obj, (obj) => (double)obj);
            this.AddRelationship<float, bool>((obj) => obj ? 1 : 0, (obj) => obj != 0 ? true : false);
        }
    }

    [Converter(ForType = typeof(double))]
    public class DoubleConverter : BaseConverter
    {
        public DoubleConverter()
        {
            this.AddRelationship<double, string>((obj) => double.Parse(obj), (obj) => obj.ToString());
            this.AddRelationship<double, bool>((obj) => obj ? 1 : 0, (obj) => obj != 0 ? true : false);
        }
    }

    [Converter(ForType = typeof(bool))]
    public class BoolConverter : BaseConverter
    {
        public BoolConverter()
        {
            this.AddRelationship<bool, string>((obj) => bool.Parse(obj), (obj) => obj.ToString());
        }
    }

    [Converter(ForType = typeof(IList<IValue>))]
    public class ListConverter : BaseConverter
    {
        public ListConverter()
        {
            this.AddRelationship<IList<IValue>, string>((obj) =>
            {
                throw new ConversionException("Cannot convert from string to List");
            },
            (obj) =>
            {
                string[] results = new string[obj.Count];
                for(int i = 0; i < obj.Count; i++)
                {
                    results[i] = obj[i].GetValue<string>();
                }

                return string.Join(", ", results);
            });
        }
    }

    [Converter(ForType = typeof(IExecutable))]
    public class ExecutableConverter : BaseConverter
    {
        public ExecutableConverter()
        {
            this.AddRelationship<IExecutable, ICodeBlock>((obj) =>
            {
                List<ICodeBlock> codeBlocks = new List<ICodeBlock>();
                codeBlocks.Add(obj);
                return new Executable()
                    {
                        CodeBlocks = codeBlocks
                    };
            },
            (obj) =>
            {
                throw new ConversionException("Cannot convert from IExecutable to ICodeBlock");
            });
        }
    }
}
