using System;
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

            //IterativeSolver solver = new IterativeSolver(A, b);
            //Console.WriteLine(solver.Solve());
            //SaveMatrixToFile(A, "A.txt");

            IterativeSolver solver = new SeidelSolver(A, b);
            Console.WriteLine(solver.Solve());
            SaveMatrixToFile(A, "A.txt");

        }

        static void Initialize()
        {
            var matrixC = ReadMatrixFromFile("C.txt");
            var matrixD = ReadMatrixFromFile("D.txt");
            
            A = K * matrixC + matrixD;
            

            b = ReadVectorFromFile("b.txt");
        }

        private static DenseMatrix ReadMatrixFromFile(string filePath)
        {
            var matrixLines = File.ReadLines(filePath);
            var matrixNumLines =
                matrixLines.Select(
                    l => l.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse));
            var matrixC = DenseMatrix.OfRows(matrixNumLines);
            return matrixC;
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
