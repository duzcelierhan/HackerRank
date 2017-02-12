using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace KingdomConnectivity
{

    /*
        Test Input:
        20 33
        10 15
        15 17
        8 18
        12 5
        11 4
        16 14
        10 17
        12 5
        4 12
        10 15
        1 7
        2 20
        1 10
        2 20
        12 5
        15 9
        8 11
        16 6
        4 12
        13 2
        15 17
        3 15
        14 8
        11 19
        1 16
        9 20
        9 13
        16 18
        14 6
        1 3
        12 5
        4 16
        6 8

        Expected Output:
        9
        */


    class Solution
    {
        static void Main(string[] args)
        {
            string[] parts = Console.ReadLine()?.Split(' ');
            int[] ints = Array.ConvertAll(parts, int.Parse);
            Stopwatch sw = new Stopwatch();

            s_N = ints[0];
            s_M = ints[1];

            if (s_N < 2 || s_N > 1e4)
                throw new ArgumentOutOfRangeException(nameof(s_N), "N must in range 2 <= N <=10^4");
            if (s_M < 1 || s_M > 1e5)
                throw new ArgumentOutOfRangeException(nameof(s_M), "M must in range 1 <= M <=10^5");

            for (int i = 0; i < s_M; i++)
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

            sw.Start();
            //Solve(ints[0], ints[1]);
            //int paths = Solve();//Alternative3.Solve(s_N, DPaths);
            int pathsFirst = FirstSolution.Solve(s_N, DPaths);
            sw.Stop();
            TimeSpan firstSolTime = sw.Elapsed;
            Console.WriteLine($"First solution: {pathsFirst} paths in {firstSolTime} time");
            sw.Restart();
            int pathsAlt3 = Alternative3.Solve(s_N, DPaths);
            sw.Stop();
            TimeSpan altSol3Time = sw.Elapsed;
            Console.WriteLine($"Alternative 3 solution: {pathsAlt3} paths in {altSol3Time} time");

            //Console.WriteLine(paths < 0 ? "INFINITE PATHS" : (paths / 2).ToString());
            //Console.WriteLine($"It took {sw.Elapsed} to calculate");
            Console.ReadLine();
        }

        private static readonly Dictionary<int, List<int>> DPaths = new Dictionary<int, List<int>>();
        private static long s_PathCount;
        private static int s_N, s_M;
        private static HashSet<int> s_PassedVillages=new HashSet<int>();

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

            //private bool MeOrParantHasStep(int stepNo)
            //{
            //    return m_StepList.Contains(stepNo) || (m_ParentBranch?.MeOrParantHasStep(stepNo) ?? false);
            //}

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
                //s_PassedVillages.Add(m_City);
                //m_StepList.Add(m_City);

                if (m_City == s_N)
                {
                    s_PathCount++;
#if PRINT
                    Console.Write("s");
                    PrintStepsWithParent();
                    Console.WriteLine($",{m_City}");
#endif
                    s_PassedVillages.ExceptWith(m_StepList);
                    return BranchEnd.Successfull;
                }
                if(s_PassedVillages.Contains(m_City))
                {
#if PRINT
                    Console.Write("R");
                    PrintStepsWithParent();
                    Console.WriteLine($"{m_City}");
#endif
                    s_PassedVillages.ExceptWith(m_StepList);
                    return BranchEnd.Recursive;
                }

                while (true)
                {

                    if (!DPaths.ContainsKey(m_City))
                    {
#if PRINT
                        Console.Write("F");
                        PrintStepsWithParent();
                        Console.WriteLine($",{m_City}");
#endif
                        s_PassedVillages.ExceptWith(m_StepList);
                        return BranchEnd.Fail;
                    }

                    var lst = DPaths[m_City];
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
                            s_PassedVillages.ExceptWith(m_StepList);
                            return BranchEnd.Successfull;
                        }
                        if (s_PassedVillages.Contains(m_City))
                        {
#if PRINT
                            Console.Write("r");
                            PrintStepsWithParent();
                            Console.WriteLine($",{m_City}");
#endif
                            s_PassedVillages.ExceptWith(m_StepList);
                            return BranchEnd.Recursive;
                        }
                        s_PassedVillages.Add(m_City);
                        m_StepList.Add(m_City);
                    }
                    else
                    {
                        s_PassedVillages.Add(m_City);
                        m_StepList.Add(m_City);
                        //BranchEnd be = lst.Select(i => new Branch(this, i)).Aggregate(BranchEnd.None, (current, b) => current | b.StartBranch());
                        BranchEnd be = BranchEnd.None;
                        foreach (int i in lst)
                        {
                            Branch b = new Branch(this, i);
                            be |= b.StartBranch();
                        }

                        s_PassedVillages.ExceptWith(m_StepList);
                        if (!be.HasFlag(BranchEnd.Successfull) && be.HasFlag(BranchEnd.Recursive) && be.HasFlag(BranchEnd.Fail))
                            return BranchEnd.Fail;

                        if (be.HasFlag(BranchEnd.Successfull) && be.HasFlag(BranchEnd.Recursive))
                            throw new InfinitePathException();

                        return be;
                    }
                }
            }
        }

        public static int Solve()
        {



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
                    return (int)s_PathCount;
                }
            }
            catch (InfinitePathException)
            {
                //Console.WriteLine("INFINITE PATHS");
                return -1;
            }
        }
    }
}
