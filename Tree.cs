using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEDT
{
        class Tree
        {

            public String data;
            public LinkedList<Tree> children;

            public Tree(String data)
            {
                this.data = data;
                children = new LinkedList<Tree>();
            }

            public void addChild(String data)
            {
                children.AddFirst(new Tree(data));
            }

            public void addChilds(String[] data)
            {
                foreach (String d in data)
                    children.AddFirst(new Tree(d));
            }

            public Tree getChild(String s)
            {
                foreach (Tree n in children)
                    if (n.data == s) return n;
                return null;
            }

            //public void traverse(Tree node, TreeVisitor visitor)
            //{
            //    visitor(node.data);
            //    foreach (Tree kid in node.children)
            //        traverse(kid, visitor);
            //}


            public static void print(Tree node, String tab = "")
            {
                Console.WriteLine(tab + node.data);
                tab += "-";
                foreach (Tree kid in node.children)
                    print(kid, tab);
            }

            public static int GetLength(Tree node, int length = 0)
            {         
                foreach (Tree kid in node.children)
                    GetLength(kid, length);

                length += 1;

                return length;
            }

            public static String FindCommon(Tree tree1, Tree tree2, String word = "")
            {
                foreach (Tree kid in tree1.children)
                {
                    if (Traverse(kid.data, tree2) != "")
                    {
                        if (word == "")
                            word = kid.data;
                        return word;
                    }
                        
                    String w = FindCommon(kid, tree2, word);
                    if (w != "")
                    {
                        return w;
                    }
                }

                return "";

            }

            private static String Traverse(String word, Tree tree)
            {
                foreach (Tree kid in tree.children)
                {
                    
                    if (kid.data == word)
                        return kid.data;

                    if (Traverse(word, kid) == word)
                        return word;
                }

                return "";
            }

        }
}
