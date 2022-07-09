using System.Collections.Concurrent;

namespace Atomic.ArtificialNeuralNetwork.libraries
{
    //the most important and essential peice of every network: neuron
    /// <summary>
    /// class about 
    /// </summary>
    class Neuron
    {
        // these are for identifying the neuron
        internal int ID;
        internal int LayerNum;
        internal int NetworkID;
        internal string name;
        //paths keeps track of paths that their from is this neuron
        internal ConcurrentBag<Path> paths;
        //Root keeps track of connectors that thier to is this neuron
        //bot paths and Root are used in Back propagation methods in neural network class
        internal List<Connector> Root;
        // value is what neuron holds
        internal decimal value;
        //bias is what neuron affect in its calculation of value
        internal decimal bias;
        //Der is how much will neuron change its bias
        decimal Der;
        /// <summary>
        /// returns Der, a private member that stores future bias change
        /// </summary>
        internal decimal Diff { get { return Der; }  }

        readonly CalcType calc;
        // the constructer should know the ID of neuron
        internal Neuron(int index, int layerNum, int networkID, decimal bais = 0)
        {
            // here, we make sure there isn't neuron with the same id
            if (NeuronDic.Neurons.ContainsKey(Naming(index, layerNum, networkID)))
            {
                throw new($"there is a previous neuron with specified index:{index}" +
                    $" and Layer index:{LayerNum} from network:{networkID}.");
            }
            else
            {
                // here, the construction
                ID = index;
                LayerNum = layerNum;
                NetworkID = networkID;
                paths = new();
                Root = new();
                bias = bais;
                value = 0m;
                calc = NetworkDic.Networks[networkID].calcType;
                name = Naming(ID, LayerNum, NetworkID);
                NetworkDic.Networks[NetworkID].neuronList.Add(this);
                NeuronDic.Neurons.Add(Naming(ID, LayerNum, NetworkID), this);
            }
        }
        //this is for naming neurons
        /// <summary>
        /// by supplying the needed parameters, the function returns the unique name of the neuron
        /// </summary>
        /// <param name="index"></param>
        /// <param name="LayerNum"></param>
        /// <param name="networkID"></param>
        /// <returns></returns>
        static string Naming (int index, int LayerNum, int networkID){
            return "neuron" + index + "layer" + LayerNum + "network" + networkID;
        }
        #region Info Methods
        /// <summary>
        /// give info about the neuron's name
        /// </summary>
        public void InfoLog() { Console.WriteLine($"    neuron{ID}");}
        /// <summary>
        /// gives info about the neuron's value
        /// </summary>
        public void InfowC() {Console.WriteLine($"    neuron{ID} :{value}");}
        /// <summary>
        /// gives info about the neuron's bias
        /// </summary>
        public void InfowB(){Console.WriteLine($"    neuron{ID} :{bias}"); }
        /// <summary>
        /// gives info to pass it to configfile so it can be saved
        /// </summary>
        /// <returns></returns>
        internal string FileInfo() { return $"nu {ID} : {bias};"; }
        #endregion
        /// <summary>
        /// change value according to bias and end connections
        /// </summary>
        internal void Calculate()
        {
            CalcType type = NetworkDic.Networks[NetworkID].calcType;
            decimal mean = 0;
            foreach (Connector connector in Root)
            {
                mean += connector.GetWieght() * NeuronDic.Neurons[connector.From].value;
            }
            mean += bias;
            value = ActivationFunctions.Activation(mean,type);
            return;
        }
        /// <summary>
        /// calculate the value of needed change according to back propagation algorathim
        /// </summary>
        /// <returns></returns>
        decimal DCalculate()
        {
            CalcType type = NetworkDic.Networks[NetworkID].calcType;
            decimal mean = 0;
            foreach (Connector connector in Root)
            {
                mean += connector.GetWieght() * NeuronDic.Neurons[connector.From].value;
            }
            mean += bias;
            return ActivationFunctions.DActivation(mean,type);
        }

        /// <summary>
        /// do back propagation algorathim that correspond to this neuron
        /// </summary>
        /// <param name="Expected"></param>
        internal void Backprop(decimal[] Expected)
        {
            if (LayerNum == 0) return;
            decimal Sum = 0;
            if (paths.IsEmpty)
            {
                Der = 2 * (value - Expected[ID]);
                return;
            }
            foreach (Path path in paths)
            {
                decimal Product = 2 * (path.Out.value - Expected[path.Out.ID]);
                foreach (Connector connector in path.connectors)
                {
                    Product *= NeuronDic.Neurons[connector.To].DCalculate();
                    Product *= connector.GetWieght();
                }
                Sum += Product;
            }
            Der += Sum;
            return;
        }
        /// <summary>
        /// change Der to 0
        /// </summary>
        internal void DefultingDer() { Der = 0; }
        /// <summary>
        /// divide Der on Batchsize
        /// </summary>
        /// <param name="PatchSize">the divisor</param>
        internal void DivideDer(int BatchSize) { Der /= BatchSize; }
        internal void SetWB(decimal diff)
        {
            bias -= diff;
        }
    }
    // this class is here to count all the neurons
    /// <summary>
    /// stores the static meber, Neurons
    /// </summary>
    class NeuronDic{
        /// <summary>
        /// dictionary that stores every neuron and its name
        /// </summary>
         internal static Dictionary<string,Neuron> Neurons = new();
    }
}