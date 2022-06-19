using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace Atomic.ArtificialNeuralNetwork.libraries
{
    //the enum below is for different kinds of networks and I'll focus majorly in 3 types
    enum NetworkType
    {
        MLP =1,
        RNN=2,
        CNN=3
    }
    enum CalcType
    {
        Atanh=1,
        Sigmoid=2,
        ReLU=3
    }
    //this class is for networks 
    class NeuralNetwork{
        //that's for keeping track of networks
        public int ID;
        public NetworkType type;
        public CalcType calcType;
        public bool IsChangable;
        //that's for keeping track of layers
        public List<Layer> layerList = new();
        List<Connector> connectorList = new();
        public ConcurrentBag<Path> PathList = new();
        //and here the constructer
        public NeuralNetwork(int id, CalcType calc, bool AddToDic = false)
        {
            if (NetworkDic.Networks.ContainsKey(id))
            {
                System.Console.WriteLine($"you can't construct the constructed!!");
            }
            else
            {
                ID = id;
                calcType = calc;
                Console.WriteLine($"a new network is constructed with ID {ID}");
                type = NetworkType.MLP;
                IsChangable = true;
                if (AddToDic)
                {
                    NetworkDic.Networks.Add(ID, this);
                }
            }
        }
        #region Change
        // addneuron is function to put more neurons in a layer
        void AddLayer(int index)
        {
            if (index >= 0 && IsChangable)
            {
                layerList.Add(new Layer(index, ID));
            }
        }
        public void AddLayer()
        {
                AddLayer(layerList.Count);
                Console.WriteLine($"a new layer with ID{layerList.Count - 1} is added to the network {ID}.");
        }
        public void connect(Neuron from, Neuron to, double w = 0)
        {
            //highly doubtful about this is working, if so then I'll change class connector as layer and neuron
            if (IsChangable)
            {
                _ = new Connector(from.name, to.name, this, w);
                if (from.LayerNum >= to.LayerNum)
                {

                    type = NetworkType.RNN;
                }
            }
        }
        // The method below is for connecting all layers with each other without any jumbing connectors
        public void connectMLPBasic(double w = 0)
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
        public void connectMLPBasicRandom()
        {
            Random rnd = new();
            foreach (Layer layer in layerList)
            {
                if (layer != layerList[layerList.Count - 1])
                {
                    int i = layerList.FindIndex(a => a.ID == layer.ID);
                    Layer NextLayer = layerList[i + 1];
                    foreach (Neuron from in layer.neuronList)
                    {
                        foreach (Neuron to in NextLayer.neuronList)
                        {
                            double w = 0.001 * rnd.Next(-20000, 20000);
                            if (connectorDic.Connectors.ContainsKey($"connector from {from.name} to {to.name} in network{ID}")) continue;
                            connect(from, to, w);
                        }
                    }
                }
            }
        }

        void DeleteLayer(int index){
            if(layerList[index] != null && IsChangable){
                for(int i =0;i>layerList[index].neuronList.Count;i++)
                {
                    layerList[index].DeleteNeuron(i);
                }
                layerList.Remove(layerList[index]);
                LayerDic.Layers.Remove(Layer.Naming(index,ID));
            }else{
                System.Console.WriteLine($"there's no element with specefied idex:{index}");
            }
        }

        public void Calculate()
        {
            if (IsChangable)
            {
                Console.WriteLine("You need to finilize the network!");
                return;
            }
            foreach (Layer layer in layerList)
            {
                layer.Calculate();
            }
        }
        public double SquereCost(List<double> Expected)
        {
            if (IsChangable) {
                Console.WriteLine("You need to finilize the network!");
                return 0;
            }
            double Diff;
            double Sum =0;
            Calculate();
            for (int i = 0; i < layerList[layerList.Count-1].neuronList.Count; i++)
            {
                Diff = Expected[i] - layerList[layerList.Count - 1].neuronList[i].value;
                Sum += System.Math.Pow(Diff, 2);
            }
            return Sum;
        }
        public double SquereCost(List<double> Expected, int index)
        {
            if (IsChangable)
            {
                Console.WriteLine("You need to finilize the network!");
                return 0;
            }
            double Diff;
            double Sum;
            Calculate();

            Diff = Expected[index] - layerList[layerList.Count - 1].neuronList[index].value;
            Sum = System.Math.Pow(Diff, 2);
            return Sum;
        }
        public double SquereCost(double expected, int index)
        {
            if (IsChangable)
            {
                Console.WriteLine("You need to finilize the network!");
                return 0;
            }
            double Diff;
            double Sum;
            Calculate();

            Diff = expected - layerList[layerList.Count - 1].neuronList[index].value;
            Sum = System.Math.Pow(Diff, 2);
            return Sum;
        }
        public void Finilize()
        {
            IsChangable = false;
            layerList[0].type = LayerType.Input;
            layerList[layerList.Count - 1].type = LayerType.output;
        }
        public void neglect()
        {
            IsChangable = true;
            layerList[0].type = LayerType.Hideen;
            layerList[layerList.Count - 1].type = LayerType.Hideen;
        }
        public void EnterVal(List<double> list)
        {
            if (!IsChangable)
            {
                foreach (Neuron neuron in layerList[0].neuronList)
                {
                    neuron.value = list[neuron.ID];
                }
            }
        }
        public List<double> ExtractVal()
        {
            List<double> list = new();
            List<Neuron> neulist = layerList.Find(x=>x.type==LayerType.output).neuronList;
            if (!IsChangable)
            {
                for (int i=0; i <= neulist.Count -1;i++)
                {
                    list.Add(neulist[i].value);
                }
            }
            return list;
        }
#endregion
        Stopwatch watch = new();
        public long Serial = 0, Paralleltime = 0; 
        public void FindPaths(bool ForBench = false, int Until = 0)
        {
            if (IsChangable) return;
            if(ForBench)
            {
                watch.Start();
                Console.WriteLine("The timer is on");
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
                Console.WriteLine($"Finding Paths took {watch.ElapsedMilliseconds}millisecond");
                Console.WriteLine("The timer is off");
                Serial = watch.ElapsedMilliseconds;
                watch.Reset();
            }
        }
        public void FindPathsParallel( bool ForBench = false, int until = 0)
        {
            if (IsChangable) return;
            if (ForBench)
            {
                watch.Start();
                Console.WriteLine("The timer is on");
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
                Console.WriteLine($"Finding Paths took {watch.ElapsedMilliseconds}millisecond");
                Console.WriteLine("The timer is off");
                Paralleltime = watch.ElapsedMilliseconds;
                watch.Reset();
            }
        }
        
        #region Info Methods
        // the function below is for debugging
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
        public void InfowC()
        {

            System.Console.WriteLine($"neuralnetwork{ID}");
            System.Console.WriteLine("structure:");
            foreach (Layer layer in layerList)
            {
                layer.InfowC();
            }
            System.Console.WriteLine("connection:");
            foreach (Connector connector in connectorDic.Connectors.Values)
            {
                connector.InfoLog();
            }
        }
        public void InfowB()
        {

            System.Console.WriteLine($"neuralnetwork{ID}:{type}:{calcType}");
            System.Console.WriteLine("structure:");
            foreach (Layer layer in layerList)
            {
                layer.InfowB();
            }
            System.Console.WriteLine("connection:");
            foreach (Connector connector in connectorDic.Connectors.Values)
            {
                connector.InfowB();
            }
        }
        public List<string> FileInfo()
        {
            List<string> data = new();
            data.Add("s");
            data.Add($"net{ID}:{(int)calcType};");
            foreach (Layer layer in layerList)
            {
                foreach (string info in layer.FileInfo())
                {
                    data.Add(info);
                }
            }
            data.Add("c");
            foreach (Connector connector in connectorList)
            {
                data.Add(connector.FileInfo());
            }
            return data;
        }
        public List<string> StructInfo()
        {
            List<string> data = new();
            data.Add($"net{ID}:{(int) calcType};");
            foreach (Layer layer in layerList)
            {
                foreach (string info in layer.FileInfo())
                {
                    data.Add(info);
                }
            }
            return data;
        }
        public List<string> ConnectInfo()
        {
            List<string> data = new();
            foreach (Connector connector in connectorList)
            {
                data.Add(connector.FileInfo());
            }
            return data;
        }
        #endregion
    }
    class NetworkDic
    {
       public static Dictionary<int, NeuralNetwork> Networks = new();
       public static List<string> FileInfo()
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
       public static void RandomNetworks(int numberOfFile = 2,string user = "user")
        {
            Random rnd = new();
            string Path = @"C:\Users\" + user + $@"\Desktop\NeuralNetwork\TestFolder\config{numberOfFile}.mn1";
            ConfigFile config = new ConfigFile(Path);
            int m = rnd.Next(20);
            for (int n = 0; n < m; n++)
            {
                NeuralNetwork network = new NeuralNetwork(n, CalcType.Sigmoid);
                for (int i = 0; i <= 10; i++)
                {
                    network.AddLayer();
                    int k = rnd.Next(1, 10);
                    for (int j = 0; j < k; j++)
                    {
                        network.layerList[i].AddNeuron();
                        double b = 0.001 * rnd.Next(-20000, 20000);
                        network.layerList[i].neuronList[j].bias = b;
                    }
                }
                network.connectMLPBasicRandom();
                int r = rnd.Next(1, 10);
                for(int a =0;a<= r; a++)
                {
                    for (int j = 0; j <= 9; j++)
                    {
                        Layer layer = network.layerList[j];
                        int i = rnd.Next(0, layer.neuronList.Count - 1);
                        int k = rnd.Next(j + 1, 10);
                        int l = rnd.Next(0, network.layerList[k].neuronList.Count - 1);
                        double w = 0.001 * rnd.Next(-20000, 20000);
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
        public static void RandomNetworks(string path = @"C:\Users\user\Desktop\NeuralNetwork\TestFolder\config.mn1")
        {
            Random rnd = new();
            ConfigFile config = new ConfigFile(path);
            int m = rnd.Next(20);
            for (int n = 0; n < m; n++)
            {
                NeuralNetwork network = new NeuralNetwork(n, CalcType.Sigmoid);
                for (int i = 0; i <= 10; i++)
                {
                    network.AddLayer();
                    int k = rnd.Next(1, 10);
                    for (int j = 0; j < k; j++)
                    {
                        network.layerList[i].AddNeuron();
                        double b = 0.001 * rnd.Next(-20000, 20000);
                        network.layerList[i].neuronList[j].bias = b;
                    }
                }
                network.connectMLPBasicRandom();
                int r = rnd.Next(1, 10);
                for (int a = 0; a <= r; a++)
                {
                    for (int j = 0; j <= 9; j++)
                    {
                        Layer layer = network.layerList[j];
                        int i = rnd.Next(0, layer.neuronList.Count - 1);
                        int k = rnd.Next(j + 1, 10);
                        int l = rnd.Next(0, network.layerList[k].neuronList.Count - 1);
                        double w = 0.001 * rnd.Next(-20000, 20000);
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
    }
}