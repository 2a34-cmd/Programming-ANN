using System;
using System.Collections.Generic;
namespace NeuralNetwork
{
    class Connector
    {
        //the 2 fields below are identyfing the line between neurons
        public string From;
        public string To;
        public  NeuralNetwork Network;
        string name;
        bool EnableBiasChange;
        //like weight, this is for lerning and application proccess
        double wieght;

        // the constructer
        public Connector(string from, string to, NeuralNetwork network){
            From = from;
            To = to;
            Network = network; 
            name = naming(From,To,Network);
            // I will initialize bias
            wieght = 0f;
            enableBiasChanging();
            System.Console.WriteLine($"a new connector is constructed from {From} to {To} in network {Network.ID}");
        }
        public Connector(string from, string to, NeuralNetwork network,double w)
        {
            From = from;
            To = to;
            Network = network;
            name = naming(From, To, Network);
            // I will initialize bias
            wieght = w;
            enableBiasChanging();
            System.Console.WriteLine($"a new connector is constructed from {From} to {To} in network {Network.ID}");
        }
        //I will later build a delete method,but in this time, I will build clear method that also disable changing bias
        public void clear()
        {
            wieght = 0f;
            EnableBiasChange = false;
            connectorDic.ActiveConnectors.Remove(name);
        }
        // The method below is just to enable bias changing
        public void enableBiasChanging()
        {
            EnableBiasChange = true;
            connectorDic.ActiveConnectors.Add(name, this);
        }
        public void Change(double x)
        {
            if (EnableBiasChange == false)
            {
                throw new Exception("you can't change inchangable connector");
            }
            else
            {
                wieght = x;
            }
        }
        public double GetWieght()
        {
            return wieght;
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
        //public double? BackProp(double[] Expected)
        //{
        //    if (Network.IsChangable) return null;
        //    double Product = 1;
        //    Product = (double)NeuronDic.Neurons[From].BackProp(Expected);
        //    Product *= NeuronDic.Neurons[From].value;
        //    return Product;
        //}
        #region InfoMethods
        char[] ILLEGALWORDS = new char[]{'n','e','u','r','o','l','a','y'};
        public void InfoLog()
        {
            string[] f = From.Split(' ')[0].Split(ILLEGALWORDS,StringSplitOptions.RemoveEmptyEntries);
            string[] t = To.Split(' ')[0].Split(ILLEGALWORDS,StringSplitOptions.RemoveEmptyEntries);
            System.Console.WriteLine($"  connector from [{f[0]},{f[1]}] to [{t[0]},{t[1]}]");
        }
        public void InfowB()
        {
            string[] f = From.Split(' ')[0].Split(ILLEGALWORDS, StringSplitOptions.RemoveEmptyEntries);
            string[] t = To.Split(' ')[0].Split(ILLEGALWORDS, StringSplitOptions.RemoveEmptyEntries);
            System.Console.WriteLine($"  connector from [{f[0]},{f[1]}] to [{t[0]},{t[1]}] :{wieght}");
        }
        public string FileInfo()
        {
            string[] f = From.Split(' ')[0].Split(ILLEGALWORDS, StringSplitOptions.RemoveEmptyEntries);
            string[] t = To.Split(' ')[0].Split(ILLEGALWORDS, StringSplitOptions.RemoveEmptyEntries);
            return$"[{f[0]},{f[1]}][{t[0]},{t[1]}]{wieght}:{Network.ID};";
        }
        #endregion
    }
    // like the previous classes, there will be dic
    class connectorDic {
        public static Dictionary<string, Connector> Connectors = new();
        public static Dictionary<string, Connector> ActiveConnectors = new();
    }

}
