using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Concurrent;

namespace Atomic.ArtificialNeuralNetwork.libraries
{
    class Path
    {
        public Neuron from;
        int OutID;
        public Neuron Out;
        public string Name;
        public List<Neuron> neurons = new();
        public List<Connector> connectors = new();
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
                return;
            }
        }
        static void NewPath(Connector connector,Path path)
        {
            List<Connector> connectors = new() { connector };
            connectors.AddRange(path.connectors);
            if (Check(connectors)) _ = new Path(connectors);
        }
        
        static void NewPathsParallel(Connector connector, ConcurrentBag<Path> path)
        {
            Parallel.ForEach(path, p => {
                NewPath(connector, p);
            });
        }
        static void NewPathsSerial(Connector connector, ConcurrentBag<Path> path)
        {
            foreach (var item in path)
            {
                NewPath(connector, item);
            }
        }
        public static void NewPathsParallel(List<Connector> connectors, ConcurrentBag<Path> paths)
        {
            Parallel.ForEach(connectors, c => {
                NewPathsParallel(c, paths);
            });
        }
        public static void NewPathsSerial(List<Connector> connectors, ConcurrentBag<Path> paths)
        {
            foreach(var c in connectors){
                NewPathsSerial(c, paths);
            }
        }
        void NewPath(Connector connector)
        {
            List<Connector> connectors = new(){ connector};
            connectors.AddRange(this.connectors);
            if (Check(connectors)) _ = new Path(connectors);
        }
        public static bool Check(List<Connector> connectors)
        {
            for (int i = 1; i < connectors.Count; i++)
            {
                if (connectors[i - 1].To != connectors[i].From) return false;
            }
            if (connectors[0].Network.layerList[connectors[0].Network.layerList.Count - 1].neuronList.Exists(
                x => x.name == connectors[connectors.Count - 1].To))
            {
                return true;
            }
            return false;
        }
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
    }
    class PathDic
    {
        public static ConcurrentDictionary<string, Path> Paths = new();
        //public static ImmutableDictionary<string,Path>.Builder Paths = ImmutableDictionary.CreateBuilder<string,Path>();
        public static void InfoOfPaths(string s = "", bool Ordered = false)
        {
            Action<string> Invoked = (string str) =>
            {
                Console.Write(s);
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
    }
}