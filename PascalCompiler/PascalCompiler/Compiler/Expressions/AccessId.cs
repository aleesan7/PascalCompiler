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
                    Return retorno = new Return("", false, symbol.type, symbol);
                    this.trueLabel = this.trueLabel == "" ? gen.NewLabel() : this.trueLabel;
                    this.falseLabel = this.falseLabel == "" ? gen.NewLabel() : this.falseLabel;
                    gen.AddIf(temporal, "1", "==", this.trueLabel);
                    gen.AddGoto(this.falseLabel);
                    retorno.trueLabel = this.trueLabel;
                    retorno.falseLabel = this.falseLabel;
                    return retorno;
                }
                else 
                {
                    string tempAux = gen.NewTemporal(); 
                    gen.FreeTemp(tempAux);
                    gen.AddExpression(tempAux, "p", symbol.position, "+");
                    gen.AddGetStack(temporal, tempAux);
                    if (symbol.type.type != Types.BOOLEAN) 
                        return new Return(temporal, true, symbol.type, symbol);

                    Return retorno = new Return("", false, symbol.type, null);
                    this.trueLabel = this.trueLabel == "" ? gen.NewLabel() : this.trueLabel;
                    this.falseLabel = this.falseLabel == "" ? gen.NewLabel() : this.falseLabel;
                    gen.AddIf(temporal, "1", "==", this.trueLabel);
                    gen.AddGoto(this.falseLabel);
                    retorno.trueLabel = this.trueLabel;
                    retorno.falseLabel = this.falseLabel;
                    return retorno;
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
