using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Hash
{
    public class HashTable<T>
    {
        class HashEntry
        {
            public HashEntry(T element, bool active)
            {
                m_data = element;
                m_active = active;
            }

            public T m_data;
            public bool m_active = false;
        }
        private const int DefaultSize = 101;

        private HashEntry[] m_element_list = null;
        private int m_occupied_size = 0;
        private int m_valid_size = 0;
        private IEqualityComparer<T> m_comparer = null;
        private float lamda = 0.5f;
        
        public HashTable()
            : this(DefaultSize, null)
        {
        }

        public HashTable(IEqualityComparer<T> comparer)
            : this(DefaultSize, comparer)
        {
        }

        public HashTable(int size, IEqualityComparer<T> comparer)
        {
            m_comparer = comparer == null ? EqualityComparer<T>.Default : comparer;
            size = (int)MathHelper.NextPrime((uint)size);
            m_element_list = new HashEntry[size];
            Clear();
        }

        public int Count { get { return m_valid_size; } }

        public void Clear()
        {
            for (int i = 0; i < m_element_list.Length; ++i)
                m_element_list[i] = null;
            m_valid_size = m_occupied_size = 0;
        }

        public bool Insert(T element)
        {
            int pos = FindPos(element);
            if (IsActive(pos))
                return false;
            m_element_list[pos] = new HashEntry(element, true);
            ++m_valid_size;
            if (++m_occupied_size > m_element_list.Length * lamda)
                ReHash();
            return true;

        }

        public bool Remove(T element)
        {
            int pos = FindPos(element);
            if (!IsActive(pos))
                return false;
            --m_valid_size;
            m_element_list[pos].m_active = false;
            return true;
        }

        public bool Contains(T element)
        {
            return IsActive(FindPos(element));
        }

        private bool IsActive(int pos)
        {
            HashEntry element = m_element_list[pos];
            return element == null ? false : element.m_active;
        }

        private int FindPos(T element)
        {
            int offset = 1;
            int current_pos = Hash(element);
            //平方探测,f(i) = i ^ 2
            //f(i) = f(i - 1) + 2i -1;每次让偏移多2即可
            while (m_element_list[current_pos] != null &&
                !m_comparer.Equals(m_element_list[current_pos].m_data, element))
            {
                current_pos += offset;
                offset += 2;
            }
            return current_pos;
        }

        private int Hash(T element)
        {
            int hash_value = ToHashCode(element) % m_element_list.Length;
            if (hash_value < 0)
                hash_value += m_element_list.Length;
            return hash_value;
        }

        private int ToHashCode(T element)
        {
            return element == null ? 0 : element.GetHashCode();
        }

        private void ReHash()
        {
            HashEntry[] old_list = m_element_list;
            uint new_size = (uint)(old_list.Length / lamda);
            m_element_list = new HashEntry[(int)MathHelper.NextPrime(new_size)];
            Clear();
            HashEntry element = null;
            for (int i = 0; i < old_list.Length; ++i)
            {
                element = old_list[i];
                if (element != null && element.m_active)
                    Insert(element.m_data);
            }
        }
    }
}
