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
    //this class is for networks 
    class NeuralNetwork{
        //that's for keeping track of networks
        public int ID;
        NetworkType type;
        //that's for keeping track of layers
        public List<Layer> layerList = new List<Layer>();
        //and here the constructer
        public NeuralNetwork()
        {
            NetworkDic.Networks.Add(this, ID);
            layerList.Add(new Layer(0,ID));
            type = NetworkType.MLP;
        }

        // addneuron is function to put more neurons in a layer
        void AddLayer(int index){
            if(index >= 1){
            layerList.Add(new Layer(index,ID));
            }else{ 
                System.Console.WriteLine("you can't add neuron with index below 1");
            }
        }
        void AddLayer()
        {
            AddLayer(layerList.Count +1);
        }

        void connect(neuron from, neuron to)
        {
            //highly doubtful about this is working, if so then I'll change class connector as layer and neuron
            connectorDic.Connectors.Add(connector.naming(from.name, to.name, this),
                new connector(from.name, to.name, this));
            if(from.LayerNum >= to.LayerNum)
            {
                type = NetworkType.RNN;
            }
        }
        // The method below is for connecting all layers with each other without any jumbing connectors
        void connectMLPBasic()
        {
            foreach (Layer layer in layerList) 
            {
                if (layer != layerList[layerList.Count])
                {
                    int i = layerList.FindIndex(a => a.ID == layer.ID);
                    Layer NextLayer = layerList[i + 1];
                    foreach (neuron from in layer.neuronList)
                    {
                        foreach (neuron to in NextLayer.neuronList)
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
    }
    class NetworkDic
    {
       public static Dictionary<NeuralNetwork, int> Networks = new Dictionary<NeuralNetwork, int>();
    }
}