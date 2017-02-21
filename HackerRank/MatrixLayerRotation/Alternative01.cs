using System;

namespace MatrixLayerRotation
{
    class Alternative01
    {
        enum Direction { None = 0, Down, Right, Up, Left }

        private static int[,] matrix;
        private static int[,] vertical;
        private static int[,] horizontal;


        static void Main(string[] args)
        {
            int[] parts = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
            int row = parts[0];
            int col = parts[1];
            int r = parts[2];

            matrix = new int[row, col];

            PrepareDirectionMatrix(row, col);

            // Read and create the matrix
            for (int i = 0; i < row; i++)
            {
                var line = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                for (int j = 0; j < col; j++)
                {
                    matrix[i, j] = line[j];
                }
            }

            for (int i = 0; i < r; i++)
            {
                RotateMatrix();
            }
            PrintMatrix();
        }

        private static void PrepareDirectionMatrix(int row, int col)
        {
            vertical = new int[row, col];
            horizontal = new int[row, col];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (i >= j && (j < row - i - 1) && (j < col / 2))
                        vertical[i, j] = 1;
                    else if ((j >= row - i - 1) && (i >= row / 2) && (j < col - row + i))
                        horizontal[i, j] = 1;
                    else if ((j >= col - row + i) && (j >= col / 2) && (i >= col - j))
                        vertical[i, j] = -1;
                    else if ((j > i) && (i < row / 2) && (j < col - i))
                        horizontal[i, j] = -1;
                }
            }
        }

        private static void RotateMatrix()
        {
            int row = matrix.GetLength(0);
            int col = matrix.GetLength(1);
            int[,] newMatrix = new int[row, col];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    newMatrix[i + vertical[i, j], j + horizontal[i, j]] = matrix[i, j];
                }
            }
            matrix = newMatrix;
        }

        private static void PrintMatrix()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
