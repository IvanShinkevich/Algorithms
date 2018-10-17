using System;
using System.Collections.Generic;
using ConsoleApp1.Utils;

namespace ConsoleApp1.Entities
{
    public class BTree
    {
        public BNode _root;
        private int _count;
        private IComparer<int> _comparer = Comparer<int>.Default;


        public BTree()
        {
            _root = null;
            _count = 0;
        }

        public BNode Search(int data)
        {
            BNode node = Search(_root, data);
            return node;
        }

        private BNode Search(BNode node, int data)
        {
            if (node == null)
            {
                return null;
            }
            if (data < node.item)
            {
                node.left = Search(node.left, data);
            }
            else if (data > node.item)
            {
                node.right = Search(node.right, data);
            }
            return node;
        }

        public bool ContainsNode(int data)
        {
            return ContainsNodeRecursive(_root, data);
        }

        private bool ContainsNodeRecursive(BNode node, int data)
        {
            if (node == null)
            {
                return false;
            }
            if (data == node.item)
            {
                return true;
            }
            return data < node.item
                ? ContainsNodeRecursive(node.left, data)
                : ContainsNodeRecursive(node.right, data);
        }

        private BNode DeleteN(BNode root, BNode deleteNode)
        {
            if (root == null)
            {
                return root;
            }
            if (deleteNode.item < root.item)
            {
                root.left = DeleteN(root.left, deleteNode);
            }
            if (deleteNode.item > root.item)
            {
                root.right = DeleteN(root.right, deleteNode);
            }
            if (deleteNode.item == root.item)
            {
                //No child nodes
                if (root.left == null && root.right == null)
                {
                    root = null;
                    return root;
                }
                //No left child 
                else if (root.left == null)
                {
                    BNode temp = root;
                    root = root.right;
                }
                //No right child
                else if (root.right == null)
                {
                    BNode temp = root;
                    root = root.left;
                }
                //Has both child nodes
                else
                {
                    BNode min = FindMin2(root.right);
                    root.item = min.item;
                    root.right = DeleteN(root.right, min);
                }
            }
            return root;
        }

        private BNode FindMin2(BNode cur)
        {
            if (cur.left != null)
            {
                return FindMin2(cur.left);
            }
            return cur;
        }

        public void DeleteNode(int x)
        {
            BNode deleteNode = new BNode(x);
            DeleteN(_root, deleteNode);
        }

        public BNode RotateRight(BNode node)
        {
            var oldroot = node;
            var newroot = node.left;
            oldroot.left = newroot.right;
            newroot.right = oldroot;
            return newroot;
        }

        public BNode RotateLeft(BNode node)
        {
            var oldroot = node;
            var newroot = node.right;
            oldroot.right = newroot.left;
            newroot.left = oldroot;
            return newroot;
        }

        public void InsertInRoot(int data)
        {
            _root = InsertInRoot(data, _root);
        }

        private BNode InsertInRoot(int data, BNode root)
        {
            if (root == null)
                return new BNode(data);
            if (data < root.item)
            {
                root.left = InsertInRoot(data, root.left);
                root = RotateRight(root);
            }
            else if (data > root.item)
            {
                root.right = InsertInRoot(data, root.right);
                root = RotateLeft(root);
            }
            return root;

        }

        public bool Add(int data)
        {
            if (_root == null)
            {
                _root = new BNode(data);
                _count++;
                return true;
            }
            else
            {
                return Add_Sub(_root, data);
            }
        }

        private bool Add_Sub(BNode node, int data)
        {
            if (_comparer.Compare(node.item, data) < 0)
            {
                if (node.right == null)
                {
                    node.right = new BNode(data);
                    _count++;
                    return true;
                }
                else
                {
                    return Add_Sub(node.right, data);
                }
            }
            else if (_comparer.Compare(node.item, data) > 0)
            {
                if (node.left == null)
                {
                    node.left = new BNode(data);
                    _count++;
                    return true;
                }
                else
                {
                    return Add_Sub(node.left, data);
                }
            }
            else
            {
                return false;
            }
        }

        public BNode Root { get { return _root; } }

        public void Print()
        {
            Print(_root, 4);
        }

       

        public void Print(BNode p, int padding)
        {
            if (p != null)
            {
                if (p.right != null)
                {
                    Print(p.right, padding + 4);
                }
                if (padding > 0)
                {
                    Console.Write(" ".PadLeft(padding));
                }
                if (p.right != null)
                {
                    Console.Write("/\n");
                    Console.Write(" ".PadLeft(padding));
                }
                Console.Write(p.item.ToString() + "\n ");
                if (p.left != null)
                {
                    Console.Write(" ".PadLeft(padding) + "\\\n");
                    Print(p.left, padding + 4);
                }
            }
        }
    }
}
