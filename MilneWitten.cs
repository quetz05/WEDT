using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEDT.DataProvider;



namespace WEDT
{

    class ArticlePair
    {
        public ArticlePair(int f, int s)
        {
            first = f;
            second = s;
        }
        public int first;
        public int second;

    }




    class MilneWitten
    {
        String word1;
        String word2;

        private WikiPagelinksProvider wpp;
        private WikiRedirectsProvider wrp;
        private WikiCategoryProvider wcp;

        // pobrane w dniu 14.01.2015r.
        const int amountLinks = 1086603;

        private String art1;
        private String art2;


        public MilneWitten(String word1, String word2)
        {
            wpp = new WikiPagelinksProvider();
            wrp = new WikiRedirectsProvider();
            wcp = new WikiCategoryProvider();
            this.word1 = word1;
            this.word2 = word2;


        }


        public int Run()
        {
            Console.WriteLine("--- Algorytm Strube-Ponzetto...");
            Console.WriteLine("Poszukiwanie znaczenia słowa...");
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
                    value = Math.Log(amountLinks / wpp.pagelinksTo(link).Length);

                vec.Add(value);
            }

            return vec;
        }


        private double GetCosinus(double a, double b)
        {
            // TODO poprawić jak cosinus ujemny
            if (b != 0)
                return Math.Cos(a / b) * 180.0 / Math.PI;
            else
                return 90;

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

            // sprawdzenie ujednoznacznienia
            String word1Disam = word1 + " (ujednoznacznienie)";
            String word2Disam = word2 + " (ujednoznacznienie)";


            List<String> lexicalAssociationList1 = GetMeanings(word1Disam);
            List<String> lexicalAssociationList2 = GetMeanings(word2Disam);

            if (lexicalAssociationList1.Count == 0)
            {
                lexicalAssociationList1 = GetMeanings(word1Redirects);
            }

            if (lexicalAssociationList2.Count == 0)
            {
                lexicalAssociationList2 = GetMeanings(word2Redirects);
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

            Dictionary<ArticlePair, double> cosVec = new Dictionary<ArticlePair, double>();


            // Step 1.75
           for (int i = 0; i<vec1.Count; i++)
               for (int j = 0; j<vec2.Count; j++)
               {
                   cosVec.Add(new ArticlePair(i, j), GetCosinus(vec1[i], vec2[j]));
               }

           double min = 90;
           int index1 = 0, index2 = 0;

           foreach(var el in cosVec)
           {
               if (el.Value < min)
               {
                   min = el.Value;
                   index1 = el.Key.first;
                   index2 = el.Key.second;
               }
           }

           art1 = lexAssList.ToArray()[index1];
           art2 = lexAssList.ToArray()[index2];

       

          // ArticlePair articles = max.Value;

            return true;
        }





        private bool MeasuringRelatedness()
        {


            





            return false;
        }
    }
}
