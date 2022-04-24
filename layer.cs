using System;
using System.Collections.Generic;
public namespace NeuralNetwork{
    public class Layer{
        // what below is for identifying
        public int ID;
        //that's for keeping track of neurons
        public List<neuron> neuronList = new List<neuron>();
        public string name;
        //the constructer
        public Layer(int index){
            if(NeuronDic.Neurons.ContainsKey(index)){
            System.Console.WriteLine($"there is a previous layer with specified index:{index}.");
            }else{
              // here, the construction
            this.ID = index;
            LayerDic.Layers.Add(index,this);
            // I prefer constructing initial neuron for every layer
            this.neuronList.Add(new neuron(0,index));
            System.Console.WriteLine($"A layer{this.ID} is constructed with initial neuron0");
            }
        }
        // addneuron is function to put more neurons in a layer
        void AddNeuron(int index){
            if(index >= 1){
            this.neuronList.Add(new neuron(index,this.ID));
            }else{ 
                System.Console.WriteLine("you can't add neuron with index below 1");
            }
        }
        
    }
    public static class LayerDic{
        public Dictionary<int, Layer> Layers = new Dictionary<int, Layer>();
    }
}