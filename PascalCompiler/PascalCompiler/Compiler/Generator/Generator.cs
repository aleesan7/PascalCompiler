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
            this.temporal = this.label = 10;
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

        public void ResetGenerator() 
        {
            this.temporal = this.label = 10;
            this.code.Clear();
            this.tempStorage.Clear();
            this.isFunc = "";
        }

        public void FreeTemp(string temp) 
        {
            if (this.tempStorage.Contains(temp)) 
            {
                this.tempStorage.Remove(temp);
            }
        }

        public void AddPrintf(string format, object value)
        {
            this.code.Add($"{this.isFunc}printf(\"%{format}\"," +$"{value});");
        }

        public void AddPrintfNewLine(string format, string value) 
        {
            this.code.Add($"{this.isFunc}printf(\"%{format}\"," + $"\"{value}\");");
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
            this.code.Add($"{ this.isFunc}Heap[" + $"(int){ index}] =" + $"{ value};");
        }

        public void AddGetHeap(object target, object index)
        {
            this.code.Add($"{ this.isFunc}" + $"{ target}" + "= Heap[" + $"(int){ index}];");
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

        public void AddCall(string id)
        {
            this.code.Add($"{this.isFunc}" + $"{ id}();");
        }

        public void AddPrint(string format, object value) 
        {
            this.code.Add($"{this.isFunc}printf(\"%" + $"{format}\"" + "," + $"{value});");
        }

        public void AddCode(string code) 
        {
            this.code.Add(code);
        }

        public void AddPrintTrue() 
        {
            this.AddPrint("c", (int)'t');
            this.AddPrint("c", (int)'r');
            this.AddPrint("c", (int)'u');
            this.AddPrint("c", (int)'e');
        }

        public void AddPrintFalse()
        {
            this.AddPrint("c", (int)'f');
            this.AddPrint("c", (int)'a');
            this.AddPrint("c", (int)'l');
            this.AddPrint("c", (int)'s');
            this.AddPrint("c", (int)'e');
        }

        public void AddNextEnv(int size)
        {
            this.code.Add($"{ this.isFunc}p = p + " + $"{size};");
        }

        public void addAntEnv(int size)
        {
            this.code.Add($"{ this.isFunc}p = p - " + $"{ size};");
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

        public void NextHeap() 
        {
            this.code.Add(this.isFunc + "h = h + 1;");
        }

        public List<string> GetCode() 
        {
            return this.code;
        }

        public void SetCode(List<string> code) 
        {
            this.code = code;
        }

        public int GetTempAmount() 
        {
            return this.temporal;
        }

        public int GetLabelAmount() 
        {
            return this.label;
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
