using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DScript.Context;
using DScript.Language;
using Antlr4.Runtime;

namespace DScript.Utility
{
    public static class LanguageUtilities
    {
        public static IExecutable ParseString(string input)
        {
            return Parse(new AntlrInputStream(input));
        }

        public static IExecutable ParseFile(string filename)
        {
            return Parse(new AntlrFileStream(filename));
        }

        public static IExecutable Parse(ICharStream input)
        {
            DSLangLexer lexer = new DSLangLexer(input);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(LexerErrorListener.Instance);

            CommonTokenStream tokenStream = new CommonTokenStream(lexer);

            DSLangParser parser = new DSLangParser(tokenStream);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(ParserErrorListener.Instance);

            IExecutable executable = parser.compileUnit().executable;

            return executable;
        }
    }
}
