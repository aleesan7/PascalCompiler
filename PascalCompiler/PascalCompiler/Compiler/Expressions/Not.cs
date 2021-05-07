using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Expressions
{
    class Not : Expression
    {
        public Expression val;

        public Not(Expression val, int line, int column) : base(line, column) 
        {
            this.val = val;
        }

        public override Return Compile(Environment env)
        {
            var gen = Generator.Generator.GetInstance();

            this.trueLabel = this.trueLabel == "" ? gen.NewLabel() : this.trueLabel;
            this.falseLabel = this.falseLabel == "" ? gen.NewLabel() : this.falseLabel;

            this.val.trueLabel = this.falseLabel;
            this.val.falseLabel = this.trueLabel;

            Return value = this.val.Compile(env);
            if (value.type.type == Types.BOOLEAN)
            {
                Return ret = new Return("", false, value.type, null);
                ret.trueLabel = this.trueLabel;
                ret.falseLabel = this.falseLabel;
                return ret;
            }

            throw new PascalError(this.line, this.column, "No se puede realizar la operacion NOT. " + $"NOT {value.type.type}", "Semantico");
        }
    }
}
