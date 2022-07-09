namespace Atomic.ArtificialNeuralNetwork.libraries
{
    /// <summary>
    /// classifies layers into thier types in the network
    /// </summary>
    enum LayerType
    {
        Hideen=0,Input=1,output=2
    }
    /// <summary>
    /// collection of neurons in parallel that are considered to be one 'layer'
    /// </summary>
    class Layer{
        // what below is for identifying
        internal int ID;
        int NetworkID;
        internal LayerType type;
        //that's for keeping track of neurons
        internal List<Neuron> neuronList = new();
        //the constructer
        internal Layer(int index, int networkID){
            if(LayerDic.Layers.ContainsKey(Naming(index,networkID) ) ){
                throw new Exception("You can't do that");
            }else{
              // here, the construction
                ID = index;
                NetworkID = networkID;
                type = LayerType.Hideen;
                LayerDic.Layers.Add(Naming(index,networkID),this);
            }
        }
        // addneuron is function to put more neurons in a layer
        void AddNeuron(int index){
            if(index >= 0){
            neuronList.Add(new Neuron(index,ID, NetworkID));
            }else{ 
                throw new("you can't add neuron with index below 1");
            }
        }
        /// <summary>
        /// adds neuron to the layer and aotumate the idex of the neuron
        /// </summary>
        internal void AddNeuron()
        {
            AddNeuron(neuronList.Count);
        }
        //this is for naming neurons
        internal static string Naming(int LayerNum, int networkID)
        {
            return "layer" + LayerNum + "network" + networkID;
        }

        #region Info Methods
        /// <summary>
        /// gives info about layer and its neurons
        /// </summary>
        public void InfoLog()
        {
            System.Console.WriteLine($"  layer{ID}");
            foreach (Neuron neuron in neuronList)
            {
                neuron.InfoLog();
            }
        }
        /// <summary>
        /// gives info about layer and its neurons' values
        /// </summary>
        public void InfowC()
        {
            System.Console.WriteLine($"  layer{ID}");
            foreach (Neuron neuron in neuronList)
            {
                neuron.InfowC();
            }
        }
        /// <summary>
        /// give info about layer and its neurons' biases
        /// </summary>
        public void InfowB()
        {
            System.Console.WriteLine($"  layer{ID}");
            foreach (Neuron neuron in neuronList)
            {
                neuron.InfowB();
            }
        }
        /// <summary>
        /// gives info about layer and its neurons to configfile so it save their info
        /// </summary>
        /// <returns></returns>
        internal List<string> FileInfo()
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
        /// <summary>
        /// change the values of neurons in the layer according to thier biases and end connections 
        /// </summary>
        internal void Calculate()
        {
            foreach (Neuron neuron in neuronList)
            {
                neuron.Calculate();
            }
        }
    }
    // the class below is to count all layers
    /// <summary>
    /// stores dictionary about every layer in the program
    /// </summary>
    class LayerDic{
        /// <summary>
        /// the dictionary that saves every layer in the program
        /// </summary>
        internal static Dictionary<string, Layer> Layers = new();
    }
}