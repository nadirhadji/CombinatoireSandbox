using System.Numerics;

namespace CombinatoireSandbox.Experimentation
{
    public class CatalanNumber
    {
        public static BigInteger NthCatalanNumber(int n)
        {
            var factorialN = Factorial(n, 1);
            var factorialNplus1 = factorialN * (n + 1);
            var factorialNtime2 = Factorial(n * 2, 1);

            return factorialNtime2 / (factorialNplus1 * factorialN);
        }

        public static BigInteger Factorial(int n, BigInteger startValue)
        {
            BigInteger result = startValue;
            for (BigInteger i = startValue; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
    }
}
