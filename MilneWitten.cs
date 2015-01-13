using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEDT
{

    class MilneWitten
    {
        public String word1;
        public String word2;

        private String word1Meaning;
        private String word2Meaning;

        DataProvider.WikiPagelinksProvider wap;
        DataProvider.WikiRedirectsProvider wrp;
        DataProvider.WikiCategoryProvider wcp;

        static const int maxCategoryDepth = 4;

        public MilneWitten(String word1, String word2)
        {
            this.word1 = word1;
            this.word2 = word2;
        }



       private List<String> GetMeanings(String [] wordRedirects)
       {
           List<String> lexicalAssociationList = new List<String>();

           foreach (String word in wordRedirects)
           {
               String[] ambiguous = wap.Get(word);
               foreach (String w in ambiguous)
                   lexicalAssociationList.Add(w);
           }

           return lexicalAssociationList;
       }

       private List<String> GetCroppedMeanings(String[] Array)
       {
           List<String> croppedWordList = new List<String>();

           foreach (String word in Array)
           {
               int i1 = word.IndexOf('(');
               int i2 = word.IndexOf(')');

               if (i1 == -1 || i2 == -1)
                   continue;

               String croppedWord = word.Substring(i1,i2 - i1);
               croppedWordList.Add(croppedWord);
           }

           return croppedWordList;
       }

       String FindWord(String chars, List<String> array)
       {
           foreach (String w in array)
               if (w.Contains(chars))
                   return w;

           return "";
       }

       // Step 1
       private bool ChooseMeaning()
       {
           String[] word1Redirects = wrp.Get(word1);
           String[] word2Redirects = wrp.Get(word2);

           if(word1Redirects.Length == 0 || word2Redirects.Length == 0 )
               return false;

           List<String> lexicalAssociationList1 = GetMeanings(word1Redirects);
           List<String> lexicalAssociationList2 = GetMeanings(word2Redirects);

           List<String> lexicalCroppedList1 = GetCroppedMeanings(lexicalAssociationList1.ToArray());
           List<String> lexicalCroppedList2 = GetCroppedMeanings(lexicalAssociationList2.ToArray());

           String[] intersected = (String[])lexicalCroppedList1.Intersect(lexicalCroppedList2);

           if(intersected.Length == 0)
           {
               word1Meaning = lexicalAssociationList1[0];
               word2Meaning = lexicalAssociationList2[0];

           }
           else
           {
               word1Meaning = FindWord(intersected[0], lexicalAssociationList1);
               word2Meaning = FindWord(intersected[0], lexicalAssociationList2);
           }

           return true;
       }


       private bool CategoryTreeSearch()
       {


           String[] word1Categories = wcp.Get(word1Meaning);
           String[] word2Categories = wcp.Get(word2Meaning);

           foreach (String cat1 in word1Categories)
               foreach(String cat2 in word2Categories)
               {
                   // Tworzenie i dodawanie drzew

               }


           return true;
       }

    }
}
