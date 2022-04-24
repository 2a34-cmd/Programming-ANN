using System;
using System.Collections.Generic;
using NeuralNetwork;
//we will put every class in big library and we'll call it "neural network" 
public namespace NeuralNetwork
{

    // first construct neuron
    public class neuron {
      // these are for identifying the neuron
      private int index;
      private int LayerNum;
      private string name;
      // these aren't identifying the neuron
      private float value;
      private float bias;
       // the constructer should know the ID of neuron
      public neuron(int index, int LayerNum){
        // here, we make sure there isn't neuron with the same id
        if(NeuronDic.Neurons.ContainsKey("neuron" + index + "layer" + LayerNum)){
            System.Console.WriteLine($"there is a previous neuron with specified index:{index} and Layer index:{LayerNum}.");
        }else{
        // here, the construction
        this.index = index;
        this.LayerNum = LayerNum;
        this.name = Naming(index, LayerNum);
        NeuronDic.Neurons.Add(Naming(index, LayerNum),this);
        System.Console.WriteLine($"A neuron is constructed with index:{this.index} and in Layer:{this.LayerNum}");
        }
      }
     //this is for naming neurons
      static string Naming (int index, int LayerNum){
        return "neuron" + index + "layer" + LayerNum;
      }
    }

    // this class is here to count all the neurons
    public static class NeuronDic{
         public Dictionary<string,neuron> Neurons = new Dictionary<string, neuron>();
    }
}