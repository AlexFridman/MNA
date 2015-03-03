using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWork_3
{
    class Program
    {        
        private static double a = -19.7997;
        static double b = 101.563;
        static double c = 562.833;
        private const double Accuracy = 0.0001;
        private static Func<double, double> func = (x) => Math.Pow(x, 3) + a * Math.Pow(x, 2) + b * x + c;
        private static Func<double, double> derivativ = (x) => 3 * Math.Pow(x, 2) + 2 * a * x + b;
        private static int _dichotomyIterations = 0;
        private static int _chordIterations = 0;
        private static int _newtonIterations = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Кол-во корней по методу Штурма");
            Solver solver = new Solver(a,b,c);
            Console.WriteLine(solver.Sturman(-10, 10));
            Console.WriteLine("=========================");
            Console.WriteLine("Метод Дихотомии");
           Console.WriteLine(Dichotomy(-10, 10));
            Console.WriteLine("Кол-во итераций {0}",_dichotomyIterations);
            Console.WriteLine("=========================");
            Console.WriteLine("Метод Хорд");
            Console.WriteLine(ChordMethod(-10, 10));
            Console.WriteLine("Кол-во итераций {0}", _chordIterations);
            Console.WriteLine("=========================");
            Console.WriteLine("Метод Ньютона");
            Console.WriteLine(NewtonMethod(-10, 10));
            Console.WriteLine("Кол-во итераций {0}", _newtonIterations);
            Console.WriteLine("=========================");
        }

        private static double Dichotomy(double left, double right)
        {
            double res = 0;
            while(right - left > Accuracy)
            {
                _dichotomyIterations++;
                res = (left + right) / 2;
                if(func(right) * func(res) < 0)
                    left = res;
                else
                    right = res;
            }

            return res;
        }
        private static double ChordMethod(double a, double b)
        {
            while(Math.Abs(b - a) > Accuracy)
            {
                _chordIterations++;
                a = b - (b - a) * func(b) / (func(b) - func(a));
                b = a - (a - b) * func(a) / (func(a) - func(b));
            }

            return b;
        }


        public static double NewtonMethod(double a, double b)
        {
            double x = (a + b) / 2;
            double y = func(x);

            while(Math.Abs(y) > Accuracy)
            {
                x -= y / derivativ(x);
                y = func(x);

                _newtonIterations++;
            }
            return x;
        }
    }
}
