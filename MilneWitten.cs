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


        int word1Path;
        int word2Path;


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
        }

        private List<String> GetMeanings(String wordRedirect)
        {
            List<String> lexicalAssociationList = new List<String>();
            String[] ambiguous = wpp.disambiguates(wordRedirect);

            foreach (String word in ambiguous)
                lexicalAssociationList.Add(word);

            return lexicalAssociationList;
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

           //Step 1.5
           double maxAngle = 0;
           int amountLinks = 10000;

           //foreach (String m1 in lexicalAssociationList1)
           //    foreach(String m2 in lexicalAssociationList2)
           //    {
           //        String [] links = wpp.pagelinks(m1);
           //        double angle = Math.Log(amountLinks/);


           //    }

            //List<String> lexicalCroppedList1 = GetCroppedMeanings(lexicalAssociationList1.ToArray());
            //List<String> lexicalCroppedList2 = GetCroppedMeanings(lexicalAssociationList2.ToArray());

            //var intersect = lexicalCroppedList1.Intersect(lexicalCroppedList2, StringComparer.OrdinalIgnoreCase);
            //String[] intersected = intersect.ToArray<String>();

            //if (intersected.Length == 0)
            //{
            //    if (lexicalAssociationList1.Count > 0)
            //        word1Meaning = lexicalAssociationList1[0];
            //    else
            //        word1Meaning = word1Redirects;

            //    if (lexicalAssociationList2.Count > 0)
            //        word2Meaning = lexicalAssociationList2[0];
            //    else
            //        word2Meaning = word2Redirects;
            //}
            //else
            //{
            //    word1Meaning = FindWord(intersected[0], lexicalAssociationList1);
            //    word2Meaning = FindWord(intersected[0], lexicalAssociationList2);
            //}

            return true;
        }





        private bool MeasuringRelatedness()
        {


            





            return false;
        }
    }
}
