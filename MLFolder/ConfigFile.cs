using System.IO;
using System.Collections.Generic;

namespace NeuralNetwork
{
    class ConfigFile
    {
        //Info about file
        public string Path;
        public List<string> Data;
        public ConfigFile(string path)
        {
            Data = new();
            Path = path;
            if (File.Exists(Path))
            {
                foreach (string line in File.ReadAllLines(Path))
                {
                    Data.Add(line);
                }
            }
            else
            {
                File.Create(Path);
                Data = null;
            }
            
        }

        void Write(string[] data)
        {
            Data.Clear();
            foreach (string line in data)
            {
                Data.Add(line);
            }
            File.WriteAllLines(Path, Data.ToArray());
        }

        void Read()
        {
            Data.Clear();
            foreach (string line in File.ReadLines(Path))
            {
                Data.Add(line);
            }
        }
        #region dummy
        //these are dummy varibles
        int calcID, networkID , layerNum, neuronID, from0, from1, to0, to1;
        string network, layer,layername, neuron, state,connoctor;
        double bias, wieght;
        Neuron from, to;
        #endregion
        public void CreateANN()
        {
            Read();
            for (int i = 0; i < Data.Count; i++)
            {
                if (Data[i] == "s") state = "s";
                if (Data[i] == "c") state = "c";
                if (state == "s")
                {
                    if (Data[i].Contains("net"))
                    {
                        network = Data[i];
                        networkID = extractint(network)[0];
                        calcID = extractint(network)[1]; 
                        if(!NetworkDic.Networks.ContainsKey(networkID))
                            NetworkDic.Networks.Add(networkID, new NeuralNetwork(networkID,(CalcType)calcID));
                    }
                    if (Data[i].Contains("l"))
                    {
                        layer = Data[i];
                        layerNum = extractint(layer)[0];
                        NetworkDic.Networks[networkID].layerList.Add(new Layer(layerNum, networkID));
                    }
                    if (Data[i].Contains("nu"))
                    {
                        layername = Layer.Naming(layerNum, networkID);
                        neuron = Data[i];
                        neuronID = extractint(neuron)[0];
                        bias = extractdnum(neuron)[1];
                        LayerDic.Layers[layername].neuronList.Add(new Neuron(neuronID, layerNum, networkID, bias));
                    }
                }
                if (state == "c")
                {
                    if (Data[i].Contains("[") && Data[i].Contains("]"))
                    {
                        connoctor = Data[i];
                        from0 = extractint(connoctor)[0];
                        from1 = extractint(connoctor)[1];
                        to0 = extractint(connoctor)[2];
                        to1 = extractint(connoctor)[3];
                        wieght = extractdnum(connoctor)[4];
                        networkID = (int)extractdnum(connoctor)[5];
                        from = NetworkDic.Networks[networkID].layerList[from1].neuronList[from0];
                        to = NetworkDic.Networks[networkID].layerList[to1].neuronList[to0];
                        NetworkDic.Networks[networkID].connect(from, to, wieght);
                    }
                }
            }
            calcID = 0; networkID = 0; layerNum = 0; neuronID = 0; from0 = 0; from1 = 0; to0 = 0; to1 = 0; wieght = 0;
            network = ""; layer = ""; layername = ""; neuron = ""; state = ""; connoctor = "";
            from = null; to = null;
        }
        public void EditFile()
        {
            Write(NetworkDic.FileInfo().ToArray());
        }
        static List<int> extractint(string input)
        {
            List<int> data = new();
            string parameter = string.Empty;
            int val = 0;
            foreach (char IsDigit in input)
            {
                if (char.IsDigit(IsDigit) || IsDigit == '-')
                {
                    parameter += IsDigit;
                }
                else if (IsDigit == '.')
                {

                }
                else
                {
                    try 
                    {
                        val = int.Parse(parameter);
                        data.Add(val);
                    }
                    catch(System.Exception ex) { System.Console.WriteLine(ex.Message); }
                    parameter = string.Empty;
                    val = 0;
                }
            }

            return data;
        }
        static List<double> extractdnum(string input)
        {
            List<double> data = new();
            string parameter = string.Empty;
            double val = 0;
            foreach (char IsDigit in input)
            {
                if (char.IsDigit(IsDigit) || IsDigit == '-' || IsDigit == '.')
                {
                    parameter += IsDigit;
                }
                else
                {
                    try
                    {
                        val = double.Parse(parameter);
                        data.Add(val);
                    }
                    catch (System.Exception ex) { System.Console.WriteLine(ex.Message); }
                    parameter = string.Empty;
                    val = 0;
                }
            }

            return data;
        }
    }
}
