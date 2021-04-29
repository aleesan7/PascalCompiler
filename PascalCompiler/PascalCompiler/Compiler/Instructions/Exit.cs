using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Instructions
{
    class Exit : Instruction
    {
        private Expression value;

        public Exit(Expression value, int line, int column) : base(line, column) 
        {
            this.value = value;
        }
        public override object Compile(Environment env)
        {
            Return value = this.value != null ? this.value.Compile(env) : new Return("0", false, new Type(Types.VOID, null), null);
            FunctionSymbol symFunc = env.actualFunction;
            var gen = Generator.Generator.GetInstance();

            if (symFunc == null) {
                //throw new Error(this.line, this.column, 'Semantico', 'Return fuera de una funcion'); 
            }

            //if (!this.sameType(symFunc.type, value.type)) {
            //    //throw new Error(this.line, this.column, 'Semantico', `Se esperaba ${ symFunc.type.type } y se obtuvo ${ value.type.type}`); 
            //}

            if (symFunc.type.type == Types.BOOLEAN)
            {
                string templabel = gen.NewLabel();
                gen.AddLabel(value.trueLabel);
                gen.AddSetStack("p", "1");
                gen.AddCode("return;");
                gen.AddGoto(templabel);
                gen.AddLabel(value.falseLabel);
                gen.AddSetStack("p", "0");
                gen.AddCode("return;");
                gen.AddLabel(templabel);
            }
            else if (symFunc.type.type != Types.VOID)
                gen.AddSetStack("p", value.GetValue());

            gen.AddGoto(env.returnVar == null ? env.returnVar : "");

            return null;
        }
    }
}
