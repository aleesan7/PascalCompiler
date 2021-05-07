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
                case Types.BOOLEAN:
                    var gen = Generator.Generator.GetInstance();
                    Return ret = new Return("", false, new Type(Types.BOOLEAN, null), null);
                    this.trueLabel = this.trueLabel == "" ? gen.NewLabel() : this.trueLabel;
                    this.falseLabel = this.falseLabel == "" ? gen.NewLabel() : this.falseLabel;

                    if (bool.Parse(this.value.ToString())) 
                    {
                        gen.AddGoto(this.trueLabel);
                    }
                    else
                    {
                        gen.AddGoto(this.falseLabel);
                    }

                    ret.trueLabel = this.trueLabel;
                    ret.falseLabel = this.falseLabel;

                    return ret;

                case Types.STRING:
                    return new Return(this.value.ToString(), false, new Type(Types.STRING, null), null);
                default:
                    return null;
            }
        }
    }
}
