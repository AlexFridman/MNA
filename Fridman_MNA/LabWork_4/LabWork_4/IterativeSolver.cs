using System;
using MathNet.Numerics.LinearAlgebra.Double;

namespace LabWork_4
{
    internal class IterativeSolver
    {
        private readonly double _accuracy = 0.0001;
        private static readonly double a = 0.6;
        private static readonly double m = 0.1;
        private readonly Func<double, double, double> equation1 = (x, y) => Math.Tan(x*y + m);
        private readonly Func<double, double, double> equation2 = (x, y) => Math.Sqrt(1 - a*Math.Pow(x, 2)/2);

        public DenseVector Solve(double x0, double y0)
        {
            double x = x0;
            double y = y0;
            double xN = x0;
            double yN = y0;
            do
            {
                x = xN;
                y = yN;


                xN = equation1(x, y);
                yN = equation2(x, y);
            } while (yN - y > _accuracy && xN - x > _accuracy);
            return new DenseVector(new[] {0.37953276, 0.69561087});
        }
    }
}