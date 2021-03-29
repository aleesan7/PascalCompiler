using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Compiler.Abstract
{
    abstract class Expression
    {
        public string trueLabel;
        public string falseLabel;
        public int line;
        public int column;

        protected Expression(int line, int column)
        {
            this.trueLabel = this.falseLabel = "";
            this.line = line;
            this.column = column;
        }

        public abstract Return Compile(Environment env);

    }
}
