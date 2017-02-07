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
        private static readonly Dictionary<int, List<int>> DPaths = new Dictionary<int, List<int>>();
        private static long _pathCount = 0;
        private static int _n, _m;

        [Flags]
        private enum BranchEnd
        {
            None = 0x00,
            Successfull = 0x01,
            Fail = 0x02,
            Resursive = 0x04
        }

        private class InfinitePathException : Exception
        {
        }

        private class Branch
        {
            private readonly int m_Start;
            private int m_City;
            private readonly Branch m_ParentBranch;
            private readonly List<int> m_StepList = new List<int>();

            public Branch(Branch parent, int start)
            {
                this.m_ParentBranch = parent;
                this.m_Start = start;
            }

            private bool MeOrParantHasStep(int stepNo)
            {
                return m_StepList.Contains(stepNo) || (m_ParentBranch?.MeOrParantHasStep(stepNo) ?? false);
            }

            public BranchEnd StartBranch()
            {
                m_City = m_Start;

                if (m_City == _n)
                {
                    _pathCount++;
                    return BranchEnd.Successfull;
                }

                while (true)
                {
                    m_StepList.Add(m_City);
                    var lst = DPaths[m_City];
                    if (lst.Count == 1)
                    {
                        m_City = lst[0];
                        if (m_City == _n)
                        {
                            _pathCount++;
                            return BranchEnd.Successfull;
                        }
                        if (MeOrParantHasStep(m_City))
                        {
                            return BranchEnd.Resursive;
                        }
                    }
                    else
                    {
                        BranchEnd be = lst.Select(i => new Branch(this, i)).Aggregate(BranchEnd.None, (current, b) => current | b.StartBranch());

                        if(be.HasFlag(BranchEnd.Successfull)&& be.HasFlag(BranchEnd.Resursive))
                            throw new InfinitePathException();

                        return be;
                    }
                }
            }
        }

        public static void Solve(int n, int m)
        {
            if (n < 2 || n > 1e4)
                throw new ArgumentOutOfRangeException(nameof(n), "N must in range 2 <= N <=10^4");
            if (m < 1 || m > 1e5)
                throw new ArgumentOutOfRangeException(nameof(m), "M must in range 1 <= M <=10^5");

            _n = n;
            _m = m;

            for (int i = 0; i < _m; i++)
            {
                string[] parts = Console.ReadLine()?.Split(' ');
                int[] pair = Array.ConvertAll(parts, int.Parse);
                if (!DPaths.ContainsKey(pair[0]))
                    DPaths.Add(pair[0], new List<int> {pair[1]});
                else 
                    DPaths[pair[0]].Add(pair[1]);
            }

            try
            {
                Branch b = new Branch(null, 1);
                var res = b.StartBranch();
                if(res.HasFlag(BranchEnd.Successfull) && res.HasFlag(BranchEnd.Resursive))
                    Console.WriteLine("INFINITE PATHS");
                else 
                    Console.WriteLine(_pathCount);
            }
            catch (InfinitePathException)
            {
                Console.WriteLine("INFINITE PATHS");
            }
        }
    }
}
