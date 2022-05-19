namespace NeuralNetwork
{
    class ActivationFunctions
    {
        static double y;
        #region AFunction
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
        #endregion
        #region Derivetives
        public static double DAtnh(double x)
        {
            y = 1 / (1 - System.Math.Pow(x,2));
            return y;
        }
        public static double DSigmoid(double x)
        {
            y = System.Math.Exp(-x) /System.Math.Pow(System.Math.Exp(-x)+1,2);
            return y;
        }
        public static double DReLU(double x)
        {
            if (x> 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        #endregion

    }
}
