using System;

namespace HackerRank
{
    public class Staircase
    {
        public static void StairCase(int n)
        {
            for (int i = 1; i <= n; i++)
                Console.WriteLine(new string('#', i).PadRight(n, ' '));
        }
    }
}
