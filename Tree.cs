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
            LinkedList<Tree> children;

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

            public static bool FindCommon(Tree tree1, Tree tree2)
            {



                return true;

            }

        }
}
