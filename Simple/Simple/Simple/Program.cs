﻿using System;
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
            BinarySearchTree<int> tree = new BinarySearchTree<int>();
            PrintNodeValue<int> visitor = new PrintNodeValue<int>();
            tree.Insert(6);
            tree.Insert(2);
            tree.Insert(1);
            tree.Insert(5);
            tree.Insert(3);
            tree.Insert(4);
            tree.Insert(2);
            tree.Insert(8);
            tree.Remove(2);
            tree.Remove(6);
            tree.PostorderRecursiveTraverse(visitor);
            Logger.Error("split");
            tree.PostorderTraverse(visitor);
        }
    }
}