using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
            foreach (string line in File.ReadAllLines(Path))
            {
                Data.Add(line);
            }
        }

        void Write(string[] data)
        {
            Data = null;
            foreach (string line in data)
            {
                Data.Add(line);
            }
            File.WriteAllLines(Path, Data.ToArray());
        }

        void Read()
        {
            Data = null;
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
            for (int i = 0; i < Data.Count-1; i++)
            {
                if (Data[i] == "s") state = "s";
                if (Data[i] == "c") state = "c";
                if (state == "s")
                {
                    if (Data[i].Contains("net"))
                    {
                        network = Data[i];
                        networkID = Extractnumbers(network)[0];
                        calcID = Extractnumbers(network)[1]; 
                        if(NetworkDic.Networks[networkID] == null)
                            NetworkDic.Networks.Add(networkID, new NeuralNetwork(networkID,(CalcType)calcID));
                    }
                    if (Data[i].Contains("l"))
                    {
                        layer = Data[i];
                        layerNum = Extractnumbers(layer)[0];
                        NetworkDic.Networks[networkID].layerList.Add(new Layer(layerNum, networkID));
                    }
                    if (Data[i].Contains("nu"))
                    {
                        layername = Layer.Naming(layerNum, networkID);
                        neuron = Data[i];
                        neuronID = Extractnumbers(neuron)[0];
                        bias = Extractdoubles(neuron)[1];
                        LayerDic.Layers[layername].neuronList.Add(new Neuron(neuronID, layerNum, networkID, bias));
                    }
                }
                if (state == "c")
                {
                    if (Data[i].Contains("[") && Data[i].Contains("]"))
                    {
                        connoctor = Data[i];
                        from0 = Extractnumbers(connoctor)[0];
                        from1 = Extractnumbers(connoctor)[1];
                        to0 = Extractnumbers(connoctor)[2];
                        to1 = Extractnumbers(connoctor)[3];
                        wieght = Extractdoubles(connoctor)[4];
                        from = NetworkDic.Networks[networkID].layerList[from1].neuronList[from0];
                        to = NetworkDic.Networks[networkID].layerList[to1].neuronList[to0];
                        NetworkDic.Networks[networkID].connect(from, to, wieght);
                    }
                }
                calcID = 0; networkID = 0; layerNum = 0; neuronID = 0; from0 = 0; from1 = 0; to0 = 0; to1 = 0;wieght = 0;
                network = ""; layer = ""; layername = ""; neuron = ""; state = ""; connoctor = "";
                from = null; to = null;
            }
        }
        public void EditFile(NeuralNetwork network)
        {
            Write(network.FileInfo().ToArray());
        }
        static List<int> Extractnumbers(string input) 
        {
            List<int> number = new();
            // Split on one or more non-digit characters.
            string[] numbers = Regex.Split(input, @"\D+");
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int i = int.Parse(value);
                    number.Add(i);
                }
            }
            return number;
        }
        static List<double> Extractdoubles(string inputd)
        {
            List<double> number = new();
            // Split on one or more non-digit characters.
            string[] numbers = Regex.Split(inputd, @"\D+");
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    double i = double.Parse(value);
                    number.Add(i);
                }
            }
            return number;
        }
    }
}
