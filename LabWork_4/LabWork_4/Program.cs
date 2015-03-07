#region

using System;
using System.Xml;

#endregion

namespace LabWork_4
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Итерационный метод:");
            var newton = new IterativeSolver();
            var res = newton.Solve(0.4, 0.7);
            Console.WriteLine(res);

            Console.WriteLine("Метод Ньютона:");
            var newton1 = new NewtonSolver();
            res = newton1.DefRoots(0.4, 0.7);
            Console.WriteLine(res);
        }
    }
}
