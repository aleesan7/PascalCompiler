using PascalCompiler.Compiler.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Compiler.Instructions
{
    class Writeln : Instruction
    {
        private Expression value;
        private bool isLine;
        public Writeln(Abstract.Expression value, bool isLine, int line, int column):base(line,column)
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
                    gen.AddPrintfNewLine("s", "\\n");
                    break;
                case Types.REAL:
                    gen.AddPrintf("f", value.GetValue());
                    gen.AddPrintfNewLine("s", "\\n");
                    break;
                case Types.BOOLEAN:
                    string tempLabel = gen.NewLabel();
                    gen.AddLabel(value.trueLabel);
                    gen.AddPrintTrue();
                    gen.AddGoto(tempLabel);
                    gen.AddLabel(value.falseLabel);
                    gen.AddPrintFalse();
                    gen.AddLabel(tempLabel);
                    gen.AddPrintfNewLine("s", "\\n");
                    break;
                case Types.STRING:
                    //gen.AddNextEnv(env.size);
                    //gen.AddSetStack('p', value.GetValue());
                    gen.AddExpression("T5", value.GetValue(), "", "");
                    gen.AddCall("native_print_str");
                    //gen.addAntEnv(env.size);
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
