using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Compiler
{
    class Environment
    {
        Dictionary<string, Symbol> variables;
        //Dictionary<string, Function> functions;
        //Dictionary<string, Procedure> procedures;
        //Dictionary<string, Struct> structs;
        public Environment previous;
        public int size;
        public string breakVar;
        public string continueVar;
        public string returnVar;
        public string prop;
        //Function actualFunction;

        public Environment(Environment previous) 
        {
            this.variables = new Dictionary<string, Symbol>();
            this.previous = previous;
            this.size = previous != null ? previous.size : 0;
            this.breakVar = previous != null ? previous.breakVar : "";
            this.continueVar = previous != null ? previous.continueVar : "";
            this.returnVar = previous != null ? previous.returnVar : "";
            this.prop = "main";
            // this.actualFunc = anterior?.actualFunc || null;
        }

        public Symbol AddVar(string id, Type type, bool isConst, bool isRef) 
        {
            id = id.ToLower();
            if (this.variables.ContainsKey(id)) 
            {
                return null;
            }

            Symbol newVar = new Symbol(type, id, this.size++, isConst, this.previous == null ? true : false, isRef);
            this.variables.Add(id, newVar);

            return newVar;
        }

        public Symbol GetVar(string id) 
        {
            Environment env = this;

            while (env != null) 
            {
                if (env.variables.ContainsKey(id)) 
                {
                    Symbol variable = env.variables[id];
                    if (variable != null)
                    {
                        return variable;
                    }
                }
                env = env.previous;
            }

            return null;
        }

    }
}
