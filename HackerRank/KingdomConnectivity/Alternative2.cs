using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingdomConnectivity
{
    class Alternative2
    {
        #region Fields
        
        private static int s_pathCount = 0;
        private static Stack<Call> s_CallStack = new Stack<Call>();
        private static HashSet<int> s_PassedCities = new HashSet<int>();

        #endregion


        #region Nested Class

        private class Call
        {
            public int parent;
            public Queue<int> children;
            public bool success = false;
            public bool fail = false;
            public bool recursive = false;
        }

        #endregion

        #region Methods

        private static int Solve(int dest, Dictionary<int, List<int>> nexts)
        {
            int city = 1; // Initial city
            Call currentCall = null;


            while (true)
            {
                if (city == dest)
                {
                    s_pathCount++;
                    
                    if(currentCall == null)
                        break;

                    currentCall.success = true;

                    if (currentCall.children.Count > 0)
                    {

                        city = currentCall.children.Dequeue();
                        
                    }
                    else
                    {
                        if (currentCall.success && currentCall.recursive)
                            return -1;
                        currentCall = s_CallStack.Pop();
                        if(currentCall == null)
                            break;
                        city = currentCall.children.Dequeue();
                    }
                    continue;
                }

                if (s_PassedCities.Contains(city))
                {
                    if (currentCall != null)
                        currentCall.recursive = true;

                }

                if (nexts.ContainsKey(city))
                {
                    var lst = nexts[city];
                    if (lst.Count == 1)
                    {
                        city = lst[0];
                    }
                    else
                    {
                        s_CallStack.Push(currentCall);
                        currentCall = new Call
                        {
                            parent = city,
                            children = new Queue<int>(lst)
                        };
                        city = currentCall.children.Dequeue();
                    }
                }
                else
                {
                    if(currentCall==null)
                        break;
                    
                }
            }
            return s_pathCount;
        }

        //private static int GetCity()
        //{

        //}

        public static int Solve2(int dest, Dictionary<int, List<int>> nexts)
        {
            long cityCount = 0;
            Stack<int> nextStack = new Stack<int>();
            bool success = false;
            bool recursive = false;
            bool fail = false;

            int city = 1;
            while (true)
            {
                if (city == dest)
                {
                    success = true;
                    cityCount++;
                    // return
                }
                else if (!nexts.ContainsKey(city))
                {

                }
            }
        }


        #endregion


    }
}
