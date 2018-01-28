﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Tree
{
    interface INodeVisitor<T>
    {
        void Accept(BinaryTreeNode<T> node);
    }
}
