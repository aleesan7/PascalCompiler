using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Instructions
{
    class Declare : Abstract.Instruction
    {
        private Type type;
        private Expression value;
        private List<string> idList;

        public Declare(Type type, Expression value, List<string> idList, int line, int column):base(line, column) 
        {
            this.type = type;
            this.value = value;
            this.idList = idList;
        }


        public override object Compile(Environment env)
        {
            var gen = Generator.Generator.GetInstance();
            Return value = this.value.Compile(env);
            if(!this.SameType(this.type, value.type)) 
            {
                //TODO Throw semantic error, the types aren´t the same
            }

            //this.validateType(enviorement);

            foreach(string id in this.idList) 
            {
                Symbol newVar = env.AddVar(id, value.type.type == Types.NULL ? this.type : value.type, false, false);

                if(newVar == null) 
                {
                    //TODO Throw new semantic error, the variable already exists
                }

                if (newVar.isGlobal) 
                {
                    if (this.type.type == Types.BOOLEAN)
                    {
                        string templabel = gen.NewLabel();
                        gen.AddLabel(value.trueLabel);
                        gen.AddSetStack(newVar.position, "1");
                        gen.AddGoto(templabel);
                        gen.AddLabel(value.falseLabel);
                        gen.AddSetStack(newVar.position, "0");
                        gen.AddLabel(templabel);
                    }
                    else
                    {
                        gen.AddSetStack(newVar.position, value.GetValue());
                    }
                }
                else 
                {
                    string temporal = gen.NewTemporal();
                    gen.FreeTemp(temporal);
                    gen.AddExpression(temporal, "p", newVar.position, "+");
                    if (this.type.type == Types.BOOLEAN)
                    {
                        string templabel = gen.NewLabel();
                        gen.AddLabel(value.trueLabel);
                        gen.AddSetStack(temporal, "1");
                        gen.AddGoto(templabel);
                        gen.AddLabel(value.falseLabel);
                        gen.AddSetStack(temporal, "0");
                        gen.AddLabel(templabel);
                    }
                    else
                    {
                        gen.AddSetStack(temporal, value.GetValue());
                    }
                }
            }

            return null;
        }
    }
}
