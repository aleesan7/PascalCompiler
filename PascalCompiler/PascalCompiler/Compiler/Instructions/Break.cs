using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Instructions
{
    class Break : Instruction
    {
        public Break(int line, int column) : base(line, column)
        {
        
        }

        public override object Compile(Environment env)
        {
            if (env.breakVar == null)
            {
                throw new PascalError(this.line, this.column, "Se encontro un break en un ambito incorrecto.", "Semantico");
            }

            Generator.Generator.GetInstance().AddGoto(env.breakVar);

            return null;
        }
    }
}
