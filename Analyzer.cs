using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEDT
{
    class Analyzer
    {
        static public double pl(int length)
        {
            return length;
        }

        static public double lch(int pl, int maxDepth = 4)
        {
            return -Math.Log(pl/2*maxDepth);
        }

        static public double res()
        {


            return 0.0;
        }

        static public double gloss()
        {

            return 0.0;
        }

    }
}
