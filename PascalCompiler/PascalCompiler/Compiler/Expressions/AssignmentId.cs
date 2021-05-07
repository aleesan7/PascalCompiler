using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Expressions
{
    class AssignmentId : Expression
    {
        public string id;
        public Expression previous;

        public AssignmentId(string id, Expression previous, int line, int column) : base(line, column) 
        {
            this.id = id;
            this.previous = previous;
        }

        public override Return Compile(Environment env)
        {
            var gen = Generator.Generator.GetInstance();

            if(this.previous == null) 
            {
                Symbol symbol = env.GetVar(this.id);

                if (symbol == null) 
                {
                    throw new PascalError(this.line, this.column, "La variable '" + this.id + "' no existe.", "Semantico");
                }

                if (symbol.isGlobal)
                {
                    return new Return(symbol.position + "", false, symbol.type, symbol);
                }
                else
                {
                    string temp = gen.NewTemporal();
                    gen.AddExpression(temp, "p", symbol.position, "+");
                    return new Return(temp, true, symbol.type, symbol);
                }
            }

            return null;
        }
    }
}
