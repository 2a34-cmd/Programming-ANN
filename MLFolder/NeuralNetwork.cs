using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Atomic.ArtificialNeuralNetwork.libraries
{
    //the enum below is for different kinds of networks and I'll focus majorly in 3 types
    public enum NetworkType
    {
        //most networks will be MLP (multilayer perceptron) and major work will be about it
        MLP =1,
        //RNN (recurrent neural network) has another way to deal with it.
        //In the next update, I will prograb BPTT (back propagation throuth time)
        RNN=2,
        //CNN (convolotional neural network) is really like MLP,
        //but the first hidden layers has activation function Id, so the same proccess as MLP
        CNN=3
    }
    //the enum below is for identifying the major used activation function 
    //For more info, check ActivationFunction.cs 
    public enum CalcType
    {
        Tanh=1,
        Sigmoid=2,
        ReLU=3,
        Id = 4
    }
    //this class is for networks 
    public class NeuralNetwork
    {
        //that's for keeping track of networks
        public int ID;
        public NetworkType type;
        public CalcType calcType;
        /// <summary>
        /// decimal value used in BP to control how much the network approachs the minima
        /// </summary>
        public decimal LearningRate { get; set; }
        //that's for keeping track of layers, connectors, neurons, and paths that are related to the network
        internal List<Layer> layerList = new();
        List<Connector> connectorList = new();
        internal List<Neuron> neuronList = new();
        public ConcurrentBag<Path> PathList = new();
        //and here the constructer
        public NeuralNetwork(int id, CalcType calc, bool AddToDic = false, decimal lr = 1)
        {
            if (NetworkDic.Networks.ContainsKey(id))
            {
                throw new Exception($"can't construct a network with the same id");
            }
            else
            {
                //initializing the fields and properties
                ID = id;
                LearningRate = lr;
                calcType = calc;
                type = NetworkType.MLP;
                if (AddToDic)
                {
                    NetworkDic.Networks.Add(ID, this);
                }
            }
        }
        #region Change
        // addlayer is function to put layer to the network
        void AddLayer(int index)
        {
            if (index >= 0)
            {
                layerList.Add(new Layer(index, ID));
            }
        }
        internal void AddLayer()
        {
                AddLayer(layerList.Count);
        }
        /// <summary>
        /// function that construct connector from one nueron to another
        /// it can also change the network type to RNN id if 'from' neuron is in bigger layer id than 'to'  
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="w"></param>
        internal void connect(Neuron from, Neuron to, decimal w = 0)
        {
            //highly doubtful about this is working, if so then I'll change class connector as layer and neuron
                connectorList.Add(new Connector(from.name, to.name, this, w));
                if (from.LayerNum >= to.LayerNum)
                {

                    type = NetworkType.RNN;
                }
        }
        // The method below is for connecting all layers with each other without any jumbing connectors
        internal void connectMLPBasic(decimal w = 0)
        {
            foreach (Layer layer in layerList) 
            {
                if (layer != layerList[layerList.Count-1])
                {
                    int i = layerList.FindIndex(a => a.ID == layer.ID);
                    Layer NextLayer = layerList[i + 1];
                    foreach (Neuron from in layer.neuronList)
                    {
                        foreach (Neuron to in NextLayer.neuronList)
                        {
                            if (connectorDic.Connectors.ContainsKey($"connector from {from.name} to {to.name} in network{ID}")) continue;
                            connect(from, to, w);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// function that connects every layer without jumbing connectors, but with random wieghts
        /// </summary>
        internal void connectMLPBasicRandom()
        {
            Random rnd = new();
            foreach (Layer layer in layerList)
            {
                if (layer != layerList[^1])
                {
                    int i = layerList.FindIndex(a => a.ID == layer.ID);
                    Layer NextLayer = layerList[i + 1];
                    foreach (Neuron from in layer.neuronList)
                    {
                        foreach (Neuron to in NextLayer.neuronList)
                        {
                            decimal w = Convert.ToDecimal(0.001 * rnd.Next(-20000, 20000));
                            if (connectorDic.Connectors.ContainsKey($"connector from {from.name} to {to.name} in network{ID}")) continue;
                            connect(from, to, w);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// takes input layer values and biases-weights vector to generate values for every neuron in the network
        /// </summary>
        public void Calculate()
        {
            for (int i = 1; i < layerList.Count; i++)
            {
                layerList[i].Calculate();
            }
        }
        /// <summary>
        /// do the same calculate(), but with input array for input layer neurons' values
        /// </summary>
        /// <param name="Input"></param>
        public void Calculate(decimal[] Input)
        {
            EnterVal(Input);
            Finilize();
            Calculate();
        }
        /// <summary>
        /// function that do calculate the cost and farness of network from expected result 
        /// </summary>
        /// <param name="Expected"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public decimal SquereCost(List<decimal> Expected, decimal[] input)
        {
            decimal Diff;
            decimal Sum = 0;
            Calculate(input);
            for (int i = 0; i < layerList[^1].neuronList.Count; i++)
            {
                Diff = Expected[i] - layerList[layerList.Count - 1].neuronList[i].value;
                Sum += DecimalMath.PowerN(Diff, 2);
            }
            return Sum;
        }
        //public decimal SquereCost(List<decimal> Expected, int index)
        //{
        //    if (IsChangable)
        //    {
        //        Console.WriteLine("You need to finilize the network!");
        //        return 0;
        //    }
        //    decimal Diff;
        //    decimal Sum;
        //    Calculate();

        //    Diff = Expected[index] - layerList[layerList.Count - 1].neuronList[index].value;
        //    Sum = DecimalMath.PowerN(Diff, 2);
        //    return Sum;
        //}
        //public decimal SquereCost(decimal expected, int index)
        //{
        //    if (IsChangable)
        //    {
        //        Console.WriteLine("You need to finilize the network!");
        //        return 0;
        //    }
        //    decimal Diff;
        //    decimal Sum;
        //    Calculate();

        //    Diff = expected - layerList[layerList.Count - 1].neuronList[index].value;
        //    Sum = DecimalMath.PowerN(Diff, 2);
        //    return Sum;
        //}
        /// <summary>
        /// change IsChangable to false so mehods calculate and squerecost and findpaths can be used
        /// </summary>
        public void Finilize()
        {
            layerList[0].type = LayerType.Input;
            layerList[^1].type = LayerType.output;
        }
        /// <summary>
        /// change IsChangable to true so methods addlayer and connect and BP can be used
        /// </summary>
        public void Neglect()
        {
            layerList[0].type = LayerType.Hideen;
            layerList[^1].type = LayerType.Hideen;
        }
        /// <summary>
        /// changing values of input layer
        /// </summary>
        /// <param name="list"></param>
        public void EnterVal(decimal[] list)
        {
            foreach (Neuron neuron in layerList[0].neuronList)
            {
                neuron.value = list[neuron.ID];
            }
        }
        /// <summary>
        /// getting values of output layer
        /// </summary>
        /// <returns></returns>
        public List<decimal> ExtractVal()
        {
            List<decimal> list = new();
            List<Neuron> neulist = layerList[^1].neuronList;
                for (int i=0; i <= neulist.Count -1;i++)
                {
                    list.Add(neulist[i].value);
                }
            return list;
        }
        #endregion
        #region Find Paths
        /// <summary>
        /// dummy varible so diagnosing findpaths can be possible
        /// </summary>
        Stopwatch watch = new();
        /// <summary>
        /// dummy so diagnosing findpaths can be possible
        /// </summary>
        public long Serial = 0, Paralleltime = 0; 
        /// <summary>
        /// constructing paths related to the network in serial manner
        /// </summary>
        /// <param name="ForBench">diagnostics varible</param>
        /// <param name="Until"></param>
        public void FindPaths(bool ForBench = false, int Until = 0)
        {
            if(ForBench)
            {
                watch.Start();
            }
            foreach (Neuron neuron in layerList[layerList.Count-1].neuronList)
            {
                List<Connector> y = new();
                foreach (Connector connector in neuron.Root)
                {
                    y = new() { connector };
                    _ = new Path(y);
                }
            }
            for(int i =layerList.Count - 1; i >= Until; i--)
            {
                foreach(Neuron n in layerList[i].neuronList)
                {
                    Path.NewPathsSerial(n.Root, n.paths);
                }
            }
            if (ForBench)
            {
                watch.Stop();
                Serial = watch.ElapsedMilliseconds;
                watch.Reset();
            }
        }
        /// <summary>
        /// constructing paths related to the network in parallel manner
        /// </summary>
        /// <param name="ForBench">diagnostics varible</param>
        /// <param name="Until"></param>
        public void FindPathsParallel( bool ForBench = false, int until = 0)
        {
            if (ForBench)
            {
                watch.Start();
            }
            Parallel.ForEach(layerList[layerList.Count - 1].neuronList, (neuron) =>
            {
                List<Connector> y = new();
                foreach (Connector connector in neuron.Root)
                {
                    y = new() { connector };
                    _ = new Path(y);
                }
            });
            for (int i = layerList.Count - 1; i >= until; i--)
            {
                Parallel.ForEach(layerList[i].neuronList, n =>
                {
                    Path.NewPathsParallel(n.Root, n.paths);
                });
            }
            if (ForBench)
            {
                watch.Stop();
                Paralleltime = watch.ElapsedMilliseconds;
                watch.Reset();
            }
        }
        #endregion
        /// <summary>
        /// function that does back propagation for every neuron in the network serially.
        /// recommended for small networks
        /// </summary>
        /// <param name="Expected"></param>
        /// <param name="Input"></param>
        void BP(decimal[] Expected, decimal[] Input)
        {
            Calculate(Input);
            for (int i = 0; i < neuronList.Count; i++)
            {
                neuronList[i].Backprop(Expected);
            }
        }
        /// <summary>
        /// function that does back propagation for every neuron in the network parallelly.
        /// recommended for big networks
        /// </summary>
        /// <param name="Expected"></param>
        /// <param name="input"></param>
        void BPP(decimal[] Expected, decimal[] input)
        {
            Calculate(input);
            Parallel.For(0, neuronList.Count, i => neuronList[i].Backprop(Expected));
        }
        /// <summary>
        /// function that proccess batches for learning in serial manner.
        /// recommended for small networks
        /// </summary>
        /// <param name="Expected"></param>
        /// <param name="inputs"></param>
        /// <param name="ChangeLR"></param>
        /// <exception cref="Exception">not changable exception</exception>
        public void BatchProccess(decimal[][] Expected, decimal[][] inputs, decimal ChangeLR = 0)
        {
            if (Expected.Length != inputs.Length) throw new("expected and inputs' lenghts are not the same");
            for (int i = 0; i < Expected.Length; i++)
            {
                BP(Expected[i], inputs[i]);
            }
            for (int i = 0; i < neuronList.Count; i++)
            {
                neuronList[i].SetWB(neuronList[i].Diff *
                    LearningRate /
                    Expected.Length);
            }
            for (int i = 0; i < connectorList.Count; i++)
            {
                connectorList[i].SetWB(NeuronDic.Neurons[connectorList[i].To].Diff *
                    LearningRate *
                    NeuronDic.Neurons[connectorList[i].From].value /
                    Expected.Length);
            }
            for (int i = 0; i < neuronList.Count; i++)
            {
                neuronList[i].DefultingDer();
            }
            LearningRate -= ChangeLR;
        }
        /// <summary>
        /// function that proccess batches for learning in parallel manner.
        /// recommended for big networks
        /// </summary>
        /// <param name="Batch"></param>
        /// <param name="inputs"></param>
        /// <param name="ChangeLR"></param>
        /// <exception cref="Exception"></exception>
        public void BatchProccessParallel(decimal[][] Batch, decimal[][] inputs, decimal ChangeLR = 0)
        {
            if (Batch.Length != inputs.Length) throw new Exception("patch and inputs' lenghts are not the same");
            for (int i = 0; i < Batch.Length; i++)
            {
                BPP(Batch[i], inputs[i]);
            }
            Parallel.For(0, neuronList.Count, i =>
            {
                neuronList[i].SetWB(neuronList[i].Diff *
                    LearningRate /
                    Batch.Length);
            });
            Parallel.For(0, connectorList.Count, i =>
            {
                connectorList[i].SetWB(NeuronDic.Neurons[connectorList[i].To].Diff *
                    LearningRate *
                    NeuronDic.Neurons[connectorList[i].From].value /
                    Batch.Length);
            });
            Parallel.For(0, neuronList.Count, i => neuronList[i].DefultingDer());
            LearningRate -= ChangeLR;
        }
        #region Info Methods
        // the functions below are for debugging
        // they also used as a way to know what happened and the latest changes 
        /// <summary>
        /// used as a way to show the structure and all the connections of the network
        /// </summary>
        public void InfoLog()
        {
            System.Console.WriteLine($"neuralnetwork{ID}");
            System.Console.WriteLine("structure:");
            foreach (Layer layer in layerList)
            {
                layer.InfoLog();
            }
            System.Console.WriteLine("connection:");
            foreach (Connector connector in connectorDic.Connectors.Values)
            {
                connector.InfoLog();
            }
        }
        /// <summary>
        /// used as a way to show the values of every neuron in the time of invoking
        /// </summary>
        public void InfowC()
        {

            Console.WriteLine($"neuralnetwork{ID}");
            foreach (Layer layer in layerList)
            {
                layer.InfowC();
            }
            Console.WriteLine();
        }
        /// <summary>
        /// used as a way to show the latest biases and wieghts of all neurons and connectors in the network
        /// </summary>
        public void InfowB()
        {

            Console.WriteLine($"neuralnetwork{ID}:{type}:{calcType}");
            Console.WriteLine("structure:");
            foreach (Layer layer in layerList)
            {
                layer.InfowB();
            }
            Console.WriteLine("connection:");
            foreach (Connector connector in connectorDic.Connectors.Values)
            {
                connector.InfowB();
            }
        }
        /// <summary>
        /// used to save the structure of the network in files
        /// </summary>
        /// <returns></returns>
        internal List<string> StructInfo()
        {
            List<string> data = new() { $"net{ID}:{(int)calcType};" };
            foreach (Layer layer in layerList)
            {
                foreach (string info in layer.FileInfo())
                {
                    data.Add(info);
                }
            }
            return data;
        }
        #endregion
    }
    /// <summary>
    /// class for keeping all the networks and their info and for saving in and opening from files
    /// </summary>
    public class NetworkDic
    {
        /// <summary>
        /// the main component of this class, a dictionary for all networks and their interior methods and feilds
        /// </summary>
       public static Dictionary<int, NeuralNetwork> Networks = new();
        /// <summary>
        /// function that used to save all the networks in "Networks" field in a file
        /// </summary>
        /// <returns></returns>
       internal static List<string> FileInfo()
       {
            List<string> data = new() { "s" };
            foreach (NeuralNetwork network in Networks.Values)
            {
                foreach (string info in network.StructInfo())
                {
                    data.Add(info);
                }
            }
            data.Add("c");
            foreach (Connector info in connectorDic.Connectors.Values)
            {
                data.Add(info.FileInfo());
            }
            return data;
       }
        /// <summary>
        /// fuction that construct random number of networks with random number of layers and neurons and connectors
        /// </summary>
        /// <param name="numberOfFile"></param>
        /// <param name="user"></param>
       public static void RandomNetworks(string path)
       {
            Random rnd = new();
            ConfigFile config = new(path);
            int m = rnd.Next(20);
            for (int n = 0; n < m; n++)
            {
                NeuralNetwork network = new NeuralNetwork(n, CalcType.Sigmoid,true);
                for (int i = 0; i <= rnd.Next(1,10); i++)
                {
                    network.AddLayer();
                    int k = rnd.Next(1, 10);
                    for (int j = 0; j < k; j++)
                    {
                        network.layerList[i].AddNeuron();
                        decimal b = Convert.ToDecimal( 0.001 * rnd.Next(-20000, 20000));
                        network.layerList[i].neuronList[j].bias = b;
                    }
                }
                network.connectMLPBasicRandom();
                int r = rnd.Next(1, 10);
                for(int a =0;a<= r; a++)
                {
                    for (int j = 0; j <= network.layerList.Count-1; j++)
                    {
                        Layer layer = network.layerList[j];
                        int i = rnd.Next(0, layer.neuronList.Count - 1);
                        int k = rnd.Next(j, network.layerList.Count-1);
                        int l = rnd.Next(0, network.layerList[k].neuronList.Count - 1);
                        decimal w = Convert.ToDecimal(0.001 * rnd.Next(-20000, 20000));
                        if (connectorDic.Connectors.ContainsKey(Connector.naming(network.layerList[j].neuronList[i].name,
                            network.layerList[k].neuronList[l].name,
                            network))) continue;
                        network.connect(network.layerList[j].neuronList[i], network.layerList[k].neuronList[l], w);
                    }
                }
                network.InfowB();
            }
            config.EditFile();
        }
        public static void RandomNetworks(string path, int number_of_networks)
        {
            Random rnd = new();
            ConfigFile config = new(path);
            for (int n = 0; n < number_of_networks; n++)
            {
                NeuralNetwork network = new NeuralNetwork(n, CalcType.Sigmoid,true);
                for (int i = 0; i <= rnd.Next(1,10); i++)
                {
                    network.AddLayer();
                    int k = rnd.Next(1, 10);
                    for (int j = 0; j < k; j++)
                    {
                        network.layerList[i].AddNeuron();
                        decimal b = Convert.ToDecimal(0.001 * rnd.Next(-20000, 20000));
                        network.layerList[i].neuronList[j].bias = b;
                    }
                }
                network.connectMLPBasicRandom();
                int r = rnd.Next(1, 10);
                for (int a = 0; a <= r; a++)
                {
                    for (int j = 0; j <= network.layerList.Count-1; j++)
                    {
                        Layer layer = network.layerList[j];
                        int i = rnd.Next(0, layer.neuronList.Count - 1);
                        int k = rnd.Next(j, network.layerList.Count-1);
                        int l = rnd.Next(0, network.layerList[k].neuronList.Count - 1);
                        decimal w = Convert.ToDecimal(0.001 * rnd.Next(-20000, 20000));
                        if (connectorDic.Connectors.ContainsKey(Connector.naming(network.layerList[j].neuronList[i].name,
                            network.layerList[k].neuronList[l].name,
                            network))) continue;
                        network.connect(network.layerList[j].neuronList[i], network.layerList[k].neuronList[l], w);
                    }
                }
                network.InfowB();
            }
            config.EditFile();
        }
        public static void RandomNetworks(int number_of_networks)
        {
            Random rnd = new();
            for (int n = 0; n < number_of_networks; n++)
            {
                NeuralNetwork network = new(n, CalcType.Sigmoid,true);
                for (int i = 0; i <= rnd.Next(1,10); i++)
                {
                    network.AddLayer();
                    int k = rnd.Next(1, 10);
                    for (int j = 0; j < k; j++)
                    {
                        network.layerList[i].AddNeuron();
                        decimal b = Convert.ToDecimal(0.001 * rnd.Next(-20000, 20000));
                        network.layerList[i].neuronList[j].bias = b;
                    }
                }
                network.connectMLPBasicRandom();
                int r = rnd.Next(1, 10);
                for (int a = 0; a <= r; a++)
                {
                    for (int j = 0; j <= network.layerList.Count-1; j++)
                    {
                        Layer layer = network.layerList[j];
                        int i = rnd.Next(0, layer.neuronList.Count - 1);
                        int k = rnd.Next(j, network.layerList.Count-1);
                        int l = rnd.Next(0, network.layerList[k].neuronList.Count - 1);
                        decimal w = Convert.ToDecimal(0.001 * rnd.Next(-20000, 20000));
                        if (connectorDic.Connectors.ContainsKey(Connector.naming(network.layerList[j].neuronList[i].name,
                            network.layerList[k].neuronList[l].name,
                            network))) continue;
                        network.connect(network.layerList[j].neuronList[i], network.layerList[k].neuronList[l], w);
                    }
                }
                network.InfowB();
            }
        }
        public static void RandomNetworks()
        {
            Random rnd = new();
            for (int n = 0; n < rnd.Next(20); n++)
            {
                NeuralNetwork network = new NeuralNetwork(n, CalcType.Sigmoid,true);
                for (int i = 0; i <= rnd.Next(1,10); i++)
                {
                    network.AddLayer();
                    int k = rnd.Next(1, 10);
                    for (int j = 0; j < k; j++)
                    {
                        network.layerList[i].AddNeuron();
                        decimal b = Convert.ToDecimal(0.001 * rnd.Next(-20000, 20000));
                        network.layerList[i].neuronList[j].bias = b;
                    }
                }
                network.connectMLPBasicRandom();
                int r = rnd.Next(1, 10);
                for (int a = 0; a <= r; a++)
                {
                    for (int j = 0; j <= network.layerList.Count-1; j++)
                    {
                        Layer layer = network.layerList[j];
                        int i = rnd.Next(0, layer.neuronList.Count - 1);
                        int k = rnd.Next(j, network.layerList.Count-1);
                        int l = rnd.Next(0, network.layerList[k].neuronList.Count - 1);
                        decimal w = Convert.ToDecimal(0.001 * rnd.Next(-20000, 20000));
                        if (connectorDic.Connectors.ContainsKey(Connector.naming(network.layerList[j].neuronList[i].name,
                            network.layerList[k].neuronList[l].name,
                            network))) continue;
                        network.connect(network.layerList[j].neuronList[i], network.layerList[k].neuronList[l], w);
                    }
                }
                network.InfowB();
            }
        }
    }
}