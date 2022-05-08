using System.Collections.Generic;
namespace NeuralNetwork
{
    //I know that refreshing class is not working as intended, but I will leave it for later
    public class Refreshing
    {
        public static void Refresh(List<Layer> list)
        {
            int counter = list.Count;
            List<Layer> refreshed = new();
            int minID;
            for (int i = 0; i <= counter; i++)
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
                System.Console.WriteLine($"the list{list} cannot be used to search for minimum value");
                throw new System.Exception("you can't find min ID if there are no elements");
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

        public static void Refresh(List<Neuron> list)
        {
            int counter = list.Count;
            List<Neuron> refreshed = new List<Neuron>();
            int minID;
            int i = 0;
            while (i < counter)
            {
                minID = FindMinID(list);
                refreshed.Add(list[minID]);
                refreshed[minID].ID = minID;
                list.Remove(list[minID]);
                i++;
            }
            list = refreshed;
        }

        // The function below will be used to refresh lists
        private static int FindMinID(List<Neuron> list)
        {
            if (list.Count == 0)
            {
                System.Console.WriteLine($"the list{list.ToString()} cannot be used to search for minimum value");
                throw new System.Exception("you can't find min ID if there are no elements");
            }
            int minID = int.MaxValue;
            foreach (Neuron type in list)
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