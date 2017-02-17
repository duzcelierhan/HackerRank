using System;
using System.Collections.Generic;

namespace KingdomConnectivity
{
    class Solution
    {
        private static readonly Dictionary<int, List<int>> DPaths = new Dictionary<int, List<int>>();
        private static readonly Dictionary<int, long> KeyNodePaths = new Dictionary<int, long>();
        private static readonly HashSet<int> TraversedCities = new HashSet<int>();
        private static int s_N;
        private const int c_Modulus = (int)1e9;

        static void Main(string[] args)
        {
            string[] parts = Console.ReadLine()?.Split(' ');
            int[] ints = Array.ConvertAll(parts, int.Parse);
            s_N = ints[0];
            for (int i = 0; i < ints[1]; i++)
            {
                parts = Console.ReadLine()?.Split(' ');
                int[] pair = Array.ConvertAll(parts, int.Parse);
                if (!DPaths.ContainsKey(pair[0]))
                    DPaths.Add(pair[0], new List<int> { pair[1] });
                else
                {
                    DPaths[pair[0]].Add(pair[1]);
                }
            }

            var paths = Solve();

            Console.WriteLine(paths < 0 ? "INFINITE PATHS" : paths.ToString());
        }

        [Flags]
        private enum BranchEnd
        {
            None = 0x00,
            Successfull = 0x01,
            Fail = 0x02,
            Recursive = 0x04
        }

        private class InfinitePathException : Exception
        {
        }

        private class Branch
        {
            private readonly int m_Start;
            private int m_City;
            private readonly List<int> m_StepList = new List<int>();
            private long m_PathCount;

            public Branch(int start)
            {
                this.m_Start = start;
            }

            private bool MeOrParentHasStep(int stepNo)
            {
                return TraversedCities.Contains(stepNo);
            }

            public BranchEnd StartBranch(out long pathCount)
            {
                m_City = m_Start;
                pathCount = 0;
                m_StepList.Add(m_City);
                long tmp;

                if (m_City == s_N)
                {
                    m_PathCount++;
                    TraversedCities.ExceptWith(this.m_StepList);
                    pathCount = m_PathCount;
                    return BranchEnd.Successfull;
                }
                if (MeOrParentHasStep(m_City))
                {
                    TraversedCities.ExceptWith(this.m_StepList);
                    pathCount = 0;//m_PathCount;
                    return BranchEnd.Recursive;
                }

                if (KeyNodePaths.TryGetValue(m_City, out tmp))
                {
                    TraversedCities.ExceptWith(m_StepList);
                    pathCount = tmp;
                    return tmp == 0 ? BranchEnd.Fail : BranchEnd.Successfull;
                }
                m_StepList.Clear();
                while (true)
                {
                    m_StepList.Add(m_City);
                    TraversedCities.Add(m_City);
                    if (!DPaths.ContainsKey(m_City))
                    {
                        TraversedCities.ExceptWith(this.m_StepList);
                        pathCount = 0;
                        return BranchEnd.Fail;
                    }

                    var lst = DPaths[m_City];
                    if (lst.Count == 1)
                    {
                        m_City = lst[0];
                        if (m_City == s_N)
                        {
                            m_PathCount++;
                            TraversedCities.ExceptWith(this.m_StepList);
                            pathCount = m_PathCount;
                            return BranchEnd.Successfull;
                        }
                        if (MeOrParentHasStep(m_City))
                        {
                            TraversedCities.ExceptWith(this.m_StepList);
                            pathCount = 0;
                            return BranchEnd.Recursive;
                        }

                        if (KeyNodePaths.TryGetValue(m_City, out tmp))
                        {
                           TraversedCities.ExceptWith(m_StepList);
                            pathCount = tmp;
                            return tmp == 0 ? BranchEnd.Fail : BranchEnd.Successfull;
                        }
                    }
                    else
                    {
                        BranchEnd be = BranchEnd.None;
                        foreach (int i in lst)
                        {
                            long subPathCount;
                            if (KeyNodePaths.ContainsKey(i))
                            {
                                subPathCount = KeyNodePaths[i];
                            }
                            else
                            {
                                Branch b = new Branch(i);
                                be |= b.StartBranch(out subPathCount);
                            }
                            this.m_PathCount += subPathCount;

                            if (m_PathCount < 0 || m_PathCount > c_Modulus)
                            {
                                m_PathCount %= c_Modulus;
                            }
                        }

                        if (!be.HasFlag(BranchEnd.Successfull) && be.HasFlag(BranchEnd.Recursive) &&
                            be.HasFlag(BranchEnd.Fail))
                        {
                            if (!KeyNodePaths.ContainsKey(this.m_Start))
                                KeyNodePaths.Add(m_Start, 0);
                            TraversedCities.ExceptWith(this.m_StepList);
                            pathCount = 0;
                            return BranchEnd.Fail;
                        }

                        if (be.HasFlag(BranchEnd.Successfull) && be.HasFlag(BranchEnd.Recursive))
                            throw new InfinitePathException();

                        if (!KeyNodePaths.ContainsKey(this.m_Start))
                            KeyNodePaths.Add(m_Start, m_PathCount);
                        TraversedCities.ExceptWith(this.m_StepList);
                        pathCount = m_PathCount;
                        return be;
                    }
                }
            }
        }

        public static int Solve()
        {
            try
            {
                Branch b = new Branch(1);
                long mainPathCount;
                var res = b.StartBranch(out mainPathCount);
                if (res.HasFlag(BranchEnd.Successfull) && res.HasFlag(BranchEnd.Recursive))
                {
                    return -1;
                }
                
                return (int)mainPathCount;
            }
            catch (InfinitePathException)
            {
                return -1;
            }
        }
    }
}
