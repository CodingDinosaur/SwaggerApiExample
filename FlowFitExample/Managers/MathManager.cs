using System;

namespace FlowFitExample.Managers
{
    public class MathManager : IMathManager
    {
        public int Add(int a, int b) => a + b;

        public double SquareRoot(double a) => Math.Sqrt(a);

        public long Factorial(int a)
        {
            for (var i = a; i > 0; i--)
            {
                a *= i;
            }

            return a;
        }
    }
}
