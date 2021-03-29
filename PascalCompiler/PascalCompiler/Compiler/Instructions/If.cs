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
                //TODO throw new Error(this.line,this.column,'Semantico',`La condicion no es booleana: ${condition?.type.type}`);
            }

            return null;
        }
    }
}
