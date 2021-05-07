using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Instructions
{
    class Repeat : Instruction
    {
        public Expression condition;
        public Sentences bodySentences;

        public Repeat(Expression condition, Sentences bodySentences, int line, int column) : base(line, column) 
        {
            this.condition = condition;
            this.bodySentences = bodySentences;
        }
        public override object Compile(Environment env)
        {
            var gen = Generator.Generator.GetInstance();
            Environment newEnv = new Environment(env);

            gen.AddComment("inicia repeat");
            newEnv.continueVar = this.condition.trueLabel = gen.NewLabel();
            newEnv.breakVar = this.condition.falseLabel = gen.NewLabel();

            gen.AddLabel(this.condition.falseLabel);

            this.bodySentences.Compile(newEnv);

            Return condition = this.condition.Compile(env);

            if(condition.type.type == Types.BOOLEAN) 
            {
                gen.AddLabel(condition.trueLabel);
                gen.AddComment("finaliza repeat");
            }
            else 
            {
                throw new PascalError(this.line, this.column, "La condicion del if no es de tipo booleano: " + $"{condition.type.type}", "Semantico");
            }

            return null;
        }
    }
}
