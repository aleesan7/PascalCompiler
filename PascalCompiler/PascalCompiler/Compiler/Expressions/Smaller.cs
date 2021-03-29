using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Expressions
{
    class Smaller : Expression
    {
        public Expression left;
        public Expression right;
        public bool isSmallerOrEqual;

        public Smaller(Expression left, Expression right, bool isSmallerOrEqual, int line, int column) : base(line, column) 
        {
            this.left = left;
            this.right = right;
            this.isSmallerOrEqual = isSmallerOrEqual;
        }

        public override Return Compile(Environment env)
        {
            Return left = this.left.Compile(env);
            Return right = this.right.Compile(env);

            Types leftType = left.type.type;
            Types rightType = right.type.type;

            var gen = Generator.Generator.GetInstance();

            if ((leftType == Types.INTEGER || leftType == Types.REAL || leftType == Types.CHAR) && (rightType == Types.INTEGER || rightType == Types.REAL || rightType == Types.CHAR))
            {
                this.trueLabel = this.trueLabel == "" ? gen.NewLabel() : this.trueLabel;
                this.falseLabel = this.falseLabel == "" ? gen.NewLabel() : this.falseLabel;

                if (this.isSmallerOrEqual)
                {
                    gen.AddIf(left.GetValue(), right.GetValue(), "<=", this.trueLabel);
                }
                else
                {
                    gen.AddIf(left.GetValue(), right.GetValue(), "<", this.trueLabel);
                }

                gen.AddGoto(this.falseLabel);
                Return ret = new Return("", false, new Type(Types.BOOLEAN, null), null);
                ret.trueLabel = this.trueLabel;
                ret.falseLabel = this.falseLabel;
                return ret;
            }
            return null;
            //TODO throw new Error(this.line, this.column, 'Semantico', `No se puede ${lefType} > ${rightType}`);
        }
    }
}
