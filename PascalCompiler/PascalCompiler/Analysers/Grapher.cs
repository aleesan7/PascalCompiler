using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Analysers
{
    class Grapher
    {
        private static int counter;
        private static string graph;

        public static string getDot(ParseTreeNode root)
        {
            graph = "digraph G{";
            graph += "node0[label=\"" + escape(root.ToString()) + "\"];\n";
            counter = 1;
            ASTTraversal("node0", root);
            graph += "}";
            return graph;
        }

        private static void ASTTraversal(string parent, ParseTreeNode root)
        {
            foreach (ParseTreeNode child in root.ChildNodes)
            {
                String childName = "node" + counter.ToString();
                graph += childName + "[label=\"" + escape(child.ToString()) + "\"];\n";
                graph += parent + "->" + childName + ";\n";
                counter++;
                ASTTraversal(childName, child);
            }
        }

        private static string escape(string graphString)
        {
            graphString = graphString.Replace("\\", "\\\\");
            graphString = graphString.Replace("\"", "\\\"");
            return graphString;
        }
    }
}
