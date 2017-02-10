using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingdomConnectivity
{
    class Alternative3
    {
        //public static int Solve(int dest, Dictionary<int, List<int>> nexts)
        //{
        //    Stack<int> stack = new Stack<int>();
        //    HashSet<int> successfull = new HashSet<int>();
        //    HashSet<int> fails = new HashSet<int>();
        //}
        [Flags]
        enum Branch
        {
            None = unchecked((int)0x80000000),
            Success = unchecked((int)0x80000001),
            Fail = unchecked((int)0x80000002),
            Recursive = unchecked((int)0x80000004)
        }

        public static int Solve(int dest, Dictionary<int, List<int>> nexts)
        {
            Stack<int> stack = new Stack<int>((int) 1e9);
            Stack<int> counters = new Stack<int>((int)1e9);
            int tmpPathCount = 0;
            Dictionary<int, int> passed = new Dictionary<int, int>(dest);
            int paths = 0;
            bool success = false;
            bool fail = false;
            bool recursive = false;
            int city = 1;
            List<int> lst;

            while (true)
            {
                if (nexts.TryGetValue(city, out lst))
                {
                    if (lst.Count == 1)
                    {
                        city = lst[0];
                    }
                    else
                    {
                        stack.Push(city);
                        counters.Push(tmpPathCount);
                        tmpPathCount = 0;
                        // Push flags to stack
                        var flag = (success ? Branch.Success : Branch.None) | (fail ? Branch.Fail : Branch.None) |
                                   (recursive ? Branch.Recursive : Branch.None);
                        stack.Push((int)flag);
                        // Reset the flags
                        success = fail = recursive = false;

                        using (var en = lst.GetEnumerator())
                        {
                            // Take first element
                            en.MoveNext();
                            city = en.Current;
                            // Push rest to stack
                            while(en.MoveNext()) 
                                stack.Push(en.Current);
                        }
                    }
                }
                else
                {
                    fail = true;
                    if(stack.Count==0)
                        break;
                    city = stack.Pop();

                }

                if (city < 0)
                {
                    var flags = (Branch)city;
                    if (!success && recursive && fail)
                    {
                        recursive = false;
                    }
                    else if (success && recursive)
                    {
                        return -1; // Infinite path
                    }

                    success |= flags.HasFlag(Branch.Success);
                    fail |= flags.HasFlag(Branch.Fail);
                    recursive |= flags.HasFlag(Branch.Recursive);

                    city = stack.Pop();
                    tmpPathCount += counters.Pop();
                    passed[city] += tmpPathCount;
                }
                else if (city == dest)
                {
                    success = true;
                    tmpPathCount++;
                    paths++;
                }
            }

            return paths;
        }
    }
}
