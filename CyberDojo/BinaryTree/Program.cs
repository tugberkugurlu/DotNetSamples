using System;

namespace ConsoleApplication
{
    public class Program
    {
        /*
            A binary tree is a special kind of tree, one in which all nodes have at most two children. 
            For a given node in a binary tree, the first child is referred to as the left child, while 
            the second child is referred to as the right child. Figure 2 depicts two binary trees.

             - A node doesn't need to have both a left child and right child. Furthermore, a node can have no children.
             - Nodes that have no children are referred to as leaf nodes.
             - Nodes that have one or two children are referred to as internal nodes.
        */

        /*
            refs:

             - https://msdn.microsoft.com/en-us/library/ms379572(v=vs.80).aspx
             - https://msdn.microsoft.com/en-us/library/ms379573(v=vs.80).aspx
             - http://stackoverflow.com/questions/3262947/is-there-a-built-in-binary-search-tree-in-net-4-0
             - SortedList<T>: https://msdn.microsoft.com/en-us/library/dd412070.aspx
             - http://geekswithblogs.net/BlackRabbitCoder/archive/2011/06/16/c.net-fundamentals-choosing-the-right-collection-class.aspx
        */

        public static void Main(string[] args)
        {
            var tree = new BinaryTree<int>(6);
            tree.Add(2);
            tree.Add(10);
            tree.Add(3);
            tree.Add(20);
            tree.Add(5);

            Console.WriteLine(tree.Count());
            Console.WriteLine(tree.Find(10));
        }
    }

    public class BinaryTree<T> : BinaryTreeNode<T> where T : IComparable
    {
        public BinaryTree(T rootItem) : base(rootItem)
        {
        }
    }

    public class BinaryTreeChildNode<T> : BinaryTreeNode<T> where T : IComparable
    {
        public BinaryTreeChildNode(T value, BinaryTreeNode<T> parent) : base(value)
        {
            if(parent == null) 
            {
                throw new ArgumentNullException(nameof(parent));
            }

            Parent = parent;
        }

        public BinaryTreeNode<T> Parent { get; }
    }

    public abstract class BinaryTreeNode<T> where T : IComparable
    {
        public BinaryTreeNode(T value)
        {
            if(value == null) 
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }

        public T Value { get; }
        public BinaryTreeChildNode<T> Left { get; private set; }
        public BinaryTreeChildNode<T> Right { get; private set; }

        public void Add(T item)
        {
            if(item == null) 
            {
                throw new ArgumentNullException(nameof(item));
            }

            if(item.CompareTo(Value) >= 0)
            {
                // item is bigger than or equal to the current value. So, it goes to right.
                if(Right == null) 
                {
                    Right = new BinaryTreeChildNode<T>(item, this);
                }
                else 
                {
                    Right.Add(item);
                }
            }
            else 
            {
                // item is lower than the current value. So, it goes to left.
                if(Left == null) 
                {
                    Left = new BinaryTreeChildNode<T>(item, this);
                }
                else 
                {
                    Left.Add(item);
                }
            }
        }

        public BinaryTreeNode<T> Find(T item)
        {
            if(item == null) throw new ArgumentNullException(nameof(item));

            if(item.CompareTo(Value) == 0)
            {
                return this;
            }

            if(item.CompareTo(Value) > 0)
            {
                return Right?.Find(item);
            }
            else 
            {
                return Left?.Find(item);
            }
        }

        public int Count()
        {
            var leftCount = Left != null ? 1 + Left.Count() : 0;
            var rightCount = Right != null ? 1 + Right.Count() : 0;

            return leftCount + rightCount;
        }
    }
}
