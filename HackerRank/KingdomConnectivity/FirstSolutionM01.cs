using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingdomConnectivity
{
    class FirstSolutionM01

    {
        private static Dictionary<int, List<int>> s_DPaths = new Dictionary<int, List<int>>();
        private static Dictionary<int, int> keyNodePaths = new Dictionary<int, int>();
        private static long s_PathCount;
        private static HashSet<int> traversedCities = new HashSet<int>();
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
            private readonly List<int> m_StepList = new List<int>();
            private int m_pathCount = 0;

            public Branch(Branch parent, int start)
            {
                this.m_ParentBranch = parent;
                this.m_Start = start;
            }

            private bool MeOrParantHasStep(int stepNo)
            {
                //return m_StepList.Contains(stepNo) || (m_ParentBranch?.MeOrParantHasStep(stepNo) ?? false);
                return traversedCities.Contains(stepNo);
            }

            private void PrintStepsWithParent()
            {
                m_ParentBranch?.PrintStepsWithParent();
                var steps = string.Join(",", m_StepList.Select(x => x.ToString()));
                Console.Write(",");
                Console.Write(steps);
            }

            public BranchEnd StartBranch(out int pathCount)
            {
                m_City = m_Start;

                if (m_City == s_N)
                {
                    m_pathCount++;
#if PRINT
                    Console.Write("s");
                    PrintStepsWithParent();
                    Console.WriteLine($",{m_City}");
#endif
                    if (!keyNodePaths.ContainsKey(this.m_Start))
                        keyNodePaths.Add(m_Start, m_pathCount);
                    traversedCities.ExceptWith(this.m_StepList);
                    pathCount = m_pathCount;
                    return BranchEnd.Successfull;
                }
                if (MeOrParantHasStep(m_City))
                {
#if PRINT
                    Console.Write("R");
                    PrintStepsWithParent();
                    Console.WriteLine($"{m_City}");
#endif
                    if (!keyNodePaths.ContainsKey(this.m_Start))
                        keyNodePaths.Add(m_Start, m_pathCount);
                    traversedCities.ExceptWith(this.m_StepList);
                    pathCount = m_pathCount;
                    return BranchEnd.Recursive;
                }

                while (true)
                {
                    m_StepList.Add(m_City);
                    traversedCities.Add(m_City);
                    if (!s_DPaths.ContainsKey(m_City))
                    {
#if PRINT
                        Console.Write("F");
                        PrintStepsWithParent();
                        Console.WriteLine($",{m_City}");
#endif
                        if (!keyNodePaths.ContainsKey(this.m_Start))
                            keyNodePaths.Add(m_Start, m_pathCount);
                        traversedCities.ExceptWith(this.m_StepList);
                        pathCount = m_pathCount;
                        return BranchEnd.Fail;
                    }

                    var lst = s_DPaths[m_City];
                    if (lst.Count == 1)
                    {
                        m_City = lst[0];
                        if (m_City == s_N)
                        {
                            m_pathCount++;
#if PRINT
                            Console.Write("S");
                            PrintStepsWithParent();
                            Console.WriteLine($",{m_City}");
#endif
                            if (!keyNodePaths.ContainsKey(this.m_Start))
                                keyNodePaths.Add(m_Start, m_pathCount);
                            traversedCities.ExceptWith(this.m_StepList);
                            pathCount = m_pathCount;
                            return BranchEnd.Successfull;
                        }
                        if (MeOrParantHasStep(m_City))
                        {
#if PRINT
                            Console.Write("r");
                            PrintStepsWithParent();
                            Console.WriteLine($",{m_City}");
#endif
                            if(!keyNodePaths.ContainsKey(this.m_Start))
                                keyNodePaths.Add(m_Start, m_pathCount);
                            traversedCities.ExceptWith(this.m_StepList);
                            pathCount = m_pathCount;
                            return BranchEnd.Recursive;
                        }
                    }
                    else
                    {
                        BranchEnd be = BranchEnd.None;
                        foreach (int i in lst)
                        {
                            int subPathCount;
                            if (keyNodePaths.ContainsKey(i))
                                subPathCount = keyNodePaths[i];
                            else
                            {
                                Branch b = new Branch(this, i);
                                be |= b.StartBranch(out subPathCount);
                            }
                            this.m_pathCount += subPathCount;
                        }

                        if (!be.HasFlag(BranchEnd.Successfull) && be.HasFlag(BranchEnd.Recursive) &&
                            be.HasFlag(BranchEnd.Fail))
                        {
                            if (!keyNodePaths.ContainsKey(this.m_Start))
                                keyNodePaths.Add(m_Start, m_pathCount);
                            traversedCities.ExceptWith(this.m_StepList);
                            pathCount = m_pathCount;
                            return BranchEnd.Fail;
                        }

                        if (be.HasFlag(BranchEnd.Successfull) && be.HasFlag(BranchEnd.Recursive))
                            throw new InfinitePathException();

                        if (!keyNodePaths.ContainsKey(this.m_Start))
                            keyNodePaths.Add(m_Start, m_pathCount);
                        traversedCities.ExceptWith(this.m_StepList);
                        pathCount = m_pathCount;
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
            foreach (KeyValuePair<int, List<int>> path in DPaths)
            {
                Console.WriteLine($"{path.Key} : {string.Join(",", path.Value.Select(x => x.ToString()))}");
            }

            Console.WriteLine("==== Paths End ====");
#endif
            
            try
            {
                s_PathCount = 0;
                Branch b = new Branch(null, start);
                int mainPathCount;
                var res = b.StartBranch(out mainPathCount);
                if (res.HasFlag(BranchEnd.Successfull) && res.HasFlag(BranchEnd.Recursive))
                {
                    //Console.WriteLine("INFINITE PATHS");
                    return -1;
                }
                //Console.WriteLine(s_PathCount);
                return mainPathCount;
            }
            catch (InfinitePathException)
            {
                //Console.WriteLine("INFINITE PATHS");
                return -1;
            }
        }
    }
}
