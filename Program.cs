using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEDT
{
    class Program
    {
        static void Main(string[] args)
        {

            // SP
            StrubePonzetto sp = new StrubePonzetto("wieża (ujednoznacznienie)", "król (ujednoznacznienie)");

            int result = sp.Run();

            if (result == 1)
                Console.WriteLine("Brak jednego ze słów w bazie!");
            else if (result == 2)
                Console.WriteLine("Brak wspólnej kategorii");

            Console.WriteLine();
            Console.WriteLine();

            // MW
            MilneWitten mw = new MilneWitten("wieża (ujednoznacznienie)", "król (ujednoznacznienie)");
            result = mw.Run();

            if (result == 1)
                Console.WriteLine("Brak jednego ze słów w bazie!");
            else if (result == 2)
                Console.WriteLine("Brak wspólnej kategorii");





            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Koniec programu...");

            Console.ReadLine();

        }
    }
}
