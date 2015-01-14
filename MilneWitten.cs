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


        int word1Path;
        int word2Path;
        int amountLinks;

        private String word1Meaning;
        private String word2Meaning;

        public MilneWitten(String word1, String word2)
        {
            wpp = new WikiPagelinksProvider();
            wrp = new WikiRedirectsProvider();
            wcp = new WikiCategoryProvider();
            this.word1 = word1;
            this.word2 = word2;

            word1Path = -1;
            word2Path = -1;

            // TODO Zmienić na prawdziwą wartość
            amountLinks = 10000;
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
                    // TODO zmienić na poprawną funkcję
                    value = Math.Log(amountLinks / wpp.pagelinks(link).Length);

                vec.Add(value);
            }

            return vec;
        }


        // TODO NIE ROZUMIĘ JAK!!?
        private double GetCosinus(double a, double b)
        {
            return 0.0;


        }

        // Step 1
        private bool ChooseMeaning()
        {

            String word1Redirects = wrp.redirect(word1);
            String word2Redirects = wrp.redirect(word2);

            if (word1Redirects == "")
                word1Redirects = word1;
            if (word2Redirects == "")
                word2Redirects = word2;

            List<String> lexicalAssociationList1 = GetMeanings(word1Redirects);
            List<String> lexicalAssociationList2 = GetMeanings(word2Redirects);
            List<String> lexAssList = new List<String>();

            foreach (String w in lexicalAssociationList1)
                lexAssList.Add(w);

            foreach (String w in lexicalAssociationList2)
                lexAssList.Add(w);


           //Step 1.5
            List<double> vec1 = GetVector(lexicalAssociationList1, lexAssList);
            List<double> vec2 = GetVector(lexicalAssociationList2, lexAssList);

            Dictionary<double, ArticlePair> cosVec = new Dictionary<double, ArticlePair>();


            // Step 1.75
           for (int i = 0; i<vec1.Count; i++)
               for (int j = 0; j<vec2.Count; j++)
               {
                   cosVec.Add(GetCosinus(vec1[i], vec2[j]), new ArticlePair(i, j));
               }

           var max = cosVec.Max();

          // ArticlePair articles = max.Value;

            return true;
        }





        private bool MeasuringRelatedness()
        {


            





            return false;
        }
    }
}
