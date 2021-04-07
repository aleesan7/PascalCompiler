using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Instructions
{
    class For : Instruction
    {
        public Assignment assignment;
        public Expression condition;
        public Assignment increment;
        public Sentences bodySentences;

        public For(Assignment assignment, Expression condition, Assignment increment, Sentences bodySentences, int line, int column) : base(line, column) 
        {
            this.assignment = assignment;
            this.condition = condition;
            this.increment = increment;
            this.bodySentences = bodySentences;
        }
        public override object Compile(Environment env)
        {
            var gen = Generator.Generator.GetInstance();
            Environment newEnv = new Environment(env);

            string lblFor = gen.NewLabel();
            gen.AddComment("Inicia For");

            assignment.Compile(env);
            gen.AddLabel(lblFor);

            Return condition = this.condition.Compile(env);

            string lblActualizar = gen.NewLabel();
            gen.AddLabel(lblActualizar);
            this.increment.Compile(newEnv);
            gen.AddGoto(lblFor);

            if (condition.type.type == Types.BOOLEAN) 
            {
                newEnv.continueVar = lblActualizar;
                newEnv.breakVar = condition.falseLabel;
                gen.AddLabel(condition.trueLabel);
                this.bodySentences.Compile(newEnv);
                gen.AddGoto(lblActualizar);
                gen.AddLabel(condition.falseLabel);
                gen.AddComment("Finaliza for");
            }

            return null;
        }
    }
}
