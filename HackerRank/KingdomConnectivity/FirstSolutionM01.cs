using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingdomConnectivity
{
    class FirstSolutionM01

    {
        private static Dictionary<int, List<int>> s_DPaths = new Dictionary<int, List<int>>();
        private static Dictionary<int, long> keyNodePaths = new Dictionary<int, long>();
        //private static long s_PathCount;
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
            private long m_PathCount = 0;

            public Branch(Branch parent, int start)
            {
                this.m_ParentBranch = parent;
                this.m_Start = start;
            }

            private bool MeOrParentHasStep(int stepNo)
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

            public BranchEnd StartBranch(out long pathCount)
            {
                m_City = m_Start;
                pathCount = 0;
                m_StepList.Add(m_City);
                long tmp;

                if (m_City == s_N)
                {
                    m_PathCount++;
#if PRINT
                    Console.Write("s");
                    PrintStepsWithParent();
                    Console.WriteLine($",{m_City}");
#endif
                    //if (!keyNodePaths.ContainsKey(this.m_Start))
                    //    keyNodePaths.Add(m_Start, m_PathCount);
                    traversedCities.ExceptWith(this.m_StepList);
                    pathCount = m_PathCount;
                    return BranchEnd.Successfull;
                }
                if (MeOrParentHasStep(m_City))
                {
#if PRINT
                    Console.Write("R");
                    PrintStepsWithParent();
                    Console.WriteLine($"{m_City}");
#endif
                    //if (!keyNodePaths.ContainsKey(this.m_Start))
                    //    keyNodePaths.Add(m_Start, 0/*m_PathCount*/);
                    traversedCities.ExceptWith(this.m_StepList);
                    pathCount = 0;//m_PathCount;
                    return BranchEnd.Recursive;
                }

                if (keyNodePaths.TryGetValue(m_City, out tmp))
                {
                    //if (!keyNodePaths.ContainsKey(this.m_Start))
                    //    keyNodePaths.Add(m_Start, tmp);
                    traversedCities.ExceptWith(m_StepList);
                    pathCount = tmp;
                    return tmp == 0 ? BranchEnd.Fail : BranchEnd.Successfull;
                }
                m_StepList.Clear();
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
                        //if (!keyNodePaths.ContainsKey(this.m_Start))
                        //    keyNodePaths.Add(m_Start, 0/*m_PathCount*/);
                        traversedCities.ExceptWith(this.m_StepList);
                        pathCount = 0;//m_PathCount;
                        return BranchEnd.Fail;
                    }

                    var lst = s_DPaths[m_City];
                    if (lst.Count == 1)
                    {
                        m_City = lst[0];
                        if (m_City == s_N)
                        {
                            m_PathCount++;
#if PRINT
                            Console.Write("S");
                            PrintStepsWithParent();
                            Console.WriteLine($",{m_City}");
#endif
                            //if (!keyNodePaths.ContainsKey(this.m_Start))
                            //{
                            //    keyNodePaths.Add(m_Start, m_PathCount);
                            //}
                            traversedCities.ExceptWith(this.m_StepList);
                            pathCount = m_PathCount;
                            return BranchEnd.Successfull;
                        }
                        if (MeOrParentHasStep(m_City))
                        {
#if PRINT
                            Console.Write("r");
                            PrintStepsWithParent();
                            Console.WriteLine($",{m_City}");
#endif
                            //if (!keyNodePaths.ContainsKey(this.m_Start))
                            //    keyNodePaths.Add(m_Start, 0/*m_PathCount*/);
                            traversedCities.ExceptWith(this.m_StepList);
                            pathCount = 0;//m_PathCount;
                            return BranchEnd.Recursive;
                        }

                        if (keyNodePaths.TryGetValue(m_City, out tmp))
                        {
                            //if(!keyNodePaths.ContainsKey(this.m_Start))
                            //    keyNodePaths.Add(m_Start, tmp);
                            traversedCities.ExceptWith(m_StepList);
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
                            if (keyNodePaths.ContainsKey(i))
                            {
                                subPathCount = keyNodePaths[i];

#if PRINT
                                if(subPathCount>0)
                                {
                                    Console.Write("ß");
                                    PrintStepsWithParent();
                                    Console.WriteLine($",{i}");
                                }
#endif
                            }
                            else
                            {
                                Branch b = new Branch(this, i);
                                be |= b.StartBranch(out subPathCount);
                                Debug.WriteLine($">> City {m_Start} -> SubPathCount {i} = {subPathCount}");
                            }
                            this.m_PathCount += subPathCount;

                            if (m_PathCount < 0 || m_PathCount > (int)1e9)
                            {
                                m_PathCount = 0;
                                break;
                            }
                        }

                        if (!be.HasFlag(BranchEnd.Successfull) && be.HasFlag(BranchEnd.Recursive) &&
                            be.HasFlag(BranchEnd.Fail))
                        {
                            if (!keyNodePaths.ContainsKey(this.m_Start))
                                keyNodePaths.Add(m_Start, 0/*m_PathCount*/);
                            traversedCities.ExceptWith(this.m_StepList);
                            pathCount = 0;//m_PathCount;
                            return BranchEnd.Fail;
                        }

                        if (be.HasFlag(BranchEnd.Successfull) && be.HasFlag(BranchEnd.Recursive))
                            throw new InfinitePathException();

                        if (!keyNodePaths.ContainsKey(this.m_Start))
                            keyNodePaths.Add(m_Start, m_PathCount);
                        traversedCities.ExceptWith(this.m_StepList);
                        pathCount = m_PathCount;
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
                //s_PathCount = 0;
                Branch b = new Branch(null, start);
                long mainPathCount;
                var res = b.StartBranch(out mainPathCount);
                if (res.HasFlag(BranchEnd.Successfull) && res.HasFlag(BranchEnd.Recursive))
                {
                    //Console.WriteLine("INFINITE PATHS");
                    return -1;
                }
                //Console.WriteLine(s_PathCount);
                return checked((int)mainPathCount);
            }
            catch (InfinitePathException)
            {
                //Console.WriteLine("INFINITE PATHS");
                return -1;
            }
        }
    }
}
