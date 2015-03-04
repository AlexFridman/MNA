#region

using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

#endregion

namespace LabWork_4
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var newton = new IterativeSolver();
            var res = newton.Solve(0.4, 0.7);
            Console.WriteLine(res);

             var newton1 = new NewtonSolver();
             res = newton1.DefRoots(0.4, 0.7);
            Console.WriteLine(res);
        }
    }

    internal class IterativeSolver
    {
        private readonly double _accuracy = 0.0001;
        private static readonly double a = 0.6;
        private static readonly double m = 0.1;
        private readonly Func<double, double, double> equation1 = (x, y) => Math.Tan(x * y + m);
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
            } while(yN - y > _accuracy && xN - x > _accuracy);

            return new DenseVector(new[] { xN, yN });
        }
    }
    internal class NewtonSolver
    {
        private readonly double _accuracy = 0.0001;
        private static readonly double a = 0.6;
        private static readonly double m = 0.1;
        private readonly Func<double, double, double> equation1 = (x, y) => Math.Tan(x*y + m) - x;
        private readonly Func<double, double, double> equation2 = (x, y) => a*Math.Pow(x, 2) + 2*Math.Pow(y, 2) - 1;
        private readonly Func<double, double, double> equationDer1X = (x, y) => y/Math.Pow(Math.Cos(x*y + m), 2) - 1;
        private readonly Func<double, double, double> equationDer2X = (x, y) => 2*a*x;
        private readonly Func<double, double, double> equationDer1Y = (x, y) => x/Math.Pow(Math.Cos(x*y + m), 2);
        private readonly Func<double, double, double> equationDer2Y = (x, y) => 4*y;


        private Vector<double> LinearSystem(double x, double y)
        {
            var A = Matrix<double>.Build.DenseOfArray(new[,]
            {
                {equationDer1X(x, y), equationDer1Y(x, y)},
                {equationDer2X(x, y), equationDer2Y(x, y)}
            });
            var b = Vector<double>.Build.Dense(new[] {-equation1(x, y), -equation2(x, y)});
            return A.Solve(b);
        }

        public DenseVector DefRoots(double x0, double y0)
        {
            double x = x0;
            double y = y0;
            double xN = x0;
            double yN = y0;
            do
            {
                x = xN;
                y = yN;
                var linearSystemSolution = LinearSystem(x, y);

                xN = x + linearSystemSolution[0];
                yN = y + linearSystemSolution[1];
            } while (yN - y > _accuracy && xN - x > _accuracy);

            return new DenseVector(new[] {xN, yN});
        }
    }
}
