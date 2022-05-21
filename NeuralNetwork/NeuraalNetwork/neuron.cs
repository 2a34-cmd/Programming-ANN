using System;
using System.Collections.Generic;
using NeuralNetwork;
//we will put every class in big library and we'll call it "neural network" 
namespace NeuralNetwork
{

    // first construct neuron
    class Neuron {
        // these are for identifying the neuron
          public int ID;
          public int LayerNum;
          public int NetworkID;
          public string name;
          // these aren't identifying the neuron
          public double value;
          public double bias;
        // the constructer should know the ID of neuron
        //public Neuron(int index, int layerNum,int networkID){
        //  // here, we make sure there isn't neuron with the same id
        //  if(NeuronDic.Neurons.ContainsKey(Naming(index,layerNum,networkID) ) ){
        //      System.Console.WriteLine($"there is a previous neuron with specified index:{index}" +
        //          $" and Layer index:{LayerNum} from network:{networkID}.");
        //  }else{
        //  // here, the construction
        //      ID = index;
        //      LayerNum = layerNum;
        //      NetworkID = networkID;
        //      value = 0d; 
        //      name = Naming(ID, LayerNum, NetworkID);
        //      System.Console.WriteLine($"A neuron is constructed with index:{ID} and in Layer:{LayerNum}" +
        //          $" in the network:{NetworkID}");
        //      NeuronDic.Neurons.Add(Naming(ID, LayerNum, NetworkID),this);
        //  }
        //}
        CalcType calc;
        public Neuron(int index, int layerNum, int networkID, double bais = 0)
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
                bias = bais;
                value = 0d;
                calc = NetworkDic.Networks[networkID].calcType;
                name = Naming(ID, LayerNum, NetworkID);
                System.Console.WriteLine($"A neuron is constructed with index:{ID} and in Layer:{LayerNum}" +
                    $" in the network:{NetworkID}");
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
            double mean = 0;
            List<Connector> connectors = new();
            foreach (Connector connector in connectorDic.ActiveConnectors.Values)
            {
                if (connector.To == name)
                {
                    connectors.Add(connector);
                }
            }
            foreach (Connector connector in connectors)
            {
                mean += connector.GetWieght() * NeuronDic.Neurons[connector.From].value;
            }
            mean += bias;
            if (type == CalcType.Atanh) value = ActivationFunctions.Atanh(mean);
            if (type == CalcType.ReLU) value = ActivationFunctions.ReLU(mean);
            if (type == CalcType.Sigmoid) value = ActivationFunctions.Sigmoid(mean);
            return;
        }
        //public double BackProp(int index)
        //{
        //    double Sum = 0;
        //    foreach (Connector connector in )
        //    {

        //    }
        //    return Sum;
        //}
        public List<Connector> Root()
        {
            List<Connector> connectors = new();
            foreach (Connector connector in connectorDic.Connectors.Values)
            {
                if (connector.To == name) {
                    connectors.Add(connector); 
                }
            }
            return connectors;
        }
        public List<Connector> Tree()
        {
            List<Connector> connectors = new();
            foreach (Connector connector in connectorDic.Connectors.Values)
            {
                if (connector.From == name)
                {
                    connectors.Add(connector);
                }
            }
            return connectors;
        }
    }

    // this class is here to count all the neurons
    class NeuronDic{
         public static Dictionary<string,Neuron> Neurons = new();
    }
}