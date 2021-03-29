using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Instructions
{
    class While : Instruction
    {
        public Expression condition;
        public Sentences bodySentences;

        public While(Expression condition, Sentences bodySentences, int line, int column) : base(line, column) 
        {
            this.condition = condition;
            this.bodySentences = bodySentences;
        }

        public override object Compile(Environment env)
        {
            var gen = Generator.Generator.GetInstance();
            Environment newEnv = new Environment(env);

            string lblWhile = gen.NewLabel();
            gen.AddComment("inicia while");

            gen.AddLabel(lblWhile);

            Return condition = this.condition.Compile(env);

            if(condition.type.type == Types.BOOLEAN) 
            {
                newEnv.breakVar = condition.falseLabel;
                newEnv.continueVar = lblWhile;
                gen.AddLabel(condition.trueLabel);
                this.bodySentences.Compile(newEnv);
                gen.AddGoto(lblWhile);
                gen.AddLabel(condition.falseLabel);
                gen.AddComment("finaliza while");

            }
            else 
            {
                //TODO throw new Error(this.line,this.column,'Semantico',`La condicion no es booleana: ${condition?.type.type}`);
            }

            return null;
        }
    }
}
