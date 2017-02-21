using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLayerRotation
{
    class Program
    {
        enum Direction{Down, Right, Up, Left}

        private static Direction[,] directionMatrix;

        static void Main(string[] args)
        {
            int[] parts = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
            int row = parts[0];
            int col = parts[1];
            int r = parts[2];

            int[,] matrix = new int[row,col];

            // Read and create the matrix
            for (int i = 0; i < row; i++)
            {
                var line = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                for (int j = 0; j < col; j++)
                {
                    matrix[i, j] = line[j];
                }
            }

        }

        private static void PrepareDirectionMatrix(int row, int col)
        {
            directionMatrix = new Direction[row, col];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if(i >= j && i < row-j-1 && i < col-i-1)
                        directionMatrix[i,j] = Direction.Down;
                    else if()
                }
            }
        }
    }
}
