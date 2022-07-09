namespace Atomic.ArtificialNeuralNetwork.libraries
{
    /// <summary>
    /// connector is a class where wieghts lives in.
    /// </summary>
    public class Connector
    {
        //the 2 fields below are identyfing the line and its direction between neurons
        internal string From;
        internal string To;
        //network is the network where the connector is.
        internal  NeuralNetwork Network;
        //name is the unique thing about the connector. it combines from and to and network togather
        readonly string name;
        //like bias, this is for lerning and calculating proccess
        decimal wieght;

        // the constructer
        internal Connector(string from, string to, NeuralNetwork network,decimal w = 0m)
        {
            From = from;
            To = to;
            Network = network;
            name = naming(From, To, Network);
            // initialize bias
            wieght = w;
            NeuronDic.Neurons[To].Root.Add(this);
            connectorDic.Connectors.Add(name, this);
        }
        //since wieght is private, the method below is needed
        /// <returns>wieght of connector</returns>
        internal decimal GetWieght()
        {
            return wieght;
        }
        /// <summary>
        /// by supplying the info, naming will provide the name of the connnector
        /// </summary>
        /// <param name="from">the neuron's name that is the beginning of connector's line</param>
        /// <param name="to">the neuron's name that is the end of connector's line</param>
        /// <param name="network">the network that the connector is living</param>
        /// <returns>name of the connector</returns>
        internal static string naming(string from, string to, NeuralNetwork network)
        {
            return $"connector from {from} to {to} in network{network.ID}";
        }
        /// <summary>
        /// subtract value from wieght 
        /// </summary>
        /// <param name="diff">the value which is subtracted</param>
        internal void SetWB(decimal diff)
        {
            wieght -= diff;
        }
        #region InfoMethods
        char[] ILLEGALWORDS = new char[]{'n','e','u','r','o','l','a','y'};
        /// <summary>
        /// gives the info about the unique features of the connector
        /// </summary>
        public void InfoLog()
        {
            string[] f = From.Split(' ')[0].Split(ILLEGALWORDS,StringSplitOptions.RemoveEmptyEntries);
            string[] t = To.Split(' ')[0].Split(ILLEGALWORDS,StringSplitOptions.RemoveEmptyEntries);
            System.Console.WriteLine($"  connector from [{f[0]},{f[1]}] to [{t[0]},{t[1]}]");
        }
        /// <summary>
        /// gives the info of the connector including wiegth and its name
        /// </summary>
        public void InfowB()
        {
            string[] f = From.Split(' ')[0].Split(ILLEGALWORDS, StringSplitOptions.RemoveEmptyEntries);
            string[] t = To.Split(' ')[0].Split(ILLEGALWORDS, StringSplitOptions.RemoveEmptyEntries);
            System.Console.WriteLine($"  connector from [{f[0]},{f[1]}] to [{t[0]},{t[1]}] :{wieght}");
        }
        /// <summary>
        /// gives configfile the neccessary info to save it in file
        /// </summary>
        /// <returns></returns>
        internal string FileInfo()
        {
            string[] f = From.Split(' ')[0].Split(ILLEGALWORDS, StringSplitOptions.RemoveEmptyEntries);
            string[] t = To.Split(' ')[0].Split(ILLEGALWORDS, StringSplitOptions.RemoveEmptyEntries);
            return$"[{f[0]},{f[1]}][{t[0]},{t[1]}]{wieght}:{Network.ID};";
        }
        #endregion
    }
    /// <summary>
    /// class that stores the static dictionary about connectors
    /// </summary>
    class connectorDic {
        /// <summary>
        /// static member that stores every connector
        /// </summary>
        internal static Dictionary<string, Connector> Connectors = new();
    }

}
