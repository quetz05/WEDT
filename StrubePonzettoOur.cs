using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEDT
{
    class StrubePonzettoOur : StrubePonzetto
    {


        public List<int> lengthList;
        public StrubePonzettoOur(String word1, String word2) : base(word1, word2)
        {
            lengthList = new List<int>();
        }


        override public int Run()
        {
            Console.WriteLine("--- Algorytm Strube-Ponzetto - ulepszony ("+word1 + "," + word2+")");
            Console.WriteLine("Poszukiwanie znaczenia słowa...");
            if (!ChooseMeaning())
                return 1;

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

            if (lexicalAssociationList1.Count == 0)
            {
                lexicalAssociationList1.Add(word1Redirects);
            }

            if (lexicalAssociationList2.Count == 0)
            {
                lexicalAssociationList2.Add(word2Redirects);
            }


            foreach (String w1 in lexicalAssociationList1)
                foreach (String w2 in lexicalAssociationList2)
                {

                    int length = CategoryTreeSearch(w1, w2);
                    lengthList.Add(length);
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
                return pathLength;
            }

            // 1 - depth
            word1Tree.addChilds(word1Categories);
            word2Tree.addChilds(word2Categories);
            String common = Tree.FindCommon(word1Tree, word2Tree);
            if (common != "")
            {
                pathLength = -1;
                commonCategory = common;
                return GetLength(word1Tree, common) + GetLength(word2Tree, common);
            }

            // 2 - depth
            foreach (String cat1 in word1Categories)
            {
                List<String> l = wcp.getUbercategory(cat1);
                if (l != null)
                    word1Tree.getChild(cat1).addChilds(l.ToArray());
            }


            foreach (String cat2 in word2Categories)
            {
                List<String> l = wcp.getUbercategory(cat2);
                if(l != null)
                    word2Tree.getChild(cat2).addChilds(l.ToArray());
            }


            common = Tree.FindCommon(word1Tree, word2Tree);

            if (common != "")
            {
                commonCategory = common;
                return GetLength(word1Tree, common) + GetLength(word2Tree, common);
            }

            // 3 - depth
            foreach (Tree cat in word1Tree.children)
                foreach (Tree subCat in cat.children)
                {
                    List<String> l = wcp.getUbercategory(subCat.data);
                    if (l == null)
                        continue;

                    String[] tab = l.ToArray();
                    cat.getChild(subCat.data).addChilds(tab);
                }


            foreach (Tree cat in word2Tree.children)
                foreach (Tree subCat in cat.children)
                {
                    List<String> l = wcp.getUbercategory(subCat.data);
                    if (l == null)
                        continue;

                    String[] tab = l.ToArray();
                    cat.getChild(subCat.data).addChilds(tab);
                }

            common = Tree.FindCommon(word1Tree, word2Tree);
            if (common != "")
            {
                commonCategory = common;
                return GetLength(word1Tree, common) + GetLength(word2Tree, common);
            }

            // 4 - depth
            foreach (Tree cat in word1Tree.children)
                foreach (Tree subCat in cat.children)
                    foreach (Tree subsubCat in subCat.children)
                    {
                        List<String> l = wcp.getUbercategory(subsubCat.data);
                        if (l == null)
                            continue;

                        String[] tab = l.ToArray();
                        subCat.getChild(subsubCat.data).addChilds(tab);
                    }

            foreach (Tree cat in word2Tree.children)
                foreach (Tree subCat in cat.children)
                    foreach (Tree subsubCat in subCat.children)
                    {
                        List<String> l = wcp.getUbercategory(subsubCat.data);
                        if (l == null)
                            continue;

                        String[] tab = l.ToArray();
                        subCat.getChild(subsubCat.data).addChilds(tab);
                    }

            common = Tree.FindCommon(word1Tree, word2Tree);

            if (common != "")
            {
                commonCategory = common;
                return GetLength(word1Tree, common) + GetLength(word2Tree, common);
            }

            return -1;
        }

        public override String ClassifyWords()
        {
            int p = lengthList.ToArray().Max();
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
