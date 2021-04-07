using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Expressions
{
    class Plus : Expression
    {
        public Expression left;
        public Expression right;

        public Plus(Expression left, Expression right, int line, int column) : base (line, column) 
        {
            this.left = left;
            this.right = right;
        }

        public override Return Compile(Environment env)
        {
            Return left = this.left.Compile(env);
            Return right = this.right.Compile(env);

            var gen = Generator.Generator.GetInstance();

            string temp = gen.NewTemporal();

            switch (left.type.type)
            {
                case Types.INTEGER:
                    switch (right.type.type)
                    {
                        case Types.INTEGER:
                        case Types.REAL:
                            gen.AddExpression(temp, left.GetValue(), right.GetValue(), "+");
                            return new Return(temp, true, right.type.type == Types.REAL ? right.type : left.type, null);
                        //case Types.STRING:
                        //    const tempAux = generator.newTemporal(); generator.freeTemp(tempAux);
                        //    generator.addExpression(tempAux, 'p', enviorement.size + 1, '+');
                        //    generator.addSetStack(tempAux, left.getValue());
                        //    generator.addExpression(tempAux, tempAux, '1', '+');
                        //    generator.addSetStack(tempAux, right.getValue());
                        //    generator.addNextEnv(enviorement.size);
                        //    generator.addCall('native_concat_int_str');
                        //    generator.addGetStack(temp, 'p');
                        //    generator.addAntEnv(enviorement.size);
                        //    return new Retorno(temp, true, new Type(Types.STRING));
                        default:
                            break;
                    }
                    break;
                case Types.REAL:
                    switch (right.type.type)
                    {
                        case Types.INTEGER:
                        case Types.REAL:
                        case Types.CHAR:
                            gen.AddExpression(temp, left.GetValue(), right.GetValue(), "+");
                            return new Return(temp, true, left.type, null);
                        //case Types.STRING:
                        //    const tempAux = generator.newTemporal(); generator.freeTemp(tempAux);
                        //    generator.addExpression(tempAux, 'p', enviorement.size + 1, '+');
                        //    generator.addSetStack(tempAux, left.getValue());
                        //    generator.addExpression(tempAux, tempAux, '1', '+');
                        //    generator.addSetStack(tempAux, right.getValue());
                        //    generator.addNextEnv(enviorement.size);
                        //    generator.addCall('native_concat_dbl_str');
                        //    generator.addGetStack(temp, 'p');
                        //    generator.addAntEnv(enviorement.size);
                        //    return new Retorno(temp, true, new Type(Types.STRING));
                        default:
                            break;
                    }
                    break;
                case Types.CHAR:
                    switch (right.type.type)
                    {
                        case Types.INTEGER:
                        case Types.CHAR:
                        case Types.REAL:
                            gen.AddExpression(temp, left.GetValue(), right.GetValue(), "+");
                            return new Return(temp, true, right.type.type == Types.REAL ? right.type : new Type(Types.INTEGER, null), null);
                        //case Types.STRING:
                        //    const tempAux = generator.newTemporal(); generator.freeTemp(tempAux);
                        //    generator.addExpression(tempAux, 'p', enviorement.size + 1, '+');
                        //    generator.addSetStack(tempAux, left.getValue());
                        //    generator.addExpression(tempAux, tempAux, '1', '+');
                        //    generator.addSetStack(tempAux, right.getValue());
                        //    generator.addNextEnv(enviorement.size);
                        //    generator.addCall('native_concat_chr_str');
                        //    generator.addGetStack(temp, 'p');
                        //    generator.addAntEnv(enviorement.size);
                        //    return new Retorno(temp, true, new Type(Types.STRING));
                        default:
                            break;
                    }
                    break;
                case Types.STRING:
                    string tempAux = gen.NewTemporal();
                    gen.FreeTemp(tempAux);
                    switch (right.type.type)
                    {
                        /*case Types.INTEGER:
                            generator.addExpression(tempAux, 'p', enviorement.size + 1, '+');
                            generator.addSetStack(tempAux, left.getValue());
                            generator.addExpression(tempAux, tempAux, '1', '+');
                            generator.addSetStack(tempAux, right.getValue());
                            generator.addNextEnv(enviorement.size);
                            generator.addCall('native_concat_str_int');
                            generator.addGetStack(temp, 'p');
                            generator.addAntEnv(enviorement.size);
                            return new Retorno(temp, true, new Type(Types.STRING));
                        case Types.CHAR:
                            generator.addExpression(tempAux, 'p', enviorement.size + 1, '+');
                            generator.addSetStack(tempAux, left.getValue());
                            generator.addExpression(tempAux, tempAux, '1', '+');
                            generator.addSetStack(tempAux, right.getValue());
                            generator.addNextEnv(enviorement.size);
                            generator.addCall('native_concat_str_chr');
                            generator.addGetStack(temp, 'p');
                            generator.addAntEnv(enviorement.size);
                            return new Retorno(temp, true, new Type(Types.STRING));
                        case Types.DOUBLE:
                            generator.addExpression(tempAux, 'p', enviorement.size + 1, '+');
                            generator.addSetStack(tempAux, left.getValue());
                            generator.addExpression(tempAux, tempAux, '1', '+');
                            generator.addSetStack(tempAux, right.getValue());
                            generator.addNextEnv(enviorement.size);
                            generator.addCall('native_concat_str_dbl');
                            generator.addGetStack(temp, 'p');
                            generator.addAntEnv(enviorement.size);
                            return new Retorno(temp, true, new Type(Types.STRING));*/
                        case Types.STRING:
                            gen.AddExpression("T1", left.GetValue(), "", "");
                            gen.AddExpression("T2", right.GetValue(), "", "");
                            gen.AddCall("native_concat_str_str");
                            gen.AddExpression(temp, "T3", "", "");
                            //generator.addExpression(tempAux, 'p', enviorement.size + 1, '+');
                            //generator.addSetStack(tempAux, left.getValue());
                            //generator.addExpression(tempAux, tempAux, '1', '+');
                            //generator.addSetStack(tempAux, right.getValue());
                            //generator.addNextEnv(enviorement.size);
                            //generator.addCall('native_concat_str_str');
                            //generator.addGetStack(temp, 'p');
                            //generator.addAntEnv(enviorement.size);
                            return new Return(temp, true, new Type(Types.STRING,null), null);
                        /*case Types.BOOLEAN:
                            const lblTemp = generator.newLabel();
                            generator.addExpression(tempAux, 'p', enviorement.size + 1, '+');
                            generator.addSetStack(tempAux, left.getValue());
                            generator.addExpression(tempAux, tempAux, '1', '+');

                            generator.addLabel(right.trueLabel);
                            generator.addSetStack(tempAux, '1');
                            generator.addGoto(lblTemp);

                            generator.addLabel(right.falseLabel);
                            generator.addSetStack(tempAux, '0');
                            generator.addLabel(lblTemp);

                            generator.addNextEnv(enviorement.size);
                            generator.addCall('native_concat_str_bol');
                            generator.addGetStack(temp, 'p');
                            generator.addAntEnv(enviorement.size);
                            return new Retorno(temp, true, new Type(Types.STRING));*/

                        default:
                            break;
                    }
                    break;
                    //case Types.BOOLEAN:
                    //    switch (right.type.type)
                    //    {
                    //        case Types.STRING:
                    //            const tempAux = generator.newTemporal(); generator.freeTemp(tempAux);
                    //            const lblTemp = generator.newLabel();
                    //            generator.addExpression(tempAux, 'p', enviorement.size + 1, '+');
                    //            generator.addLabel(left.trueLabel);
                    //            generator.addSetStack(tempAux, '1');
                    //            generator.addGoto(lblTemp);
                    //            generator.addLabel(left.falseLabel);
                    //            generator.addSetStack(tempAux, '0');
                    //            generator.addLabel(lblTemp);
                    //            generator.addExpression(tempAux, tempAux, '1', '+');
                    //            generator.addSetStack(tempAux, right.getValue());
                    //            generator.addNextEnv(enviorement.size);
                    //            generator.addCall('native_concat_bol_str');
                    //            generator.addGetStack(temp, 'p');
                    //            generator.addAntEnv(enviorement.size);
                    //            return new Retorno(temp, true, new Type(Types.STRING));
                    //        default:
                    //            break;
                    //    }
            }
            return null;
            // TODO throw new Error(this.line, this.column, 'Semantico', `No se puede sumar ${ left.type.type } + ${ right.type.type}`);
        }
    }
}
