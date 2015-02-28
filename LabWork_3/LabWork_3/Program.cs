using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWork_3
{
    class Program
    {

        static void Main(string[] args)
        {
            //double a = -14.4621;
            //double b = 60.6959;

            //double c = -70.9238;
            double a = -2;

            double b = -9;

            double c = 18;
            Args myArgs = new Args {A = -99, B = -100, C = 0};

            //var solver = new Solver(a, b, c);
            var solver = new Solver(myArgs);
            var res = solver.Sturman(-10, 10);
            Console.WriteLine(res);
        }
    }
    public struct Args
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
    }
}
