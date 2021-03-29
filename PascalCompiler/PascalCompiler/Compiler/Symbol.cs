using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Compiler
{
    class Symbol
    {
        public Type type;
        public string identifier;
        public int position;
        public bool isConst;
        public bool isGlobal;
        public bool isHeap;

        public Symbol(Type type, string identifier, int position, bool isConst, bool isGlobal, bool isHeap)
        {
            this.type = type;
            this.identifier = identifier;
            this.position = position;
            this.isConst = isConst;
            this.isGlobal = isGlobal;
            this.isHeap = isHeap;
        }
    }
}
