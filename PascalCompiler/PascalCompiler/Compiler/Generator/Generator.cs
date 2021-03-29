using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PascalCompiler.Compiler.Generator
{
    class Generator
    {
        private static Generator generator = null;
        private int temporal;
        private int label;
        private List<string> code;
        private HashSet<string> tempStorage;
        string isFunc = "";

        private Generator() 
        {
            this.temporal = this.label = 0;
            this.code = new List<string>();
            this.tempStorage = new HashSet<string>();
        }

        public static Generator GetInstance() 
        {
            if (generator == null) 
            {
                generator = new Generator();
            }
            return generator;
        }

        public void FreeTemp(string temp) 
        {
            if (this.tempStorage.Contains(temp)) 
            {
                this.tempStorage.Remove(temp);
            }
        }

        public void AddPrintln(string format, object value)
        {
            this.code.Add($"{this.isFunc}printf(\"%{format}\"," +$"{value});");
            //this.code.push(`${ this.isFunc}print("%${format}",${ value});`);
        }

        public void AddComment(string comment)
        {
            this.code.Add($"{ this.isFunc}/*****" + $"{comment} *****/");
        }

        public void AddSetStack(object index, string value) 
        {
            this.code.Add($"{ this.isFunc}Stack[" + $"{ index}] =" + $"{ value};");
        }

        public void AddGetStack(object target, object index) 
        {
            this.code.Add($"{this.isFunc}" + $"{target}" + "= Stack[" + $"{index}];");
        }

        public void AddSetHeap(object index, object value) 
        {
            this.code.Add($"{ this.isFunc}Heap[" + $"{ index}] =" + $"{ value};");
        }

        public void AddGetHeap(object target, object index)
        {
            this.code.Add($"{ this.isFunc}" + $"{ target}" + "= Heap[" + $"{ index}];");
        }

        public void AddExpression(string target, Object left, Object right, string op) 
        {
            this.code.Add($"{ this.isFunc}" + $"{ target} =" + $"{ left}" + $"{op}" + $"{ right};");
        }

        public void AddIf(object left, object right, string op, string label) 
        {
            this.code.Add($"{ this.isFunc}if(" + $"{ left} " + $"{op}" + $"{ right}) goto " + $"{ label};");
        }

        public void AddGoto(string label) 
        {
            this.code.Add($"{this.isFunc}goto " + $"{label};");
        }

        public void AddLabel(string label) 
        {
            this.code.Add($"{this.isFunc}" + $"{ label}:");
        }

        public string NewTemporal()
        {
            string temp = "T" + this.temporal++;
            this.tempStorage.Add(temp);

            return temp;

        }

        public string NewLabel() 
        {
            return "L" + this.label++;
        }

        public void ExportC3D()
        {
            string path = "C3D.txt";
            try
            {
                foreach(string input in this.code) 
                {
                    if (!File.Exists(path))
                    {
                        File.WriteAllText(path, input + System.Environment.NewLine);
                    }
                    else
                    {
                        File.AppendAllText(path, input + System.Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
