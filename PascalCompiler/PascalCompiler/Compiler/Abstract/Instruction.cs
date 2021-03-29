using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Compiler.Abstract
{
    abstract class Instruction
    {
        int line;
        int column;

        protected Instruction(int line, int column)
        {
            this.line = line;
            this.column = column;
        }

        public abstract object Compile(Environment env);

        public bool SameType(Type type1, Type type2) 
        {
            if(type1.type == type2.type) 
            {
                //TODO Struct type
                return true;
            }
            else 
            {
                //TODO struct type
            
            }


            return false;
        }

    }
}
