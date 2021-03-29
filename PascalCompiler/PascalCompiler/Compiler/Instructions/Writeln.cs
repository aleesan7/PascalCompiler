using PascalCompiler.Compiler.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Compiler.Instructions
{
    class Writeln : Abstract.Instruction
    {
        private Abstract.Expression value;
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
                    gen.AddPrintln("i", value.GetValue());
                    break;
                case Types.REAL:
                    gen.AddPrintln("d", value.GetValue());
                    break;
                case Types.STRING:
                    gen.AddPrintln("s", value.GetValue());
                    break;
                //TODO throw semantic error on default
            }

            if (this.isLine)
            {
                gen.AddPrintln("c", 10);
            }

            return null;
        }
    }
}
