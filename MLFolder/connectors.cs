using System;
using System.Collections.Generic;
namespace NeuralNetwork
{
    class connector
    {
        //the 2 fields below are identyfing the line between neurons
        neuron From;
        neuron To;
        string name;
        bool EnableBiasChange;
        //like weight, this is for lerning and application proccess
        float bias;

        // the constructer
        public connector(neuron from, neuron to){
            From = from;
            To = to;
            name = naming(this);
            // I will initialize bias
            bias = 0f;

            //add to Dic
            connectorDic.Connectors.Add(name, this);
            connectorDic.ActiveConnectors.Add(name, this);
            System.Console.WriteLine($"a new connector is constructed from {From.name} to {To.name}");
        }
        //I will not build a delete method, instead, I will build clear method that also disable changing bias
        public void clear()
        {
            bias = 0f;
            EnableBiasChange = false;
            connectorDic.ActiveConnectors.Remove(name);
        }
        // The method below is just to enable bias changing
        public void enableBiasChanging()
        {
            EnableBiasChange = true;
            connectorDic.ActiveConnectors.Add(name, this);
        }
        public string naming(connector connector)
        {
            return $"connector from {connector.From.name} to {connector.To.name}";
        }

    }
    // like the previous classes, there will be dic
    class connectorDic {
        public static Dictionary<string, connector> Connectors = new Dictionary<string, connector>();
        public static Dictionary<string, connector> ActiveConnectors = new Dictionary<string, connector>();
    }

}
