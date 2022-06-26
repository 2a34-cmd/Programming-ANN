using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
//we will put every class in big library and we'll call it "neural network" 
namespace Atomic.ArtificialNeuralNetwork.libraries
{

    // first construct neuron
    class Neuron //: IWeightBias
    {
        // these are for identifying the neuron
        public int ID;
        public int LayerNum;
        public int NetworkID;
        public string name;
        public ConcurrentBag<Path> paths;
        public List<Connector> Root;
        // these aren't identifying the neuron
        public decimal value;
        public decimal bias;
        decimal Der;
        public decimal Diff { get { return Der; } }
        CalcType calc;
        // the constructer should know the ID of neuron
        public Neuron(int index, int layerNum, int networkID, decimal bais = 0)
        {
            // here, we make sure there isn't neuron with the same id
            if (NeuronDic.Neurons.ContainsKey(Naming(index, layerNum, networkID)))
            {
                System.Console.WriteLine($"there is a previous neuron with specified index:{index}" +
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
                Console.WriteLine($"A neuron is constructed with index:{ID} and in Layer:{LayerNum}" +
                    $" in the network:{NetworkID}");
                NetworkDic.Networks[NetworkID].neuronList.Add(this);
                NeuronDic.Neurons.Add(Naming(ID, LayerNum, NetworkID), this);
            }
        }
        //this is for naming neurons
        static string Naming (int index, int LayerNum, int networkID){
            return "neuron" + index + "layer" + LayerNum + "network" + networkID;
        }
        #region Info Methods
        public void InfoLog() { System.Console.WriteLine($"    neuron{ID}");}
        public void InfowC() {Console.WriteLine($"    neuron{ID} :{value}");}
        public void InfowB(){Console.WriteLine($"    neuron{ID} :{bias}"); }
        public string FileInfo() { return $"nu {ID} : {bias};"; }
        #endregion
        public void Calculate()
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
        public decimal DCalculate()
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
        //need more work


        public void Backprop(decimal[] Expected)
        {
            if (LayerNum == 0) return;
            if (NetworkDic.Networks[NetworkID].IsChangable) return;
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
        public void DefultingDer() { Der = 0; }
        public void DivideDer(int PatchSize) { Der /= PatchSize; }
        //public decimal? BP(decimal[] Expected)
        //{
        //    if (LayerNum == 0) return 1;
        //    if (NetworkDic.Networks[NetworkID].IsChangable) return null;
        //    decimal Sum = 0;
        //    if (paths.IsEmpty)
        //    {
        //        return 2 * (value - Expected[ID]);
        //    }
        //    foreach (Path path in paths)
        //    {
        //        decimal Product = 2 * (path.Out.value - Expected[path.Out.ID]);
        //        foreach (Connector connector in path.connectors)
        //        {
        //            Product *= NeuronDic.Neurons[connector.To].DCalculate();
        //            Product *= connector.GetWieght();
        //        }
        //        Sum += Product;
        //    }
        //    return Sum;
        //}
        //public decimal? BackProp(decimal[] Expected)
        //{
        //    if (NetworkDic.Networks[NetworkID].IsChangable) return null;
        //    decimal Product = Der;
        //    return Product;
        //}
        //public void SetWB(decimal[] Expected)
        //{
        //    Backprop(Expected);
        //    bias -= Der * NetworkDic.Networks[NetworkID].LearningRate;
        //}
        public void SetWB(decimal diff)
        {
            bias -= diff;
        }
    }
    // this class is here to count all the neurons
    class NeuronDic{
         public static Dictionary<string,Neuron> Neurons = new();
    }
}