using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Expressions
{
    class Literal : Abstract.Expression
    {
        private Types type;
        private object value;

        public Literal(Types type, object value, int line, int column):base(line, column)
        {
            this.type = type;
            this.value = value;
        }

        public override Return Compile(Environment env)
        {
            switch (this.type) 
            {
                case Types.INTEGER:
                    return new Return(this.value.ToString(), false, new Type(Types.INTEGER, null), null);
                case Types.REAL:
                    return new Return(this.value.ToString(), false, new Type(Types.REAL, null), null);
                case Types.STRING:
                    return new Return(this.value.ToString(), false, new Type(Types.STRING, null), null);
                default:
                    return null;
                //TODO Boolean
            }
        }
    }
}
