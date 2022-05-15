using System.IO;
using System.Collections.Generic;
using System;
namespace NeuralNetwork
{
    class IO
    {
        public string path;
        string Data;
        public List<double> IorO;
        
        public IO(string path)
        {
            this.path = path;
            IorO = new();
            if (File.Exists(this.path))
            {
                Data = File.ReadAllText(this.path);
            }
            else
            {
                File.Create(this.path);
                Data = String.Empty;
            }
        }
        void read()
        {
            string data = File.ReadAllText(path);
            char[] illegal = { ' ', ',', '\n' };
            List<double> list = new();
            string[] Seperated = data.Split(illegal, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in Seperated)
            {
                list.Add(double.Parse(str));
            }
            IorO = list;
        }
        public void ConverToNN(int networkID)
        {
            read();
            NetworkDic.Networks[networkID].EnterVal(IorO);
        }
        void write()
        {
            Data = String.Empty;
            for (int i =0;i>= IorO.Count; i++)
            {
                Data += $", {IorO[i]}";
                if(i%5 == 0) { Data += "\n"; }
            }
            File.WriteAllText(path, Data);
        }
        public void ConvertFromNN(int networkID)
        {
            IorO = NetworkDic.Networks[networkID].ExtractVal();
            write();
        }
    }
}
