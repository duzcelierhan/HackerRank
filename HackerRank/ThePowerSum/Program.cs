using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;
using System.Runtime.CompilerServices;

class Solution
{
    // A challenging one
    // Complete the powerSum function below.
    static int powerSum(int X, int N)
    {
        if (X == 0) return 0;
        var start = NthRoot(X, N);
        int rest;
        HashSet<List<int>> sums = new HashSet<List<int>>(new ListEqualityComparer());
        for (int i = start; i > 0; i--)
        {
            var list = new List<int> { i };
            var pw = (int)MathF.Pow(i, N);
            rest = X - pw;
            if (rest > pw) break;

            while (true)
            {
                if(rest == 0)
                {
                    //list.Sort();
                    sums.Add(list);
                    break;
                }
                var r = NthRoot(rest, N);
                if (r == 0) break;
                list.Add(r);
                rest -= r;
            } 
        }

        return sums.Count;
    }

    private class ListEqualityComparer : IEqualityComparer<List<int>>
    {
        public bool Equals([AllowNull] List<int> x, [AllowNull] List<int> y)
        {
            if (x.Count != y.Count) return false;
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] != y[i]) return false;
            }
            return true;
        }
        public int GetHashCode([DisallowNull] List<int> obj)
        {
            int h = 0;
            for (int i = 0; i < obj.Count; i++)
            {
                h ^= obj[i].GetHashCode();
            }
            return h;
        }
    }

    static int NthRoot(int x, int n)
    {
        if (x == 1) return 1;
        return (int)(Math.Pow(x, 1.0/ n));
    }

    static void Main(string[] args)
    {
        //TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);
        TextWriter textWriter = Console.Out;

        int X = Convert.ToInt32(Console.ReadLine());

        int N = Convert.ToInt32(Console.ReadLine());

        int result = powerSum(X, N);

        textWriter.WriteLine(result);

        textWriter.Flush();
        textWriter.Close();
    }
}
