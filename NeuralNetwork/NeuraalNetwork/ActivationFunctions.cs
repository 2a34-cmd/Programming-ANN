namespace NeuralNetwork
{
    class ActivationFunctions
    {
        static double y;
        public static double Atanh(double x)
        {
            y = System.Math.Atanh(x);
            return y;
        }
        public static double Sigmoid(double x)
        {
            y = 1/(1+ System.Math.Exp(-x));
            return y;
        }
        public static double ReLU(double x)
        {
            if (x > 0)
            {
                return x;
            }
            else
            {
                return 0;
            } 
        }
        
    }
}
