using System;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        private static void Main()
        {
            const int size = 11;
            var xValues = Enumerable.Range(0, 11).Select(i => ((double)i) / 10).ToArray();
            var yValues = new[] { 1, 1.41, 1.79, 2.13, 2.46, 2.76, 3.04, 3.3, 3.55, 3.79, 4.01 };


            Console.WriteLine(InterpolateLagrangePolynomial(0.47, xValues, yValues, size));
        }

        static double InterpolateLagrangePolynomial(double x, double[] xValues, double[] yValues, int size)
        {
            double lagrangePol = 0;

            for(int i = 0; i < size; i++)
            {
                double basicsPol = 1;
                for(int j = 0; j < size; j++)
                {
                    if(j != i)
                    {
                        basicsPol *= (x - xValues[j]) / (xValues[i] - xValues[j]);
                    }
                }
                lagrangePol += basicsPol * yValues[i];
            }

            return lagrangePol;
        }

        static double TestF(double x)
        {
            return x * x * x + 3 * x * x + 3 * x + 1; // for example
        }
    }
}