using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingdomConnectivity
{
    class Alternative
    {
        struct Call
        {
            public long returnAddress;
            public Queue<int> branches;
        }
        private const long c_MaxPathLength = (long)1e9;
        private static int[] path = new int[c_MaxPathLength];
        private static HashSet<int> passedVillages = new HashSet<int>();
        private static long s_Position = 0;
        private static Stack<Call> s_CallStack = new Stack<Call>();
        private static long s_PathCount;
        private static int s_N, s_M;
        private static readonly Dictionary<int, List<int>> DPaths = new Dictionary<int, List<int>>();
        private static bool failed = false;
        private static bool recursive = false;
        private static bool success = false;

        public static void Solve(int n, int m)
        {
            if (n < 2 || n > 1e4)
                throw new ArgumentOutOfRangeException(nameof(n), "N must in range 2 <= N <=10^4");
            if (m < 1 || m > 1e5)
                throw new ArgumentOutOfRangeException(nameof(m), "M must in range 1 <= M <=10^5");

            s_N = n;
            s_M = m;

            for (int i = 0; i < s_M; i++)
            {
                string[] parts = Console.ReadLine()?.Split(' ');
                int[] pair = Array.ConvertAll(parts, int.Parse);
                if (!DPaths.ContainsKey(pair[0]))
                    DPaths.Add(pair[0], new List<int> { pair[1] });
                else
                {
                    DPaths[pair[0]].Add(pair[1]);
                }
            }
        }

        private static void StartTraversing()
        {
            // Init with village 1 (Capital)
            path[0] = 1;
            passedVillages.Add(1);
            while (true)
            {
                if (DPaths.ContainsKey(path[s_Position]))
                {
                    var lst = DPaths[path[s_Position]];
                    if (lst.Count == 1)
                    {
                        var city = lst[0];
                        if (city == s_N)
                        {
                            s_PathCount++;
                            if (s_CallStack.Count == 0)
                            {
                                // Nowhere to go, just finish
                                break;
                            }
                            else
                            {
                                Return();
                            }
                        }
                        else
                        {
                            path[s_Position] = city;
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                    failed = true;
                    Return();
                }
            }
        }

        private static void Return()
        {
            var ret = s_CallStack.Peek();
            ret
        }
    }
}
