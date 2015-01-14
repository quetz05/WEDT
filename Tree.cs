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


            public Tree getChild(int i)
            {
                foreach (Tree n in children)
                    if (--i == 0) return n;
                return null;
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

            public static String FindCommon(Tree tree1, Tree tree2)
            {
                foreach (Tree kid in tree1.children)
                {
                    if (Traverse(kid.data, tree2) != "")
                        return kid.data;

                    if (FindCommon(kid, tree2) != "")
                        return kid.data;
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
