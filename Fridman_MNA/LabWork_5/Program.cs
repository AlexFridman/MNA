#region

using System;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

#endregion

namespace LabWork_5
{
    internal class Program
    {
        private const int K = 28;
        private static Matrix<double> A;
        private static Vector<double> b;

        private static void Main(string[] args)
        {
            Initialize();
            Console.WriteLine(A);
            var jacobi = new JakobiMethod(new DenseMatrix(3,3,new double[]{5,1,2,1,4,1,2,1,3}));
            jacobi.Find();
        }

        private static void Initialize()
        {
            var matrixC = ReadMatrixFromFile("C.txt");
            var matrixD = ReadMatrixFromFile("D.txt");

            A = K*matrixC + matrixD;


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
            if (vectorStr == null)
                throw new InvalidOperationException();
            var vectorNum = vectorStr.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse);
            return DenseVector.OfEnumerable(vectorNum);
        }
    }
}