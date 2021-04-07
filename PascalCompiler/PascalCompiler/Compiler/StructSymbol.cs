using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Compiler
{
    class StructSymbol
    {
        public string id;
        public int size;
        public List<Param> attributes;

        public StructSymbol(string id, int size, List<Param> attributes) 
        {
            this.id = id;
            this.size = size;
            this.attributes = attributes;
        }

        public Param GetAttribute(string id) 
        {
            foreach(Param par in this.attributes) 
            {
                if (par.id.Equals(id)) 
                {
                    return par;
                }
            }

            return null;
        }

        public int GetAttributeIndex(Param attribute) 
        {
            if (this.attributes.Contains(attribute)) 
            {
                return this.attributes.IndexOf(attribute);
            }

            return -1;
        }
    }
}
