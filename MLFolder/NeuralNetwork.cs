using System;
using System.Collections.Generic;
public namespace NeuralNetwork{
    //I'll do it as there's just one neural network in all my program so you will just see the word 'static' in this class
    public static class NeuralNetwork{
        //that's for keeping track of layers
        public static List<Layer> layerList = new List<Layer>();

        // addneuron is function to put more neurons in a layer
        void AddLaayer(int index){
            if(layerList.Count = 0){
                layerList.Add(new Layer(0));
            }
            if(index >= 1){
            layerList.add(new Layer(index));
            }else{ 
                System.Console.WriteLine("you can't add neuron with index below 1");
            }
        }
        // The 2 functions below are refreshing lists because after editting lists, 
        // the layers or neurons will still have the previous ID and we have to change it

        //this is refresh function
        public static void Refresh(List<T> list){
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
        private int FindMinID(List<MyType> list)
        {
            if (list.Count == 0)
            {
                System.Console.WriteLine($"the list{list.ToString()} cannot be used to search for minimum value");
                return;
            }
            int minID = int.MaxValue;
            foreach (MyType type in list)
            {
                if (type.ID < minID)
                {
                    minID = type.Age;
                }
            }
            return minID;
        }

        void DeleteLayer(int index){
            if(layerList[index] != null){
                layerList.Remove(layerList[index]);
                Refresh(layerList);
            }else{
                System.Console.WriteLine($"there's no element with specefied idex:{index}");
            }
        }
    }
}