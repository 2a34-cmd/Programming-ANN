using System.Collections.Generic;
namespace NeuralNetwork
{
    public class Refreshing
    {
        public static void Refresh(List<Layer> list)
        {
            int counter = list.Count;
            List<Layer> refreshed = new List<Layer>();
            int minID;
            for (int i = 0; i < counter; i++)
            {
                minID = FindMinID(list);
                refreshed.Add(list[minID]);
                refreshed[minID].ID = minID;
                list.Remove(list[minID]);
            }
            list = refreshed;
        }

        // The function below will be used to refresh lists
        private static int FindMinID(List<Layer> list)
        {
            if (list.Count == 0)
            {
                System.Console.WriteLine($"the list{list.ToString()} cannot be used to search for minimum value");
            }
            int minID = int.MaxValue;
            foreach (Layer type in list)
            {
                if (type.ID < minID)
                {
                    minID = type.ID;
                }
            }
            return minID;
        }

        public static void Refresh(List<neuron> list)
        {
            int counter = list.Count;
            List<neuron> refreshed = new List<neuron>();
            int minID;
            for (int i = 0; i < counter; i++)
            {
                minID = FindMinID(list);
                refreshed.Add(list[minID]);
                refreshed[minID].ID = minID;
                list.Remove(list[minID]);
            }
            list = refreshed;
        }

        // The function below will be used to refresh lists
        private static int FindMinID(List<neuron> list)
        {
            if (list.Count == 0)
            {
                System.Console.WriteLine($"the list{list.ToString()} cannot be used to search for minimum value");
            }
            int minID = int.MaxValue;
            foreach (neuron type in list)
            {
                if (type.ID < minID)
                {
                    minID = type.ID;
                }
            }
            return minID;
        }
    }
}