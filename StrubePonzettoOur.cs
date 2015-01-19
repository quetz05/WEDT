using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEDT
{
    class StrubePonzettoOur : StrubePonzetto
    {
        public StrubePonzettoOur(String word1, String word2) : base(word1, word2)
        { 

        }

        protected List<String> Meanings;


        override public int Run()
        {
            Console.WriteLine("--- Algorytm Strube-Ponzetto - OUR...");
            Console.WriteLine("Poszukiwanie znaczenia słowa...");
            if (!ChooseMeaning())
                return 1;
            Console.WriteLine("Przeszukiwanie drzewa kategorii...");
            if (!CategoryTreeSearch())
                return 2;
            Console.WriteLine("Pobieranie długości ścieżki...");
            GetLength();
            Console.WriteLine("pl: " + Analyzer.pl(pathLength));
            Console.WriteLine("lch: " + Analyzer.lch(pathLength));
            return 0;

        }



        // Step 1
        override protected bool ChooseMeaning()
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


            foreach (String w1 in lexicalAssociationList1)
                foreach (String w2 in lexicalAssociationList2)
                {

                    int length = CategoryTreeSearch(w1, w2);

                    Console.WriteLine("\t\t" + w1 + " " + w2 + ": " + length);


                }

            return true;
        }

        protected int CategoryTreeSearch(String w1, String w2)
        {
            Tree word1Tree = new Tree(w1);
            Tree word2Tree = new Tree(w2);

            String[] word1Categories = wcp.getCategories(w1);
            String[] word2Categories = wcp.getCategories(w2);


            if (w1 == w2)
            {
                pathLength = 0;
                return GetLength(word1Tree, word2Tree);
            }

            // 1 - depth
            word1Tree.addChilds(word1Categories);
            word2Tree.addChilds(word2Categories);
            String common = Tree.FindCommon(word1Tree, word2Tree);
            if (common != "")
            {
                commonCategory = common;
                return GetLength(word1Tree, word2Tree);
            }

            // 2 - depth
            foreach (String cat1 in word1Categories)
                //word1Tree.getChild(cat1).addChilds(wcp.getSubcategories(cat1));
                word1Tree.getChild(cat1).addChilds(wcp.getUbercategory(cat1));

            foreach (String cat2 in word2Categories)
                //word2Tree.getChild(cat2).addChilds(wcp.getSubcategories(cat2));
                word2Tree.getChild(cat2).addChilds(wcp.getUbercategory(cat2));

            common = Tree.FindCommon(word1Tree, word2Tree);

            if (common != "")
            {
                commonCategory = common;
                return GetLength(word1Tree, word2Tree);
            }

            // 3 - depth
            foreach (Tree cat in word1Tree.children)
                foreach (Tree subCat in cat.children)
                {
                    //String[] tab = wcp.getSubcategories(subCat.data);
                    String[] tab = wcp.getUbercategory(subCat.data);
                    if (tab != null)
                        // cat.getChild(subCat.data).addChilds(tab);
                        cat.getChild(subCat.data).addChilds(tab);
                }


            foreach (Tree cat in word2Tree.children)
                foreach (Tree subCat in cat.children)
                {
                    //String[] tab = wcp.getSubcategories(subCat.data);
                    String[] tab = wcp.getUbercategory(subCat.data);
                    if (tab != null)
                        //cat.getChild(subCat.data).addChilds(tab);
                        cat.getChild(subCat.data).addChilds(tab);
                }


            if (common != "")
            {
                commonCategory = common;
                return GetLength(word1Tree, word2Tree);
            }

            // 4 - depth
            foreach (Tree cat in word1Tree.children)
                foreach (Tree subCat in cat.children)
                    foreach (Tree subsubCat in subCat.children)
                    {
                        //String[] tab = wcp.getSubcategories(subsubCat.data);
                        String[] tab = wcp.getUbercategory(subsubCat.data);
                        if (tab != null)
                            //subCat.getChild(subsubCat.data).addChilds(tab);
                            subCat.getChild(subsubCat.data).addChilds(tab);
                    }

            foreach (Tree cat in word2Tree.children)
                foreach (Tree subCat in cat.children)
                    foreach (Tree subsubCat in subCat.children)
                    {
                        //String[] tab = wcp.getSubcategories(subsubCat.data);
                        String[] tab = wcp.getUbercategory(subsubCat.data);
                        if (tab != null)
                            //subCat.getChild(subsubCat.data).addChilds(tab);
                            subCat.getChild(subsubCat.data).addChilds(tab);
                    }

            if (common != "")
            {
                commonCategory = common;
                return GetLength(word1Tree, word2Tree);
            }

            return -1;
        }

        protected int GetLength(Tree w1, Tree w2)
        {
            if (w1.data == w2.data)
            {
                return 0;
            }
            return Tree.GetLength(w1) + Tree.GetLength(w2);
        }



    }
}
