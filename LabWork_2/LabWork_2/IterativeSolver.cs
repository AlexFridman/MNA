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
        private readonly Vector<double> _vectorB;

        private int _decimals;
        private double _accuracy;

        public int Decimals
        {
            get { return _decimals; }
            set
            {
                if(value < 0)
                {
                    throw new ArgumentException("Decimals must be equal or greather than zero.");
                }

                _decimals = value;
                _accuracy = 1 / Math.Pow(10, value);
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
        }

        private Vector<double> GetZeroVector(int length)
        {
            const double zero = 0;
            return new DenseVector(Enumerable.Repeat(zero, _matrixA.RowCount).ToArray());
        }

        public Vector<double> Solve()
        {
            var oldX = GetZeroVector(_matrixA.RowCount);
            var curX = CalcNewXVector(_matrixA, _vectorB, oldX);
            while(!IsAccuracyReached(oldX, curX, _accuracy))
            {
                oldX = curX;
                curX = CalcNewXVector(_matrixA, _vectorB, oldX);
            }

            return curX;
        }

        private Vector<double> CalcNewXVector(Matrix<double> A, Vector<double> b, Vector<double> oldX)
        {
            var currentVariableValues = b.Clone();

            for(int i = 0; i < A.RowCount; i++)
            {

                for(int j = 0; j < A.RowCount; j++)
                {
                    if(i != j)
                    {
                        currentVariableValues[i] -= A[i, j] * oldX[j];
                    }
                }
            }
            currentVariableValues /= A.Diagonal();

            return currentVariableValues;
        }

        private bool IsAccuracyReached(Vector<double> arg1, Vector<double> arg2, double accuracy)
        {
            return (arg1 - arg2).Select(Math.Abs).Sum() < accuracy;
        }


    }
}