using System;
using System.Collections.Generic;
using System.Linq;

namespace LabWork_1
{
    internal class GaussMethodSolver
    {
        private struct MatrixElement
        {
            public double AbsValue { get; set; }
            public int Cell { get; set; }
            public int Row { get; set; }

            public override string ToString()
            {
                return string.Format("Row: {0}, Cell: {1}", Row, Cell);
            }
        }

        private double[][] _sourceA;
        private double[] _sourceB;
        private double[][] _matrixA;
        private double[] _vectorB;
        private double[] _vectorX;
        private Stack<int> _nonSignificantVariables;

        private int _rowCount;
        private int _cellCount;

        public int doubles { get; set; }

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
            _nonSignificantVariables = new Stack<int>();
            doubles = 4;
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
                var maxElement = FindMaxElementOfCellPosition(i, currentCell);

                if (maxElement.AbsValue == 0)
                {
                    CurrentXIsEqualToAnyNumber(maxElement.Cell);
                    continue;
                }

                int numberOfRowWithMaxElement = maxElement.Row;

                SwapTwoRows(i, numberOfRowWithMaxElement);

                SubtractCurrentRowFromTheLower(i, currentCell);

                currentCell++;
            }
        }

        private MatrixElement FindMaxElementOfCellPosition(int minRow, int ñell)
        {
            var maxElement = new MatrixElement { Cell = ñell, Row = minRow, AbsValue = Math.Abs(_matrixA[minRow][ñell]) };
            
            for(int i = minRow; i < _rowCount; i++)
            {
                if(Math.Abs(_matrixA[i][ñell]) > maxElement.AbsValue)
                {
                    maxElement.AbsValue = Math.Abs(_matrixA[i][ñell]);
                    maxElement.Row = i;
                }
            }

            return maxElement;
        }

        private void CurrentXIsEqualToAnyNumber(int cell)
        {
            _vectorX[cell] = double.PositiveInfinity;
            _nonSignificantVariables.Push(cell);

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
                    _matrixA[i][j] = Math.Round(_matrixA[i][j] - Math.Round(_matrixA[row][j] * q, doubles),
                        doubles);
                }

                _vectorB[i] = Math.Round(_vectorB[i] - Math.Round(_vectorB[row] * q, doubles), doubles);
            }
        }

        private double FindQForTwoRows(int dividendRow, int row2, int cell)
        {
            double q = Math.Round(_matrixA[dividendRow][cell] / _matrixA[row2][cell], doubles);

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
                    if (_nonSignificantVariables.Count > 0 && _nonSignificantVariables.Peek() == j)
                    {
                        _nonSignificantVariables.Pop();
                        continue;                        
                    }
                    temp = Math.Round(temp - Math.Round(_matrixA[i][j] * _vectorX[j], doubles), doubles);
                }

                _vectorX[i] = Math.Round(temp / _matrixA[i][j], doubles);
            }
        }
    }
}