﻿﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace KingdomConnectivity
{
    class FirstSolution
    {
        private static Dictionary<int, List<int>> s_DPaths = new Dictionary<int, List<int>>();
        private static long s_PathCount;
        private static int s_N, s_M;

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
            private readonly Branch m_ParentBranch;
            private readonly HashSet<int> m_StepList = new HashSet<int>();

            public Branch(Branch parent, int start)
            {
                this.m_ParentBranch = parent;
                this.m_Start = start;
            }

            private bool MeOrParantHasStep(int stepNo)
            {
                return m_StepList.Contains(stepNo) || (m_ParentBranch?.MeOrParantHasStep(stepNo) ?? false);
            }

            private void PrintStepsWithParent()
            {
                m_ParentBranch?.PrintStepsWithParent();
                var steps = string.Join(",", m_StepList.Select(x => x.ToString()));
                Console.Write(",");
                Console.Write(steps);
            }

            public BranchEnd StartBranch()
            {
                m_City = m_Start;

                if (m_City == s_N)
                {
                    s_PathCount++;
#if PRINT
                    Console.Write("s");
                    PrintStepsWithParent();
                    Console.WriteLine($",{m_City}");
#endif
                    return BranchEnd.Successfull;
                }
                if (MeOrParantHasStep(m_City))
                {
#if PRINT
                    Console.Write("R");
                    PrintStepsWithParent();
                    Console.WriteLine($"{m_City}");
#endif
                    return BranchEnd.Recursive;
                }

                while (true)
                {
                    m_StepList.Add(m_City);
                    if (!s_DPaths.ContainsKey(m_City))
                    {
#if PRINT
                        Console.Write("F");
                        PrintStepsWithParent();
                        Console.WriteLine($",{m_City}");
#endif
                        return BranchEnd.Fail;
                    }

                    var lst = s_DPaths[m_City];
                    if (lst.Count == 1)
                    {
                        m_City = lst[0];
                        if (m_City == s_N)
                        {
                            s_PathCount++;
#if PRINT
                            Console.Write("S");
                            PrintStepsWithParent();
                            Console.WriteLine($",{m_City}");
#endif
                            return BranchEnd.Successfull;
                        }
                        if (MeOrParantHasStep(m_City))
                        {
#if PRINT
                            Console.Write("r");
                            PrintStepsWithParent();
                            Console.WriteLine($",{m_City}");
#endif
                            return BranchEnd.Recursive;
                        }
                    }
                    else
                    {
                        BranchEnd be = BranchEnd.None;
                        foreach (int i in lst)
                        {
                            Branch b = new Branch(this, i);
                            be |= b.StartBranch();
                        }

                        if (!be.HasFlag(BranchEnd.Successfull) && be.HasFlag(BranchEnd.Recursive) &&
                            be.HasFlag(BranchEnd.Fail))
                            return BranchEnd.Fail;

                        if (be.HasFlag(BranchEnd.Successfull) && be.HasFlag(BranchEnd.Recursive))
                            throw new InfinitePathException();

                        return be;
                    }
                }
            }
        }

        public static int Solve(int start, int dest, Dictionary<int, List<int>> nexts)
        {


            s_N = dest;

            s_DPaths = nexts;
#if PRINT
            Console.WriteLine("==== Paths Start ====");
            foreach (KeyValuePair<int, List<int>> path in nexts)
            {
                Console.WriteLine($"{path.Key} : {string.Join(",", path.Value.Select(x => x.ToString()))}");
            }

            Console.WriteLine("==== Paths End ====");
#endif

            try
            {
                s_PathCount = 0;
                Branch b = new Branch(null, start);
                var res = b.StartBranch();
                if (res.HasFlag(BranchEnd.Successfull) && res.HasFlag(BranchEnd.Recursive))
                {
                    //Console.WriteLine("INFINITE PATHS");
                    return -1;
                }
                //Console.WriteLine(s_PathCount);
                return (int)s_PathCount;
            }
            catch (InfinitePathException)
            {
                //Console.WriteLine("INFINITE PATHS");
                return -1;
            }
        }
    }
}

