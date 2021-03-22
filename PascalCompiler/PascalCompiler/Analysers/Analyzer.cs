using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
                //LinkedList<Instruction> variableDeclaration = instructions(root.ChildNodes[2].ChildNodes[1]);
                //LinkedList<Instruction> functionAndProcedureDeclaration = instructions(root.ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[0]);
                //LinkedList<Instruction> instructionsList = instructions(root.ChildNodes[2].ChildNodes[4]);
                //execute(constantDefinition, variableDeclaration, functionAndProcedureDeclaration, instructionsList);


            }
            else
            {
                //We print the errors in the output 
                syntacticErrorsFound = true;
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
