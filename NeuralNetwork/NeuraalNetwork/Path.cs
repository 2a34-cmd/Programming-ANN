using System.Collections.Generic;
namespace NeuralNetwork
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
                Name = Path.Naming(from,neurons);
                if (PathDic.Paths.ContainsKey(Name)) return;
                PathDic.Paths.Add(Name, this);
            }
            else
            {
                return;
            }
        }
        public static void NewPath(Connector connector,Path path)
        {
            List<Connector> connectors = new();
            connectors.Add(connector);
            foreach (Connector connector1 in path.connectors)
            {
                connectors.Add(connector1);
            }
            if (Check(connectors)) _ = new Path(connectors);
        }
        public static void NewPath(Path path,Connector connector)
        {
            List<Connector> connectors = new();
            foreach (Connector connector1 in path.connectors)
            {
                connectors.Add(connector1);
            }
            connectors.Add(connector);
            if (Check(connectors)) _ = new Path(connectors);
        }
        public void NewPath(Connector connector)
        {
            List<Connector> connectors = new();
            connectors.Add(connector);
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
        string Naming()
        {
            string str = "path from[" + from.ID +"," + from.LayerNum +"]";
            foreach(Connector neuron in connectors)
            {
                if (NeuronDic.Neurons[neuron.From] == from) continue;
                str += $"to[{ NeuronDic.Neurons[neuron.To].ID},{ NeuronDic.Neurons[neuron.To].LayerNum}]";
            }
            str += $" in network {neurons[0].NetworkID}";
            return str; 
        }
        public static string Naming(Neuron from, List<Neuron> neurons)
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
        public static Dictionary<string, Path> Paths = new();
        public static void InfoOfPaths()
        {
            System.Console.WriteLine("Paths:");
            foreach (Path path in Paths.Values)
            {
                System.Console.Write($"  Path from [{path.from.ID},{path.from.LayerNum}] ");
                foreach (var neuron in path.neurons)
                {
                    if (neuron == path.from) continue;
                    System.Console.Write($"to[{neuron.ID},{neuron.LayerNum}]");
                }
                System.Console.Write($" in network {path.neurons[0].NetworkID}" + '\n');
            }
            System.Console.WriteLine(Paths.Count);
        }
        static List<int> extractint(string input)
        {
            List<int> data = new();
            string parameter = string.Empty;
            int val = 0;
            foreach (char IsDigit in input)
            {
                if (char.IsDigit(IsDigit) || IsDigit == '-')
                {
                    parameter += IsDigit;
                }
                else if (IsDigit == '.')
                {

                }
                else
                {
                    if (parameter == "") continue;
                    try
                    {
                        val = int.Parse(parameter);
                        data.Add(val);
                    }
                    catch (System.Exception ex) { System.Console.WriteLine(ex.Message); }
                    parameter = string.Empty;
                    val = 0;
                }
            }

            return data;
        }
        public static void CheckUpsideDown()
        {
            List<int> ints = new();
            foreach (var path in Paths.Values)
            {
                ints.Add(extractint(path.Name)[0]);
                ints.Add(extractint(path.Name)[1]);
                path.connectors[0].From = $"neuron{ints[0]}layer{ints[1]}network{path.connectors[0].Network.ID}";
                ints.Clear();
            }
        }
    }
}