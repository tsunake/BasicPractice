using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Tree
{
    //TODO:改用IRecyclable减少生成node的gc
    class BinarySearchTree<T> where T : IComparable<T>
    {
        BinaryTreeNode<T> m_root = null;

        public bool Contains(T item)
        {
            BinaryTreeNode<T> current = m_root;
            while (current != null)
            {
                int compare_result = item.CompareTo(current.Value);
                if (compare_result > 0)
                    current = current.Right;
                else if (compare_result < 0)
                    current = current.Left;
                else
                    return true;
            }
            return false;
        }

        public void Insert(T item)
        {
            if (m_root == null)
            {
                m_root = new BinaryTreeNode<T>(item);
                return;
            }
            BinaryTreeNode<T> current = m_root;
            while (current != null)
            {
                int compare_result = item.CompareTo(current.Value);
                if (compare_result > 0)
                {
                    if (current.Right == null)
                    {
                        current.Right = new BinaryTreeNode<T>(item);
                        break;
                    }
                    current = current.Right;
                }
                else if (compare_result < 0)
                {
                    if (current.Left == null)
                    {
                        current.Left = new BinaryTreeNode<T>(item);
                        break;
                    }
                    current = current.Left;
                }
                else
                {
                    Logger.Error("Duplicated value", item.ToString());
                    break;
                }
            }

        }

        public void Remove(T item)
        {
            Remove(item, ref m_root);
        }

        public void InorderRecursiveTraverse(INodeVisitor<T> visitor)
        {
            InorderTraverse(visitor, m_root);
        }

        public void InorderTraverse(INodeVisitor<T> visitor)
        {
            if (m_root == null)
                return;
            Stack<BinaryTreeNode<T>> sequence = new Stack<BinaryTreeNode<T>>();
            BinaryTreeNode<T> current = m_root;
            while (current != null || sequence.Count > 0)
            {
                while (current != null)
                {
                    sequence.Push(current);
                    current = current.Left;
                }
                current = sequence.Pop();
                visitor.Accept(current.Value);
                current = current.Right;
            }
        }

        public void PreorderRecursiveTraverse(INodeVisitor<T> visitor)
        {
            PreorderTraverse(visitor, m_root);
        }

        public void PreorderTraverse(INodeVisitor<T> visitor)
        {
            if (m_root == null)
                return;
            Stack<BinaryTreeNode<T>> sequence = new Stack<BinaryTreeNode<T>>();
            BinaryTreeNode<T> current = null;
            sequence.Push(m_root);
            while (sequence.Count > 0)
            {
                current = sequence.Pop();
                visitor.Accept(current.Value);
                if (current.Right != null)
                    sequence.Push(current.Right);
                if (current.Left != null)
                    sequence.Push(current.Left);
            }
        }

        public void PostorderRecursiveTraverse(INodeVisitor<T> visitor)
        {
            PostorderTraverse(visitor, m_root);
        }

        public void PostorderTraverse(INodeVisitor<T> visitor)
        {
            if (m_root == null)
                return;
            Stack<BinaryTreeNode<T>> sequence = new Stack<BinaryTreeNode<T>>();
            HashSet<BinaryTreeNode<T>> record = new HashSet<BinaryTreeNode<T>>();
            BinaryTreeNode<T> current = null;
            sequence.Push(m_root);
            while (sequence.Count > 0)
            {
                current = sequence.Peek();
                bool left_child_visited = current.Left != null ? record.Contains(current.Left) : true;
                bool right_child_visited = current.Right != null ? record.Contains(current.Right) : true;
                if (left_child_visited && right_child_visited)
                {
                    sequence.Pop();
                    visitor.Accept(current.Value);
                    record.Add(current);
                }
                if (!right_child_visited)
                    sequence.Push(current.Right);
                if (!left_child_visited)
                    sequence.Push(current.Left);
            }
        }

        public void LayerTraverse(INodeVisitor<T> visitor)
        {
            if (m_root == null)
                return;
            Queue<BinaryTreeNode<T>> sequence = new Queue<BinaryTreeNode<T>>();
            sequence.Enqueue(m_root);
            BinaryTreeNode<T> current = null;
            while(sequence.Count > 0)
            {
                current = sequence.Dequeue();
                visitor.Accept(current.Value);
                if (current.Left != null)
                    sequence.Enqueue(current.Left);
                if (current.Right != null)
                    sequence.Enqueue(current.Right);
            }
        }

        #region internal
        private void Remove(T item, ref BinaryTreeNode<T> node)
        {
            if (node == null)
                return;
            int compare_result = item.CompareTo(node.Value);
            if (compare_result > 0)
                Remove(item, ref node.Right);
            else if (compare_result < 0)
                Remove(item, ref node.Left);
            else
            {
                if (node.IsFull())
                {
                    //使用右子树中最小的节点代替node，并从右子树中删除该最小节点
                    node.Value = FindMin(node.Right).Value;
                    Remove(node.Value, ref node.Right);
                }
                else
                {
                    //直接删除node，如果一个孩子为null，则用另一个孩子代替node
                    BinaryTreeNode<T> origin = node;
                    node = (node.Left == null) ? node.Right : node.Left;
                    origin.Left = origin.Right = null;
                }
            }
        }

        private BinaryTreeNode<T> FindMin(BinaryTreeNode<T> root)
        {
            if (root == null)
                return null;
            BinaryTreeNode<T> current = root;
            while (current.Left != null)
                current = current.Left;
            return current;
        }

        private void InorderTraverse(INodeVisitor<T> visitor, BinaryTreeNode<T> node)
        {
            if (node == null)
                return;
            InorderTraverse(visitor, node.Left);
            visitor.Accept(node.Value);
            InorderTraverse(visitor, node.Right);
        }

        private void PreorderTraverse(INodeVisitor<T> visitor, BinaryTreeNode<T> node)
        {
            if (node == null)
                return;
            visitor.Accept(node.Value);
            PreorderTraverse(visitor, node.Left);
            PreorderTraverse(visitor, node.Right);
        }

        private void PostorderTraverse(INodeVisitor<T> visitor, BinaryTreeNode<T> node)
        {
            if (node == null)
                return;
            PostorderTraverse(visitor, node.Left);
            PostorderTraverse(visitor, node.Right);
            visitor.Accept(node.Value);
        }
        #endregion
    }
}
