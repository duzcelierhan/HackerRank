using System;
using System.Linq;

namespace BiggerIsGreater
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = biggerIsGreater("dkhc");

            Console.WriteLine(str);
        }

        // Complete the biggerIsGreater function below.
        static string biggerIsGreater(string w)
        {
            var ca = w.ToCharArray();
            (int iFinal, int jFinal) = FindSwap2(ca);

            Console.WriteLine($"{iFinal} - {jFinal}");
            if (iFinal == -1 || jFinal == -1)
                return "no answer";
            var unTouched = w.Substring(0, jFinal);
            var sSwap1 = w.Substring(jFinal + 1, iFinal - jFinal - 1);
            var sSwap2 = iFinal < w.Length - 1 ? w.Substring(iFinal + 1) : string.Empty;
            var swap = sSwap1 + w[jFinal] + sSwap2;
            var swapRev = new string(swap.Reverse().ToArray());

            return unTouched + w[iFinal] + swapRev;
        }


        static (int iFinal, int jFinal) FindSwap(char[] ca)
        {
            var l = ca.Length;

            for (var i = l - 1; i > 0; i--)
            {
                for (var j = i - 1; j >= 0; j--)
                {
                    if (ca[i] > ca[j])
                    {
                        return (i, j);
                    }
                }
            }

            return (-1, -1);
        }

        static (int iFinal, int jFinal) FindSwap2(char[] ca)
        {
            var l = ca.Length;
            var iFinal = -1;
            var jFinal = -1;
            for (var i = l - 1; i > 0; i--)
            {
                if (ca[i] > ca[i - 1])
                {
                    jFinal = i;
                    break;
                }
            }
            if (jFinal == 0 || jFinal == -1) return (-1, -1);

            var pivot = ca[jFinal - 1];
            var swapChar = char.MaxValue;
            var swapIndex = -1;

            for (var i = l - 1; i >= jFinal; i--)
            {
                if (ca[i] > pivot)
                {
                    if (ca[i] < swapChar)
                    {
                        swapChar = ca[i];
                        swapIndex = i;
                    }
                }
            }
            if (swapIndex == -1)
            {
                Console.WriteLine("SwapIndex is -1!!!");
            }
            iFinal = swapIndex;

            return (iFinal, jFinal - 1);
        }
    }
}
