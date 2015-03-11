#region

using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

#endregion

namespace LabWork_5
{
    public class JakobiMethod
    {
        private readonly Matrix<double> _aMatrix;
        public double Accuracy { get; set; }
        private Func<double, double, double, double> Pk = (Aij, Aii, Ajj) => 2*Aij/(Aii - Ajj);
        private Func<double, double> sin = Pk => Math.Sign(Pk)*Math.Sqrt(0.5*(1 - 1/Math.Sqrt(1 + Math.Pow(Pk, 2))));
        private Func<double, double> cos = Pk => Math.Sign(Pk) * Math.Sqrt(0.5 * (1 + 1 / Math.Sqrt(1 + Math.Pow(Pk, 2))));
        private DenseVector ownNumbers;
        public JakobiMethod(Matrix<double> aMatrix)
        {
            _aMatrix = aMatrix;
            Accuracy = 0.00000001;
            Find();
        }       

        public void Find()
        {
            int k = 0;
            var aK =  _aMatrix.Clone();
            
            while (k < 1000000)
            {
                var maxElement = FindMaxElement();
                if (Math.Abs(maxElement.Element) <= Accuracy)
                {
                    break;
                }

                var jacobiMatrix = BuildJacobyMatrix(maxElement);
                aK *= jacobiMatrix;
                aK *= jacobiMatrix.Transpose();
                k++;
            }

            ownNumbers = new DenseVector(aK.Diagonal().ToArray());
        }

        private MatrixElement FindMaxElement()
        {
            MatrixElement absMax = new MatrixElement {Element = _aMatrix[0, 1], I = 0, J = 1};

            for (int i = 0; i < _aMatrix.RowCount; i++)
            {
                for (int j = 0; j < _aMatrix.ColumnCount; j++)
                {
                    if (i < j && Math.Abs(_aMatrix[i, j]) > absMax.Element)
                    {
                        absMax.Element = _aMatrix[i, j];
                        absMax.I = i;
                        absMax.J = j;
                    }
                }
            }

            return new MatrixElement {Element = _aMatrix[absMax.I, absMax.J], I = absMax.I, J = absMax.J};
        }

        private Matrix<double> BuildJacobyMatrix(MatrixElement maxElement)
        {
            var pK = Pk(_aMatrix[maxElement.I, maxElement.J], _aMatrix[maxElement.I, maxElement.I],
                    _aMatrix[maxElement.J, maxElement.J]);
            var cosK = cos(pK);
            var sinK = sin(pK);

            var order = _aMatrix.ColumnCount-1;
            var jacobiMatrix = DenseMatrix.CreateIdentity(order+1);
            jacobiMatrix[0, 0] = cosK;
            jacobiMatrix[order, order] = cosK;
            jacobiMatrix[order, 0] = sinK;
            jacobiMatrix[0, order] = -sinK;

            return jacobiMatrix;
        }

        private struct MatrixElement
        {
            public double Element { get; set; }
            public int I { get; set; }
            public int J { get; set; }
        }
    }
}