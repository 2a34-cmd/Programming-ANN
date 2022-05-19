using System;
using System.Collections.Generic;
namespace NeuralNetwork{
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
        public double LearningRate;
        public NetworkType type;
        public CalcType calcType;
        public bool IsChangable;
        //that's for keeping track of layers
        public List<Layer> layerList = new();
        List<Connector> connectorList = new();
        //and here the constructer
        public NeuralNetwork(int id, CalcType calc)
        {
            if (NetworkDic.Networks.ContainsKey(id))
            {
                System.Console.WriteLine($"you can't construct the constructed!!");
            }
            else
            {
                ID = id;
                calcType = calc;
                System.Console.WriteLine($"a new network is constructed with ID {ID}");
                //NetworkDic.Networks.Add(ID, this);
                //AddLayer();
                type = NetworkType.MLP;
                IsChangable = true;
            }
        }

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
                System.Console.WriteLine($"a new layer with ID{layerList.Count - 1} is added to the network {ID}.");
        }
        public void connect(Neuron from, Neuron to, double w = 0)
        {
            //highly doubtful about this is working, if so then I'll change class connector as layer and neuron
            if (IsChangable)
            {
                connectorDic.Connectors.Add(Connector.naming(from.name, to.name, this),
                new Connector(from.name, to.name, this, w));
                if (from.LayerNum >= to.LayerNum)
                {

                    type = NetworkType.RNN;
                }
            }
        }
        // The method below is for connecting all layers with each other without any jumbing connectors
        public void connectMLPBasic()
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
                            connect(from, to);
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
        public void FindPaths()
        {
            List<Path> paths = new List<Path>(PathDic.Paths.Values);
            List<Path> x = new();
            foreach (Neuron neuron in layerList[layerList.Count-1].neuronList)
            {
                _= new Path(neuron.Root());
            }
            for(int i =layerList.Count - 1; i > 0; i--)
            {
                foreach(Neuron neuron in layerList[i].neuronList)
                {
                    x = paths.FindAll(y => y.from == neuron);
                    foreach (Connector connector in neuron.Root())
                    {
                        foreach (Path path in x)
                        {
                            path.NewPath(connector);
                        }
                    }
                }
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
            foreach (Connector connector in connectorDic.ActiveConnectors.Values)
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
            foreach (Connector connector in connectorDic.ActiveConnectors.Values)
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
            foreach (Connector connector in connectorDic.ActiveConnectors.Values)
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
            List<string> data = new();
            data.Add("s");
            foreach (NeuralNetwork network in Networks.Values)
            {
                foreach (string info in network.StructInfo())
                {
                    data.Add(info);
                }
            }
            data.Add("c");
            foreach (NeuralNetwork network in Networks.Values)
            {
                foreach (string info in network.ConnectInfo())
                {
                    data.Add(info);
                }
            }
            return data;
       }
    }
}