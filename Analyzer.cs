using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEDT
{

    enum Classify
    {
        NotConnected = 0,
        WeakConnected = 1,
        MediumConnected = 2,
        StrongConnected = 3,
        TheSame = 4
    }

    class Analyzer
    {
        static public void PrintConnection(Classify c)
        {
            switch (c)
            {
                case Classify.NotConnected:
                    Console.WriteLine("Brak powiązania"); break;
                case Classify.WeakConnected:
                    Console.WriteLine("Słabe powiązanie"); break;
                case Classify.MediumConnected:
                    Console.WriteLine("Średnie powiązanie"); break;
                case Classify.StrongConnected:
                    Console.WriteLine("Silne powiązanie"); break;
                case Classify.TheSame:
                    Console.WriteLine("To samo"); break;
                default:
                    Console.WriteLine("Brak powiązania"); break;
            }
        }


        static public double pl(int length)
        {
            return length;
        }

        static public double lch(int pl, int maxDepth = 4)
        {
            return -Math.Log(pl/2*maxDepth);
        }

    }
}
