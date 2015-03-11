using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWork_1
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecuteMainElementMethodSolver();
            Console.WriteLine("=======================================================");
            ExecuteGaussMethodSolver();
        } 

        private static void ExecuteMainElementMethodSolver()
        {
            Console.WriteLine("Метод Гаусса с выбором главного элемента");
            var C = SquareMatrix.ReadFromFile("C.txt");
            var D = SquareMatrix.ReadFromFile("D.txt");
            var b = ReadB("b.txt");
            var k = 28;

            Console.WriteLine("Матрица C");
            C.PrintMatrix();


            Console.WriteLine("Матрица D");
            D.PrintMatrix();

            Console.WriteLine("Вектор-столбец B");
            PrintVector(b);

            SquareMatrix A = C * k + D;
            Console.WriteLine("Матрица A");
            A.PrintMatrix();

            var solver = new MainElementMethodSolver(A.ToRaggedArray(), b);
            solver.Solve();

            Console.WriteLine("Вектор-столбец X");
            PrintVector(solver.LastSolveResult.ToArray());

            SaveToFile("result.txt", solver.LastSolveResult.AsEnumerable());
        }


        private static void ExecuteGaussMethodSolver()
        {
            Console.WriteLine("Метод Гаусса");
            var C = SquareMatrix.ReadFromFile("C.txt");
            var D = SquareMatrix.ReadFromFile("D.txt");
            var b = ReadB("b.txt");
            var k = 28;

            Console.WriteLine("Матрица C");
            C.PrintMatrix();


            Console.WriteLine("Матрица D");
            D.PrintMatrix();

            Console.WriteLine("Вектор-столбец B");
            PrintVector(b);

            SquareMatrix A = C * k + D;            
            Console.WriteLine("Матрица A");
            A.PrintMatrix();

            var solver = new GaussMethodSolver(A.ToRaggedArray(), b);
            solver.Solve();

            Console.WriteLine("Вектор-столбец X");
            PrintVector(solver.LastSolveResult.ToArray());

            SaveToFile("result.txt", solver.LastSolveResult.AsEnumerable());
        }


       
        #region Helpers 
        static void SaveToFile(string path, IEnumerable<double> vector)
        {
            using (var stream = File.CreateText(path))
            {
                foreach (var item in vector)
                {
                    stream.WriteLine(item);
                }
            }
        }
        static double[][] ReadA()
        {
            var symNumLines = File.ReadLines("A.txt").Select(s => s.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
            double[][] A = new double[symNumLines.Count()][];

            for(int i = 0; i < symNumLines.Count(); i++)
            {
                A[i] = symNumLines[i].Select(s => double.Parse(s)).ToArray();
            }

            return A;
        }

        static double[] ReadB(string path)
        {
            return File.ReadLines(path).Select(s => double.Parse(s)).ToArray();
        }

        static void PrintVector(double[] array)
        {
            array.ToList().ForEach(Console.WriteLine);
        }
        #endregion
    }
}
