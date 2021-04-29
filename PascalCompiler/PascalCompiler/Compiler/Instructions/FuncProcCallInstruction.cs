using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Instructions
{
    class FuncProcCallInstruction : Instruction
    {
        public string id;
        public Expression previous;
        public LinkedList<Expression> parameters;

        public FuncProcCallInstruction(string id, Expression previous, LinkedList<Expression> parameters, int line, int column) : base(line, column) 
        {
            this.id = id;
            this.previous = previous;
            this.parameters = parameters == null ? new LinkedList<Expression>() : parameters;
        }
        
        public override object Compile(Environment env)
        {
            if (this.previous == null)
            {
                FunctionSymbol funcSym = env.SearchFunction(this.id);
                if (funcSym == null)
                {
                    //TODO Semantic Error throw new Error(this.line, this.column, 'Semantico',`No se encontro la funcion: ${ this.id }`);
                }

                var parameterValues = new LinkedList<Return>();
                var gen = Generator.Generator.GetInstance();
                int size = gen.SaveTemps(env);
                int index = 0;

                foreach (Expression paramm in this.parameters)
                {
                    parameterValues.AddLast(paramm.Compile(env));
                }

                string temp = gen.NewTemporal();
                gen.FreeTemp(temp);

                if (parameterValues.Count > 0)
                {
                    gen.AddExpression(temp, "p", env.size + 1, "+");


                    foreach (Return ret in parameterValues)
                    {
                        gen.AddSetStack(temp, ret.GetValue());
                        if (index != parameterValues.Count - 1)
                        {
                            gen.AddExpression(temp, temp, "1", "+");
                        }
                        index++;
                    }
                }

                gen.AddNextEnv(env.size);
                gen.AddCall(funcSym.uniqueId);
                gen.AddGetStack(temp, "p");
                gen.AddAntEnv(env.size);

                gen.RecoverTemps(env, size);
                gen.AddTemp(temp);

                if (funcSym.type.type != Types.BOOLEAN)
                    return new Return(temp, true, funcSym.type, null);

                //Return retorno = new Return("", false, funcSym.type, null);
                //this.trueLabel = this.trueLabel == "" ? gen.NewLabel() : this.trueLabel;
                //this.falseLabel = this.falseLabel == "" ? gen.NewLabel() : this.falseLabel;
                //gen.AddIf(temp, "1", "==", this.trueLabel);
                //gen.AddGoto(this.falseLabel);
                //retorno.trueLabel = this.trueLabel;
                //retorno.falseLabel = this.falseLabel;
                //return retorno;
            }


            //TODO throw new Error(this.line, this.column, 'Semantico', 'Funcion no implementada');

            return null;
        }
    }
}
