using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Expressions
{
    class String : Expression
    {
        public Type type;
        public string value;

        public String(Type type, string value, int line, int column) : base(line, column) 
        {
            this.type = type;
            this.value = value;
        }

        public override Return Compile(Environment env)
        {
            var gen = Generator.Generator.GetInstance();

            string temp = gen.NewTemporal();
            gen.AddExpression(temp, "h", "", "");

            foreach(char c in this.value.Trim().Substring(1,this.value.Length-2)) 
            {
                gen.AddSetHeap("h", (int)c);
                gen.NextHeap();
            }


            gen.AddSetHeap("h", "-1");
            gen.NextHeap();

            return new Return(temp, true, new Type(Types.STRING, null), null);
        }
    }
}
