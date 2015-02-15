﻿using System;
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
            Execute();
        }

        private static void GetValue()
        {
            var C = SquareMatrix.ReadFromFile("C.txt");
            var D = SquareMatrix.ReadFromFile("D.txt");
            var dict = new Dictionary<int, string> {{0, "a"}, {1, "b"}, {2, "c"}, {3, "d"}, {4, "f"}};
            SquareMatrix A = C*28 + D;
            var b = ReadB("b.txt");
            var result = new StringBuilder("solve ");
            for (int i = 0; i < A.Rank; i++)
            {
                for (int j = 0; j < A.Rank; j++)
                {
                    if (A[i, j] >= 0)
                    {
                        result.AppendFormat("+{0}*{1} ", A[i, j].ToString(new CultureInfo("en-US")).Replace(",", "."), dict[j]);
                    }
                    else
                    {
                        result.AppendFormat("{0}*{1} ", A[i, j].ToString(new CultureInfo("en-US")).Replace(",", "."), dict[j]);
                    }
                }
                result.AppendFormat(" = {0},", b[i].ToString(new CultureInfo("en-US")).Replace(",", "."));
            }
            result.Replace(" ", "");
            //result.Replace("*", "");
        }

        private static void Execute()
        {
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

            SquareMatrix A = C*k + D;
            Console.WriteLine("Матрица A");
            A.PrintMatrix();

            var solver = new GaussMethodSolver(A.ToRaggedArray(), b);
            solver.Solve();

            Console.WriteLine("Вектор-столбец X");
            PrintVector(solver.LastSolveResult.ToArray());

            SaveToFile("result.txt", solver.LastSolveResult.AsEnumerable());
        }

        static void SaveToFile(string path, IEnumerable<decimal> vector)
        {
            using (var stream = File.CreateText(path))
            {
                foreach (var item in vector)
                {
                    stream.WriteLine(item);
                }
            }
        }

        static decimal[][] ReadA()
        {
            var symNumLines = File.ReadLines("A.txt").Select(s => s.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
            decimal[][] A = new decimal[symNumLines.Count()][];

            for(int i = 0; i < symNumLines.Count(); i++)
            {
                A[i] = symNumLines[i].Select(s => decimal.Parse(s)).ToArray();
            }

            return A;
        }

        static decimal[] ReadB(string path)
        {
            return File.ReadLines(path).Select(s => decimal.Parse(s)).ToArray();
        }

        static void PrintVector(decimal[] array)
        {
            array.ToList().ForEach(Console.WriteLine);
        }
    }
}
