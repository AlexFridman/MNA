using System;
using System.Collections.Generic;
using System.Linq;

namespace LabWork_1
{
    public class MainElementMethodSolver : GaussMethodSolver
    {

        public MainElementMethodSolver(double[][] matrixA, double[] vectorB)
            : base(matrixA, vectorB)
        {
        }

        protected override void ForwardStroke()
        {
            int currentCell = 0;

            for(int i = 0; i < RowCount - 1; i++)
            {
                var elementPosition = FindMaxElementOfCellPosition(i, currentCell);


                int numberOfRowWithMaxElement = elementPosition.Row;
                if(IsElementEqualToZero(numberOfRowWithMaxElement, currentCell))
                {
                    IfNoSolution();
                    throw new InvalidOperationException("No Solution");
                }
                SwapTwoRows(i, numberOfRowWithMaxElement);

                SubtractCurrentRowFromTheLower(i, currentCell);

                currentCell++;
            }
        }
        private PositionInMatrix FindMaxElementOfCellPosition(int minRow, int ñell)
        {
            var position = new PositionInMatrix { Cell = ñell, Row = minRow };
            double currentMax = Math.Abs(MatrixA[position.Row][position.Cell]);

            for(int i = minRow; i < RowCount; i++)
            {
                if(Math.Abs(MatrixA[i][ñell]) > currentMax)
                {
                    currentMax = Math.Abs(MatrixA[i][ñell]);
                    position.Row = i;
                }
            }

            return position;
        }
    }
}