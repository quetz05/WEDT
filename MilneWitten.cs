using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEDT.DataProvider;



namespace WEDT
{

    class MilneWitten
    {
        String word1;
        String word2;

        private WikiPagelinksProvider wpp;
        private WikiRedirectsProvider wrp;
        private WikiCategoryProvider wcp;

        // pobrane w dniu 14.01.2015r.
        const int amountLinks = 1086603;

        public double Cosinus;


        public MilneWitten(String word1, String word2)
        {
            wpp = new WikiPagelinksProvider();
            wrp = new WikiRedirectsProvider();
            wcp = new WikiCategoryProvider();
            this.word1 = word1;
            this.word2 = word2;
            Cosinus = -1;

        }


        public int Run()
        {
            Console.WriteLine("--- Algorytm Milne-Witten...");
            ChooseMeaning();


            return 0;
        }

        private List<String> GetMeanings(String wordRedirect)
        {
            List<String> lexicalAssociationList = new List<String>();
            String[] ambiguous = wpp.disambiguates(wordRedirect);

            foreach (String word in ambiguous)
                lexicalAssociationList.Add(word);

            return lexicalAssociationList;
        }


        private List<double> GetVector(List<String> ownLinks, List<String> allLinks)
        {
            List<double> vec = new List<double>();
            foreach(String link in allLinks)
            {
                double value = 0;
                if(ownLinks.Contains(link))
                {
                    List<String> list = wpp.pagelinksTo(link);

                    if(list != null)
                        value = Math.Log(amountLinks / list.Count);
                    else
                        value = 0;
                }
                   
                vec.Add(value);
            }

            return vec;
        }


        private double GetCosinus(List<double> vec1, List<double> vec2)
        {
            if(vec1.Count != vec2.Count)
                return -1;

            double up = 0;
            double a = 0;
            double b = 0;

            for(int i = 0 ; i<vec1.Count; i++)
            {
                up += vec1.ToArray()[i] * vec2.ToArray()[i];
                a += vec1.ToArray()[i] * vec1.ToArray()[i];
                b += vec2.ToArray()[i] * vec2.ToArray()[i];
            }

            double angle = up / (Math.Sqrt(a) * Math.Sqrt(b));

            return angle;

        }

        // Step 1
        private bool ChooseMeaning()
        {

            String word1Redirects = wrp.redirect(word1);
            String word2Redirects = wrp.redirect(word2);

            // sprawdzenie przekierowania
            if (word1Redirects == "" || word1Redirects == null)
                word1Redirects = word1;
            if (word2Redirects == "" || word2Redirects == null)
                word2Redirects = word2;

            List<String> lexicalAssociationList1 = wpp.pagelinksFrom(word1Redirects);
            List<String> lexicalAssociationList2 = wpp.pagelinksFrom(word2Redirects);


            String[] intersected = lexicalAssociationList1.Intersect(lexicalAssociationList2).ToArray<String>();

            if(intersected.Length == 0)
            {
                Cosinus = 90;
                return true;
            }

            List<String> lexAssList = new List<String>();

            foreach (String w in lexicalAssociationList1)
                lexAssList.Add(w);

            foreach (String w in lexicalAssociationList2)
                lexAssList.Add(w);


           //Step 1.5
            List<double> vec1 = GetVector(lexicalAssociationList1, lexAssList);
            List<double> vec2 = GetVector(lexicalAssociationList2, lexAssList);

            if (lexAssList.Count == 0)
                return false;

            Cosinus = GetCosinus(vec1, vec2);

            return true;
        }


        public void ClassifyWords()
        {
            Classify c = Classify.NotConnected;

            Console.WriteLine("Powiązanie semantyczne między wyrazami: ");

            if (Cosinus == 0)
                c = Classify.NotConnected;
            else if (Cosinus == 1)
                c = Classify.TheSame;
            else if (Cosinus <= 0.3)
                c = Classify.WeakConnected;
            else if (Cosinus <= 0.7)
                c = Classify.MediumConnected;
            else
                c = Classify.StrongConnected;

            Analyzer.PrintConnection(c);
        }

    }
}
