using Atomic.ArtificialNeuralNetwork.libraries;
namespace Atomic.NeuralNetworkLib.Driver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = @"TestFolder\config2.mn1";
            ConfigFile config = new(path);
            config.CreateANN();
            NeuralNetwork network = NetworkDic.Networks[1];
            network.Finilize();
            network.InfowB();
            System.Console.WriteLine("------------------------------------------------------");
            PathDic.Paths.Clear();
            // the following comments are for single loop test
            //bool PoverS = true;
            //if (PoverS == true)
            //{
            //    network.FindPathsParallel(true);
            //    System.Console.WriteLine($"Parallel   found:{PathDic.Paths.Count} path --  time taken:{network.Paralleltime}");
            //}
            //else
            //{
            //    network.FindPaths(true);
            //    System.Console.WriteLine($"Serial   found:{PathDic.Paths.Count} path --  time taken:{network.Serial}");
            //}

            // the following comments are for double loop test
            //bool PfSl = true;
            //if (PfSl)
            //{
            //    network.FindPathsParallel(true);
            //    System.Console.WriteLine($"Parallel   found:{PathDic.Paths.Count} path --  time taken:{network.Paralleltime}");
            //    PathDic.Paths.Clear();
            //    network.FindPaths(true);
            //    System.Console.WriteLine($"Serial   found:{PathDic.Paths.Count} path --  time taken:{network.Serial}");
            //}
            //else
            //{
            //    network.FindPaths(true);
            //    System.Console.WriteLine($"Serial   found:{PathDic.Paths.Count} path --  time taken:{network.Serial}");
            //    PathDic.Paths.Clear();
            //    network.FindPathsParallel(true);
            //    System.Console.WriteLine($"Parallel found:{PathDic.Paths.Count} path --  time taken:{network.Paralleltime}");
            //}
        }
    }
}
//the old commands
//NeuralNetwork network = new NeuralNetwork(0, CalcType.ReLU);
//network.AddLayer();
//network.AddLayer();
//network.layerList[0].AddNeuron();
//network.layerList[1].AddNeuron();
//network.layerList[0].AddNeuron();
//network.connectMLPBasic();
//network.connect(network.layerList[0].neuronList[0], network.layerList[2].neuronList[0]);
//network.InfoLog();
//int i = -2;
//foreach (Connector connector in connectorDic.ActiveConnectors.Values)
//{
//    connector.Change(i);
//    i++;
//}
//i = 10;
//foreach (Neuron neuron in NeuronDic.Neurons.Values)
//{
//    neuron.bias = i;
//    i--;
//}
//i = 8;
//foreach (Neuron neuron in network.layerList[0].neuronList)
//{
//    neuron.value = i;
//    i += 2;
//}
//network.InfowB();
//network.layerList[0].InfowC();
//network.Calculate();
//network.InfowC();
#region //newer commands
//string user = "someone";
//string path = @"C:\Users\" + user + @"\Desktop\NeuraalNetwork\TestFolder\config.mn1";
//ConfigFile config = new ConfigFile(path);
//NeuralNetwork network = new NeuralNetwork(0, CalcType.ReLU);
//network.AddLayer();
//network.AddLayer();
//network.layerList[0].AddNeuron();
//network.layerList[1].AddNeuron();
//network.layerList[0].AddNeuron();
//network.connectMLPBasic();
//network.connect(network.layerList[0].neuronList[0], network.layerList[2].neuronList[0]);
//network.InfoLog();
//int i = -2;
//foreach (Connector connector in connectorDic.ActiveConnectors.Values)
//{
//    connector.Change(i);
//    i++;
//}
//i = 10;
//foreach (Neuron neuron in NeuronDic.Neurons.Values)
//{
//    neuron.bias = i;
//    i--;
//}
//network.InfowB();
//config.EditFile();
#endregion
#region //newest commands
//string user = "someone";
//string path = @"C:\Users\" + user + @"\Desktop\NeuraalNetwork\TestFolder\config.mn1";//ConfigFile config = new ConfigFile(path);
//config.CreateANN();
//NetworkDic.Networks[0].InfowB();
#endregion
#region othher commands
//network.FirstWork();
//ThreadsDic.Initialize(12);
//for (int i = 0; i < 12; i++)
//{
//    if (i < 4) ThreadsDic.threads["Thread" + i].Action = network.DividedWorkOfLayer;
//    else ThreadsDic.threads["Thread" + i].Action = network.DividedWorkOfNeuron;
//}
//List<ParallelProccess> threads = new();
//for (int i = 0; i < 4; i++)
//{
//    threads.Add(ThreadsDic.threads["Thread" + i]);
//}
//List<List<ParallelProccess>> parallels = new List<List<ParallelProccess>>();
//for (int i = 0; i < 4; i++)
//{
//    parallels.Add(new List<ParallelProccess> { ThreadsDic.threads[$"Thread{i + 4}"], ThreadsDic.threads[$"Thread{i + 8}"] });
//}

//for (int j = 9; j > 7; j--)
//{
//    List<List<Neuron>> ListofLists = NeuralNetwork.DivideList(network.layerList[j].neuronList, 4);
//    for (int i = 0; i < 4; i++)
//    {
//        threads[i].Start(new Int4
//        {
//            i = i,
//            j = 2,
//            k = 4,
//            l = j,
//            Threads = parallels[i]
//        });
//    }
//    foreach (var thr in threads) thr.Join();
//}
//foreach (var item in ThreadsDic.threads.Values)
//{
//    item.Kill();
//}
#endregion
