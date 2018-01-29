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
            TestBST();
        }

        private static void TestBST()
        {
            Logger.Error(MathHelper.NextPrime(299).ToString());
        }
    }
}
