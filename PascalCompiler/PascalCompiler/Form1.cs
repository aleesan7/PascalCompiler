//using compiler_app;
//using CompiPascal.Analysers;
//using CompiPascal.Utils;
//using graphviz_cSharp.external;
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
        //private CompiPascal.Interpreter.Environment globalEnv;

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string inputString = txtInputEditor.Text.ToString();

            //Syntax syntax = new Syntax();
            //syntax.Analyze(inputString);

            //txtOutputEditor.Text = "";

            //if (!syntax.syntacticErrorsFound) 
            //{ 

            //    string text = File.ReadAllText("results.txt");
            //    txtOutputEditor.Text = text;

            //    File.Create("results.txt").Close();

            //    if (syntax.errorsList.Count > 0) 
            //    {
            //        foreach(PascalError error in syntax.errorsList) 
            //        {
            //            txtOutputEditor.Text += error.GetMesage();
            //            txtOutputEditor.Text += Environment.NewLine;
            //        }

            //        GenerateSemanticErrors(syntax.errorsList);
            //    }
            //}
            //else 
            //{
            //    MessageBox.Show("Syntax errors found, please verify the syntax errors report.");
            //}
        } //old execute button

        private void button1_Click(object sender, EventArgs e)
        {
            //string inputString = txtInputEditor.Text.ToString();

            //Syntax syntax = new Syntax();
            //syntax.AnalyzeTranslator(inputString);

            //txtOutputEditor.Text = "";

            //if (!syntax.strResult.Equals("")) 
            //{
            //    txtOutputEditor.Text += syntax.strResult;
            //    txtOutputEditor.Text += Environment.NewLine;
            //}

            //if (syntax.errorsList.Count > 0)
            //{
            //    foreach (CompiPascal.Utils.PascalError error in syntax.errorsList)
            //    {
            //        txtOutputEditor.Text += error.GetMesage();
            //        txtOutputEditor.Text += Environment.NewLine;
            //    }
            //}
        } //old translate button

        private void button3_Click(object sender, EventArgs e)
        {
            
                //try
                //{
                //    //String fileInputPath = "ast.txt";
                //    //String filename = Path.GetFileName(fileInputPath);
                //    //MessageBox.Show(filename);

                //    String graphVizString = @"" + File.ReadAllText("ast.txt");
                //    Bitmap bm = FileDotEngine.Run(graphVizString);

                //    for (int x = 0; x < bm.Width; x++)
                //    {
                //        for (int y = 0; y < bm.Height; y++)
                //        {
                //            Color clr = bm.GetPixel(x, y);
                //            Color newClr = Color.FromArgb(clr.R, 0, 0);
                //        }
                //    }
                //    bm.Save(@"ast.jpg");
                //    MessageBox.Show("AST Image generation completed!");
                //    //ProcessStartInfo startInfo = new ProcessStartInfo("dot.exe");
                //    //String action  = "-Tpng ast.txt -o graph.png";
                //    //startInfo.Arguments = action;
                //    //Process.Start(startInfo);

                //}
                //catch (Exception exception)
                //{
                //    MessageBox.Show("Error al generar archivos\n" + exception.Message);
                //}
            
        }

        //private void GenerateTablaSimbolosGeneral(CompiPascal.Interpreter.Environment env) 
        //{
            //CompiPascal.Interpreter.Environment actual = env;
            //string content = string.Empty;

            ////while (actual != null)
            ////{
            // content += "<html>\n <body> <h2>Compi Pascal Symbols Table</h2> <table style=\"width:100%\" border=\"1\"> <tr><th>Name</th><th>Object Type</th><th>Type</th><th>Environment</th><th>Line</th><th>Column</th></tr> \n";

            //Dictionary<string, CompiPascal.Interpreter.Symbol> variables = actual.GetVariables();
            //Dictionary<string, CompiPascal.Interpreter.Function> functions = actual.GetFunctions();
            //Dictionary<string, CompiPascal.Interpreter.Procedure> procedures = actual.GetProcedures();

            //foreach (KeyValuePair<string, CompiPascal.Interpreter.Symbol> variable in variables)
            //{
            //    content += "<tr>" +
            //        "<td>" + variable.Key +
            //        "</td>" +
            //        "<td>" + "VARIABLE" +
            //        "</td>" +
            //        "<td>" + variable.Value.type.type.ToString() +
            //        "</td>" +
            //        "<td>" + actual.GetEnvName() +
            //        "</td>" +
            //        "<td>" + variable.Value.line.ToString() +
            //        "</td>" +
            //        "<td>" + variable.Value.column.ToString() +
            //        "</td>" +
            //        "</tr>";
            //}

            //foreach (KeyValuePair<string, CompiPascal.Interpreter.Function> function in functions)
            //{
            //    content += "<tr>" +
            //        "<td>" + function.Key +
            //        "</td>" +
            //        "<td>" + "FUNCTION" +
            //        "</td>" +
            //        "<td>" + function.Value.GetFunctionType().ToString() +
            //        "</td>" +
            //        "<td>" + actual.GetEnvName() +
            //        "</td>" +
            //        "<td>" + function.Value.line.ToString() +
            //        "</td>" +
            //        "<td>" + function.Value.column.ToString() +
            //        "</td>" +
            //        "</tr>";
            //}

            //foreach (KeyValuePair<string, CompiPascal.Interpreter.Procedure> procedure in procedures)
            //{
            //    content += "<tr>" +
            //        "<td>" + procedure.Key +
            //        "</td>" +
            //        "<td>" + "PROCEDURE" +
            //        "</td>" +
            //        "<td>" + "VOID" +
            //        "</td>" +
            //        "<td>" + actual.GetEnvName() +
            //        "</td>" +
            //        "<td>" + procedure.Value.line.ToString() +
            //        "</td>" +
            //        "<td>" + procedure.Value.column.ToString() +
            //        "</td>" +
            //        "</tr>";
            //}

            //content += "</table> </body> </html>";

            ////actual = actual.GetParent();
            ////}

            //using (StreamWriter outputFile = new StreamWriter("GlobalSymbolsTable" + "_" + actual.GetEnvName() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".html"))
            //{
            //    outputFile.WriteLine(content);
            //}

        //}

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //switch (e.ClickedItem.Text) 
            //{
            //    case "&Execute":
            //        if (!txtInputEditor.Text.Equals("")) 
            //        {
            //            Execute();
            //        }
            //        else 
            //        {
            //            MessageBox.Show("Please enter the code you want to execute.");
            //        }
                    
            //        break;
            //    case "&Translate":
            //        if (!txtInputEditor.Text.Equals(""))
            //        {
            //            Translate();
            //        }
            //        else 
            //        {
            //            MessageBox.Show("Please enter the code you want to translate.");
            //        }
            //        break;
            //    case "&Semantic Report":
            //        //System.Diagnostics.Process.Start("SemanticErrorsReport.html");
            //        break;
            //    case "&ViewSymbolsTable":
            //        if (this.globalEnv == null) 
            //        {
            //            MessageBox.Show("You must first execute some code in order to generate the symbols table.");
            //        }
            //        else 
            //        {
            //            GenerateTablaSimbolosGeneral(this.globalEnv);
            //        }
            //        break;
            //    case "&ViewAST":
            //        if (this.globalEnv == null)
            //        {
            //            MessageBox.Show("You must first execute some code in order to generate the AST");
            //        }
            //        else 
            //        {
            //            GenerateAST();
            //        }
            //        break;
            //    case "&Exit":
            //        Application.Exit();
            //        break;
            //}
        }

        private void Execute() 
        {
            //string inputString = txtInputEditor.Text.ToString();

            //Syntax syntax = new Syntax();
            //syntax.Analyze(inputString);

            //txtOutputEditor.Text = "";

            //if (!syntax.syntacticErrorsFound)
            //{
            //    this.globalEnv = syntax.globalEnv;
            //    string text = File.ReadAllText("results.txt");
            //    txtOutputEditor.Text = text;

            //    File.Create("results.txt").Close();

            //    if (syntax.errorsList.Count > 0)
            //    {
            //        foreach (PascalError error in syntax.errorsList)
            //        {
            //            txtOutputEditor.Text += error.GetMesage();
            //            txtOutputEditor.Text += Environment.NewLine;
            //        }

            //        GenerateSemanticErrors(syntax.errorsList);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Syntax errors found, please verify the syntax errors report.");
            //}
        }

        private void Translate() 
        {
            //string inputString = txtInputEditor.Text.ToString();

            //Syntax syntax = new Syntax();
            //syntax.AnalyzeTranslator(inputString);

            //txtOutputEditor.Text = "";

            //if (!syntax.strResult.Equals(""))
            //{
            //    txtOutputEditor.Text += syntax.strResult;
            //    txtOutputEditor.Text += Environment.NewLine;
            //}
        }

        //private void GenerateSemanticErrors(List<PascalError> semanticErrors)
        //{
            //string errors = "<html>\n <body> <h2>Compi Pascal Errors</h2> <table style=\"width:100%\" border=\"1\"> <tr> <th>Type</th> <th>Error Description</th> <th>row</th> <th>column</th></tr> \n";

            //foreach (CompiPascal.Utils.PascalError error in semanticErrors)
            //{
            //    errors += "<tr>" +
            //            "<td>" + error.GetType() +
            //            "</td>" +
            //            "<td>" + error.GetMesage() +
            //            "</td>" +
            //            "<td>" + error.GetLine() +
            //            "</td>" +
            //            "<td>" + error.GetColumn() +
            //            "</td>" +
            //            "</tr>";
            //}

            //errors += "</table> </body> </html>";
            //using (StreamWriter outputFile = new StreamWriter("SemanticErrorsReport.html"))
            //{
            //    outputFile.WriteLine(errors);
            //}
        //}

        private void GenerateAST() 
        {
            //try
            //{
            //    //String fileInputPath = "ast.txt";
            //    //String filename = Path.GetFileName(fileInputPath);
            //    //MessageBox.Show(filename);

            //    String graphVizString = @"" + File.ReadAllText("ast.txt");
            //    Bitmap bm = FileDotEngine.Run(graphVizString);

            //    for (int x = 0; x < bm.Width; x++)
            //    {
            //        for (int y = 0; y < bm.Height; y++)
            //        {
            //            Color clr = bm.GetPixel(x, y);
            //            Color newClr = Color.FromArgb(clr.R, 0, 0);
            //        }
            //    }
            //    bm.Save(@"ast.jpg");
            //    MessageBox.Show("AST Image generation completed!");
            //    //ProcessStartInfo startInfo = new ProcessStartInfo("dot.exe");
            //    //String action  = "-Tpng ast.txt -o graph.png";
            //    //startInfo.Arguments = action;
            //    //Process.Start(startInfo);

            //}
            //catch (Exception exception)
            //{
            //    MessageBox.Show("Error al generar archivos\n" + exception.Message);
            //}
        }
    }
}
