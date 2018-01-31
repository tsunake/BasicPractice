using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Tree;

namespace Simple
{
    class PrintNodeValue<T> : INodeVisitor<T>
    {
        public void Accept(T value)
        {
            Logger.Error(value.ToString());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Logger.Error(MathHelper.NextPrime(299).ToString());
            Simple.Hash.HashTable<int> hash_table = new Hash.HashTable<int>();
            for (int i = 0; i < 58; ++i)
                hash_table.Insert(i);
            hash_table.Contains(1);
            hash_table.Insert(1);
            hash_table.Remove(101);
            hash_table.Remove(1);
            hash_table.Print();
        }
    }
}
