using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace LabWork_1
{
    public class SquareMatrix
    {
        private readonly double[][] _matrix;

        public int Rank { get; private set; }        

        public double this[int row, int cell]
        {
            get
            {
                if (!IsInRange(row, cell))
                {
                    throw new IndexOutOfRangeException();
                }

                return _matrix[row][cell];
            }
            set
            {
                if(!IsInRange(row, cell))
                {
                    throw new IndexOutOfRangeException();
                }

                _matrix[row][cell] = value;
            }
        }

        private bool IsInRange(int row, int cell)
        {
            return row >= 0 && row < Rank && cell >= 0 && cell < Rank;
        }
        public IReadOnlyCollection<IReadOnlyCollection<double>> Matrix
        {
            get
            {
                return new ReadOnlyCollection<IReadOnlyCollection<double>>(_matrix);
            }
        }
        
        public SquareMatrix(double[][] matrix)
        {
            if (!IsSquareMatrix(matrix))
            {
                throw new ArgumentException("Matrix has to be square.");
            }
            _matrix = matrix;
            Rank = _matrix.Length;
        }

        public SquareMatrix(int rank)
        {
            if (rank < 0)
            {
                throw new ArgumentException("Rank must be greather than zero.");
            }

            _matrix = new double[rank][];
            for (int i = 0; i < rank; i++)
            {
                _matrix[i] = new double[rank];
            }

            Rank = _matrix.Length;
        }

        private bool IsSquareMatrix(double[][] matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("matrix");
            }

            if (matrix.All(r => r.Length == matrix.Length))
            {
                return true;
            }

            return false;
        }

        public static SquareMatrix ReadFromFile(string path)
        {
            try
            {
                var lines = File.ReadLines(path).ToList();
                double[][] matrix = new double[lines.Count()][];
                int row = 0;
                lines.ForEach(l =>
                {
                    matrix[row] =
                        l.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
                    row++;
                });

                return new SquareMatrix(matrix);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("There was a mistake. Look on iner exception.", e);
            }
        }

        public static bool IsJointMatrix(SquareMatrix a, SquareMatrix b)
        {
            return a.Rank == b.Rank;
        }
        public static SquareMatrix operator +(SquareMatrix a, SquareMatrix b)
        {
            if (!IsJointMatrix(a,b))
            {
                throw new InvalidOperationException("Matrix are not joint");
            }

            var result = new SquareMatrix(a.Rank);
            for (int i = 0; i < a.Rank; i++)
            {
                for (int j = 0; j < a.Rank; j++)
                {
                    result[i, j] = a[i, j] + b[i, j];
                }
            }

            return result;
        }

        public static SquareMatrix operator -(SquareMatrix a, SquareMatrix b)
        {
            return a + (b*(-1));
        }

        public static SquareMatrix operator *(SquareMatrix a, double b)
        {
            var result = new SquareMatrix(a.Rank);
            for(int i = 0; i < a.Rank; i++)
            {
                for(int j = 0; j < a.Rank; j++)
                {
                    result[i, j] = b * a[i, j];
                }
            }

            return result;
        }

        public static SquareMatrix operator /(SquareMatrix a, double b)
        {
            return a*(1/b);
        }

        public void PrintMatrix()
        {
            var lines = new List<StringBuilder>(Rank);
            for (int i = 0; i < Rank; i++)
            {
                lines.Add(new StringBuilder());
            }

            for (int i = 0; i < Rank; i++)
            {
                var currentCell = _matrix.Select(l => l.Where((item, pos) => pos == i).FirstOrDefault()).Select(item => item.ToString(CultureInfo.InvariantCulture)).ToList();
                var maxLength = currentCell.Max(item => item.Length);

                var alignedItems = currentCell.Select(item => item + new string(' ', maxLength - item.Length) + "    ").ToList();

                for (int j = 0; j < Rank; j++)
                {
                    lines[j].Append(alignedItems[j]);
                }
            }
            

            lines.ForEach(Console.WriteLine);
        }

        public double[][] ToRaggedArray()
        {
            var result = new double[Rank][];
            for (int i = 0; i < Rank; i++)
            {
                result[i] = new double[Rank];
                _matrix[i].CopyTo(result[i], 0);
            }

            return result;
        }
    }
}