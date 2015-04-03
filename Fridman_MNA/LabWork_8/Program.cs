using System;
using System.IO;

namespace LabWork_8
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var function = new Func<double, double>(Math.Log);
            var xMin = 1;
            var xMax = 3;
            int n = 100000;
            var x0 = 2;
            Console.WriteLine("Первая производная");
            var fDer = Differenciator.FirstDerivative(function, xMin, xMax, n, x0);
            Console.WriteLine(fDer);
            Console.WriteLine("=================================");
            Console.WriteLine("Вторая производная");
            var sDer = Differenciator.SecondDerivative(function, xMin, xMax, n, x0);
            Console.WriteLine(sDer);
            Console.WriteLine("=================================");
            Console.WriteLine("Метод трапеций");
            Console.WriteLine(Integrator.TrapeziumMethod(function, xMin, xMax, n));
            Console.WriteLine("=================================");
            Console.WriteLine("Метод прямоугольников");
            Console.WriteLine(Integrator.RectangleMethod(function, xMin, xMax, n));
        }


    }
}