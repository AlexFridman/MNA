using System.Xml;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using DenseVector = MathNet.Numerics.LinearAlgebra.Double.DenseVector;

namespace LabWork_2
{
    public class SeidelSolver : IterativeSolver
    {
        public SeidelSolver(Matrix<double> matrixA, Vector<double> vectorB) : base(matrixA, vectorB)
        {
        }
    }
}