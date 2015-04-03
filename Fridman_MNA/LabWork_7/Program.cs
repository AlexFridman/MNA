// Интерполирование функций естественными кубическими сплайнами

#region

using System;
using System.Linq;

#endregion

namespace LabWork_7
{
    public static class Program
    {
        public static void Main()
        {
            Func<double, double> func = x => Math.Exp(-x);

            var xValues = Enumerable.Range(0, 500).Select(v => v/(double) 100).ToArray();
            var yValues = xValues.Select(v => func(v)).ToArray();
            var spline = new CubicSpline();
            spline.BuildSpline(xValues, yValues, 500);
            Console.WriteLine(spline.Interpolate(2.1, true));
            Console.WriteLine(func(2.1));
        }
    }
}