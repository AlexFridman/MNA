using System;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        private static double[] xValues = Enumerable.Range(0, 11).Select(i => ((double)i) / 10).ToArray();
        private static double[] yValues = new[] { 1, 1.41, 1.79, 2.13, 2.46, 2.76, 3.04, 3.3, 3.55, 3.79, 4.01 };
        const int size = 11;
        private static void Main()
        {
            
           


            Console.WriteLine(InterpolateLagrangePolynomial(47, xValues, yValues, size));
            Console.WriteLine(Newton(47, xValues, yValues, size));
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

        static double Newton(double x, double[] xValues, double[] yValues, int size)
        {
            double res = yValues[0], F, den;
            int i, j, k;
            for(i = 1; i < size; i++)
            {
                F = 0;
                for(j = 0; j <= i; j++)
                {//следующее слагаемое полинома
                    den = 1;
                    //считаем знаменатель разделенной разности
                    for(k = 0; k <= i; k++)
                    {
                        if(k != j)
                            den *= (xValues[j] - xValues[k]);
                    }
                    //считаем разделенную разность
                    F += yValues[j] / den;
                }
                //домножаем разделенную разность на скобки (x-x[0])...(x-x[i-1])
                for(k = 0; k < i; k++)
                    F *= (x - xValues[k]);
                res += F;//полином
            }
            return res;
        }
    }
}