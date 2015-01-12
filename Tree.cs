using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEDT
{
    delegate void TreeVisitor<T>(T nodeData);

    class Tree<T>
    {
        T data;
        LinkedList<Tree<T>> children;

        public Tree(T data)
        {
            this.data = data;
            children = new LinkedList<Tree<T>>();
        }

        public void addChild(T data)
        {
            children.AddFirst(new Tree<T>(data));
        }

        public Tree<T> getChild(int i)
        {
            foreach (Tree<T> n in children)
                if (--i == 0) return n;
            return null;
        }

        public void traverse(Tree<T> node, TreeVisitor<T> visitor)
        {
            visitor(node.data);
            foreach (Tree<T> kid in node.children)
                traverse(kid, visitor);
        }
    }
}
