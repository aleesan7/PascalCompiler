using PascalCompiler.Compiler.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Compiler.Expressions
{
    class AccessId : Expression
    {
        private string id;
        private Expression previous;

        public AccessId(string id, Expression previous, int line, int column):base(line, column) 
        {
            this.id = id;
            this.previous = previous;
        }

        public override Return Compile(Environment env)
        {
            var gen = Generator.Generator.GetInstance();

            if (this.previous == null) 
            {
                Symbol symbol = env.GetVar(this.id);

                if(symbol== null) 
                {
                    //TODO Throw semantic error: the variable doesn´t exist
                }
                string temporal = gen.NewTemporal();

                if (symbol.isGlobal) 
                {
                    gen.AddGetStack(temporal, symbol.position);
                    if (symbol.type.type != Types.BOOLEAN)
                        return new Return(temporal, true, symbol.type, symbol);

                    //TODO Boolean vars
                    //const retorno = new Retorno('', false, symbol.type, symbol);
                    //this.trueLabel = this.trueLabel == '' ? generator.newLabel() : this.trueLabel;
                    //this.falseLabel = this.falseLabel == '' ? generator.newLabel() : this.falseLabel;
                    //generator.addIf(temp, '1', '==', this.trueLabel);
                    //generator.addGoto(this.falseLabel);
                    //retorno.trueLabel = this.trueLabel;
                    //retorno.falseLabel = this.falseLabel;
                    //return retorno;
                }
                else 
                {
                    //TODO Not global vars
                }
            }
            else 
            {
                //TODO Nested environments vars
            }

            throw new NotImplementedException();
        }
    }
}
