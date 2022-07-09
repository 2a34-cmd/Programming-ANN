using System.Collections.Concurrent;

namespace Atomic.ArtificialNeuralNetwork.libraries
{
    //  as a way to do things, I thought using 'path' to do BP
    //  it's a one big calculation proccess to find all paths in network
    //  after finding all paths, we can do back propagation algorathim according to the paths in the network
    //  path is defined as one serial ordered way from one neuron to one of neurons of output layer,
    //      containing every connector in its way
    /// <summary>
    /// provides one ordered serial way from one neuron to end ones
    /// </summary>
    public class Path
    {
        /// <summary>
        /// the beginning neuron
        /// </summary>
        internal Neuron from;
        /// <summary>
        /// ID of last neuron, the output one
        /// </summary>
        int OutID;
        /// <summary>
        /// the output and last neuron
        /// </summary>
        internal Neuron Out;
        /// <summary>
        /// name of the path
        /// </summary>
        public string Name;
        /// <summary>
        /// the ordered list of neurons from beginning to end
        /// </summary>
        internal List<Neuron> neurons = new();
        /// <summary>
        /// the ordered directed way from A to B
        /// </summary>
        internal List<Connector> connectors = new();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectors">the directed road from beginning to output layer </param>
        public Path(List<Connector> connectors)
        {
            if (Check(connectors))
            {
                from = NeuronDic.Neurons[connectors[0].From];
                OutID = NeuronDic.Neurons[connectors[connectors.Count - 1].To].ID;
                Out = NeuronDic.Neurons[connectors[connectors.Count - 1].To];
                this.connectors = connectors;
                neurons.Add(from);
                for(int i =0;i<connectors.Count;i++)
                {
                    neurons.Add(NeuronDic.Neurons[connectors[i].To]);
                }
                Name = Naming(from,neurons);
                //Console.WriteLine($"Path is constructed with name of {Name}");
                PathDic.Paths.TryAdd(Name, this);
                NetworkDic.Networks[from.NetworkID].PathList.Add(this);
                from.paths.Add(this);
            }
            else
            {
                throw new("cannot construct this path");
            }
        }
        #region NewPaths methods
        //in this region, you can find all the ways to add and find more paths in a network
        /// <summary>
        /// a reccursive method to add and find more paths
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="path"></param>
        static void NewPath(Connector connector,Path path)
        {
            List<Connector> connectors = new() { connector };
            connectors.AddRange(path.connectors);
            if (Check(connectors)) _ = new Path(connectors);
        }
        /// <summary>
        /// parallel way to find paths which share the same startin connector
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="path"></param>
        static void NewPathsParallel(Connector connector, ConcurrentBag<Path> path)
        {
            Parallel.ForEach(path, p => {
                NewPath(connector, p);
            });
        }
        /// <summary>
        /// serial way to find paths which share the same startin connector
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="path"></param>
        static void NewPathsSerial(Connector connector, ConcurrentBag<Path> path)
        {
            foreach (var item in path)
            {
                NewPath(connector, item);
            }
        }
        /// <summary>
        /// parallel way to find paths that share anchor point in the middle
        /// </summary>
        /// <param name="connectors"></param>
        /// <param name="paths"></param>
        public static void NewPathsParallel(List<Connector> connectors, ConcurrentBag<Path> paths)
        {
            Parallel.ForEach(connectors, c => {
                NewPathsParallel(c, paths);
            });
        }
        /// <summary>
        /// serial way to find paths that share anchor point in the middle
        /// </summary>
        /// <param name="connectors"></param>
        /// <param name="paths"></param>
        public static void NewPathsSerial(List<Connector> connectors, ConcurrentBag<Path> paths)
        {
            foreach(var c in connectors){
                NewPathsSerial(c, paths);
            }
        }
        #endregion
        /// <summary>
        /// the secure way to check if the connector list is ordered and directed
        /// </summary>
        /// <param name="connectors">the list to check</param>
        /// <returns>check boolean value</returns>
        public static bool Check(List<Connector> connectors)
        {
            for (int i = 1; i < connectors.Count; i++)
            {
                if (connectors[i - 1].To != connectors[i].From) return false;
            }
            if (connectors[0].Network.layerList[^1].neuronList.Exists(
                x => x.name == connectors[^1].To)) return true;
            return false;
        }
        /// <summary>
        /// gives unique name of the path
        /// </summary>
        /// <param name="from"></param>
        /// <param name="neurons"></param>
        /// <returns></returns>
        static string Naming(Neuron from, List<Neuron> neurons)
        {
            string str = "path from[" + from.ID + "," + from.LayerNum + "]";
            foreach (Neuron neuron in neurons)
            {
                if (neuron == from) continue;
                str += "to[" + neuron.ID + "," + neuron.LayerNum + "]";
            }
            str += $" in network {neurons[0].NetworkID}";
            return str;
        }
        /// <summary>
        /// gives info about the path to be stored in a file
        /// </summary>
        /// <returns></returns>
        internal string FileInfo()
        {
            string info = "";
            foreach (var item in neurons)
            {
                info += $"[{item.ID},{item.LayerNum}]";
            }
            return info + $":{from.NetworkID};";
        }
    }
    /// <summary>
    /// stores the static member, Paths, that contains info about every path.
    /// also contains methods to save and giving info about them
    /// </summary>
    public class PathDic
    {
        /// <summary>
        /// the dictionary that stores every path in the program
        /// </summary>
        public static ConcurrentDictionary<string, Path> Paths = new();
        /// <summary>
        /// gives info about every path in the program
        /// </summary>
        /// <param name="BeginningSpace">the beginning space in the writing</param>
        public static void InfoOfPaths(string BeginningSpace = "")
        {
            Action<string> Invoked = (string str) =>
            {
                Console.Write(BeginningSpace);
                Console.Write(str);
            };
            Invoked("Paths:");
            Console.WriteLine();
            foreach (Path path in Paths.Values.OrderBy(p => p.Name))
            {
                Invoked($"  Path from [{path.from.ID},{path.from.LayerNum}]");
                foreach (var neuron in path.neurons)
                {
                    if (neuron == path.from) continue;
                    Console.Write($"to[{neuron.ID},{neuron.LayerNum}]");
                }
                Console.Write($" in network {path.neurons[0].NetworkID}" + '\n');
            }
            Invoked(Paths.Count.ToString());
        }
        /// <summary>
        /// gives info to configfile to save it in parallel manner
        /// </summary>
        /// <returns></returns>
        public static string[] FileInfoParallel()
        {
            ConcurrentBag<string> Info = new() { "p"};
            Parallel.ForEach(Paths.Values, p => Info.Add(p.FileInfo()));
            return Info.ToArray();
        }
        /// <summary>
        /// gives info to configfile to save it in serial manner
        /// </summary>
        /// <returns></returns>
        public static string[] FileInfo()
        {
            List<string> Info = new() { "p" };
            foreach (var p in Paths.Values){ Info.Add(p.FileInfo()); };
            return Info.ToArray();
        }
    }
}