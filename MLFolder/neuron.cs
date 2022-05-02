using System;
using System.Collections.Generic;
using NeuralNetwork;
//we will put every class in big library and we'll call it "neural network" 
namespace NeuralNetwork
{

    // first construct neuron
    public class neuron {
        // these are for identifying the neuron
      public int ID;
      public int LayerNum;
      private int NetworkID;
      public string name;
      // these aren't identifying the neuron
      private float value;
      private float bias;
       // the constructer should know the ID of neuron
      public neuron(int index, int layerNum,int networkID){
        // here, we make sure there isn't neuron with the same id
        if(NeuronDic.Neurons.ContainsKey("neuron" + index + "layer" + LayerNum + "network" + networkID)){
            System.Console.WriteLine($"there is a previous neuron with specified index:{index}" +
                $" and Layer index:{LayerNum} from network:{networkID}.");
        }else{
        // here, the construction
            ID = index;
            LayerNum = layerNum;
            NetworkID = networkID;
            name = Naming(ID, LayerNum, NetworkID);
            NeuronDic.Neurons.Add(Naming(ID, LayerNum, NetworkID),this);
            System.Console.WriteLine($"A neuron is constructed with index:{ID} and in Layer:{LayerNum}" +
              $" in the network:{NetworkID}");
        }
      }
     //this is for naming neurons
      static string Naming (int index, int LayerNum, int networkID){
        return "neuron" + index + "layer" + LayerNum + "network" + networkID;
      }
    }

    // this class is here to count all the neurons
    public class NeuronDic{
         public static Dictionary<string,neuron> Neurons = new Dictionary<string, neuron>();
    }
}