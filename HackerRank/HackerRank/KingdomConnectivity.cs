using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerRank
{
    //https://www.hackerrank.com/challenges/kingdom-connectivity?h_r=next-challenge&h_v=zen
    class KingdomConnectivity
    {
        private static readonly Dictionary<int, List<int>> dPaths = new Dictionary<int, List<int>>();
        private static long pathCount = 0;

        private enum BranchEnd { Successfull, Fail, Resursive }
        private class Branch
        {
            private int start;
            public BranchEnd branchEnd;
            private Branch parentBranch;
            public List<int> StepList = new List<int>();

            public Branch(Branch parent, int start)
            {
                this.parentBranch = parent;
                this.start = start;
            }

            public bool MeOrParantHasStep(int stepNo)
            {
                return StepList.Contains(stepNo) || (parentBranch?.MeOrParantHasStep(stepNo) ?? false);
            }

            public BranchEnd StartBranch()
            {
                while (true)
                {

                }
            }
        }
        public static void Solution(int N, int M)
        {
            if (N < 2 || N > 1e4)
                throw new ArgumentException("N must in range 2 <= N <=10^4", nameof(N));
            if (M < 1 || M > 1e5)
                throw new ArgumentException("M must in range 1 <= M <=10^5", nameof(M));

            
            Queue<int> jobQueue = new Queue<int>();
            for (int i = 0; i < M; i++)
            {
                string[] parts = Console.ReadLine()?.Split(' ');
                int[] pair = Array.ConvertAll(parts, int.Parse);
                if (!dPaths.ContainsKey(pair[0]))
                    dPaths.Add(pair[0], new List<int> {pair[1]});
                else 
                    dPaths[pair[0]].Add(pair[1]);
            }
            

        }
    }
}
