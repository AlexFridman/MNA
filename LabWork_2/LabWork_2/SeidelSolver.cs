using System;
using System.Linq;
using System.Xml;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using DenseVector = MathNet.Numerics.LinearAlgebra.Double.DenseVector;

namespace LabWork_2
{
    public class SeidelSolver : IterativeSolver
    {

        public SeidelSolver(Matrix<double> matrixA, Vector<double> vectorB)
            : base(matrixA, vectorB)
        {
        }

        public override Vector<double> Solve()
        {
            DenseVector prev;
            var curr = new DenseVector(_matrixA.RowCount);
            do
            {
                prev = new DenseVector(curr.ToArray());
                for(int i = 0; i < _matrixA.RowCount; i++)
                {
                    double entry = _vectorB[i];
                    double diagonal = _matrixA[i, i];
                    if(Math.Abs(diagonal) < _accuracy)
                    {
                        throw new ArgumentException("Diagonal element is too small.");
                    }
                    for(int j = 0; j < i; j++)
                    {
                        entry -= _matrixA[i, j] * curr[j];
                    }

                    for(int j = i + 1; j < _matrixA.RowCount; j++)
                    {
                        entry -= _matrixA[i, j] * prev[j];
                    }

                    curr[i] = entry / diagonal;
                }
            } while(!IsAccuracyReached(curr, prev, _accuracy));

            return curr;
        }
    }
}