using System;
using System.Collections.Generic;
using System.Linq;

namespace LabWork_1
{
    internal class GaussMethodSolver
    {
        private double[][] _sourceA;
        private double[] _sourceB;
        private double[][] _matrixA;
        private double[] _vectorB;
        private double[] _vectorX;

        private int _rowCount;
        private int _cellCount;
        private int _decimals;

        public int Decimals
        {
            get { return _decimals; }
            set
            {
                if(_decimals < 0)
                {
                    throw new ArgumentException("Decimal must be greather or equal to zero.", "value");
                }
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
            _matrixA = matrixA;
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
            _rowCount = _matrixA.Length;
            _cellCount = _matrixA[0].Length;
            _vectorX = new double[_cellCount];
            Decimals = 4;
        }

        private struct PositionInMatrix
        {
            public int Cell { get; set; }
            public int Row { get; set; }

            public override string ToString()
            {
                return string.Format("Row: {0}, Cell: {1}", Row, Cell);
            }
        }

        public void Solve()
        {
            ForwardStroke();
            RevereseStroke();
            _sourceA.CopyTo(_matrixA, 0);
            _sourceB.CopyTo(_vectorB, 0);
        }

        private void ForwardStroke()
        {
            int currentCell = 0;

            for(int i = 0; i < _rowCount - 1; i++)
            {
                var elementPosition = FindMaxElementOfCellPosition(i, currentCell);


                int numberOfRowWithMaxElement = elementPosition.Row;

                SwapTwoRows(i, numberOfRowWithMaxElement);

                SubtractCurrentRowFromTheLower(i, currentCell);

                currentCell++;
            }
        }

        private PositionInMatrix FindMaxElementOfCellPosition(int minRow, int �ell)
        {
            var position = new PositionInMatrix { Cell = �ell, Row = minRow };
            double currentMax = Math.Abs(_matrixA[position.Row][position.Cell]);

            for(int i = minRow; i < _rowCount; i++)
            {
                if(Math.Abs(_matrixA[i][�ell]) > currentMax)
                {
                    currentMax = Math.Abs(_matrixA[i][�ell]);
                    position.Row = i;
                }
            }

            return position;
        }

        private void SwapTwoRows(int row1, int row2)
        {
            if(row1 == row2)
            {
                return;
            }

            double[] bufferArray = _matrixA[row1];
            _matrixA[row1] = _matrixA[row2];
            _matrixA[row2] = bufferArray;

            double buffer = _vectorB[row1];
            _vectorB[row1] = _vectorB[row2];
            _vectorB[row2] = buffer;
        }

        private void SubtractCurrentRowFromTheLower(int row, int cell)
        {
            for(int i = row + 1; i < _rowCount; i++)
            {
                var q = FindQForTwoRows(i, row, cell);

                for(int j = cell; j < _cellCount; j++)
                {
                    _matrixA[i][j] = Math.Round(_matrixA[i][j] - Math.Round(_matrixA[row][j] * q, Decimals),
                        Decimals);
                }

                _vectorB[i] = Math.Round(_vectorB[i] - Math.Round(_vectorB[row] * q, Decimals), Decimals);
            }
        }

        private double FindQForTwoRows(int dividendRow, int row2, int cell)
        {
            double q = Math.Round(_matrixA[dividendRow][cell] / _matrixA[row2][cell], Decimals);

            return q;
        }

        private void RevereseStroke()
        {
            for(int i = _rowCount - 1; i >= 0; i--)
            {
                double temp = _vectorB[i];

                int j = _cellCount - 1;
                for(; j > i; j--)
                {
                    temp = Math.Round(temp - Math.Round(_matrixA[i][j] * _vectorX[j], Decimals), Decimals);
                }

                _vectorX[i] = Math.Round(temp / _matrixA[i][j], Decimals);
            }
        }
    }
}