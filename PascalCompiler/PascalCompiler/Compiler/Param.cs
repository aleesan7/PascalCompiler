using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Compiler
{
    class Param
    {
        public string id;
        public Type type;

        public Param(string id, Type type) 
        {
            this.id = id;
            this.type = type;
        }

        public string GetUniqueType() 
        {
            if(this.type.type == Types.STRUCT) 
            {
                return this.type.typeId;
            }

            return this.type.type.ToString();
        }

        public string ToString() 
        {
            return "{ id: " + $"{ this.id}, type: " + $"{ this.type}" +  "}";
        }
    }
}
