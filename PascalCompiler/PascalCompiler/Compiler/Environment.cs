using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Instructions;

namespace PascalCompiler.Compiler
{
    class Environment
    {
        Dictionary<string, Symbol> variables;
        Dictionary<string, FunctionSymbol> functions;
        //Dictionary<string, Struct> structs;
        public Environment previous;
        public int size;
        public string breakVar;
        public string continueVar;
        public string returnVar;
        public string prop;
        public FunctionSymbol actualFunction;

        public Environment(Environment previous) 
        {
            this.variables = new Dictionary<string, Symbol>();
            this.functions = new Dictionary<string, FunctionSymbol>();
            this.previous = previous;
            this.size = previous != null ? previous.size : 0;
            this.breakVar = previous != null ? previous.breakVar : "";
            this.continueVar = previous != null ? previous.continueVar : "";
            this.returnVar = previous != null ? previous.returnVar : "";
            this.prop = "main";
            this.actualFunction = previous != null ? previous.actualFunction : null;
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
                if (env.variables.ContainsKey(id.ToLower())) 
                {
                    Symbol variable = env.variables[id.ToLower()] ;
                    if (variable != null)
                    {
                        return variable;
                    }
                }
                env = env.previous;
            }

            return null;
        }

        public Dictionary<string, Symbol> GetVariables() 
        {
            return this.variables;
        }


        public bool AddFunc(Function func, string uniqueId) 
        {
            if (this.functions.ContainsKey(func.id)) 
            {
                return false;
            }

            this.functions.Add(func.id, new FunctionSymbol(func, uniqueId));

            return true;
        }

        public FunctionSymbol GetFunc(string id) 
        {
            return this.functions[id];
        }

        public Dictionary<string, FunctionSymbol> GetFunctions()
        {
            return this.functions;
        }

        public void SetEnvironmentFunc(string prop, FunctionSymbol actualFunc, string ret) 
        {
            this.size = 1;
            this.prop = prop;
            this.returnVar = ret;
            this.actualFunction = actualFunc;
        }

        public FunctionSymbol SearchFunction(string id) 
        {
            Environment env = this;
            while (env != null) 
            {
                if(env.functions.ContainsKey(id))
                    return env.functions[id];
                env = env.previous;
            }

            return null;
        }
    }
}
