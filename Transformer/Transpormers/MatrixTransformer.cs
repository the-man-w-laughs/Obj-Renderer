//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;

//namespace Transformer.Transpormers
//{
//    public class MatrixTransformer
//    {
//        public static Matrix4x4 InvertMatrix4x4(Matrix4x4 matrix)
//        {
//            double[,] m = matrix.To2DArray(); // Convert Matrix4x4 to a 2D array for easier indexing

//            double det = CalculateDeterminant4x4(m);

//            if (Math.Abs(det) < 1e-10)
//            {
//                throw new InvalidOperationException("Matrix is singular; it doesn't have an inverse.");
//            }

//            double[,] adjugate = CalculateAdjugate4x4(m);

//            double invDet = 1.0 / det;

//            for (int i = 0; i < 4; i++)
//            {
//                for (int j = 0; j < 4; j++)
//                {
//                    adjugate[i, j] *= invDet;
//                }
//            }

//            return new Matrix4x4(adjugate);
//        }

//        public static double CalculateDeterminant4x4(double[,] matrix)
//        {
//            double det = 0;

//            for (int i = 0; i < 4; i++)
//            {
//                det += matrix[0, i] * CalculateCofactor(matrix, 0, i);
//            }

//            return det;
//        }

//        public static double CalculateCofactor(double[,] matrix, int row, int col)
//        {
//            double[,] minorMatrix = new double[3, 3];

//            int minorRow = 0;
//            for (int i = 0; i < 4; i++)
//            {
//                if (i == row) continue;
//                int minorCol = 0;
//                for (int j = 0; j < 4; j++)
//                {
//                    if (j == col) continue;
//                    minorMatrix[minorRow, minorCol] = matrix[i, j];
//                    minorCol++;
//                }
//                minorRow++;
//            }

//            double minorDeterminant = CalculateDeterminant3x3(minorMatrix);

//            return ((row + col) % 2 == 0) ? minorDeterminant : -minorDeterminant;
//        }

//        public static double CalculateDeterminant3x3(double[,] matrix)
//        {
//            return
//                matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1]) -
//                matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0]) +
//                matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);
//        }
//    }
//}
