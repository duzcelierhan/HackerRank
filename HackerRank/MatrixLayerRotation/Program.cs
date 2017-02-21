using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayerRotation
{
    class Program
    {
        enum Direction { None = 0, Down, Right, Up, Left }

        private static Direction[,] directionMatrix;
        private static int[,] matrix;

        private static Dictionary<Direction, string> characters = new Dictionary<Direction, string>
        {
            { Direction.Down, "↓" },
            { Direction.Right, "→" },
            { Direction.Up, "↑" },
            { Direction.Left, "←" },
            { Direction.None, "⦿" },
        };

        //static void Main(string[] args)
        //{
        //    int[] parts = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
        //    int row = parts[0];
        //    int col = parts[1];
        //    int r = parts[2];

        //    matrix = new int[row, col];

        //    PrepareDirectionMatrix(row, col);
        //    //PrintDirectionMatrix();

        //    // Read and create the matrix
        //    for (int i = 0; i < row; i++)
        //    {
        //        var line = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
        //        for (int j = 0; j < col; j++)
        //        {
        //            matrix[i, j] = line[j];
        //        }
        //    }

        //    for (int i = 0; i < r; i++)
        //    {
        //        RotateMatrix();
        //    }
        //    PrintMatrix();
        //}

        private static void PrepareDirectionMatrix(int row, int col)
        {
            directionMatrix = new Direction[row, col];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (i >= j && (j < row - i - 1) && (j < col / 2))
                        directionMatrix[i, j] = Direction.Down;
                    else if ((j >= row - i - 1) && (i >= row / 2) && (j < col - row + i))
                        directionMatrix[i, j] = Direction.Right;
                    else if ((j >= col - row + i) && (j >= col / 2) && (i >= col - j))
                        directionMatrix[i, j] = Direction.Up;
                    else if ((j > i) && (i < row / 2) && (j < col - i))
                        directionMatrix[i, j] = Direction.Left;
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
                    var dir = directionMatrix[i,j];
                    newMatrix[i + (dir == Direction.Down ? 1 : (dir == Direction.Up) ? -1 : 0), j + (dir == Direction.Right ? 1 : dir == Direction.Left ? -1 : 0)] = matrix[i, j];
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

        private static void PrintDirectionMatrix()
        {
            var encoding = Console.OutputEncoding;
            Console.OutputEncoding = Encoding.Unicode;
            for (int i = 0; i < directionMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < directionMatrix.GetLength(1); j++)
                {
                    Console.Write(characters[directionMatrix[i, j]]);
                }
                Console.WriteLine();
            }

            Console.OutputEncoding = encoding;
        }
    }
}
