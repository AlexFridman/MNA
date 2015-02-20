using System.Linq;
using System.Xml;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using DenseVector = MathNet.Numerics.LinearAlgebra.Double.DenseVector;

namespace LabWork_2
{
    public class SeidelSolver : IterativeSolver
    {
        private Vector<double> _vectorC;
        private Matrix<double> _matrixB;

        public SeidelSolver(Matrix<double> matrixA, Vector<double> vectorB)
            : base(matrixA, vectorB)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            CalcVectorC();
            CalcMatrixB();
        }

        private void CalcVectorC()
        {
            _vectorC = _vectorB.Clone();
            for(int i = 0; i < _vectorC.Count; i++)
            {
                _vectorC[i] /= _matrixA[i, i];
            }
        }

        private void CalcMatrixB()
        {
            var eMatrix = DenseMatrix.Create(_matrixA.RowCount, _matrixA.RowCount, 0);
            double one = 1;
            eMatrix.SetDiagonal(Enumerable.Repeat(one, _matrixA.RowCount).ToArray());

            _matrixB = eMatrix - _matrixA;

            for(int i = 0; i < _matrixB.RowCount; i++)
            {
                for(int j = 0; j < _matrixB.ColumnCount; j++)
                {
                    _matrixB /= _matrixA[i, i];
                }
            }

            _matrixB.SetDiagonal(GetZeroVector(_matrixB.RowCount));
        }

        public override Vector<double> Solve()
        {
            var prev = new DenseVector(_matrixA.RowCount);
            var cur = new DenseVector(_matrixA.RowCount);
            do
            {
                prev = new DenseVector(cur.ToArray());
                for(int i = 0; i < _matrixB.RowCount; i++)
                {
                    double temp = 0;
                    for(int j = 0; j < i - 1; j++)
                    {
                        temp += _matrixB[i, j] * cur[j];
                    }

                    for(int j = i; j < _matrixB.RowCount; j++)
                    {
                        temp += _matrixB[i, j] * prev[j];
                    }

                    temp += _vectorC[i];
                    cur[i] = temp;
                }
            } while(!IsAccuracyReached(prev, cur, _accuracy));

            return cur;
        }
    }
}