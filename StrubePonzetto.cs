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
        public String trueWord1;
        public String trueWord2;
        public String word1;
        public String word2;

        protected String word1Meaning;
        protected String word2Meaning;

        protected WikiPagelinksProvider wpp;
        protected WikiRedirectsProvider wrp;
        protected WikiCategoryProvider wcp;

        protected String commonCategory;

        protected Tree word1Tree;
        protected Tree word2Tree;

        public int pathLength;

        protected const int maxCategoryDepth = 4;

        public StrubePonzetto(String word1, String word2)
        {
            wpp = new WikiPagelinksProvider();
            wrp = new WikiRedirectsProvider();
            wcp = new WikiCategoryProvider();
            trueWord1 = word1;
            trueWord2 = word2;
            this.word1 = word1;
            this.word2 = word2;

            pathLength = -1;
        }

        virtual public int Run()
        {
            Console.WriteLine("--- Algorytm Strube-Ponzetto ("+word1 + "," + word2+")");
            Console.WriteLine("Poszukiwanie znaczenia słowa...");
            if (!ChooseMeaning())
                return 1;
            Console.WriteLine("Przeszukiwanie drzewa kategorii...");
            if (!CategoryTreeSearch())
                return 2;
            Console.WriteLine("Pobieranie długości ścieżki...");
            GetLength(commonCategory);
            Console.WriteLine("Długość ścieżki: " + Analyzer.pl(pathLength));
            //Console.WriteLine("lch: "+ Analyzer.lch(pathLength));
            return 0;

        }

       protected List<String> GetMeanings(String wordRedirect)
       {
           List<String> lexicalAssociationList = new List<String>();
           String[] ambiguous = wpp.disambiguates(wordRedirect);

           foreach (String word in ambiguous)
                   lexicalAssociationList.Add(word);

           return lexicalAssociationList;
       }

       protected List<String> GetCroppedMeanings(String[] Array)
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

       protected String FindWord(String chars, List<String> array)
       {
           foreach (String w in array)
               if (w.Contains(chars))
                   return w;

           return "";
       }

       // Step 1
       virtual protected bool ChooseMeaning()
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
       protected bool CategoryTreeSearch()
       {
           word1Tree = new Tree(word1Meaning);
           word2Tree = new Tree(word2Meaning);

           String[] word1Categories = wcp.getCategories(word1Meaning);
           String[] word2Categories = wcp.getCategories(word2Meaning);
           

           if(word2Meaning == word1Meaning)
           {
               pathLength = 0;
               return true;
           }

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
           {

               List<String> tab = wcp.getUbercategory(cat1);
               Tree child = word1Tree.getChild(cat1);
               if (tab != null && child != null)
                   child.addChilds(tab.ToArray());

           }

           foreach (String cat2 in word2Categories)
           {

               List<String> tab = wcp.getUbercategory(cat2);
               Tree child = word2Tree.getChild(cat2);
               if (tab != null && child != null)
                   child.addChilds(tab.ToArray());
           }
               //if (cat2 != null)
               //     word2Tree.getChild(cat2).addChilds(wcp.getUbercategory(cat2).ToArray());

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
                   List<String> tab = wcp.getUbercategory(subCat.data);
                   Tree child = subCat.getChild(subCat.data);
                   if (tab != null && child != null)
                       child.addChilds(tab.ToArray());
               }


           foreach (Tree cat in word2Tree.children)
               foreach (Tree subCat in cat.children)
               {
                   List<String> tab = wcp.getUbercategory(subCat.data);
                   Tree child = subCat.getChild(subCat.data);
                   if (tab != null && child != null)
                       child.addChilds(tab.ToArray());
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
                       List<String> tab = wcp.getUbercategory(subsubCat.data);
                       Tree child = subCat.getChild(subsubCat.data);
                       if (tab != null && child != null)
                           child.addChilds(tab.ToArray());
                   }

           foreach (Tree cat in word2Tree.children)
               foreach (Tree subCat in cat.children)
                   foreach (Tree subsubCat in subCat.children)
                   {
                       List<String> tab = wcp.getUbercategory(subsubCat.data);
                       Tree child = subCat.getChild(subsubCat.data);
                       if (tab != null && child != null)
                           child.addChilds(tab.ToArray());
                   }

           if (common != "")
           {
               commonCategory = common;
               return true;
           }

           return false;
       }
       
       // zwraca podobieństwo od 0 - 10 (0 - niepołączone; 10 - to samo słowo)
       public int GetSimilarity()
       {
           switch(pathLength)
           {
               case 0: return 10;
               case 1: return 9;
               case 2: return 8;
               case 3: return 7;
               case 4: return 6;
               case 5: return 5;
               case 6: return 4;
               case 7: return 2;
               case 8: return 1;
               case -1: return 0;
               default: return -1;
           }

       }

       protected int GetLength(Tree tree, String word)
       {
           if (tree.data == word)
               return 0;


           foreach (Tree cat in tree.children)
               if (cat.data == word)
                   return 1;

           foreach (Tree cat in tree.children)
               foreach (Tree subCat in cat.children)
                   if (subCat.data == word)
                       return 2;

           foreach (Tree cat in tree.children)
               foreach (Tree subCat in cat.children)
                   foreach (Tree subsubCat in subCat.children)
                       if (subsubCat.data == word)
                           return 3;


           foreach (Tree cat in tree.children)
               foreach (Tree subCat in cat.children)
                   foreach (Tree subsubCat in subCat.children)
                       foreach (Tree subsubsubCat in subsubCat.children)
                           if (subsubsubCat.data == word)
                               return 4;

           return -1;

       }


       protected void GetLength(String common)
       {
           if (word1Meaning == word2Meaning)
           {
               pathLength = 0;
               return;
           }
           pathLength = GetLength(word1Tree, common) + GetLength(word2Tree, common);
       }


       public virtual String ClassifyWords()
       {
           int p = pathLength;
           Classify c = Classify.NotConnected;

           Console.WriteLine("Powiązanie semantyczne między wyrazami: ");

           if (p < 0)
               c = Classify.NotConnected;
           else if (p == 0)
               c = Classify.TheSame;
           else if (p <= 3)
               c = Classify.StrongConnected;
           else if (p <= 6)
               c = Classify.MediumConnected;
           else
               c = Classify.WeakConnected;

           return Analyzer.PrintConnection(c);
       }


    }
}
