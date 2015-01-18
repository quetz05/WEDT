using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEDT
{
    class Program
    {

        static void Algo1(String word1, String word2)
        {
            // SP
            StrubePonzetto sp = new StrubePonzetto(word1, word2);

            int result = sp.Run();

            if (result == 1)
                Console.WriteLine("Brak jednego ze słów w bazie!");
            else if (result == 2)
                Console.WriteLine("Brak wspólnej kategorii");

            Console.WriteLine("");
            Console.WriteLine("Podobieństwo (0-10) wynosi: " + sp.GetSimilarity());

            Console.WriteLine();
            Console.WriteLine();
        }

        static void Algo2(String word1, String word2)
        {

            // MW
            MilneWitten mw = new MilneWitten(word1, word2);
            int result = mw.Run();

            if (result == 1)
                Console.WriteLine("Brak jednego ze słów w bazie!");
            else if (result == 2)
                Console.WriteLine("Brak wspólnej kategorii");

        }


        static void Main(string[] args)
        {
            String algo = "";
            String finishKey = "";


            while (finishKey != "0")
            {
                while (algo != "0" && algo != "1" && algo != "2")
                {
                    Console.WriteLine("Ktorego algorytmu chcesz uzyc? Wpisz odpowiedni numer");
                    Console.WriteLine("(0) Oba algorytmy");
                    Console.WriteLine("(1) Strube & Ponzetto");
                    Console.WriteLine("(2) Milne & Witten");
                    algo = Console.ReadLine();

                    if (algo != "0" && algo != "1" && algo != "2")
                        Console.WriteLine("Wpisz poprawna wartosc!");
                    Console.WriteLine();
                }

                Console.WriteLine("Wpisz dwa slowa, ktorych relacje chcesz sprawdzic:");
                String word1 = Console.ReadLine();
                String word2 = Console.ReadLine();


                switch (algo)
                {
                    case "0": Algo1(word1, word2); Algo2(word1, word2); break;
                    case "1": Algo1(word1, word2); break;
                    case "2": Algo2(word1, word2); break;
                    default: Console.WriteLine("BLAD!!"); break;
                }

                Console.WriteLine("");
                Console.WriteLine("");

                Console.WriteLine("Jeśli chcesz zakończyć program kliknij 0 (w przeciwnym wpisz cokolwiek):");
                finishKey = Console.ReadLine();
                Console.WriteLine("");
                Console.WriteLine("");
            }

        }
    }
}
