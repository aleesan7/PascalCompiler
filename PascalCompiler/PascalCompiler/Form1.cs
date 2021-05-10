using PascalCompiler.Analysers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PascalCompiler
{
    public partial class Form1 : Form
    {
        private PascalCompiler.Compiler.Environment globalEnv;

        public Form1()
        {
            InitializeComponent();
        }

        private void GenerateTablaSimbolosGeneral(PascalCompiler.Compiler.Environment env)
        {
            PascalCompiler.Compiler.Environment actual = env;
            string content = string.Empty;

            //while (actual != null)
            //{
            content += "<html>\n <body> <h2>Compi Pascal Symbols Table</h2> <table style=\"width:100%\" border=\"1\"> <tr><th>Name</th><th>Object Type</th><th>Type</th><th>Environment</th><th>Position/Size (Procs or Funcs)</th></tr> \n";

            Dictionary<string, PascalCompiler.Compiler.Symbol> variables = actual.GetVariables();
            Dictionary<string, PascalCompiler.Compiler.FunctionSymbol> functions = actual.GetFunctions();

            foreach (KeyValuePair<string, PascalCompiler.Compiler.Symbol> variable in variables)
            {
                content += "<tr>" +
                    "<td>" + variable.Key +
                    "</td>" +
                    "<td>" + ((!variable.Value.isConst) ? "VARIABLE" : "CONSTANT" )+
                    "</td>" +
                    "<td>" + variable.Value.type.type.ToString() +
                    "</td>" +
                    "<td>" + "Global" +
                    "</td>" +
                    "<td>" + variable.Value.position +
                    "</td>" +
                    "</tr>";
            }

            foreach (KeyValuePair<string, PascalCompiler.Compiler.FunctionSymbol> function in functions)
            {
                content += "<tr>" +
                    "<td>" + function.Key +
                    "</td>" +
                    "<td>" + ( (function.Value.type.type.ToString().ToLower().Equals("void")) ? "PROCEDURE" : "FUNCTION") +
                    "</td>" +
                    "<td>" + function.Value.type.type.ToString() +
                    "</td>" +
                    "<td>" + "Global" +
                    "</td>" +
                    "<td>" + ((function.Value.type.type.ToString().ToLower().Equals("void")) ? function.Value.size : function.Value.size+1) +
                    "</td>" +
                    "</tr>";
            }
            content += "</table> </body> </html>";

            //actual = actual.GetParent();
            //}

            using (StreamWriter outputFile = new StreamWriter("GlobalSymbolsTable" + "_Global_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".html"))
            {
                outputFile.WriteLine(content);
            }

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text) 
            {
                case "&Execute":
                    if (!txtInputEditor.Text.Equals(""))
                    {
                        Execute();
                    }
                    else
                    {
                        MessageBox.Show("Please enter the code you want to execute.");
                    }
                    break;
                case "&ViewSymbolsTable":
                    if (this.globalEnv == null)
                    {
                        MessageBox.Show("You must first execute some code in order to generate the symbols table.");
                    }
                    else
                    {
                        GenerateTablaSimbolosGeneral(this.globalEnv);
                    }
                    break;
                case "&Semantic Report":
                    //System.Diagnostics.Process.Start("SemanticErrorsReport.html");
                    break;
                case "&Exit":
                    Application.Exit();
                    break;

            }
        }

        private void GenerateSemanticErrors(List<PascalCompiler.Compiler.PascalError> errorsList)
        {
            try
            {
                string errors = "<html>\n <body> <h2>Pascal Compiler Errors</h2> <table style=\"width:100%\" border=\"1\"> <tr> <th>Type</th> <th>Error Description</th> <th>row</th> <th>column</th></tr> \n";


                // Read a text file line by line.  
                foreach (PascalCompiler.Compiler.PascalError error in errorsList)
                {
                    errors += "<tr>" +
                        "<td>" + error.GetType() +
                        "</td>" +
                        "<td>" + error.GetMessage() +
                        "</td>" +
                        "<td>" + error.GetLine() +
                        "</td>" +
                        "<td>" + error.GetColumn() +
                        "</td>" +
                        "</tr>";
                }


                errors += "</table> </body> </html>";
                FileStream fs = new FileStream("SemanticErrorsReport.html", FileMode.Create);
                using (StreamWriter outputFile = new StreamWriter(fs))
                {
                    outputFile.WriteLine(errors);
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            
        }

        private void GenerateSemanticErrors2(List<PascalCompiler.Compiler.PascalError> errorsList)
        { }

        private void Execute() 
        {
            string inputString = txtInputEditor.Text.ToString();

            Analyzer syntax = new Analyzer();
            syntax.Analyze(inputString);

            txtOutputEditor.Text = "";

            if (!syntax.syntacticErrorsFound)
            {
                this.globalEnv = syntax.globalEnv;
                if (!syntax.semanticErrorsFound) 
                {
                    string text = File.ReadAllText("C3D.txt");
                    txtOutputEditor.Text = text;

                    File.Create("C3D.txt").Close();
                }
                else 
                {
                    GenerateSemanticErrors(syntax.errorsList);
                    string errors = File.ReadAllText("SemanticErrors.txt");
                    txtOutputEditor.Text = errors;

                    File.Create("SemanticErrors.txt").Close();
                    File.Create("C3D.txt").Close();

                }
            }
            else
            {
                MessageBox.Show("Syntax errors found, please verify the syntax errors report.");
            }
        }
    }
}
