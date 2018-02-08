using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Tree
{
    /// <summary>
    /// 小（大）顶堆是一棵完全二叉树，每个节点的两个儿子大于等于（小于等于）节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Heap<T> where T : IComparable<T>
    {
        public enum SortType
        {
            Min,
            Max,
        }

        private SortType m_sort_type = SortType.Min;
        private List<T> m_elements = new List<T>();
        public Heap(SortType sort = SortType.Min)
        {
            m_sort_type = sort;
        }

        public Heap(IEnumerable<T> items, SortType sort = SortType.Min)
        {
            m_elements.AddRange(items);
            BuildHeap();
        }

        public int Count { get { return m_elements.Count; } }

        private void BuildHeap()
        {
            for (int loc = m_elements.Count / 2 - 1; loc > 0; --loc)
                PercolateDown(loc);
        }


        public void Insert(T value)
        {
            m_elements.Add(value);
            PercolateUp( m_elements.Count - 1);
        }

        private bool AgainstSortType(T child, T parent)
        {
            if (m_sort_type == SortType.Min)
                return child.CompareTo(parent) < 0;
            return child.CompareTo(parent) > 0;
        }

        public void Remove(T item)
        {
            //如果限制T为引用类型，可以进行remove
            //值类型的remove，视情况进行
            //TODO:把最后一个数拿来填充移除的位置，堆尺寸减1，视情况进行PercolateDown或者PercolateUp
        }

        public T Peek()
        {
            if (m_elements.Count <= 0)
                throw new Exception("No Element in Heap");
            return m_elements[0];
        }

        /// <summary>
        /// 删除并返回堆顶元素
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            int last_loc = m_elements.Count - 1;
            if (last_loc < 0)
                throw new Exception("No Element in Heap");
            T result = m_elements[0];
            m_elements[0] = m_elements[last_loc];
            m_elements.RemoveAt(last_loc);
            PercolateDown(0);
            return result;
        }

        private int FindTarget(int index)
        {
            if (m_sort_type == SortType.Min)
            {
                //找到两个孩子中较小的（如果只有一个孩子，就是左孩子）
                if (index + 1 < m_elements.Count && m_elements[index].CompareTo(m_elements[index + 1]) > 0)
                    ++index;
            }
            else
            {
                //找到两个孩子中较大的
                if (index + 1 < m_elements.Count && m_elements[index].CompareTo(m_elements[index + 1]) < 0)
                    ++index;
            }
            return index;
        }

        //指定位置处的元素下沉
        private void PercolateDown(int loc)
        {
            T value = m_elements[loc];
            int parent = loc;
            int target_child = 2 * parent + 1;
            while (target_child < m_elements.Count)
            {
                //找到两个孩子中较小(大)的
                target_child = FindTarget(target_child);
                //如果孩子仍然小(大)于该元素，元素继续下沉（与孩子交换位置），否则停留在该位置，调整完成
                if (AgainstSortType(m_elements[target_child], value))
                    m_elements[parent] = m_elements[target_child];
                else
                    break;
                parent = target_child;
                target_child = 2 * parent + 1;
            }
            m_elements[parent] = value;
        }

        //指定位置处的元素上浮
        private void PercolateUp(int loc)
        {
            T value = m_elements[loc];
            int child = loc;
            int parent = (child - 1) / 2;
            while(parent >= 0)
            {
                if (AgainstSortType(value, m_elements[parent]))
                    m_elements[child] = m_elements[parent];
                else
                    break;
                child = parent;
                parent = (loc - 1) / 2;
            }
            m_elements[child] = value;
        }

    }
}
