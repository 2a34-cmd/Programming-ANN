namespace NeuralNetwork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NeuralNetwork network = new NeuralNetwork(0, CalcType.ReLU);
            network.AddLayer();
            network.AddLayer();
            network.layerList[0].AddNeuron();
            network.layerList[1].AddNeuron();
            network.layerList[0].AddNeuron();
            network.connectMLPBasic();
            network.connect(network.layerList[0].neuronList[0], network.layerList[2].neuronList[0]);
            network.InfoLog();
            int i = -2;
            foreach (Connector connector in connectorDic.ActiveConnectors.Values)
            {
                connector.Change(i);
                i++;
            }
            i = 10;
            foreach (Neuron neuron in NeuronDic.Neurons.Values)
            {
                neuron.bias = i;
                i--;
            }
            i = 8;
            foreach (Neuron neuron in network.layerList[0].neuronList)
            {
                neuron.value = i;
                i += 2;
            }
            network.InfowB();
            network.layerList[0].InfowC();
            network.Calculate();
            network.InfowC();
        }
    }
}
