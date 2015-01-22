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
        static public String PrintConnection(Classify c)
        {
            switch (c)
            {
                case Classify.NotConnected:
                    Console.WriteLine("Brak powiązania"); return "Brak powiązania";
                case Classify.WeakConnected:
                    Console.WriteLine("Słabe powiązanie"); return "Słabe powiązanie";
                case Classify.MediumConnected:
                    Console.WriteLine("Średnie powiązanie"); return "Średnie powiązanie";
                case Classify.StrongConnected:
                    Console.WriteLine("Silne powiązanie"); return "Silne powiązanie";
                case Classify.TheSame:
                    Console.WriteLine("To samo"); return "To samo";
                default:
                    Console.WriteLine("Brak powiązania"); return "Brak powiązania";
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
