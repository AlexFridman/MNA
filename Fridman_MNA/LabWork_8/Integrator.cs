using System;
using System.Collections.Generic;
using System.Linq;

internal class Integrator
{
    public static double RectangleMethod(Func<double, double> func, double xMin, double xMax, int n)
    {
        var X = GetX(xMin, xMax, n).ToArray();
        var Y = GetY(func, X).ToArray();
        var h = (xMax - xMin)/(double) n;
        return (Y.Skip(1).Sum() - Y.Last() + (Y.First() + Y.Last())/(double) 2)*h;
    }

    public static double TrapeziumMethod(Func<double, double> func, double xMin, double xMax, int n)
    {
        var X = GetX(xMin, xMax, n).ToArray();
        var Y = GetY(func, X).ToArray();
        var h = (xMax - xMin)/(double) n;
        double result = 0;
        for (int i = 0; i < n; i++)
        {
            result += (Y[i] + Y[i + 1]) / (double)2 * h;
        }

        return result;
    }

    private static IEnumerable<double> GetX(double xMin, double xMax, int n)
    {
        var h = (xMax - xMin)/(double) n;
        var result = new double[n + 1];
        result[0] = xMin;

        for (var i = 1; i <= n; i++)
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