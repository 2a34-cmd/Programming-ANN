using System.Collections.Generic;
namespace NeuralNetwork{
    public enum LayerType
    {
        Hideen=0,Input=1,output=2
    }
    public class Layer{
        // what below is for identifying
        public int ID;
        int NetworkID;
        public LayerType type;
        //that's for keeping track of neurons
        public List<Neuron> neuronList = new List<Neuron>();
        //the constructer
        public Layer(int index, int networkID){
            if(LayerDic.Layers.ContainsKey(Naming(index,networkID) ) ){
                System.Console.WriteLine($"there is a previous layer with specified index:{index}.");
            }else{
              // here, the construction
                ID = index;
                NetworkID = networkID;
                type = LayerType.Hideen;
                System.Console.WriteLine($"A layer{ID} network{NetworkID} is constructed.");
                LayerDic.Layers.Add(Naming(index,networkID),this);
            }
        }
        // addneuron is function to put more neurons in a layer
        void AddNeuron(int index){
            if(index >= 1 && NetworkDic.Networks[NetworkID].IsChangable){
            this.neuronList.Add(new Neuron(index,ID, NetworkID));
            }else{ 
                System.Console.WriteLine("you can't add neuron with index below 1");
            }
        }
        public void AddNeuron()
        {
            AddNeuron(neuronList.Count);
        }
        //this is for naming neurons
        public static string Naming(int LayerNum, int networkID)
        {
            return "layer" + LayerNum + "network" + networkID;
        }

        //deleteneuron is used to remove neuron that are not needed
        public void DeleteNeuron(int index){
            if(neuronList[index] != null && NetworkDic.Networks[NetworkID].IsChangable){
                neuronList.Remove(this.neuronList[index]);
                NeuronDic.Neurons.Remove("neuron" + index + "layer" + ID);
               // Refreshing.Refresh(this.neuronList);
                
            }else{
                System.Console.WriteLine($"there's no element with specefied idex:{index}");
            }
        }
        #region Info Methods
        public void InfoLog()
        {
            System.Console.WriteLine($"  layer{ID}");
            foreach (Neuron neuron in neuronList)
            {
                neuron.InfoLog();
            }
        }
        public void InfowC()
        {
            System.Console.WriteLine($"  layer{ID}");
            foreach (Neuron neuron in neuronList)
            {
                neuron.InfowC();
            }
        }
        public void InfowB()
        {
            System.Console.WriteLine($"  layer{ID}");
            foreach (Neuron neuron in neuronList)
            {
                neuron.InfowB();
            }
        }
        public List<string> FileInfo()
        {
            List<string> data = new();
            data.Add($"l{ID};");
            foreach (Neuron neuron in neuronList)
            {
                data.Add(neuron.FileInfo());
            }
            return data;
        }
        #endregion
        public void Calculate()
        {
            foreach (Neuron neuron in neuronList)
            {
                neuron.Calculate();
            }
        }
    }
    // the class below is to count all layers
    public class LayerDic{
        public static Dictionary<string, Layer> Layers = new Dictionary<string, Layer>();
    }
}