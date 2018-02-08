using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Common
{
    #region for local lifetime
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
    #endregion

    #region for global life time
    public class RecyclableObject
    {
        public static TClass Create<TClass>() where TClass : class, IRecyclable, new()
        {
            return ResuableObjectPool<IRecyclable>.Instance.Create<TClass>();
        }
        public static void Recycle(IRecyclable instance)
        {
            ResuableObjectPool<IRecyclable>.Instance.Recycle(instance);
        }
    }

    public class ResuableObjectPool<TBaseClassOrInterface> : Singleton<ResuableObjectPool<TBaseClassOrInterface>> where TBaseClassOrInterface : IRecyclable
    {
        Dictionary<System.Type, List<TBaseClassOrInterface>> m_pools = new Dictionary<System.Type, List<TBaseClassOrInterface>>();

        private ResuableObjectPool()
        {
        }

        protected override void OnDestroy()
        {
            Clear();
        }

        private void Clear()
        {
            m_pools.Clear();
        }

        public TClass Create<TClass>() where TClass : class, TBaseClassOrInterface, new()
        {
            System.Type type = typeof(TClass);
            TClass instance = null;
            List<TBaseClassOrInterface> pool = null;
            if (!m_pools.TryGetValue(type, out pool))
            {
                instance = new TClass();
            }
            else
            {
                int cache_count = pool.Count;
                if (cache_count > 0)
                {
                    instance = (TClass)pool[cache_count - 1];
                    pool.RemoveAt(cache_count - 1);
                }
                else
                {
                    instance = new TClass();
                }
            }
            return instance;
        }

        public void Recycle(TBaseClassOrInterface instance)
        {
            if (instance == null)
                return;
            instance.Recycle();
            System.Type type = instance.GetType();
            List<TBaseClassOrInterface> pool = null;
            if (!m_pools.TryGetValue(type, out pool))
            {
                pool = new List<TBaseClassOrInterface>();
                m_pools[type] = pool;
            }
            pool.Add(instance);
        }

        public System.Object Create(System.Type type)
        {
            List<TBaseClassOrInterface> pool = null;
            if (!m_pools.TryGetValue(type, out pool))
            {
                System.Object instance = System.Activator.CreateInstance(type);
                return instance;
            }
            else
            {
                int cache_count = pool.Count;
                if (cache_count > 0)
                {
                    System.Object instance = pool[cache_count - 1];
                    pool.RemoveAt(cache_count - 1);
                    return instance;
                }
                else
                {
                    System.Object instance = System.Activator.CreateInstance(type);
                    return instance;
                }
            }
        }
    }
    #endregion
}
