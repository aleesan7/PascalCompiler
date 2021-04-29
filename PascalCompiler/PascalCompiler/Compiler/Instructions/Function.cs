using System;
using System.Collections.Generic;
using System.Text;
using PascalCompiler.Compiler.Abstract;

namespace PascalCompiler.Compiler.Instructions
{
    class Function : Instruction
    {
        public string id;
        public LinkedList<Param> parameters;
        LinkedList<Instruction> variableDeclaration;
        public Type type;
        public Sentences bodyStatements;
        public bool preCompile;

        public Function(string id, LinkedList<Param> parameters, LinkedList<Instruction> variableDeclaration, Type type, Sentences bodyStatements, bool preCompile, int line, int column) : base(line, column)
        {
            this.id = id;
            this.parameters = parameters == null ? new LinkedList<Param>() : parameters;
            this.variableDeclaration = variableDeclaration == null ? new LinkedList<Instruction>() : variableDeclaration;
            this.type = type;
            this.bodyStatements = bodyStatements;
            this.preCompile = preCompile;
        }

        public override object Compile(Environment env)
        {
            if (this.preCompile) 
            {
                this.preCompile = false;
                //this.validateParams(enviorement);
                //this.validateType(enviorement);
                string uniqueId = this.UniqueId(env);
                if (!env.AddFunc(this, uniqueId)) 
                {
                    //    throw new Error(this.line, this.column, 'Semantico',`Ya existe una funcion con el id: ${ this.id }`);
                }

                //return;
            }

            FunctionSymbol funcSymb = env.GetFunc(this.id);
            if(funcSymb != null) 
            {
                var gen = Generator.Generator.GetInstance();
                Environment newEnv = new Environment(env);
                string returnLbl = gen.NewLabel();
                var tempStorage = gen.GetTempStorage();

                newEnv.SetEnvironmentFunc(this.id, funcSymb, returnLbl);

                if(this.parameters != null) 
                {
                    foreach (Param par in this.parameters)
                    {
                        newEnv.AddVar(par.id, par.type, false, false);
                    }
                }

                gen.ClearTempStorage();
                gen.isFunc = "\t";
                gen.AddBegin(funcSymb.uniqueId);

                if (this.variableDeclaration != null) 
                {
                    foreach (var variable in this.variableDeclaration)
                    {
                        variable.Compile(newEnv);
                    }
                }

                //gen.ClearTempStorage();
                //gen.isFunc = "\t";
                //gen.AddBegin(funcSymb.uniqueId);
                this.bodyStatements.Compile(newEnv);
                gen.AddLabel(returnLbl);
                gen.AddPrintfNewLine("s", "");
                gen.AddEnd();
                gen.isFunc = "";
                gen.SetTempStorage(tempStorage);
            }

            return null;
        }

        public string UniqueId(Environment env) 
        {
            string id = $"{ env.prop}_" + $"{ this.id}";

            if(this.parameters == null) 
            {
                return id + "_empty";
            }

            if(this.parameters != null) 
            {
                foreach (Param par in this.parameters)
                {
                    id += "_" + $"{ par.GetUniqueType()}";
                }
            }
            
            return id;
        }
    }
}
