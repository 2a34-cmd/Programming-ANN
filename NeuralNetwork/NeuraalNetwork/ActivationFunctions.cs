namespace Atomic.ArtificialNeuralNetwork.libraries
{
    class ActivationFunctions
    {
        static decimal y;
        #region AFunction
        public static decimal Tanh(decimal x)
        {
            y = DecimalMath.Tanh(x);
            return y;
        }
        public static decimal Sigmoid(decimal x)
        {
            y = 1/(1+ DecimalMath.Exp(-x));
            return y;
        }
        public static decimal ReLU(decimal x)
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
        public static decimal Id(decimal x)
        {
            return x;
        }
        public static decimal Activation(decimal x, CalcType type)
        {
            return type switch
            {
                CalcType.Atanh => Tanh(x),
                CalcType.Sigmoid => Sigmoid(x),
                CalcType.ReLU => ReLU(x),
                CalcType.Id => Id(x),
                _ => 0,
            };
        }
        #endregion
        #region Derivetives
        public static decimal DTanh(decimal x)
        {
            y = DecimalMath.PowerN(1 - Tanh(x),2);
            return y;
        }
        public static decimal DSigmoid(decimal x)
        {
            y = DecimalMath.Exp(-x) /DecimalMath.PowerN(DecimalMath.Exp(-x)+1,2);
            return y;
        }
        public static decimal DReLU(decimal x)
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
        public static decimal DId(decimal x)
        {
            return 1;
        }
        public static decimal DActivation(decimal x,CalcType type)
        {
            return type switch
            {
                CalcType.Atanh => DTanh(x),
                CalcType.Sigmoid => DSigmoid(x),
                CalcType.ReLU => DReLU(x),
                CalcType.Id => DId(x),
                _ => 0,
            };
        }
        #endregion

    }
}
