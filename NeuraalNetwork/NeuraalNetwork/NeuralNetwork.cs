using System;
using System.Collections.Generic;
namespace NeuralNetwork{
    //this class is for networks and there's no need for Dic for this kind of big structures
    public class NeuralNetwork{
        //that's for keeping track of layers
        public List<Layer> layerList = new List<Layer>();
        //and here the constructer
        public NeuralNetwork()
        {
            layerList.Add(new Layer(0));
        }

        // addneuron is function to put more neurons in a layer
        void AddLayer(int index){
            if(index >= 1){
            layerList.Add(new Layer(index));
            }else{ 
                System.Console.WriteLine("you can't add neuron with index below 1");
            }
        }
        void AddLayer()
        {
            AddLayer(layerList.Count +1);
        }

        void connect()
        {
            //TO-DO: make connectors
        }
        // The 2 functions below are refreshing lists because after editting lists, 
        // the layers or neurons will still have the previous ID and we have to change it

        //this is refresh function
        

        void DeleteLayer(int index){
            if(layerList[index] != null){
                layerList.Remove(layerList[index]);
                LayerDic.Layers.Remove(index);
                Refreshing.Refresh(layerList);
            }else{
                System.Console.WriteLine($"there's no element with specefied idex:{index}");
            }
        }
    }
}