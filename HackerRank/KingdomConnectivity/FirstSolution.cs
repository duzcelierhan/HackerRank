﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace KingdomConnectivity
{
    class FirstSolution
    {
        //static void Main(string[] args)
        //{
        //    string[] parts = Console.ReadLine()?.Split(' ');
        //    int[] ints = Array.ConvertAll(parts, int.Parse);
        //    Dictionary<int, List<int>> dPaths = new Dictionary<int, List<int>>();
        //    for (int i = 0; i < ints[1]; i++)
        //    {
        //        parts = Console.ReadLine()?.Split(' ');
        //        int[] pair = Array.ConvertAll(parts, int.Parse);
        //        if (!dPaths.ContainsKey(pair[0]))
        //            dPaths.Add(pair[0], new List<int> { pair[1] });
        //        else
        //        {
        //            //if (!DPaths[pair[0]].Contains(pair[1]))
        //            dPaths[pair[0]].Add(pair[1]);
        //        }
        //    }


        //    Solve(ints[0], dPaths);

        //    Console.ReadLine();
        //}

        private static readonly Dictionary<int, List<int>> DPaths = new Dictionary<int, List<int>>();
        private static int s_PathCount;
        private static int s_Dest;
        private const bool c_PrintPaths = false;

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

                if (m_City == s_Dest)
                {
                    s_PathCount++;
                    if (c_PrintPaths)
                    {
                        Console.Write("s");
                        PrintStepsWithParent();
                        Console.WriteLine($",{m_City}");
                    }
                    return BranchEnd.Successfull;
                }
                if (MeOrParantHasStep(m_City))
                {
                    if (c_PrintPaths)
                    {
                        Console.Write("R");
                        PrintStepsWithParent();
                        Console.WriteLine($"{m_City}");
                    }
                    return BranchEnd.Recursive;
                }

                while (true)
                {
                    m_StepList.Add(m_City);
                    if (!DPaths.ContainsKey(m_City))
                    {
                        if (c_PrintPaths)
                        {
                            Console.Write("F");
                            PrintStepsWithParent();
                            Console.WriteLine($",{m_City}");
                        }
                        return BranchEnd.Fail;
                    }

                    var lst = DPaths[m_City];
                    if (lst.Count == 1)
                    {
                        m_City = lst[0];
                        if (m_City == s_Dest)
                        {
                            s_PathCount++;
                            if (c_PrintPaths)
                            {
                                Console.Write("S");
                                PrintStepsWithParent();
                                Console.WriteLine($",{m_City}");
                            }
                            return BranchEnd.Successfull;
                        }
                        if (MeOrParantHasStep(m_City))
                        {
                            if (c_PrintPaths)
                            {
                                Console.Write("r");
                                PrintStepsWithParent();
                                Console.WriteLine($",{m_City}");
                            }
                            return BranchEnd.Recursive;
                        }
                    }
                    else
                    {
                        //BranchEnd be = lst.Select(i => new Branch(this, i)).Aggregate(BranchEnd.None, (current, b) => current | b.StartBranch());
                        BranchEnd be = BranchEnd.None;
                        foreach (int i in lst)
                        {
                            Branch b = new Branch(this, i);
                            be |= b.StartBranch();
                        }

                        if (!be.HasFlag(BranchEnd.Successfull) && be.HasFlag(BranchEnd.Recursive) && be.HasFlag(BranchEnd.Fail))
                            return BranchEnd.Fail;

                        if (be.HasFlag(BranchEnd.Successfull) && be.HasFlag(BranchEnd.Recursive))
                            throw new InfinitePathException();

                        return be;
                    }
                }
            }
        }

        public static int Solve(int dest, Dictionary<int, List<int>> nexts)
        {
            if (c_PrintPaths)
            {
                Console.WriteLine("==== Paths Start ====");
                foreach (KeyValuePair<int, List<int>> path in DPaths)
                {
                    Console.WriteLine($"{path.Key} : {string.Join(",", path.Value.Select(x => x.ToString()))}");
                }

                Console.WriteLine("==== Paths End ====");
            }

            try
            {
                Branch b = new Branch(null, 1);
                var res = b.StartBranch();
                if (res.HasFlag(BranchEnd.Successfull) && res.HasFlag(BranchEnd.Recursive))
                {
                    //Console.WriteLine("INFINITE PATHS");
                    return -1;
                }
                else
                {
                    //Console.WriteLine(s_PathCount);
                    return s_PathCount;
                }
            }
            catch (InfinitePathException)
            {
                //Console.WriteLine("INFINITE PATHS");
                return -1;
            }
            return s_PathCount;
        }
    }
}
