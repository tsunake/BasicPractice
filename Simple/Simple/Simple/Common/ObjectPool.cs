using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Common
{
    public class ObjectPool<T> where T : class, IRecyclable, new()
    {
        private List<T> m_object_pool = new List<T>();

        public T Create()
        {
            int count = m_object_pool.Count;
            T last;
            if (count > 0)
            {
                count--;
                last = m_object_pool[count];
                m_object_pool.RemoveAt(count);
            }
            else
                last = new T();
            return last;
        }

        public T Create(params object[] args)
        {
            int count = m_object_pool.Count;
            T last;
            if (count > 0)
            {
                count--;
                last = m_object_pool[count];
                m_object_pool.RemoveAt(count);
            }
            else
                last = Activator.CreateInstance(typeof(T), args) as T;
            return last;
        }

        public void Delete(T item)
        {
            if (item == null)
                return;
            item.Recycle();
            m_object_pool.Add(item);
        }

    }
}
