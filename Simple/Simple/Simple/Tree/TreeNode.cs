using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Tree
{
    class BinaryTreeNode<T>
    {
        public BinaryTreeNode(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
        public BinaryTreeNode<T> Left = null;
        public BinaryTreeNode<T> Right = null;

        public bool IsLeaf() { return Left == null && Right == null; }
        public bool IsFull() { return Left != null && Right != null; }
    }
}
