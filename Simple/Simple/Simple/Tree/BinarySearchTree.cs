using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Tree
{
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
                    if(current.Right == null)
                    {
                        current.Right = new BinaryTreeNode<T>(item);
                        break;
                    }
                    current = current.Right;
                }
                else if (compare_result < 0)
                {
                    if(current.Left == null)
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
                if(node.IsFull())
                {
                    //使用右子树中最小的节点代替node，并从右子树中删除该最小节点
                    node.Value = FindMin(node.Right).Value;
                    Remove(node.Value, ref node.Right);
                }
                else
                {
                    //直接删除node
                    node = (node.Left == null) ? node.Right : node.Left;
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
        #endregion
    }
}
