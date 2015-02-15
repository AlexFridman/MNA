using System;
using System.Collections.Generic;
using System.Linq;

namespace LabWork_1
{
    class GaussMethodSolver
    {
        private decimal[][] _sourceA;
        private decimal[] _sourceB;
        private decimal[][] _matrixA;
        private decimal[] _vectorB;
        private decimal[] _vectorX;
        public int Decimals { get; set; }

        public IReadOnlyCollection<decimal> LastSolveResult
        {
            get { return _vectorX; }
        }

        private int _rowCount;
        private int _cellCount;
        private struct PositionInMatrix
        {
            public int Cell { get; set; }
            public int Row { get; set; }

            public override string ToString()
            {
                return string.Format("Row: {0}, Cell: {1}", Row, Cell);
            }
        }

        public GaussMethodSolver(decimal[][] matrixA, decimal[] vectorB)
        {
            _sourceA = matrixA;
            _matrixA = matrixA;
            _sourceB = vectorB;
            _vectorB = vectorB;
            Initialise();
        }

        private void Initialise()
        {
            _rowCount = _matrixA.Length;
            _cellCount = _matrixA[0].Length;
            _vectorX = new decimal[_cellCount];
            Decimals = 4;
        }

        private PositionInMatrix FindMaxElementOfCellPosition(int minRow, int ñell)
        {
            var position = new PositionInMatrix { Cell = ñell, Row = minRow };
            decimal currentMax = Math.Abs(_matrixA[position.Row][position.Cell]);

            for(int i = minRow; i < _rowCount; i++)
            {
                if(Math.Abs(_matrixA[i][ñell]) > currentMax)
                {
                    currentMax = Math.Abs(_matrixA[i][ñell]);
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

            decimal[] bufferArray = _matrixA[row1];
            _matrixA[row1] = _matrixA[row2];
            _matrixA[row2] = bufferArray;

            decimal buffer = _vectorB[row1];
            _vectorB[row1] = _vectorB[row2];
            _vectorB[row2] = buffer;
        }

        private decimal FindQForTwoRows(int dividendRow, int row2, int cell)
        {
            decimal q = decimal.Round(_matrixA[dividendRow][cell] / _matrixA[row2][cell], Decimals);

            return q;
        }

        private void SubtractCurrentRowFromTheLower(int row, int cell)
        {
            for(int i = row + 1; i < _rowCount; i++)
            {
                var q = FindQForTwoRows(i, row, cell);

                for(int j = cell; j < _cellCount; j++)
                {
                    _matrixA[i][j] = decimal.Round(_matrixA[i][j] - decimal.Round(_matrixA[row][j] * q, Decimals), Decimals);
                }

                _vectorB[i] = decimal.Round(_vectorB[i] - decimal.Round(_vectorB[row] * q, Decimals), Decimals);
            }
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

        private void RevereseStroke()
        {
            for(int i = _rowCount - 1; i >= 0; i--)
            {
                decimal temp = _vectorB[i];

                int j = _cellCount - 1;
                for(; j > i; j--)
                {
                    temp = decimal.Round(temp - decimal.Round(_matrixA[i][j] * _vectorX[j], Decimals), Decimals);
                }

                _vectorX[i] = decimal.Round(temp / _matrixA[i][j], Decimals);
            }
        }

        public void Solve()
        {
            ForwardStroke();
            RevereseStroke();
            _sourceA.CopyTo(_matrixA, 0);
            _sourceB.CopyTo(_vectorB,0);
        }
    }
}