using System;
using System.Collections.Generic;
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
        if(neuronDic.NeuronDic.ContainsKey("neuron" + index + "layer" + LayerNum)){
            System.Console.WriteLine($"there is a previous neuron with specified index:{index} and Layer index:{LayerNum}.");
        }else{
        // here, the construction
        this.index = index;
        this.LayerNum = LayerNum;
        this.Naming();
        neuronDic.NeuronDic.Add(this.name,this);
        }
    }
    //this is for naming neurons
    public void Naming (){
        name = "neuron" + this.index + "layer" + this.LayerNum;
    }
}

// this class is here to count all the neurons
public static class neuronDic{
    public Dictionary<string,neuron> NeuronDic = new Dictionary<string, neuron>();
}