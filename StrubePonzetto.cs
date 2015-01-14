using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEDT.DataProvider;

namespace WEDT
{

    class StrubePonzetto
    {
        public String word1;
        public String word2;

        private String word1Meaning;
        private String word2Meaning;

        private WikiPagelinksProvider wpp;
        private WikiRedirectsProvider wrp;
        private WikiCategoryProvider wcp;

        String commonCategory;

        Tree word1Tree;
        Tree word2Tree;

        int pathLength;

        const int maxCategoryDepth = 4;

        public StrubePonzetto(String word1, String word2)
        {
            wpp = new WikiPagelinksProvider();
            wrp = new WikiRedirectsProvider();
            wcp = new WikiCategoryProvider();
            this.word1 = word1;
            this.word2 = word2;

            pathLength = -1;
        }

        public int Run()
        {
            Console.WriteLine("--- Beginning of Strube-Ponzetto Algorithm...");
            Console.WriteLine("Searching for meaning of words...");
            if (!ChooseMeaning())
                return 1;
            Console.WriteLine("Category tree searching...");
            if (!CategoryTreeSearch())
                return 2;
            Console.WriteLine("Getting length of path...");
            GetLength();

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

       private List<String> GetCroppedMeanings(String[] Array)
       {
           List<String> croppedWordList = new List<String>();

           foreach (String word in Array)
           {
               int i1 = word.IndexOf('(');
               int i2 = word.IndexOf(')');

               if (i1 == -1 || i2 == -1)
                   continue;

               String croppedWord = word.Substring(i1+1,i2 -(i1+1));
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

           String word1Redirects = wrp.redirect(word1);
           String word2Redirects = wrp.redirect(word2);

           if (word1Redirects == "" || word1Redirects == null)
               word1Redirects = word1;
           if (word2Redirects == "" || word2Redirects == null)
               word2Redirects = word2;

           List<String> lexicalAssociationList1 = GetMeanings(word1Redirects);
           List<String> lexicalAssociationList2 = GetMeanings(word2Redirects);

           List<String> lexicalCroppedList1 = GetCroppedMeanings(lexicalAssociationList1.ToArray());
           List<String> lexicalCroppedList2 = GetCroppedMeanings(lexicalAssociationList2.ToArray());

           var intersect = lexicalCroppedList1.Intersect(lexicalCroppedList2, StringComparer.OrdinalIgnoreCase);
           String[] intersected = intersect.ToArray<String>();

           if(intersected.Length == 0)
           {
               if (lexicalAssociationList1.Count > 0)
                   word1Meaning = lexicalAssociationList1[0];
               else
                   word1Meaning = word1Redirects;

               if (lexicalAssociationList2.Count > 0)
                   word2Meaning = lexicalAssociationList2[0];
               else
                   word2Meaning = word2Redirects;
           }
           else
           {
               word1Meaning = FindWord(intersected[0], lexicalAssociationList1);
               word2Meaning = FindWord(intersected[0], lexicalAssociationList2);
           }

           return true;
       }

       // Step 2
       private bool CategoryTreeSearch()
       {
           word1Tree = new Tree(word1Meaning);
           word2Tree = new Tree(word2Meaning);

           String[] word1Categories = wcp.getCategories(word1Meaning);
           String[] word2Categories = wcp.getCategories(word2Meaning);
           
           // 1 - depth
           Console.WriteLine("\tCategoryTreeSearch:: 1 depth");
           word1Tree.addChilds(word1Categories);
           word2Tree.addChilds(word2Categories);
           String common = Tree.FindCommon(word1Tree, word2Tree);   
           if (common != "")
           {
               commonCategory = common;
               return true;
           }

           // 2 - depth
           Console.WriteLine("\tCategoryTreeSearch:: 2 depth");
           foreach (String cat1 in word1Categories)
               word1Tree.getChild(cat1).addChilds(wcp.getSubcategories(cat1));

           foreach (String cat2 in word2Categories)
              word2Tree.getChild(cat2).addChilds(wcp.getSubcategories(cat2));

           common = Tree.FindCommon(word1Tree, word2Tree);

           if (common != "")
           {
               commonCategory = common;
               return true;
           }

           // 3 - depth
           Console.WriteLine("\tCategoryTreeSearch:: 3 depth");
           foreach (Tree cat in word1Tree.children)
               foreach (Tree subCat in cat.children)
               {
                   String[] tab = wcp.getSubcategories(subCat.data);
                   if (tab != null)
                       cat.getChild(subCat.data).addChilds(tab);
               }


           foreach (Tree cat in word2Tree.children)
               foreach (Tree subCat in cat.children)
               {
                   String[] tab = wcp.getSubcategories(subCat.data);
                   if (tab != null)
                       cat.getChild(subCat.data).addChilds(tab);
               }


           if (common != "")
           {
               commonCategory = common;
               return true;
           }

           // 4 - depth
           Console.WriteLine("\tCategoryTreeSearch:: 4 depth");
           foreach (Tree cat in word1Tree.children)
               foreach (Tree subCat in cat.children)
                   foreach (Tree subsubCat in subCat.children)
                   {
                       String[] tab = wcp.getSubcategories(subsubCat.data);
                       if (tab != null)
                           subCat.getChild(subsubCat.data).addChilds(tab);
                   }

           foreach (Tree cat in word2Tree.children)
               foreach (Tree subCat in cat.children)
                   foreach (Tree subsubCat in subCat.children)
                   {
                       String[] tab = wcp.getSubcategories(subsubCat.data);
                       if (tab != null)
                           subCat.getChild(subsubCat.data).addChilds(tab);
                   }

           if (common != "")
           {
               commonCategory = common;
               return true;
           }

           return false;
       }


       private void GetLength()
       {
           pathLength = Tree.GetLength(word1Tree) + Tree.GetLength(word2Tree);

       }



    }
}
