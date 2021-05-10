using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Compiler
{
    
    class PascalError : Exception
    {
        private int line, column;
        private string message;
        private string type;
        private string environment;

        public PascalError(int line, int column, string message, string type)
        {
            this.line = line;
            this.column = column;
            this.message = message;
            this.type = type;
        }

        public string GetMessage()
        {
            return this.message;
        }

        public int GetLine()
        {
            return this.line;
        }

        public int GetColumn()
        {
            return this.column;
        }

        public string GetType()
        {
            return this.type;
        }

        public override string ToString()
        {
            return "An error was found: " + this.type + " - line: " + this.line + " - column: " + this.column + " - message: " + this.message;
        }
    }
    
}
