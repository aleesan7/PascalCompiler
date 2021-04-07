using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Instructions
{
    class CaseElement : Instruction
    {
        public Expression condition;
        public Sentences bodySentences;
        public string outLabel = "";
        public bool isElse = false;

        public CaseElement(Expression condition, Sentences bodySentences, bool isElse, int line, int column) : base(line, column) 
        {
            this.condition = condition;
            this.bodySentences = bodySentences;
            this.isElse = isElse;
        }

        public override object Compile(Environment env)
        {
            var gen = Generator.Generator.GetInstance();
            gen.AddComment("inicia case element");
            Environment newEnv = new Environment(env);

            if (!this.isElse) 
            {
                Return cond = this.condition.Compile(env);
                
                if (cond.type.type == Types.BOOLEAN)
                {
                    newEnv.breakVar = this.outLabel;
                    gen.AddLabel(cond.trueLabel);
                    this.bodySentences.Compile(newEnv);
                    gen.AddGoto(outLabel);
                    gen.AddLabel(cond.falseLabel);
                }
            }
            else 
            {
                newEnv.breakVar = this.outLabel;
                this.bodySentences.Compile(newEnv);
            }
            

            gen.AddComment("finaliza case element");

            return null;
        }
    }
}
