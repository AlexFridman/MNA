using System;
using System.Collections.Generic;
using System.Linq;

namespace LabWork_1
{
    public class GaussMethodSolver
    {
        private readonly double[][] _sourceA;
        private readonly double[] _sourceB;
        protected readonly double[][] MatrixA;
        private readonly double[] _vectorB;
        private double[] _vectorX;

        protected int RowCount { get; set; }
        protected int CellCount { get; set; }

        private int _decimals;
        private double _tolerance;
        public int Decimals
        {
            get { return _decimals; }
            set
            {
                if(value < 0)
                {
                    throw new ArgumentException("Decimal must be greather or equal to zero.", "value");
                }

                _decimals = value;
                _tolerance = 1 / Math.Pow(10, _decimals);
            }
        }

        public IReadOnlyCollection<double> LastSolveResult
        {
            get { return _vectorX; }
        }

        public GaussMethodSolver(double[][] matrixA, double[] vectorB)
        {
            if(!IsSquareMatrix(matrixA))
            {
                throw new ArgumentException("Matrix A has to be square.");
            }
            if(matrixA.Length != vectorB.Length)
            {
                throw new ArgumentException("Rank matrix A must be equal to vector B length.");
            }
            _sourceA = matrixA;
            MatrixA = matrixA;
            _sourceB = vectorB;
            _vectorB = vectorB;
            Initialise();
        }

        private bool IsSquareMatrix(double[][] matrix)
        {
            if(matrix == null)
            {
                throw new ArgumentNullException("matrix");
            }

            if(matrix.All(r => r.Length == matrix.Length))
            {
                return true;
            }

            return false;
        }

        private void Initialise()
        {
            RowCount = MatrixA.Length;
            CellCount = MatrixA[0].Length;
            _vectorX = new double[CellCount];
            Decimals = 4;
        }

        protected struct PositionInMatrix
        {
            public int Cell { get; set; }
            public int Row { get; set; }

            public override string ToString()
            {
                return string.Format("Row: {0}, Cell: {1}", Row, Cell);
            }
        }
        public bool TrySolve()
        {
            try
            {
                Solve();
            }
            catch(InvalidOperationException)
            {
                return false;
            }

            return true;
        }
        public void Solve()
        {
            ForwardStroke();
            RevereseStroke();
            _sourceA.CopyTo(MatrixA, 0);
            _sourceB.CopyTo(_vectorB, 0);
        }


        protected virtual void ForwardStroke()
        {
            int currentCell = 0;

            for(int i = 0; i < RowCount - 1; i++)
            {
                if(IsElementEqualToZero(i, currentCell))
                {
                    int rowToSwapNum = i;
                    while(IsElementEqualToZero(rowToSwapNum, currentCell) && rowToSwapNum < RowCount)
                    {
                        rowToSwapNum++;
                    }

                    if(rowToSwapNum == RowCount)
                    {
                        IfNoSolution();
                        throw new InvalidOperationException("No Solution");
                    }

                    SwapTwoRows(i, rowToSwapNum);
                }

                SubtractCurrentRowFromTheLower(i, currentCell);

                currentCell++;
            }
        }

        protected bool IsElementEqualToZero(int row, int cell)
        {
            return Math.Abs(MatrixA[row][cell]) < _tolerance;
        }

        protected void IfNoSolution()
        {
            for(int i = 0; i < _vectorX.Length; i++)
            {
                _vectorX[i] = double.NaN;
            }
        }

        protected void SwapTwoRows(int row1, int row2)
        {
            if(row1 == row2)
            {
                return;
            }

            double[] bufferArray = MatrixA[row1];
            MatrixA[row1] = MatrixA[row2];
            MatrixA[row2] = bufferArray;

            double buffer = _vectorB[row1];
            _vectorB[row1] = _vectorB[row2];
            _vectorB[row2] = buffer;
        }

        protected void SubtractCurrentRowFromTheLower(int row, int cell)
        {
            for(int i = row + 1; i < RowCount; i++)
            {
                var q = FindQForTwoRows(i, row, cell);

                for(int j = cell; j < CellCount; j++)
                {
                    MatrixA[i][j] = Math.Round(MatrixA[i][j] - Math.Round(MatrixA[row][j] * q, Decimals),
                        Decimals);
                }

                _vectorB[i] = Math.Round(_vectorB[i] - Math.Round(_vectorB[row] * q, Decimals), Decimals);
            }
        }

        private double FindQForTwoRows(int dividendRow, int row2, int cell)
        {
            double q = Math.Round(MatrixA[dividendRow][cell] / MatrixA[row2][cell], Decimals);

            return q;
        }

        private void RevereseStroke()
        {
            for(int i = RowCount - 1; i >= 0; i--)
            {
                double temp = _vectorB[i];

                int j = CellCount - 1;
                for(; j > i; j--)
                {
                    temp = Math.Round(temp - Math.Round(MatrixA[i][j] * _vectorX[j], Decimals), Decimals);
                }

                _vectorX[i] = Math.Round(temp / MatrixA[i][j], Decimals);
            }
        }
    }
}