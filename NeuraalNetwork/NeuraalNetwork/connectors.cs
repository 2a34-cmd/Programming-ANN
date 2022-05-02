using System;
using System.Collections.Generic;
namespace NeuralNetwork
{
    class connector
    {
        //the 2 fields below are identyfing the line between neurons
        string From;
        string To;
        NeuralNetwork Network;
        string name;
        bool EnableBiasChange;
        //like weight, this is for lerning and application proccess
        float bias;

        // the constructer
        public connector(string from, string to, NeuralNetwork network){
            From = from;
            To = to;
            Network = network; 
            name = naming(From,To,Network);
            // I will initialize bias
            bias = 0f;

            //add to ActiveDic
            connectorDic.ActiveConnectors.Add(name, this);
            System.Console.WriteLine($"a new connector is constructed from {From} to {To} in network {Network.ID}");
        }
        //I will later build a delete method,but in this time, I will build clear method that also disable changing bias
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
        // There will be delete method
        public void delete()
        {
            connectorDic.Connectors.Remove(name);
        }
        public static string naming(string from, string to, NeuralNetwork network)
        {
            return $"connector from {from} to {to} in network{network.ID}";
        }

    }
    // like the previous classes, there will be dic
    class connectorDic {
        public static Dictionary<string, connector> Connectors = new Dictionary<string, connector>();
        public static Dictionary<string, connector> ActiveConnectors = new Dictionary<string, connector>();
    }

}
