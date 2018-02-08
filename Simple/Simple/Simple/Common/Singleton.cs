using System;
using System.Collections.Generic;
using System.Reflection;

namespace Simple.Common
{
    public interface ISingleton
    {
        void DestroySingleton();
    }

    public abstract class Singleton<T> : ISingleton where T : Singleton<T>
    {
        protected static T ms_instance;

        protected Singleton()
        {
        }

        public static T Instance
        {
            get
            {
                if (ms_instance == null)
                {
                    //感谢yqq指点
                    ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                    if (ctor == null)
                        throw new Exception("Non-public ctor() not found!");
                    ms_instance = ctor.Invoke(null) as T;
                    SingletonManager.AddSingleton(ms_instance);
                }
                return ms_instance;
            }
        }

        public void DestroySingleton()
        {
            if (ms_instance != null)
                ms_instance.OnDestroy();
            ms_instance = default(T);
        }

        protected abstract void OnDestroy();
    }

    public class SingletonManager
    {
        static List<ISingleton> ms_all_singletons = new List<ISingleton>();

        public static void AddSingleton(ISingleton singleton)
        {
            ms_all_singletons.Add(singleton);
        }

        public static void DestroyAllSingletons()
        {
            for (int i = 0; i < ms_all_singletons.Count; ++i)
                ms_all_singletons[i].DestroySingleton();
            ms_all_singletons.Clear();
        }
    }
}
