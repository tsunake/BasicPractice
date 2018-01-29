using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Hash
{
    public class HashTable<T>
    {
        enum EntryStatus
        {
            Active,
            Empty,
            Deleted,
        }
        class HashEntry
        {
            public T m_element = default(T);
            public EntryStatus m_status = EntryStatus.Empty;
        }

        private List<HashEntry> m_elements = null;
        private int m_current_size = 0;
        public HashTable(int size = 101)
        {

        }

        
    }
}
