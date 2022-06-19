using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atomic.ArtificialNeuralNetwork.libraries
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
                ReadFile();
            }
            else
            {
                File.Create(Path);
                Data = null;
            }
            
        }

        async Task Write(string[] data)
        {
            Data.Clear();
            Data.AddRange(data);
            await File.WriteAllLinesAsync(Path, Data.ToArray());
        }

        async Task Read()
        {
            Data.Clear();
            Data.AddRange(await File.ReadAllLinesAsync(Path));
        }
        void ReadFile()
        {
            Task.Run(async () => await Read()).Wait();
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
            ReadFile();
            for (int i = 0; i < Data.Count; i++)
            {
                switch (Data[i])
                {
                    case "s":
                        state = "s";
                        break;
                    case "c":
                        state = "c";
                        break;
                    default:
                        switch (state)
                        {
                            case "s":
                                switch (Data[i])
                                {
                                    case string a when a.Contains("net"):
                                        network = Data[i];
                                        networkID = Extractint(network)[0];
                                        calcID = Extractint(network)[1];
                                        if (!NetworkDic.Networks.ContainsKey(networkID))
                                            NetworkDic.Networks.Add(networkID, new NeuralNetwork(networkID, (CalcType)calcID));
                                        break;
                                    case string a when a.Contains("l"):
                                        layer = Data[i];
                                        layerNum = Extractint(layer)[0];
                                        NetworkDic.Networks[networkID].layerList.Add(new Layer(layerNum, networkID));
                                        break;
                                    case string a when a.Contains("nu"):
                                        layername = Layer.Naming(layerNum, networkID);
                                        neuron = Data[i];
                                        neuronID = Extractint(neuron)[0];
                                        bias = Extractdnum(neuron)[1];
                                        LayerDic.Layers[layername].neuronList.Add(new Neuron(neuronID, layerNum, networkID, bias));
                                        break;
                                }
                                break;
                            case "c":
                                switch (Data[i])
                                {
                                    case string a when a.Contains("[") && a.Contains("]"):
                                        connoctor = Data[i];
                                        from0 = Extractint(connoctor)[0];
                                        from1 = Extractint(connoctor)[1];
                                        to0 = Extractint(connoctor)[2];
                                        to1 = Extractint(connoctor)[3];
                                        wieght = Extractdnum(connoctor)[4];
                                        networkID = (int)Extractdnum(connoctor)[5];
                                        from = NetworkDic.Networks[networkID].layerList[from1].neuronList[from0];
                                        to = NetworkDic.Networks[networkID].layerList[to1].neuronList[to0];
                                        NetworkDic.Networks[networkID].connect(from, to, wieght);
                                        break;
                                }
                                break;
                        }
                        break;

                }
            }
            #region Defultation
            calcID = 0; networkID = 0; layerNum = 0; neuronID = 0; from0 = 0; from1 = 0; to0 = 0; to1 = 0; wieght = 0;
            network = ""; layer = ""; layername = ""; neuron = ""; state = ""; connoctor = "";
            from = null; to = null;
            #endregion
        }
        public void EditFile()
        {
            Task.Run(async ()=> await Write(NetworkDic.FileInfo().ToArray())).Wait();
        }
        static List<int> Extractint(string input)
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
                    if (parameter == "") continue;
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
        
        static List<double> Extractdnum(string input)
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
                    if (parameter == "") continue;
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
