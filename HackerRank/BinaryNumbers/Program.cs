using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            int maxConsequentOnes = 0;
            int tmpConsequents = 0;
            for (int i = 0; i < sizeof(int)*8; i++, n >>= 1)
            {
                if ((n & 0x00000001) == 0)
                {
                    maxConsequentOnes = maxConsequentOnes < tmpConsequents ? tmpConsequents : maxConsequentOnes;
                    tmpConsequents = 0;
                    continue;
                }
                tmpConsequents++;
            }
            //maxConsequentOnes = maxConsequentOnes < tmpConsequents ? tmpConsequents : maxConsequentOnes;
            Console.WriteLine(maxConsequentOnes);
        }
    }
}
