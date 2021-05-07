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
                throw new PascalError(this.line, this.column, "Se encontro un continue en un ambito incorrecto.", "Semantico");
            }

            Generator.Generator.GetInstance().AddGoto(env.continueVar);

            return null;
        }
    }
}
