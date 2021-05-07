using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Instructions
{
    class If : Instruction
    {
        public Expression condition;
        public Sentences bodyInstructions;
        public Sentences ElseInstructions;

        public If(Expression condition, Sentences bodyInstructions, Sentences ElseInstructions, int line, int column) : base(line, column) 
        {
            this.condition = condition;
            this.bodyInstructions = bodyInstructions;
            this.ElseInstructions = ElseInstructions;
        }

        public override object Compile(Environment env)
        {
            var gen = Generator.Generator.GetInstance();

            gen.AddComment("If begins");

            Return cond = this.condition.Compile(env);

            Environment newEnv = new Environment(env);

            if(cond.type.type == Types.BOOLEAN) 
            {
                gen.AddLabel(cond.trueLabel);
                this.bodyInstructions.Compile(newEnv);

                if (this.ElseInstructions != null) 
                {
                    string tempLabel = gen.NewLabel();
                    gen.AddGoto(tempLabel);
                    gen.AddLabel(cond.falseLabel);
                    this.ElseInstructions.Compile(newEnv);
                    gen.AddLabel(tempLabel);
                }
                else 
                {
                    gen.AddLabel(cond.falseLabel);
                }
            }
            else 
            {
                throw new PascalError(this.line, this.column, "La condicion del if no es de tipo booleano: " + $"{cond.type.type}", "Semantico");
            }

            return null;
        }
    }
}
