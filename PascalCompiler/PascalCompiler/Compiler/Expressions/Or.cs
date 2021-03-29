using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Expressions
{
    class Or : Expression
    {
        public Expression left;
        public Expression right;

        public Or(Expression left, Expression right, int line, int column) : base (line, column) 
        {
            this.left = left;
            this.right = right;
        }

        public override Return Compile(Environment env)
        {
            var gen = Generator.Generator.GetInstance();

            this.trueLabel = this.trueLabel == "" ? gen.NewLabel() : this.trueLabel;
            this.falseLabel = this.falseLabel == "" ? gen.NewLabel() : this.falseLabel;

            this.left.trueLabel = this.right.trueLabel = this.trueLabel;
            this.left.falseLabel = gen.NewLabel();
            this.right.falseLabel = this.falseLabel;

            Return left = this.left.Compile(env);

            gen.AddLabel(this.left.falseLabel);

            Return right = this.right.Compile(env);

            if (left.type.type == Types.BOOLEAN && right.type.type == Types.BOOLEAN)
            {
                Return ret = new Return("", false, left.type, null);
                ret.trueLabel = this.trueLabel;
                ret.falseLabel = this.right.falseLabel;
                return ret;
            }

            return null;
            //TODO throw new Error(this.line, this.column, 'Semantico', `No se puede Or: ${ left.type.type } || ${ right.type.type}`);
        }
    }
}
