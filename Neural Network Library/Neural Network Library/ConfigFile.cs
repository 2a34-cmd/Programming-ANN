using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Atomic.ArtificialNeuralNetwork.libraries
{
    /// <summary>
    /// class about I/O methods and feilds. used for saving networks in and creating networks from files 
    /// </summary>
    public class ConfigFile
    {
        //Info about file
        readonly string Path = "";
        List<string> Data = new();
        /// <summary>
        /// constructing instance as represintive of file to the code
        /// </summary>
        /// <param name="path"></param>
        public ConfigFile(string path, bool create = false)
        {
            
            Path = path;
            if (File.Exists(Path))
            {
                ReadFile();
            }
            else
            {
                if (create) return;
                File.Create(Path);
            }
            calcID = 0; networkID = 0; layerNum = 0; neuronID = 0; from0 = 0; from1 = 0; to0 = 0; to1 = 0;
            intdata = new();
            decimaldata = new();
            network = ""; layer = ""; layername = ""; neuron = ""; state = ""; connoctor = "";
            bias = 0m; wieght = 0m;
            from = null;
        }
        /// <summary>
        /// function that writes array of strings to a file in async way
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        async Task Write(string[] data)
        {
            Data.Clear();
            Data.AddRange(data);
            await File.WriteAllLinesAsync(Path, Data.ToArray());
        }
        /// <summary>
        /// function that reads array of strings from a file in async way
        /// </summary>
        /// <returns></returns>
        async Task Read()
        {
            Data.Clear();
            Data.AddRange(await File.ReadAllLinesAsync(Path));
        }
        /// <summary>
        /// fuction that reads file in async way, but with sync scripting
        /// </summary>
        void ReadFile()
        {
            Task.Run(async () => await Read()).Wait();
        }
        #region dummy
        //these are dummy varibles
        int calcID, networkID, layerNum, neuronID, from0, from1, to0, to1 = 0;
        List<int> intdata = new();
        List<decimal> decimaldata = new();
        string network, layer, layername, neuron, state, connoctor = "";
        decimal bias, wieght = 0m;
        Neuron from, to;
        #endregion
        /// <summary>
        /// transofrmer of Data varible to neural networks and paths in the dictionary
        /// </summary>
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
                    case "p":
                        state = "p";
                        break;
                    default:
                        switch (state)
                        {
                            case "s":
                                switch (Data[i])
                                {
                                    case string a when a.Contains("net"):
                                        network = Data[i];
                                        intdata = Extractint(network);
                                        networkID = intdata[0];
                                        calcID = intdata[1];
                                        if (!NetworkDic.Networks.ContainsKey(networkID))
                                            NetworkDic.Networks.Add(networkID, new NeuralNetwork(networkID, (CalcType)calcID));
                                        break;
                                    case string a when a.Contains('l'):
                                        layer = Data[i];
                                        layerNum = Extractint(layer)[0];
                                        NetworkDic.Networks[networkID].layerList.Add(new Layer(layerNum, networkID));
                                        break;
                                    case string a when a.Contains("nu"):
                                        layername = Layer.Naming(layerNum, networkID);
                                        neuron = Data[i];
                                        decimaldata = Extractdnum(neuron);
                                        neuronID = (int)decimaldata[0];
                                        bias = decimaldata[1];
                                        LayerDic.Layers[layername].neuronList.Add(new Neuron(neuronID, layerNum, networkID, bias));
                                        break;
                                }
                                break;
                            case "c":
                                switch (Data[i])
                                {
                                    case string a when a.Contains('[') && a.Contains(']'):
                                        connoctor = Data[i];
                                        decimaldata = Extractdnum(connoctor);
                                        from0 = (int)decimaldata[0];
                                        from1 = (int)decimaldata[1];
                                        to0 = (int)decimaldata[2];
                                        to1 = (int)decimaldata[3];
                                        wieght = decimaldata[4];
                                        networkID = (int)decimaldata[5];
                                        from = NetworkDic.Networks[networkID].layerList[from1].neuronList[from0];
                                        to = NetworkDic.Networks[networkID].layerList[to1].neuronList[to0];
                                        NetworkDic.Networks[networkID].connect(from, to, wieght);
                                        break;
                                }
                                break;
                            case "p":
                                switch(Data[i])
                                {
                                    case string a when a.Contains('[') && a.Contains(']'):
                                        intdata = Extractint(Data[i]);
                                        List<Connector> connectors = new();
                                        networkID = intdata[^1];
                                        for(int j = 2; j< intdata.Count; j+=2)
                                        {
                                            connectors.Add(connectorDic.Connectors[Connector.naming(NetworkDic.Networks[networkID].layerList[j - 2].neuronList[j - 1].name,
                                                NetworkDic.Networks[networkID].layerList[j].neuronList[j + 1].name, NetworkDic.Networks[networkID])]);
                                        }
                                        _ = new Path(connectors);
                                        break;
                                }
                                break;
                        }
                        break;
                }
            }
            calcID =0; networkID = 0; layerNum = 0; neuronID = 0; from0 = 0; from1 = 0; to0 = 0; to1 = 0;
            intdata = new();
            decimaldata = new();
            network = ""; layer = ""; layername = ""; neuron = ""; state = ""; connoctor = "";
            bias=0m; wieght = 0m;
            from = null; to = null;
        }
        /// <summary>
        /// transformer from neural networks and paths to array of strings and then to the file
        /// </summary>
        /// <param name="parallel"></param>
        public void EditFile(bool parallel = false)
        {
            Task.Run(async ()=> 
            { 
                await Write(NetworkDic.FileInfo().ToArray());
                if (parallel)
                {
                    await Write(PathDic.FileInfoParallel());
                }
                else
                {
                    await Write(PathDic.FileInfo());
                }
            }).Wait();
        }
        /// <summary>
        /// list of integer that can be extracted from string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
                else
                {
                    if (parameter == "") continue;
                    val = int.Parse(parameter);
                    data.Add(val);
                    parameter = string.Empty;
                    val = 0;
                }
            }

            return data;
        }
        /// <summary>
        /// list of decimals that can be extracted from string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<decimal> Extractdnum(string input)
        {
            List<decimal> data = new();
            string parameter = string.Empty;
            decimal val = 0;
            foreach (char IsDigit in input)
            {
                if (char.IsDigit(IsDigit) || IsDigit == '-' || IsDigit == '.')
                {
                    parameter += IsDigit;
                }
                else
                {
                    if (parameter == "") continue;
                    val = decimal.Parse(parameter);
                    data.Add(val);
                    parameter = string.Empty;
                    val = 0;
                }
            }
            return data;
        }
    }
}
