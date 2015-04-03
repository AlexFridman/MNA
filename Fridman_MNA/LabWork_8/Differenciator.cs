using System;
using System.Collections.Generic;
using System.Linq;

namespace LabWork_8
{
    internal class Differenciator
    {
        public static double FirstDerivative(Func<double, double> func, double xMin, double xMax, int n, double x0)
        {
            var X = GetX(xMin, xMax, n);
            var Y = GetY(func, X).ToArray();
            var h = (xMax - xMin) / (double)n;
            var k = X.Select((el, i) => new {number = i, element = el}).Last(el => el.element < x0).number;

            return (Y[k + 1] - Y[k - 1])/
                   (2*h);
        }

        public static double SecondDerivative(Func<double, double> func, double xMin, double xMax, int n, double x0)
        {
            var X = GetX(xMin, xMax, n);
            var Y = GetY(func, X).ToArray();
            var h = (xMax - xMin) / (double)n;
            var k = X.Select((el, i) => new { number = i, element = el }).Last(el => el.element < x0).number;

            return (Y[k + 1] - 2*Y[k] + Y[k - 1])/
                   (h*h);
        }

        private static IEnumerable<double> GetX(double xMin, double xMax, int n)
        {
            var h = (xMax - xMin)/(double)n;
            var result = new double[n];
            result[0] = xMin;

            for (var i = 1; i < n; i++)
            {
                result[i] = xMin + i*h;
            }

            return result;
        }

        public static IEnumerable<double> GetY(Func<double, double> func, IEnumerable<double> X)
        {
            var result = new double[X.Count()];
            var i = 0;
            foreach (var x in X)
            {
                result[i] = func(x);
                i++;
            }

            return result;
        }
    }
}