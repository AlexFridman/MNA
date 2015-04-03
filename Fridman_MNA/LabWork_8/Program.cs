using System;
using System.IO;

namespace LabWork_8
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var function = new Func<double, double>(Math.Tan);
            var xMin = 1;
            var xMax = 3;
            int n = 10000000;
            var x0 = 2;
            var fDer = Differenciator.FirstDerivative(function, xMin, xMax, n, x0);
            Console.WriteLine(fDer);
            var sDer = Differenciator.SecondDerivative(function, xMin, xMax, n, x0);
            Console.WriteLine(sDer);
        }


    }
}
