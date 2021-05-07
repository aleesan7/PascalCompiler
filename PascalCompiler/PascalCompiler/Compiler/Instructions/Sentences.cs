using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Instructions
{
    class Sentences : Instruction
    {
        public LinkedList<Instruction> instructions;
        public LinkedList<string> errorsList;
        public Sentences(LinkedList<Instruction> instructions, int line, int column) : base (line, column) 
        {
            this.instructions = instructions;
            this.errorsList = new LinkedList<string>();
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
                catch (PascalError err) 
                {
                    //TODO new Semantic PascalError
                    this.errorsList.AddLast(err.GetMesage());
                }
            }

            if(this.errorsList.Count > 0)
            {
                string path = "SemanticErrors.txt";
                try
                {
                    foreach (string input in this.errorsList)
                    {
                        if (!File.Exists(path))
                        {
                            File.WriteAllText(path, input + System.Environment.NewLine);
                        }
                        else
                        {
                            File.AppendAllText(path, input + System.Environment.NewLine);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return null;
        }
    }
}
