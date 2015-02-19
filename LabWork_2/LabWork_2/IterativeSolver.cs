using System;
using System.Linq;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace LabWork_2
{
    public class IterativeSolver
    {
        private readonly Matrix<double> _matrixA;
        private Matrix<double> _matrixB;
        private Vector<double> _vectorB;
        private Vector<double> _vectorC;

        private int _decimals;
        private double _accuracy;

        public int Decimals
        {
            get { return _decimals; }
            set {
                if (value < 0)
                {
                    throw new ArgumentException("Decimals must be equal or greather than zero.");
                }

                _decimals = value;
                _accuracy = 1/Math.Pow(10, value);
            }
        }

        public IterativeSolver(Matrix<double> matrixA, Vector<double> vectorB)
        {
            _matrixA = matrixA;
            _vectorB = vectorB;
            Initialize();
        }

        private void Initialize()
        {
            Decimals = 4;            
            CalcMatrixB();
            CalcVectorC();
        }

        private void CalcMatrixB()
        {
            var eMatrix = DenseMatrix.CreateDiagonal(_matrixA.RowCount, _matrixA.ColumnCount, 1);
            _matrixB = eMatrix - _matrixA;
            for (int i = 0; i < _matrixB.RowCount; i++)
            {
                for (int j = 0; j < _matrixB.ColumnCount; j++)
                {
                    _matrixB[i, j] /= _matrixA[i, i];
                }
            }
            const double zero = 0;
            _matrixB.SetDiagonal(Enumerable.Repeat(zero, _matrixB.RowCount).ToArray());            
        }

        private void CalcVectorC()
        {
            _vectorC = _vectorB / _matrixA.Diagonal();
        }

        public Vector<double> Solve()
        {
            var oldX = CalcNewXVector(_matrixB, _vectorC, _vectorC);
            var curX = CalcNewXVector(_matrixB, _vectorC, oldX);
            while (!IsAccuracyReached(oldX, curX, _accuracy))
            {
                oldX = curX;
                curX = CalcNewXVector(_matrixB, _vectorB, oldX);
            }

            return curX;
        }

        private Vector<double> CalcNewXVector(Matrix<double> B, Vector<double> C, Vector<double> oldX)
        {
            return B*oldX + C;
        }

        private bool IsAccuracyReached(Vector<double> arg1, Vector<double> arg2, double accuracy)
        {
            return (arg1 - arg2).Select(Math.Abs).Sum() < accuracy;
        }


    }
}