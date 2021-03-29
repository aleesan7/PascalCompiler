using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Instructions
{
    class Sentences : Instruction
    {
        public LinkedList<Instruction> instructions;

        public Sentences(LinkedList<Instruction> instructions, int line, int column) : base (line, column) 
        {
            this.instructions = instructions;
        }

        public override object Compile(Environment env)
        {
            //Environment newEnvironment;
            //// const newEnv = enviorement.actualFunc == null ? new Enviorement(enviorement) : enviorement;
            //if (env.previous == null)
            //    newEnvironment = new Environment(env);
            //else
            //    newEnvironment = env;

            foreach(Instruction instruction in this.instructions) 
            {
                try 
                {
                    instruction.Compile(env);
                }
                catch (Exception ex) 
                {
                    //TODO new Semantic PascalError
                }
            }

            return null;
        }
    }
}
