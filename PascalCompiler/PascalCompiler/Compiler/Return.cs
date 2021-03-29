using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Compiler
{
    class Return
    {
        private string value;
        public bool isTemp;
        public Type type;
        public string trueLabel;
        public string falseLabel;
        public Symbol symbol;

        public Return(string value, bool isTemp, Type type, Symbol symbol)
        {
            this.value = value;
            this.isTemp = isTemp;
            this.type = type;
            this.trueLabel = this.falseLabel = "";
            this.symbol = symbol;
        }

        public string GetValue() 
        {
            Generator.Generator.GetInstance().FreeTemp(this.value);
            return this.value;
        }
    }
}
