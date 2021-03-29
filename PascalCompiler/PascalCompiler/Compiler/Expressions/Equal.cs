using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Expressions
{
    class Equal : Expression
    {
        public Expression left;
        public Expression right;

        public Equal(Expression left, Expression right, int line, int column) : base(line, column) 
        {
            this.left = left;
            this.right = right;
        }
        public override Return Compile(Environment env)
        {
            Return left = this.left.Compile(env);
            Return right;

            var gen = Generator.Generator.GetInstance();

            switch (left.type.type) 
            {
                case Types.INTEGER:
                case Types.REAL:
                case Types.CHAR:
                    right = this.right.Compile(env);
                    switch (right.type.type)
                    {
                        case Types.INTEGER:
                        case Types.CHAR:
                        case Types.REAL:
                            this.trueLabel = this.trueLabel == "" ? gen.NewLabel() : this.trueLabel;
                            this.falseLabel = this.falseLabel == "" ? gen.NewLabel() : this.falseLabel;
                            gen.AddIf(left.GetValue(), right.GetValue(), "==", this.trueLabel);
                            gen.AddGoto(this.falseLabel);
                            Return ret = new Return("", false, new Type(Types.BOOLEAN, null), null);
                            ret.trueLabel = this.trueLabel;
                            ret.falseLabel = this.falseLabel;
                            return ret;
                        default:
                            return null;
                    }
                /*case Types.BOOLEAN:
                    const trueLabel = generator.newLabel();
                    const falseLabel = generator.newLabel();

                    generator.addLabel(left.trueLabel);
                    this.right.trueLabel = trueLabel;
                    this.right.falseLabel = falseLabel;
                    right = this.right.compile(enviorement);

                    generator.addLabel(left.falseLabel);
                    this.right.trueLabel = falseLabel;
                    this.right.falseLabel = trueLabel;
                    right = this.right.compile(enviorement);
                    if (right.type.type = Types.BOOLEAN)
                    {
                        const retorno = new Retorno('', false, left.type);
                        retorno.trueLabel = trueLabel;
                        retorno.falseLabel = falseLabel;
                        return retorno;
                    }
                    break;
                case Types.STRING:
                    right = this.right.compile(enviorement);
                    switch (right.type.type)
                    {
                        case Types.STRING:
                            {
                                const temp = generator.newTemporal();
                                const tempAux = generator.newTemporal(); generator.freeTemp(tempAux);
                                generator.addExpression(tempAux, 'p', enviorement.size + 1, '+');
                                generator.addSetStack(tempAux, left.getValue());
                                generator.addExpression(tempAux, tempAux, '1', '+');
                                generator.addSetStack(tempAux, right.getValue());
                                generator.addNextEnv(enviorement.size);
                                generator.addCall('native_compare_str_str');
                                generator.addGetStack(temp, 'p');
                                generator.addAntEnv(enviorement.size);

                                this.trueLabel = this.trueLabel == '' ? generator.newLabel() : this.trueLabel;
                                this.falseLabel = this.falseLabel == '' ? generator.newLabel() : this.falseLabel;
                                generator.addIf(temp, '1', '==', this.trueLabel);
                                generator.addGoto(this.falseLabel);
                                const retorno = new Retorno('', false, new Type(Types.BOOLEAN));
                                retorno.trueLabel = this.trueLabel;
                                retorno.falseLabel = this.falseLabel;
                                return retorno;
                            }
                        case Types.NULL:
                            {
                                this.trueLabel = this.trueLabel == '' ? generator.newLabel() : this.trueLabel;
                                this.falseLabel = this.falseLabel == '' ? generator.newLabel() : this.falseLabel;
                                generator.addIf(left.getValue(), right.getValue(), '==', this.trueLabel);
                                generator.addGoto(this.falseLabel);
                                const retorno = new Retorno('', false, new Type(Types.BOOLEAN));
                                retorno.trueLabel = this.trueLabel;
                                retorno.falseLabel = this.falseLabel;
                                return retorno;
                            }
                        default:
                            break;
                    }
                case Types.NULL:
                    right = this.right.compile(enviorement);
                    switch (right.type.type)
                    {
                        case Types.STRING:
                        case Types.ARRAY:
                        case Types.STRUCT:
                        case Types.NULL:
                            this.trueLabel = this.trueLabel == '' ? generator.newLabel() : this.trueLabel;
                            this.falseLabel = this.falseLabel == '' ? generator.newLabel() : this.falseLabel;
                            generator.addIf(left.getValue(), right.getValue(), '==', this.trueLabel);
                            generator.addGoto(this.falseLabel);
                            const retorno = new Retorno('', false, new Type(Types.BOOLEAN));
                            retorno.trueLabel = this.trueLabel;
                            retorno.falseLabel = this.falseLabel;
                            return retorno;
                        default:
                            break;
                    }
                case Types.STRUCT:
                    right = this.right.compile(enviorement);
                    switch (right.type.type)
                    {
                        case Types.STRUCT:
                        case Types.NULL:
                            this.trueLabel = this.trueLabel == '' ? generator.newLabel() : this.trueLabel;
                            this.falseLabel = this.falseLabel == '' ? generator.newLabel() : this.falseLabel;
                            generator.addIf(left.getValue(), right.getValue(), '==', this.trueLabel);
                            generator.addGoto(this.falseLabel);
                            const retorno = new Retorno('', false, new Type(Types.BOOLEAN));
                            retorno.trueLabel = this.trueLabel;
                            retorno.falseLabel = this.falseLabel;
                            return retorno;
                        default:
                            break;
                    }
                case Types.ARRAY:
                    right = this.right.compile(enviorement);
                    switch (right.type.type)
                    {
                        case Types.ARRAY:
                        case Types.NULL:
                            this.trueLabel = this.trueLabel == '' ? generator.newLabel() : this.trueLabel;
                            this.falseLabel = this.falseLabel == '' ? generator.newLabel() : this.falseLabel;
                            generator.addIf(left.getValue(), right.getValue(), '==', this.trueLabel);
                            generator.addGoto(this.falseLabel);
                            const retorno = new Retorno('', false, new Type(Types.BOOLEAN));
                            retorno.trueLabel = this.trueLabel;
                            retorno.falseLabel = this.falseLabel;
                            return retorno;
                        default:
                            break;
                    }*/
                default:
                    return null;
            }

            // TODO throw new Error(this.line, this.column, 'Semantico', `No se puede ${left.type.type} == ${right?.type.type}`);
        }
    }
}
