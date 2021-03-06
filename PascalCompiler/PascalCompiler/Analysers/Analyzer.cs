using Irony.Parsing;
using PascalCompiler.Compiler.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using PascalCompiler.Compiler.Instructions;
using PascalCompiler.Compiler.Expressions;
using PascalCompiler.Compiler;
using PascalCompiler.Compiler.Generator;

namespace PascalCompiler.Analysers
{
    class Analyzer
    {
        public bool syntacticErrorsFound = false;
        public bool semanticErrorsFound = false;
        public List<string> code = new List<string>();
        public Compiler.Environment globalEnv = null;
        public List<PascalCompiler.Compiler.PascalError> errorsList = new List<PascalCompiler.Compiler.PascalError>();

        public void Analyze(string input)
        {

            CompilerGrammar grammar = new CompilerGrammar();
            LanguageData language = new LanguageData(grammar);

            foreach (var item in language.Errors)
            {
                Console.WriteLine(item);
            }

            Parser parser = new Parser(language);
            ParseTree tree = parser.Parse(input);

            ParseTreeNode root = tree.Root;

            Errors errors = new Errors(tree, root);

            if (!errors.HasErrors())
            {
                generateGraph(root);

                LinkedList<Instruction> constantDefinition = instructions(root.ChildNodes[2].ChildNodes[0]);
                LinkedList<Instruction> variableDeclaration = instructions(root.ChildNodes[2].ChildNodes[1]);
                LinkedList<Instruction> functionAndProcedureDeclaration = instructions(root.ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[0]);
                LinkedList<Instruction> instructionsList = instructions(root.ChildNodes[2].ChildNodes[4]);
                //execute(constantDefinition, variableDeclaration, functionAndProcedureDeclaration, instructionsList);
                Compile(constantDefinition,instructionsList, functionAndProcedureDeclaration, variableDeclaration);

            }
            else
            {
                //We print the errors in the output 
                syntacticErrorsFound = true;
            }

        }

        private void Compile(LinkedList<Instruction> constants, LinkedList<Instruction> instructionsList, LinkedList<Instruction> functionAndProcedureDeclaration, LinkedList<Instruction> variableDeclaration)
        {
            Compiler.Environment global = new Compiler.Environment(null);
            var gen = Generator.GetInstance();
            List<string> generatedCode = gen.GetCode();
            List<string> generatedFuncCode = new List<string>();

            this.globalEnv = global;

            this.code.Add("#include <stdio.h>");
            this.code.Add("float Heap[100000];");
            this.code.Add("float Stack[100000];");
            this.code.Add("float s;");
            this.code.Add("float h;");
            this.code.Add("float p;");

            foreach (var constant in constants)
            {
                constant.Compile(global);
            }

            foreach (var variable in variableDeclaration) 
            {
                try 
                {
                    variable.Compile(global);
                }
                catch(PascalError err) 
                {
                    WriteError(err.GetMessage());
                    this.errorsList.Add(err);
                    this.semanticErrorsFound = true;
                }
            }

            gen.funcCodeStartIndex = gen.GetCode().Count;

            foreach(var funcOrProc in functionAndProcedureDeclaration) 
            {
                try
                {
                    funcOrProc.Compile(global);
                }
                catch (PascalError err)
                {
                    WriteError(err.GetMessage());
                    this.errorsList.Add(err);
                    this.semanticErrorsFound = true;
                }
            }

            gen.funcCodeEndIndex = gen.GetCode().Count;

            generatedFuncCode.AddRange(gen.GetCode().GetRange(gen.funcCodeStartIndex, gen.funcCodeEndIndex - gen.funcCodeStartIndex));

            gen.GetCode().RemoveRange(gen.funcCodeStartIndex, gen.funcCodeEndIndex - gen.funcCodeStartIndex);

            foreach (var instruction in instructionsList) 
            {
                try 
                {
                    instruction.Compile(global);
                }
                catch (PascalError err)
                {
                    WriteError(err.GetMessage());
                    this.errorsList.Add(err);
                    this.semanticErrorsFound = true;
                }
                
            }

            int tempQty = gen.GetTempAmount();
            string temporals = string.Empty;
            //string labels = string.Empty;

            temporals = "float ";

            for(int i = 0; i < tempQty; i++) 
            {
                temporals = temporals + "T" + i + ", ";
            }

            temporals = temporals.Substring(0, temporals.Length - 2) + ";";

            this.code.Add(temporals);

            SetNativeProcs();

            this.code.AddRange(generatedFuncCode);

            this.code.Add("int main()");
            this.code.Add("{");

            this.code.AddRange(generatedCode);

            this.code.Add("return 0;");
            this.code.Add("}");

            gen.SetCode(this.code);

            gen.ExportC3D();

            gen.ResetGenerator();

            if (HasErrors()) 
            {
                this.semanticErrorsFound = true;
            }
        }

        private bool HasErrors()
        {
            bool errorsFound = false;
            string fileName = "SemanticErrors.txt";
            FileInfo fi = new FileInfo(fileName);

            if (fi.Exists) 
            {
                if (fi.Length > 0) 
                {
                    errorsFound = false;
                }
            }

            return errorsFound;
        }

        private void WriteError(string error)
        {
            string path = "SemanticErrors.txt";
            try
            {
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, error + System.Environment.NewLine);
                }
                else
                {
                    File.AppendAllText(path, error + System.Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void SetNativeProcs()
        {
            try
            {

                if (File.Exists("concatProc.txt"))
                {
                    // Read a text file line by line.  
                    string[] lines = File.ReadAllLines("concatProc.txt");
                    foreach (string line in lines)
                    {
                        this.code.Add(line);
                    }
                }


                if (File.Exists("printStringProc_1line.txt"))
                {
                    // Read a text file line by line.  
                    string[] lines = File.ReadAllLines("printStringProc_1line.txt");
                    foreach (string line in lines)
                    {
                        this.code.Add(line);
                    }
                }

                if (File.Exists("printStringProc.txt"))
                {
                    // Read a text file line by line.  
                    string[] lines = File.ReadAllLines("printStringProc.txt");
                    foreach (string line in lines)
                    {
                        this.code.Add(line);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private LinkedList<Instruction> instructions(ParseTreeNode actual)
        {
            LinkedList<Instruction> instructionsList = new LinkedList<Instruction>();
            foreach(ParseTreeNode node in actual.ChildNodes) 
            {
                instructionsList.AddLast(instruction(node));
            }

            return instructionsList;
        }

        private List<string> idList(ParseTreeNode actual) 
        {
            List<string> identifierList = new List<string>();
            foreach(ParseTreeNode node in actual.ChildNodes) 
            {
                identifierList.Add(node.Token.Text);
            }

            return identifierList;
        }

        private Sentences ListOfSentences(ParseTreeNode actual) 
        {
            LinkedList<Instruction> instructions = new LinkedList<Instruction>();
            foreach(ParseTreeNode node in actual.ChildNodes) 
            {
                instructions.AddLast(instruction(node));
            }

            return new Sentences(instructions, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
        }

        private Instruction instruction(ParseTreeNode actual)
        {
            string operationToken = actual.ChildNodes.ElementAt(0).ToString().Split(' ')[0].ToLower();
            switch (operationToken)
            {
                case "writeln":
                    return new Writeln(expression(actual.ChildNodes[2].ChildNodes[0]), false, actual.Span.Location.Line, actual.Span.Location.Column);
                case "write":
                    return new Write(expression(actual.ChildNodes[2].ChildNodes[0]), false, actual.Span.Location.Line, actual.Span.Location.Column);
                case "var":
                    if (actual.ChildNodes.Count == 5)
                    {
                        //Var declaration without explicit value specified (We assign a default value depending the type)
                        return new Declare(new Compiler.Type(GetVarType(actual.ChildNodes[3].ChildNodes[0].Token.Text), null), newLiteralWithDefaultValue(actual.ChildNodes[3]), false, idList(actual.ChildNodes[1]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                        //return new Declare(actual.ChildNodes[1].ChildNodes[0].Token.Text.ToString(), expression(actual.ChildNodes[3]), new Interpreter.Type(GetVarType(actual.ChildNodes[1].ChildNodes[2].ChildNodes[0].Token.Text), null), false, actual.ChildNodes[1].ChildNodes[0].Span.Location.Line, actual.ChildNodes[1].ChildNodes[0].Span.Location.Column);
                    }
                    else
                    {
                        if (actual.ChildNodes.Count == 3)
                        {
                            if (actual.ChildNodes[1].ChildNodes.Count == 1)
                            {
                                string type = actual.ChildNodes[1].ChildNodes[0].ChildNodes[7].ChildNodes[0].Token.Text.ToString();

                                //TODO ArrayDeclare
                                return null;
                                //return new ArrayDeclare(actual.ChildNodes[1].ChildNodes[0].ChildNodes[0].Token.Text.ToString(), new Interpreter.Type(GetVarType(type), null), ranges(actual.ChildNodes[1].ChildNodes[0].ChildNodes[4]), actual.ChildNodes[1].ChildNodes[0].ChildNodes[0].Span.Location.Line, actual.ChildNodes[1].ChildNodes[0].ChildNodes[0].Span.Location.Column);
                            }
                            else
                            {
                                return null;
                                //return new Declare(actual.ChildNodes[1].ChildNodes[0].Token.Text.ToString(), newLiteralWithDefaultValue(actual.ChildNodes[1].ChildNodes[2]), new Interpreter.Type(GetVarType(actual.ChildNodes[1].ChildNodes[2].ChildNodes[0].Token.Text), null), false, actual.ChildNodes[1].ChildNodes[0].Span.Location.Line, actual.ChildNodes[1].ChildNodes[0].Span.Location.Column);
                            }
                        }
                        else
                        {
                            //Declare of single variable with initial value (Note: Multiple declaration with init is not permited in original pascal)
                            List<string> singleId = new List<string>();
                            singleId.Add(actual.ChildNodes[1].Token.Text);

                            return new Declare(new Compiler.Type(GetVarType(actual.ChildNodes[3].ChildNodes[0].Token.Text), null), expression(actual.ChildNodes[5]), false, singleId, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                        }
                    }
                case "const":
                    if (actual.ChildNodes.Count == 5)
                    {
                        //Var declaration without explicit value specified (We assign a default value depending the type)
                        return new Declare(new Compiler.Type(GetVarType(actual.ChildNodes[3].ChildNodes[0].Token.Text), null), newLiteralWithDefaultValue(actual.ChildNodes[3]), true, idList(actual.ChildNodes[1]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                        //return new Declare(actual.ChildNodes[1].ChildNodes[0].Token.Text.ToString(), expression(actual.ChildNodes[3]), new Interpreter.Type(GetVarType(actual.ChildNodes[1].ChildNodes[2].ChildNodes[0].Token.Text), null), false, actual.ChildNodes[1].ChildNodes[0].Span.Location.Line, actual.ChildNodes[1].ChildNodes[0].Span.Location.Column);
                    }
                    else
                    {
                        if (actual.ChildNodes.Count == 3)
                        {
                            if (actual.ChildNodes[1].ChildNodes.Count == 1)
                            {
                                string type = actual.ChildNodes[1].ChildNodes[0].ChildNodes[7].ChildNodes[0].Token.Text.ToString();

                                //TODO ArrayDeclare
                                return null;
                                //return new ArrayDeclare(actual.ChildNodes[1].ChildNodes[0].ChildNodes[0].Token.Text.ToString(), new Interpreter.Type(GetVarType(type), null), ranges(actual.ChildNodes[1].ChildNodes[0].ChildNodes[4]), actual.ChildNodes[1].ChildNodes[0].ChildNodes[0].Span.Location.Line, actual.ChildNodes[1].ChildNodes[0].ChildNodes[0].Span.Location.Column);
                            }
                            else
                            {
                                return null;
                                //return new Declare(actual.ChildNodes[1].ChildNodes[0].Token.Text.ToString(), newLiteralWithDefaultValue(actual.ChildNodes[1].ChildNodes[2]), new Interpreter.Type(GetVarType(actual.ChildNodes[1].ChildNodes[2].ChildNodes[0].Token.Text), null), false, actual.ChildNodes[1].ChildNodes[0].Span.Location.Line, actual.ChildNodes[1].ChildNodes[0].Span.Location.Column);
                            }
                        }
                        else
                        {
                            //Declare of single variable with initial value (Note: Multiple declaration with init is not permited in original pascal)
                            List<string> singleId = new List<string>();
                            singleId.Add(actual.ChildNodes[1].Token.Text);

                            return new Declare(new Compiler.Type(GetVarType(actual.ChildNodes[3].ChildNodes[0].Token.Text), null), expression(actual.ChildNodes[5]), true, singleId, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                        }
                    }
                case "instruccion_if_sup":
                    if (actual.ChildNodes[0].ChildNodes.Count == 1)
                    {
                        return new If(expression(actual.ChildNodes[0].ChildNodes[0].ChildNodes[2]), ListOfSentences(actual.ChildNodes[0].ChildNodes[0].ChildNodes[6]), null, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                    }
                    else
                    {
                        return new If(expression(actual.ChildNodes[0].ChildNodes[0].ChildNodes[2]), ListOfSentences(actual.ChildNodes[0].ChildNodes[0].ChildNodes[6]), ListOfSentences(actual.ChildNodes[0].ChildNodes[1].ChildNodes[2]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                    }
                case "while":
                    return new While(expression(actual.ChildNodes[1]), ListOfSentences(actual.ChildNodes[4]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "repeat":
                    return new Repeat(expression(actual.ChildNodes[3]), ListOfSentences(actual.ChildNodes[1]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "for":
                    //if (actual.ChildNodes[5].Term.Name.ToString().Equals("downto"))
                    //{
                    //    return new For(new Assign(actual.ChildNodes[1].Token.Text, expression(actual.ChildNodes[4])),
                    //                new LogicOperation(new Literal(Types.IDENTIFIER, actual.ChildNodes[1].Token.Text), expression(actual.ChildNodes[6]) /*(newLiteralWithSetedValue(actual.ChildNodes[6])*/, ">=", actual.ChildNodes[1].Span.Location.Line, actual.ChildNodes[1].Span.Location.Column),
                    //                new Assign(actual.ChildNodes[1].Token.Text, new ArithmeticOperation(new Literal(Types.IDENTIFIER, actual.ChildNodes[1].Token.Text), new Literal(Types.INTEGER, (object)"1"), "-", actual.ChildNodes[1].Span.Location.Line, actual.ChildNodes[1].Span.Location.Column)),
                    //                instructions(actual.ChildNodes[9]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                    //}
                    //else
                    {
                        return new For(new Assignment( new AssignmentId(actual.ChildNodes[1].Token.Text, null, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column), expression(actual.ChildNodes[4]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column),
                                    new Smaller(new AccessId(actual.ChildNodes[1].Token.Text, null, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column), expression(actual.ChildNodes[6]), true, actual.ChildNodes[1].Span.Location.Line, actual.ChildNodes[1].Span.Location.Column),
                                    new Assignment(new AssignmentId(actual.ChildNodes[1].Token.Text, null, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column), new Plus(new AccessId(actual.ChildNodes[1].Token.Text, null, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column), new Literal(Types.INTEGER, (object)"1", actual.ChildNodes[1].Span.Location.Line, actual.ChildNodes[1].Span.Location.Column), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column),
                                    ListOfSentences(actual.ChildNodes[9]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                    }
                case "case_statement":
                    return new Case(expression(actual.ChildNodes[0].ChildNodes[1]), caseelements(actual.ChildNodes[0].ChildNodes[3]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "break":
                    return new Break(actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "continue":
                    return new Continue(actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "exit":
                    return new Exit(expression(actual.ChildNodes[2]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "function":
                    if (actual.ChildNodes.Count == 12)
                    {

                        if (actual.ChildNodes[7].ChildNodes.Count > 0)
                        {
                            //function with local vars and instructions
                            return new Function(actual.ChildNodes[1].Token.Text, null, instructions(actual.ChildNodes[7]), new Compiler.Type(GetVarType(actual.ChildNodes[5].ChildNodes[0].ChildNodes[0].Token.Text), null), ListOfSentences(actual.ChildNodes[9]), true, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                            //return new Function(actual.ChildNodes[1].Token.Text, GetFunctionType(actual.ChildNodes[5].ChildNodes[0].ChildNodes[0].Token.Text.ToString()), instructions(actual.ChildNodes[7]), instructions(actual.ChildNodes[9]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                        }
                        else
                        {
                            //function with only instructions
                            return new Function(actual.ChildNodes[1].Token.Text, null, null, new Compiler.Type(GetVarType(actual.ChildNodes[5].ChildNodes[0].ChildNodes[0].Token.Text), null), ListOfSentences(actual.ChildNodes[9]), true, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                            //return new Function(actual.ChildNodes[1].Token.Text, GetFunctionType(actual.ChildNodes[5].ChildNodes[0].ChildNodes[0].Token.Text.ToString()), instructions(actual.ChildNodes[9]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                        }

                    }
                    else
                    {
                        if (actual.ChildNodes[8].ChildNodes.Count > 0)
                        {
                            if (actual.ChildNodes[8].ChildNodes.Count > 0 && actual.ChildNodes[3].ChildNodes.Count == 0)
                            {
                                //function with local vars and instructions but no parameters
                                //return new Function(actual.ChildNodes[1].Token.Text, GetFunctionType(actual.ChildNodes[6].ChildNodes[0].ChildNodes[0].Token.Text.ToString()), instructions(actual.ChildNodes[8]), instructions(actual.ChildNodes[10]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                                return new Function(actual.ChildNodes[1].Token.Text, null, instructions(actual.ChildNodes[8]), new Compiler.Type(GetVarType(actual.ChildNodes[6].ChildNodes[0].ChildNodes[0].Token.Text), null), ListOfSentences(actual.ChildNodes[10]), true, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                            }
                            else
                            {
                                //function with local vars and parameters
                                //return new Function(actual.ChildNodes[1].Token.Text, GetFunctionType(actual.ChildNodes[6].ChildNodes[0].ChildNodes[0].Token.Text.ToString()), instructions(actual.ChildNodes[8]), declares(actual.ChildNodes[3]), instructions(actual.ChildNodes[10]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                                return new Function(actual.ChildNodes[1].Token.Text, ParameterList(actual.ChildNodes[3]), instructions(actual.ChildNodes[8]), new Compiler.Type(GetVarType(actual.ChildNodes[6].ChildNodes[0].ChildNodes[0].Token.Text), null), ListOfSentences(actual.ChildNodes[10]), true, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                            }

                        }
                        else
                        {
                            //function with parameters and instructions
                            //return new Function(actual.ChildNodes[1].Token.Text, GetFunctionType(actual.ChildNodes[6].ChildNodes[0].ChildNodes[0].Token.Text.ToString()), declares(actual.ChildNodes[3]), instructions(actual.ChildNodes[10]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                            return new Function(actual.ChildNodes[1].Token.Text, ParameterList(actual.ChildNodes[3]), null, new Compiler.Type(GetVarType(actual.ChildNodes[6].ChildNodes[0].ChildNodes[0].Token.Text), null), ListOfSentences(actual.ChildNodes[10]), true, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                        }
                    }
                case "procedure":
                    if (actual.ChildNodes.Count == 10)
                    {
                        //Procedure with only instructions
                        if (actual.ChildNodes[5].ChildNodes.Count == 0)
                        {
                            return new Function(actual.ChildNodes[1].Token.Text, null, null, new Compiler.Type(Types.VOID, null), ListOfSentences(actual.ChildNodes[7]), true, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                        }
                        else
                        {
                            //Procedure with parameters but without local vars and instructions
                            //if(actual.ChildNodes[3].ChildNodes.Count > 0 && actual.ChildNodes[6])
                            return new Function(actual.ChildNodes[1].Token.Text, ParameterList(actual.ChildNodes[3]), instructions(actual.ChildNodes[5]), new Compiler.Type(Types.VOID, null), ListOfSentences(actual.ChildNodes[7]), true, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                        }

                    }
                    else
                    {
                        //Procedure with parameters, local variables and instructions
                        return new Function(actual.ChildNodes[1].Token.Text, ParameterList(actual.ChildNodes[3]), instructions(actual.ChildNodes[6]), new Compiler.Type(Types.VOID, null), ListOfSentences(actual.ChildNodes[8]), true, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                    }
                case "functionorprocedurecall":
                    if (actual.ChildNodes[0].ChildNodes.Count == 4)
                    {
                        return new FuncProcCallInstruction(actual.ChildNodes[0].ChildNodes[0].Token.Text, null, null, actual.ChildNodes[0].ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].ChildNodes[0].Span.Location.Column);
                    }
                    else
                    {
                        //TODO procedure or function call with parameters
                        return new FuncProcCallInstruction(actual.ChildNodes[0].ChildNodes[0].Token.Text, null, expressions(actual.ChildNodes[0].ChildNodes[2]), actual.ChildNodes[0].ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].ChildNodes[0].Span.Location.Column);
                    }
                default:
                    if(actual.ChildNodes.Count == 5 || actual.ChildNodes.Count == 4) 
                    {
                        return new Assignment(new AssignmentId(actual.ChildNodes[0].Token.Text, null, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column), expression(actual.ChildNodes[3]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                        //return new Assignment(new AccessId(actual.ChildNodes[0].Token.Text, null, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column), expression(actual.ChildNodes[3]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                    }
                    return null;
            }
        }

        public LinkedList<Param> ParameterList(ParseTreeNode actual) 
        {
            LinkedList<Param> paramsList = new LinkedList<Param>();
            foreach(ParseTreeNode node in actual.ChildNodes) 
            {
                paramsList.AddLast(declare(node));
            }

            return paramsList;
        }

        private Param declare(ParseTreeNode actual)
        {
            if (actual.ChildNodes.Count == 5)
            {
                return null; //new Declare(actual.ChildNodes[1].Token.Text, newLiteralWithDefaultValue(actual.ChildNodes[3]), new Interpreter.Type(GetVarType(actual.ChildNodes[3].ChildNodes[0].Token.Text), null), false, actual.ChildNodes[1].Span.Location.Line, actual.ChildNodes[1].Span.Location.Column);
            }
            else
            {
                if (actual.ChildNodes.Count == 4 && actual.ChildNodes[0].Token.Text.ToString().ToLower().Equals("var"))
                {
                    return null; //new Declare(actual.ChildNodes[1].Token.Text, newLiteralWithDefaultValue(actual.ChildNodes[3]), new Interpreter.Type(GetVarType(actual.ChildNodes[3].ChildNodes[0].Token.Text), null), false, actual.ChildNodes[1].Span.Location.Line, actual.ChildNodes[1].Span.Location.Column);
                }
                else
                {
                    if (actual.ChildNodes.Count == 4)
                    {
                        return new Param(actual.ChildNodes[0].Token.Text, new Compiler.Type(GetVarType(actual.ChildNodes[2].ChildNodes[0].Token.Text), null));// new Declare(actual.ChildNodes[0].Token.Text, newLiteralWithDefaultValue(actual.ChildNodes[2]), new Interpreter.Type(GetVarType(actual.ChildNodes[2].ChildNodes[0].Token.Text), null), false, actual.ChildNodes[1].Span.Location.Line, actual.ChildNodes[1].Span.Location.Column);
                    }
                    else
                    {

                        return new Param(actual.ChildNodes[0].Token.Text, new Compiler.Type(GetVarType(actual.ChildNodes[2].ChildNodes[0].Token.Text), null)); ;// new Declare(actual.ChildNodes[0].Token.Text, newLiteralWithDefaultValue(actual.ChildNodes[2]), new Interpreter.Type(GetVarType(actual.ChildNodes[2].ChildNodes[0].Token.Text), null), false, actual.ChildNodes[1].Span.Location.Line, actual.ChildNodes[1].Span.Location.Column);

                    }
                }

            }
        }

        public LinkedList<Expression> expressions(ParseTreeNode actual)
        {
            LinkedList<Expression> expressionList = new LinkedList<Expression>();
            foreach (ParseTreeNode node in actual.ChildNodes)
            {
                expressionList.AddLast(expression(node));
            }

            return expressionList;
        }

        public Expression expression(ParseTreeNode actual)
        {
            if (actual.ChildNodes.Count == 3)
            {
                if (actual.ChildNodes[0].Token != null && actual.ChildNodes[2].Token != null)
                {
                    if (!(actual.ChildNodes[0].Token.Text.Equals("(") && actual.ChildNodes[2].Token.Text.Equals(")")))
                    {
                        string op = actual.ChildNodes[1].Token.Text;
                        return ArithmeticOrLogicOperation(op, actual);
                    }
                    else
                    {
                        return expression(actual.ChildNodes[1]);
                    }
                }
                else
                {
                    string op = actual.ChildNodes[1].Token.Text;
                    return ArithmeticOrLogicOperation(op, actual);
                }
            }
            else
            {
                if (actual.ChildNodes.Count == 2)
                {
                    //NOT
                    return ArithmeticOrLogicOperation("NOT", actual);
                }
                else
                {
                    if (!actual.ChildNodes[0].Term.Name.ToString().Equals("functionOrProcedureCall"))
                    {
                        switch (actual.ChildNodes[0].Token.Terminal.Name.ToLower())
                        {
                            case "integer":
                                return new Literal(Types.INTEGER, (object)actual.ChildNodes[0].Token.Text, actual.Span.Location.Line, actual.Span.Location.Column);
                            case "string":
                                return new Compiler.Expressions.String(new Compiler.Type(Types.STRING, null), actual.ChildNodes[0].Token.Text, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column); //new Literal(Types.STRING, (object)actual.ChildNodes[0].Token.Text, actual.Span.Location.Line, actual.Span.Location.Column);
                            case "real":
                                return new Literal(Types.REAL, (object)actual.ChildNodes[0].Token.Text, actual.Span.Location.Line, actual.Span.Location.Column);
                            case "boolean":
                                return new Literal(Types.BOOLEAN, (object)actual.ChildNodes[0].Token.Text, actual.Span.Location.Line, actual.Span.Location.Column);
                            case "identifier":
                                if (actual.ChildNodes.Count == 1)  // Simple var expression
                                {
                                    return new AccessId(actual.ChildNodes[0].Token.Text, null, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column); //TODO new Literal(Types.IDENTIFIER, actual.ChildNodes[0].Token.Text);
                                }
                                else
                                {
                                    //Array var expression
                                    return null; //TODO new ArrayAccess(actual.ChildNodes[0].Token.Text, expressions(actual.ChildNodes[2]));
                                }

                            default:
                                return null; //TODO throw new PascalError(actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column, "the obtained literal doesn´t have a valid type", "Semantic Error");
                        }
                    }
                    else
                    {
                        if (actual.ChildNodes[0].ChildNodes[2].ChildNodes.Count > 0)
                        {

                            return new FuncProcCall(actual.ChildNodes[0].ChildNodes[0].Token.Text, null, expressions(actual.ChildNodes[0].ChildNodes[2]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column); // TODO new FunctionCallExpression(actual.ChildNodes[0].ChildNodes[0].Token.Text, expressions(actual.ChildNodes[0].ChildNodes[2]));
                        }
                        else
                        {
                            return new FuncProcCall(actual.ChildNodes[0].ChildNodes[0].Token.Text, null, null, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column); 
                        }
                    }
                }
            }
        }

        public LinkedList<CaseElement> caseelements(ParseTreeNode actual)
        {
            LinkedList<CaseElement> caseElementsList = new LinkedList<CaseElement>();
            foreach (ParseTreeNode node in actual.ChildNodes)
            {
                caseElementsList.AddLast(caseElement(node));
            }

            return caseElementsList;
        }

        public CaseElement caseElement(ParseTreeNode actual)
        {
            if (actual.ChildNodes.Count > 5)
            {
                return new CaseElement(expression(actual.ChildNodes[0]), ListOfSentences(actual.ChildNodes[3]), false, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
            }
            else
            {
                return new CaseElement(null, ListOfSentences(actual.ChildNodes[2]), true, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
            }

        }

        private Types GetVarType(string type)
        {
            switch (type.ToLower())
            {
                case "integer":
                    return Types.INTEGER;
                case "string":
                    return Types.STRING;
                case "real":
                    return Types.REAL;
                case "boolean":
                    return Types.BOOLEAN;
                default:
                    return Types.ERROR;
            }
        }

        public Literal newLiteralWithDefaultValue(ParseTreeNode actual)
        {
            switch (actual.ChildNodes[0].Token.Terminal.Name.ToLower())
            {
                case "integer":
                    return new Literal(Types.INTEGER, (object)0, actual.Span.Location.Line, actual.Span.Location.Column);
                case "string":
                    return new Literal(Types.STRING, (object)"", actual.Span.Location.Line, actual.Span.Location.Column);
                case "real":
                    return new Literal(Types.REAL, (object)0.0, actual.Span.Location.Line, actual.Span.Location.Column);
                case "boolean":
                    return new Literal(Types.BOOLEAN, (object)false, actual.Span.Location.Line, actual.Span.Location.Column);
                default:
                    return null;
                    //TODO throw new PascalError(actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column, "the obtained literal doesn´t have a valid type", "Semantic Error");
            }
        }

        public Expression ArithmeticOrLogicOperation(string op, ParseTreeNode actual)
        {
            switch (op.ToLower())
            {
                case "+":
                    return new Plus(expression(actual.ChildNodes[0]), expression(actual.ChildNodes[2]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "-":
                    return new Minus(expression(actual.ChildNodes[0]), expression(actual.ChildNodes[2]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "*":
                    return new Mult(expression(actual.ChildNodes[0]), expression(actual.ChildNodes[2]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "/":
                    return new Div(expression(actual.ChildNodes[0]), expression(actual.ChildNodes[2]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                //case "div":
                //    return new ArithmeticOperation(expression(actual.ChildNodes[0]), expression(actual.ChildNodes[2]), "div", actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "%":
                    return new Mod(expression(actual.ChildNodes[0]), expression(actual.ChildNodes[2]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                //case ",":
                //    return new StringOperation(expression(actual.ChildNodes[0]), expression(actual.ChildNodes[2]), ",", actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "=":
                    return new Equal(expression(actual.ChildNodes[0]), expression(actual.ChildNodes[2]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "<>":
                    return new NotEqual(expression(actual.ChildNodes[0]), expression(actual.ChildNodes[2]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case ">":
                    return new Greater(expression(actual.ChildNodes[0]), expression(actual.ChildNodes[2]), false, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case ">=":
                    return new Greater(expression(actual.ChildNodes[0]), expression(actual.ChildNodes[2]), true, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "<":
                    return new Smaller(expression(actual.ChildNodes[0]), expression(actual.ChildNodes[2]), false, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "<=":
                    return new Smaller(expression(actual.ChildNodes[0]), expression(actual.ChildNodes[2]), true, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "and":
                    return new And(expression(actual.ChildNodes[0]), expression(actual.ChildNodes[2]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "or":
                    return new Or(expression(actual.ChildNodes[0]), expression(actual.ChildNodes[2]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                case "not":
                    return new Not(expression(actual.ChildNodes[1]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                default:
                    return null;

            }
        }

        public void generateGraph(ParseTreeNode raiz)
        {
            string graphDot = Grapher.getDot(raiz);
            string path = "ast.txt";
            try
            {
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(graphDot);
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
