using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Instructions
{
    class Case : Instruction
    {
        public Expression expr;
        public LinkedList<CaseElement> cases;

        public Case(Expression expr, LinkedList<CaseElement> cases, int line, int column) : base (line, column) 
        {
            this.expr = expr;
            this.cases = cases;
        }
        public override object Compile(Environment env)
        {
            var gen = Generator.Generator.GetInstance();

            gen.AddComment("Inicia Case");
            string outLabel = gen.NewLabel();

            foreach(CaseElement caseElement in this.cases) 
            {
                caseElement.condition = new Expressions.Equal(expr, caseElement.condition, 0, 0);
                caseElement.outLabel = outLabel;
                caseElement.Compile(env);
            }

            gen.AddLabel(outLabel);
            
            return null;
        }
    }
}
