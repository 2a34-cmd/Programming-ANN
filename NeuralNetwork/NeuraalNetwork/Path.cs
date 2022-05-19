using System.Collections.Generic;
namespace NeuralNetwork
{
    class Path
    {
        public Neuron from;
        int OutID;
        Neuron Out;
        string Name;
        List<Neuron> neurons;
        List<Connector> connectors;
        public Path(List<Connector> connectors)
        {
            if (Check(connectors))
            {
                from = NeuronDic.Neurons[connectors[0].From];
                OutID = NeuronDic.Neurons[connectors[connectors.Count - 1].To].ID;
                Out = NeuronDic.Neurons[connectors[connectors.Count - 1].To];
                Name = Naming();
                PathDic.Paths.Add(Name, this);
                neurons.Add(from);
                for(int i =0;i<connectors.Count;i++)
                {
                    neurons.Add(NeuronDic.Neurons[connectors[i].To]);
                }
            }
            else
            {
                throw new System.Exception("You can't create a path with no serial connoctor chain");
            }
        }
        public static void NewPath(Connector connector,Path path)
        {
            List<Connector> connectors = new();
            connectors.Add(connector);
            connectors.AddRange(path.connectors);
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
            string str = "path from" + from.ID +"layer" + from.LayerNum;
            foreach(Neuron neuron in neurons)
            {
                str += "to" + neuron.ID;  
            }
            return str; 
        }
    }
    class PathDic
    {
        public static Dictionary<string, Path> Paths = new();
    }
}
