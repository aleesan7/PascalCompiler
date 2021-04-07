using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Instructions
{
    class Continue : Instruction
    {
        public Continue(int line, int column) : base(line, column) 
        {
        
        }
        public override object Compile(Environment env)
        {
            if (env.continueVar == null)
            {
                //TODO throw new Error(this.line, this.column, 'Semantico', 'Continue en un ambito incorrecto');
            }

            Generator.Generator.GetInstance().AddGoto(env.continueVar);

            return null;
        }
    }
}
