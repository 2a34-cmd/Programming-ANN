using System;
using System.Collections.Generic;
namespace Atomic.ArtificialNeuralNetwork.libraries
{
    class Connector //: IWeightBias
    {
        //the 2 fields below are identyfing the line between neurons
        public string From;
        public string To;
        public  NeuralNetwork Network;
        string name;
        bool EnableBiasChange;
        //like weight, this is for lerning and application proccess
        decimal wieght;

        // the constructer
        public Connector(string from, string to, NeuralNetwork network,decimal w = 0m)
        {
            From = from;
            To = to;
            Network = network;
            name = naming(From, To, Network);
            // I will initialize bias
            wieght = w;
            NeuronDic.Neurons[To].Root.Add(this);
            //network.WBList.Add(this);
            enableBiasChanging();
            Console.WriteLine($"a new connector is constructed from {From} to {To} in network {Network.ID}");
        }
        //I will later build a delete method,but in this time, I will build clear method that also disable changing bias
        public void clear()
        {
            wieght = 0m;
            EnableBiasChange = false;
        }
        // The method below is just to enable bias changing
        public void enableBiasChanging()
        {
            EnableBiasChange = true;
            connectorDic.Connectors.Add(name, this);
        }
        public void Change(decimal x)
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
        public decimal GetWieght()
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
        //public decimal? BackProp(decimal[] Expected)
        //{
        //    if (Network.IsChangable) return null;
        //    decimal Product = (decimal)NeuronDic.Neurons[To].BackProp(Expected);
        //    Product *= NeuronDic.Neurons[From].value;
        //    return Product;
        //}
        //public void SetWB(decimal[] Expected)
        //{
        //    wieght -= Network.LearningRate * (decimal)BackProp(Expected);
        //}
        public void SetWB(decimal diff)
        {
            wieght -= diff;
        }
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
    }

}
