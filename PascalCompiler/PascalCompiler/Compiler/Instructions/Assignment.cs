using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Instructions
{
    class Assignment : Abstract.Instruction
    {
        private Expression target;
        private Expression value;

        public Assignment(Expression target, Expression value, int line, int column):base(line, column) 
        {
            this.target = target;
            this.value = value;
        }

        public override object Compile(Environment env)
        {
            Return target = this.target.Compile(env);
            Return value = this.value.Compile(env);

            var gen = Generator.Generator.GetInstance();
            Symbol symbol = target.symbol;

            if(!this.SameType(target.type, value.type)) 
            {
                //TODO Throw semantic error, the types aren´t the same
            }

            if (symbol.isGlobal) 
            {
                //if (target.type.type == Types.BOOLEAN)
                //{
                //    const templabel = generator.newLabel();
                //    generator.addLabel(value.trueLabel);
                //    generator.addSetStack(symbol.position, '1');
                //    generator.addGoto(templabel);
                //    generator.addLabel(value.falseLabel);
                //    generator.addSetStack(symbol.position, '0');
                //    generator.addLabel(templabel);
                //}else{
                gen.AddSetStack(symbol.position, value.GetValue());
                //}
            }
            else if (symbol.isHeap) 
            {
                    //if (target.type.type == Types.BOOLEAN)
                    //{
                    //    const templabel = generator.newLabel();
                    //    generator.addLabel(value.trueLabel);
                    //    generator.addSetHeap(symbol.position, '1');
                    //    generator.addGoto(templabel);
                    //    generator.addLabel(value.falseLabel);
                    //    generator.addSetHeap(symbol.position, '0');
                    //    generator.addLabel(templabel);
                    //}else{
                    gen.AddSetHeap(target.GetValue(), value.GetValue());
                //}
            }
            else 
            {
                //if (target.type.type == Types.BOOLEAN)
                //{
                //    const templabel = generator.newLabel();
                //    generator.addLabel(value.trueLabel);
                //    generator.addSetStack(target.getValue(), '1');
                //    generator.addGoto(templabel);
                //    generator.addLabel(value.falseLabel);
                //    generator.addSetStack(target.getValue(), '0');
                //    generator.addLabel(templabel);
                //}else{
                    gen.AddSetStack(target.GetValue(), value.GetValue());
                //}
            }
            

            return null;
        }
    }
}
