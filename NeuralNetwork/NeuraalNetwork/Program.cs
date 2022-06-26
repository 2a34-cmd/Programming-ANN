using Atomic.ArtificialNeuralNetwork.libraries;
using System.Linq;
namespace Atomic.ArtificialNeuralNetwork.Driver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = @"TestFolder\config4.mn1";
            ConfigFile config = new(path);
            config.CreateANN();
            NeuralNetwork network = NetworkDic.Networks[1];
            network.Finilize();
            network.InfowB();
            System.Console.WriteLine("------------------------------------------------------");
            PathDic.Paths.Clear();
            network.FindPaths();
            PathDic.InfoOfPaths();
            System.Console.WriteLine("------------------------------------------------------" + '\n' + "Before training");
            decimal[][] inputs = Enumerable.Range(0, 10).Select(i => new decimal[] { i, i * 0.1m }).ToArray();
            decimal[][] trainingSet = Enumerable.Range(0, 10).Select(i => new decimal[] { i * 0.05m }).ToArray();
            var doubles1 = Enumerable.Range(0, 10).Select(i => network.SquereCost(trainingSet[i].ToList(), inputs[i])).ToArray();
            for (int i = 0; i < inputs.Length; i++)
            {
                network.Calculate(inputs[i]);
                network.InfowC();
                System.Console.WriteLine(doubles1[i]);
            }
            decimal[] doubles2 = new decimal[10];
            for (int j = 0; j < 10; j++)
            {
                network.PatchProccess(trainingSet, inputs,0.001m);
                System.Console.WriteLine("------------------------------------------------------" + '\n' + "After training");
                network.InfowB();
                doubles1 = doubles2;
                doubles2 = Enumerable.Range(0, 10).Select(i => network.SquereCost(trainingSet[i].ToList(), inputs[i])).ToArray();
                for (int i = 0; i < inputs.Length; i++)
                {
                    network.Calculate(inputs[i]);
                    network.InfowC();
                    System.Console.WriteLine(doubles2[i]);
                }
                network.InfowB();
                for (int i = 0; i < doubles1.Length; i++)
                {
                    System.Console.WriteLine($"Total gain :{doubles1[i] - doubles2[i]}");
                }
            }
            
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
#region //othher commands
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
