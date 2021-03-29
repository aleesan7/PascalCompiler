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

                //LinkedList<Instruction> constantDefinition = instructions(root.ChildNodes[2].ChildNodes[0]);
                LinkedList<Instruction> variableDeclaration = instructions(root.ChildNodes[2].ChildNodes[1]);
                //LinkedList<Instruction> functionAndProcedureDeclaration = instructions(root.ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[0]);
                LinkedList<Instruction> instructionsList = instructions(root.ChildNodes[2].ChildNodes[4]);
                //execute(constantDefinition, variableDeclaration, functionAndProcedureDeclaration, instructionsList);
                Compile(instructionsList, variableDeclaration);

            }
            else
            {
                //We print the errors in the output 
                syntacticErrorsFound = true;
            }

        }

        private void Compile(LinkedList<Instruction> instructionsList, LinkedList<Instruction> variableDeclaration)
        {
            Compiler.Environment global = new Compiler.Environment(null);

            foreach(var variable in variableDeclaration) 
            {
                variable.Compile(global);
            }

            foreach(var instruction in instructionsList) 
            {
                instruction.Compile(global);
            }

            var gen = Generator.GetInstance();

            gen.ExportC3D();
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
                    //return new Writeln(expressions(actual.ChildNodes[2]));
                    return new Writeln(expression(actual.ChildNodes[2].ChildNodes[0]), false, actual.Span.Location.Line, actual.Span.Location.Column);
                case "var":
                    if (actual.ChildNodes.Count == 5)
                    {
                        //Var declaration without explicit value specified (We assign a default value depending the type)
                        return new Declare(new Compiler.Type(GetVarType(actual.ChildNodes[3].ChildNodes[0].Token.Text), null), newLiteralWithDefaultValue(actual.ChildNodes[3]), idList(actual.ChildNodes[1]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
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

                            return new Declare(new Compiler.Type(GetVarType(actual.ChildNodes[3].ChildNodes[0].Token.Text), null), expression(actual.ChildNodes[5]), singleId, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
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
                default:
                    if(actual.ChildNodes.Count == 5 || actual.ChildNodes.Count == 4) 
                    {
                        return new Assignment(new AccessId(actual.ChildNodes[0].Token.Text, null, actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column), expression(actual.ChildNodes[3]), actual.ChildNodes[0].Span.Location.Line, actual.ChildNodes[0].Span.Location.Column);
                    }
                    return null;
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
                                return new Literal(Types.STRING, (object)actual.ChildNodes[0].Token.Text, actual.Span.Location.Line, actual.Span.Location.Column);
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
                            return null; // TODO new FunctionCallExpression(actual.ChildNodes[0].ChildNodes[0].Token.Text, expressions(actual.ChildNodes[0].ChildNodes[2]));
                        }
                        else
                        {
                            return null; //TODO new FunctionCallExpression(actual.ChildNodes[0].ChildNodes[0].Token.Text);
                        }
                    }
                }
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
