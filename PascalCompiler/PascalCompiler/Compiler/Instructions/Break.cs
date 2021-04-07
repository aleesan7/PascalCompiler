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
                //TODO throw new Error(this.line, this.column, 'Semantico', 'Break en un ambito incorrecto');
            }

            Generator.Generator.GetInstance().AddGoto(env.breakVar);

            return null;
        }
    }
}
