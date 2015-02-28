using System;
using System.Collections.Generic;

namespace LabWork_3
{
    public class Solver
    {
        public double A { get; private set; }
        public double B { get; private set; }
        public double C { get; private set; }

        private static Func<double, double, double, double, double> _shturman0 = (x, a, b, c) => x * Math.Pow(a, 3) + x * Math.Pow(b, 2) + x * c;
        private static Func<double, double, double, double, double> _shturman1 = (x, a, b, c) => 3 * x * Math.Pow(b, 2) + 2 * a * x + b;
                
        private static Func<double, double, double> u =
            (a, b) => (double) 2*Math.Pow(a, 2)/(double) 9 - (double) 2*b/(double) 3;

        private static Func<double, double, double, double> v = (a, b, c) => a*b/(double) 9 - c;
        private static Func<double, double, double, double, double> _shturman2 = (x, a, b, c) => u(a, b)*x + v(a, b, c);

        private static Func<double, double, double, double, double> _shturman3 =
            (x, a, b, c) => v(a, b, c)*(2*a - 3*v(a, b, c)/u(a, b))/u(a, b) - b;

        public Solver(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Solver(Args args)
        {
            A = args.A;
            B = args.B;
            C = args.C;
        }

        public int Sturman(double a, double b)
        {
            //

            var rowA = new List<double>()
            {
                _shturman0(a, A, B, C),
                _shturman1(a, A, B, C),
                _shturman2(a, A, B, C),
                _shturman3(a, A, B, C)
            };
            var rowB = new List<double>()
            {
                _shturman0(b, A, B, C),
                _shturman1(b, A, B, C),
                _shturman2(b, A, B, C),
                _shturman3(b, A, B, C)
            };

            int Na = 0;
            int Nb = 0;
            for (int i = 0; i <rowA.Count-1; i++)
            {
                if (rowA[i]* rowA[i+1] < 0)
                {
                    Na++;
                }
                if(rowB[i] * rowB[i + 1] < 0)
                {
                    Nb++;
                }
            }

            return Na - Nb;
        }
    }
}