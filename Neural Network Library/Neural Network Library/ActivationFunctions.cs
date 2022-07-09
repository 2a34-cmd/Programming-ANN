namespace Atomic.ArtificialNeuralNetwork.libraries
{
    /// <summary>
    /// the porpuse of this class is to add all kind of activation functions used in neural networks. 
    /// Currently, there are 4. 
    /// </summary>
    class ActivationFunctions
    {
        /// <summary>
        /// y here is dummy varible that is used as memory allocation
        /// </summary>
        static decimal y;
        #region AFunction
        /// <summary>
        /// Tanh is activation function used a lot as alternative for sigmoid with possibility for negative values 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static decimal Tanh(decimal x)
        {
            y = DecimalMath.Tanh(x);
            return y;
        }
        /// <summary>
        /// Sigmoid is function used in  lots of situations because of smothness, non negativity,
        /// and being between 0 and 1 all the time
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static decimal Sigmoid(decimal x)
        {
            y = 1/(1+ DecimalMath.Exp(-x));
            return y;
        }
        /// <summary>
        /// ReLU is function returns 0 when negative and its input value when positive 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static decimal ReLU(decimal x)
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
        /// <summary>
        /// used in calculating the network
        /// </summary>
        /// <param name="x">the iput to the function</param>
        /// <param name="type">parameter for choosing the function needed</param>
        /// <returns>outputs decimal corresponding to the type of activation function and 0 if there's no type</returns>
        internal static decimal Activation(decimal x, CalcType type)
        {
            return type switch
            {
                CalcType.Tanh => Tanh(x),
                CalcType.Sigmoid => Sigmoid(x),
                CalcType.ReLU => ReLU(x),
                CalcType.Id => x,
                _ => 0,
            };
        }
        #endregion
        #region Derivetives
        /// <summary>
        /// d tanh(x)/dx 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static decimal DTanh(decimal x)
        {
            y = DecimalMath.PowerN(1 - Tanh(x),2);
            return y;
        }
        /// <summary>
        /// d σ(x)/dx
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static decimal DSigmoid(decimal x)
        {
            y = DecimalMath.Exp(-x) /DecimalMath.PowerN(DecimalMath.Exp(-x)+1,2);
            return y;
        }
        /// <summary>
        /// d ReLU(x)/dx
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static decimal DReLU(decimal x)
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
        /// <summary>
        /// takes two inputs x and type to return the value of corresponding function
        /// </summary>
        /// <param name="x"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static decimal DActivation(decimal x,CalcType type)
        {
            return type switch
            {
                CalcType.Tanh => DTanh(x),
                CalcType.Sigmoid => DSigmoid(x),
                CalcType.ReLU => DReLU(x),
                CalcType.Id => 1,
                _ => 0,
            };
        }
        #endregion

    }
}
