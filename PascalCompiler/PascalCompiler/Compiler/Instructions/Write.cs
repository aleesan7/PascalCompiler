using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Instructions
{
    class Write : Instruction
    {
        private Expression value;
        private bool isLine;

        public Write(Expression value, bool isLine, int line, int column) : base(line, column) 
        {
            this.value = value;
            this.isLine = isLine;
        }
        public override object Compile(Environment env)
        {
            Return value = this.value.Compile(env);
            var gen = Generator.Generator.GetInstance();
            switch (value.type.type)
            {
                case Types.INTEGER:
                    gen.AddPrintf("i", "(int)" + value.GetValue());
                    break;
                case Types.REAL:
                    gen.AddPrintf("f", value.GetValue());
                    break;
                case Types.BOOLEAN:
                    string tempLabel = gen.NewLabel();
                    gen.AddLabel(value.trueLabel);
                    gen.AddPrintTrue();
                    gen.AddGoto(tempLabel);
                    gen.AddLabel(value.falseLabel);
                    gen.AddPrintFalse();
                    gen.AddLabel(tempLabel);
                    break;
                case Types.STRING:
                    gen.AddPrintf("s", value.GetValue());
                    break;
                    //TODO throw semantic error on default
            }

            if (this.isLine)
            {
                gen.AddPrintf("c", 10);
            }

            return null;
        }
    }
}
