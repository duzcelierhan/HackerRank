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

        public static int Solve(int start, int dest, Dictionary<int, List<int>> nexts)
        {
            Stack<int> stack = new Stack<int>((int) 1e5);
            Stack<int> counters = new Stack<int>((int)1e5);
            Stack<int> tmpPath = new Stack<int>();
            int branchDepth = 0;
            int tmpPathCount = 0;
            Dictionary<int, int> keyNodes = new Dictionary<int, int>(dest);
            HashSet<int> passedNodes = new HashSet<int>();
            int paths = 0;
            bool success = false;
            bool fail = false;
            bool recursive = false;
            int city = start;
            List<int> lst;
            int tmpSuccessCount = 0;

            while (true)
            {
                
                tmpPath.Push(city);
                passedNodes.Add(city);
                branchDepth++;
            beginning:

                if (city == dest)
                {
                    Console.WriteLine(string.Join(",", tmpPath.Select(x => x.ToString())));
                    // Infinite paths
                    if (recursive)
                        return -1;
                    success = true;
                    tmpPathCount++;
                    paths++;
                    if (stack.Count == 0)
                        break;
                    for (int i = 0; i < branchDepth; i++)
                        passedNodes.Remove(tmpPath.Pop());
                    branchDepth = 0;
                    city = stack.Pop();
                    goto pass;
                }

                if (keyNodes.TryGetValue(city, out tmpSuccessCount))
                {
                    paths += tmpSuccessCount;

                    // Pop for continue
                    if (stack.Count == 0)
                        break;

                    for (int i = 0; i < branchDepth; i++)
                        passedNodes.Remove(tmpPath.Pop());

                    branchDepth = 0;// stack.Pop();
                    city = stack.Pop();
                }
                else if (nexts.TryGetValue(city, out lst))
                {

                    if (lst.Count == 1)
                    {
                        city = lst[0];
                    }
                    else
                    {
                        stack.Push(city);
                        stack.Push(branchDepth);
                        branchDepth = 0;
                        keyNodes.Add(city, 0);
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
                    
                    for (int i = 0; i < branchDepth; i++)
                        passedNodes.Remove(tmpPath.Pop());
                    branchDepth = 0;
                    //branchDepth = stack.Pop();
                    city = stack.Pop();
                }
            pass:
                if(passedNodes.Contains(city))
                {
                    // Infinite paths
                    if (success)
                        return -1;
                    recursive = true;
                    if (stack.Count == 0)
                        break;
                    for (int i = 0; i < branchDepth; i++)
                        passedNodes.Remove(tmpPath.Pop());
                    branchDepth = 0;
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

                    if (fail & success)
                        fail = false;
                    else if (!success && recursive && fail)
                        recursive = false;
                    else if (success && recursive)
                        return -1;

                    for (int i = 0; i < branchDepth; i++)
                        passedNodes.Remove(tmpPath.Pop());

                    branchDepth = stack.Pop();
                    city = stack.Pop();
                    
                    keyNodes[city] += tmpPathCount;
                    tmpPathCount += counters.Pop();
                    if (stack.Count == 0)
                        break;
                    city = stack.Pop();
                    goto pass;
                }
            }

            return paths;
        }
    }
}
