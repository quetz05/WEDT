using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;

namespace WEDT
{
    class Program
    {

        // algorytm StrubePonzetto
        static int Algo1(String word1, String word2, String fileName)
        {
            StrubePonzetto sp = new StrubePonzetto(word1, word2);

            DateTime start = DateTime.Now;
            int result = sp.Run();
            DateTime end = DateTime.Now;
            TimeSpan interval = end.Subtract(start);

            Console.WriteLine("Algorytm wykonywał się " + interval.Milliseconds + "ms.");

            if (result == 1)
                Console.WriteLine("Brak jednego ze słów w bazie!");
            else if (result == 2)
                Console.WriteLine("Brak wspólnej kategorii");

            String c = sp.ClassifyWords();
            Console.WriteLine();
            Console.WriteLine();

           // File.WriteAllLines(fileName, null);
            String toFile = word1 + ";" + word2 + ";" + sp.pathLength + ";" + c + ";" + interval.Milliseconds;
            TextWriter tw = new StreamWriter(fileName, true);
            tw.WriteLine(toFile);
            tw.Close();



            return sp.pathLength;
        }

        // algorytm MilneWitten
        static double Algo2(String word1, String word2, String fileName)
        {
            MilneWitten mw = new MilneWitten(word1, word2);

            DateTime start = DateTime.Now;
            int result = mw.Run();
            DateTime end = DateTime.Now;
            TimeSpan interval = end.Subtract(start);
            Console.WriteLine("Algorytm wykonywał się " + interval.Milliseconds + "ms.");

            if (result == 1)
                Console.WriteLine("Brak jednego ze słów w bazie!");
            Console.WriteLine("Cosinus kąta: " + mw.Cosinus);
            String c = mw.ClassifyWords();
            Console.WriteLine();
            Console.WriteLine();

            //File.WriteAllLines(fileName, null);
            String toFile = word1 + ";" + word2 + ";" + mw.Cosinus + ";" + c + ";" + interval.Milliseconds;
            TextWriter tw = new StreamWriter(fileName, true);
            tw.WriteLine(toFile);
            tw.Close();

            return mw.Cosinus;
        }

        // algorytm StrubePonzettoOur
        static int Algo3(String word1, String word2, String fileName)
        {
            StrubePonzettoOur sp = new StrubePonzettoOur(word1, word2);

            DateTime start = DateTime.Now;
            int result = sp.Run();
            DateTime end = DateTime.Now;
            TimeSpan interval = end.Subtract(start);

            Console.WriteLine("Algorytm wykonywał się " + interval.Milliseconds + "ms.");

            if (result == 1)
                Console.WriteLine("Brak jednego ze słów w bazie!"); 
                    
            String c = sp.ClassifyWords();

            Console.WriteLine();
            Console.WriteLine();
           // File.WriteAllLines(fileName, null);
            String toFile = word1 + ";" + word2 + ";" + sp.lengthList.ToArray().Max() + ";" + c + ";" + interval.Milliseconds;
            TextWriter tw = new StreamWriter(fileName, true);
            tw.WriteLine(toFile);
            tw.Close();


            return sp.lengthList.ToArray().Max();
        }


        static void Main(string[] args)
        {
             String algo = "";
             String finishKey = "";

            String inFile = "hasla.txt";
            String outFile = "wyniki.txt";

             while (finishKey != "9")
             {
                    algo = "";
                    while (algo != "1" && algo != "2" && algo != "3")
                    {
                        Console.WriteLine("_______________________________________________________________________");
                        Console.WriteLine();
                        Console.WriteLine("(plik wejściowy - " + inFile + "; plik wyjściowy - " + outFile + ")");
                        Console.WriteLine();
                        Console.WriteLine("Ktorego algorytmu chcesz uzyc? Wpisz odpowiedni numer i kliknij ENTER.");
                        Console.WriteLine("(1) Strube & Ponzetto");
                        Console.WriteLine("(2) Milne & Witten");
                        Console.WriteLine("(3) Strube & Ponzetto - ulepszony");
                        Console.WriteLine("(9) Koniec programu");
                        algo = Console.ReadLine();

                        if (algo == "9")
                            return;
                        else if (algo != "1" && algo != "2" && algo != "3")
                            Console.WriteLine("Wpisz poprawna wartosc!");
                        Console.WriteLine();

                        String[] lines = File.ReadAllLines(inFile);
                        File.WriteAllText(outFile, "");

                        foreach(String line in lines)
                        {
                            String[] words = line.Split(';');

                            switch (algo)
                            {
                                case "1": Algo1(words[0], words[1], outFile); break;
                                case "2": Algo2(words[0], words[1], outFile); break;
                                case "3": Algo3(words[0], words[1], outFile); break;
                                default: Console.WriteLine("BŁĄD Z WYBOREM ALGORYTMU!!"); break;
                            }

                        }

                    }
              }
        }

        //static void Main(string[] args)
        //{
        //    String algo = "";
        //    String finishKey = "";


        //    while (finishKey != "9")
        //    {
        //        algo = "";
        //        while (algo != "0" && algo != "1" && algo != "2" && algo != "3")
        //        {
        //            Console.WriteLine("Ktorego algorytmu chcesz uzyc? Wpisz odpowiedni numer");
        //            Console.WriteLine("(0) Wszystkie algorytmy");
        //            Console.WriteLine("(1) Strube & Ponzetto");
        //            Console.WriteLine("(2) Milne & Witten");
        //            Console.WriteLine("(3) Strube & Ponzetto - ulepszony");
        //            Console.WriteLine("(9) Koniec programu");
        //            algo = Console.ReadLine();

        //            if (algo == "9")
        //                return;
        //            else if (algo != "0" && algo != "1" && algo != "2" && algo != "3")
        //                Console.WriteLine("Wpisz poprawna wartosc!");
        //            Console.WriteLine();
        //        }

        //        Console.WriteLine("Wpisz dwa slowa, ktorych relacje chcesz sprawdzic:");
        //        String word1 = Console.ReadLine();
        //        String word2 = Console.ReadLine();


        //        switch (algo)
        //        {
        //            case "0": Algo1(word1, word2); Algo2(word1, word2); Algo3(word1, word2); break;
        //            case "1": Algo1(word1, word2); break;
        //            case "2": Algo2(word1, word2); break;
        //            case "3": Algo3(word1, word2); break;
        //            default: Console.WriteLine("BLAD!!"); break;
        //        }

        //        Console.WriteLine("");
        //        Console.WriteLine("");

        //        Console.WriteLine("Jeśli chcesz zakończyć program kliknij 9 (w przeciwnym wpisz cokolwiek):");
        //        finishKey = Console.ReadLine();
        //        Console.WriteLine("");
        //        Console.WriteLine("");
        //    }

        //}
    }
}
