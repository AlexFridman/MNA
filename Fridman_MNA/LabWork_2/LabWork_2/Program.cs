﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace LabWork_2
{
    class Program
    {
        private static Matrix<double> A;
        private static Vector<double> b;
        private const int K = 28;
        static void Main(string[] args)
        {
            Initialize();

            ExecuteIterative();

            ExecuteSeidel();
        }

        private static void ExecuteIterative()
        {
            Console.WriteLine("Решение методом простых итераций");
            IterativeSolver solver = new IterativeSolver(A.Clone(), b.Clone());
            Console.WriteLine(solver.Solve());
        }

        private static void ExecuteSeidel()
        {
            Console.WriteLine("Решение методом Зейделя");
            IterativeSolver solver = new SeidelSolver(A.Clone(), b.Clone());
            Console.WriteLine(solver.Solve());            
        }

        static void Initialize()
        {
            var matrixC = ReadMatrixFromFile("C.txt");
            var matrixD = ReadMatrixFromFile("D.txt");
            
            A = K * matrixC + matrixD;
            

            b = ReadVectorFromFile("b.txt");

            Console.WriteLine("Матрица A");
            Console.WriteLine(A);
            Console.WriteLine("=================================================");
            Console.WriteLine("Вектор Б");
            Console.WriteLine(b);
        }

        private static DenseMatrix ReadMatrixFromFile(string filePath)
        {
            var matrixLines = File.ReadLines(filePath);
            var matrixNumLines =
                matrixLines.Select(
                    l => l.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse));
            var matrix = DenseMatrix.OfRows(matrixNumLines);
            return matrix;
        }

        private static DenseVector ReadVectorFromFile(string filePath)
        {
            var vectorStr = File.ReadLines(filePath).FirstOrDefault();
            if (vectorStr == null) throw new InvalidOperationException();
            var vectorNum = vectorStr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse);
            return DenseVector.OfEnumerable(vectorNum);
        }

        private static void SaveMatrixToFile(Matrix<double> matrix, string path)
        {
            using(var stream = File.CreateText(path))
            {
                foreach (double[] row in matrix.ToRowArrays())
                {
                    StringBuilder rowString = new StringBuilder();
                    foreach (double item in row)
                    {
                        rowString.AppendFormat("{0} ", item.ToString(new CultureInfo("en-US")));
                    }
                    stream.WriteLine(rowString);
                }
            }
        }
    }
}