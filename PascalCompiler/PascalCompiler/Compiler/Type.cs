using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Compiler
{
    public enum Types
    {
        INTEGER = 0,
        REAL = 1,
        STRING = 2,
        BOOLEAN = 3,
        IDENTIFIER = 4,
        ERROR = 5,
        STRUCT = 6,
        NULL = 7,
        CHAR = 8,
        VOID = 9
    }
    class Type
    {
        public Types type;
        public string auxType;
        public string typeId;
        public StructSymbol _struct;

        public Type(Types type, string auxType)
        {
            this.type = type;
            this.auxType = auxType;
            this.typeId = "";
            this._struct = null;
        }

        public Type(Types type, string auxType, string typeId, StructSymbol _struct) 
        {
            this.type = type;
            this.auxType = auxType;
            this.typeId = typeId;
            this._struct = _struct;
        }
    }
}
