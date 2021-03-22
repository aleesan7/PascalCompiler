using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PascalCompiler.Analysers
{
    class Errors
    {
        private ParseTree tree;
        private ParseTreeNode root;
        public Errors(ParseTree tree, ParseTreeNode root)
        {
            this.tree = tree;
            this.root = root;
        }


        public bool HasErrors()
        {
            String errors = "<html>\n <body> <h2>Compi Pascal Errors</h2> <table style=\"width:100%\" border=\"1\"> <tr><th>Error Description</th><th>row</th> <th>column</th></tr> \n";
            if (root == null)
            {
                errors += "<tr>" +
                        "<td>" + "Fatal error, the analyzer couldn´t recover" +
                        "</td>" +
                        "<td>" + "0" +
                        "</td>" +
                        "<td>" + "0" +
                        "</td>" +
                        "</tr>";
            }
            if (tree.ParserMessages.Count > 0 || root == null)
            {
                for (int i = 0; i < tree.ParserMessages.Count; i++)
                {
                    errors += "<tr>" +
                        "<td>" + tree.ParserMessages[i].Message +
                        "</td>" +
                        "<td>" + tree.ParserMessages[i].Location.Line +
                        "</td>" +
                        "<td>" + tree.ParserMessages[i].Location.Column +
                        "</td>" +
                        "</tr>";
                }
                errors += "</table> </body> </html>";
                using (StreamWriter outputFile = new StreamWriter("ErrorsReport.html"))
                {
                    outputFile.WriteLine(errors);
                }
                return true;
            }
            return false;
        }
    }
}
