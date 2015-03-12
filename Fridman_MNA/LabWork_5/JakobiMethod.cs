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
        private Func<double, double> cos = Pk => Math.Sqrt(0.5 * (1 + 1 / Math.Sqrt(1 + Math.Pow(Pk, 2))));
        private Vector<double> _ownNumbers;
        private Matrix<double> _rotateMatrixMul;
        public Vector<double> OwnNumbers { get { return _ownNumbers.Clone(); } set { _ownNumbers = value; } }
        public Matrix<double> OwnVectors { get { return _rotateMatrixMul.Clone(); } set { _rotateMatrixMul = value; } }
        public JakobiMethod(Matrix<double> aMatrix)
        {
            _aMatrix = aMatrix;
            Accuracy = 0.0001;
            _rotateMatrixMul = DenseMatrix.CreateIdentity(_aMatrix.ColumnCount);
            Find();            
        }       

        public void Find()
        {
            int k = 0;
            var aK =  _aMatrix.Clone();
            
            while (k < 1000)
            {
                var maxElement = FindMaxElement(aK);
                
                if (Math.Abs(maxElement.Element) <= Accuracy)
                {
                    break;
                }                
                
                var jacobiMatrix = BuildJacobyMatrix(maxElement);
                _rotateMatrixMul *= jacobiMatrix.Transpose();
                aK = jacobiMatrix.Transpose()*aK*jacobiMatrix;                
                k++;


            }

            _ownNumbers = new DenseVector(aK.Diagonal().ToArray());
        }

        private MatrixElement FindMaxElement(Matrix<double> matrix)
        {
            MatrixElement absMax = new MatrixElement { Element = matrix[0, 1], I = 0, J = 1 };

            for(int i = 0; i < matrix.RowCount; i++)
            {
                for(int j = 0; j < matrix.ColumnCount; j++)
                {
                    if(i < j && Math.Abs(matrix[i, j]) > absMax.Element)
                    {
                        absMax.Element = matrix[i, j];
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
            jacobiMatrix[maxElement.I, maxElement.I] = cosK;
            jacobiMatrix[maxElement.J, maxElement.J] = cosK;
            jacobiMatrix[maxElement.J, maxElement.I] = sinK;
            jacobiMatrix[maxElement.I, maxElement.J] = -sinK;

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