using System;
using System.Collections.Generic;
namespace NeuralNetwork{
    //the enum below is for different kinds of networks and I'll focus majorly in 3 types
    enum NetworkType
    {
        MLP,
        RNN,
        CNN
    }
    enum CalcType
    {
        Atanh,
        Sigmoid,
        ReLU
    }
    //this class is for networks 
    class NeuralNetwork{
        //that's for keeping track of networks
        public int ID;
        NetworkType type;
        public CalcType calcType;
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
                NetworkDic.Networks.Add(ID, this);
                AddLayer();
                type = NetworkType.MLP;
            }
        }

        // addneuron is function to put more neurons in a layer
        void AddLayer(int index)
        {
            if (index >= 0)
            {
                layerList.Add(new Layer(index, ID));
            }
        }
        public void AddLayer()
        {
            AddLayer(layerList.Count);
            System.Console.WriteLine($"a new layer with ID{layerList.Count - 1} is added to the network {ID}.");
        }

        public void connect(Neuron from, Neuron to)
        {
            //highly doubtful about this is working, if so then I'll change class connector as layer and neuron
            connectorDic.Connectors.Add(Connector.naming(from.name, to.name, this),
                new Connector(from.name, to.name, this));
            connectorList.Add(connectorDic.Connectors[Connector.naming(from.name, to.name, this)]);
            if(from.LayerNum >= to.LayerNum)
            {
                type = NetworkType.RNN;
            }
        }
        public void connect(Neuron from, Neuron to,double w)
        {
            //highly doubtful about this is working, if so then I'll change class connector as layer and neuron
            connectorDic.Connectors.Add(Connector.naming(from.name, to.name, this),
                new Connector(from.name, to.name, this, w));
            if (from.LayerNum >= to.LayerNum)
            {
                type = NetworkType.RNN;
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
        // The 2 functions below are refreshing lists because after editting lists, 
        // the layers or neurons will still have the previous ID and we have to change it

        //this is refresh function
        

        void DeleteLayer(int index){
            if(layerList[index] != null){
                layerList.Remove(layerList[index]);
                LayerDic.Layers.Remove(Layer.Naming(index,ID));
                Refreshing.Refresh(layerList);
            }else{
                System.Console.WriteLine($"there's no element with specefied idex:{index}");
            }
        }

        public void Calculate()
        {
            foreach(Layer layer in layerList)
            {
                layer.Calculate();
            }
        }

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

            System.Console.WriteLine($"neuralnetwork{ID}");
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
            data.Add($"net{ID}");
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
    }
    class NetworkDic
    {
       public static Dictionary<int, NeuralNetwork> Networks = new();
    }
}