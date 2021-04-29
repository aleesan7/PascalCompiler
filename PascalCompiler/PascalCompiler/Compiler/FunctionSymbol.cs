using PascalCompiler.Compiler.Instructions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Compiler
{
    class FunctionSymbol
    {
        public Type type;
        public string id;
        public string uniqueId;
        public int size;
        public LinkedList<Param> parameters;

        public FunctionSymbol(Function func, string uniqueId)
        {
            this.type = func.type;
            this.id = func.id;
            this.uniqueId = uniqueId;
            this.size = func.parameters.Count;
            this.parameters = func.parameters;
        }
    }
}
