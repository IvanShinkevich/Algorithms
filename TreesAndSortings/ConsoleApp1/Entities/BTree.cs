using System;
using System.Collections.Generic;
using ConsoleApp1.Utils;

namespace ConsoleApp1.Entities
{
    public class BTree
    {
        private BNode _root;
        private int _count;
        private IComparer<int> _comparer = Comparer<int>.Default;


        public BTree()
        {
            _root = null;
            _count = 0;
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
                //No left child - DONT WORK
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

        public BNode rotateRight(BNode node)
        {
            var oldroot = _root;
            var newroot = _root.left;
            oldroot.left = newroot.right;
            newroot.right = oldroot;
            _root = newroot;
            return node;
        }

        public BNode rotateLeft(BNode node)
        {
            var oldroot = _root;
            var newroot = _root.right;
            oldroot.right = newroot.left;
            newroot.left = oldroot;
            _root = newroot;
            return node;
        }

        public bool Add(int Item)
        {
            if (_root == null)
            {
                _root = new BNode(Item);
                _count++;
                return true;
            }
            else
            {
                return Add_Sub(_root, Item);
            }
        }

        private bool Add_Sub(BNode Node, int Item)
        {
            if (_comparer.Compare(Node.item, Item) < 0)
            {
                if (Node.right == null)
                {
                    Node.right = new BNode(Item);
                    _count++;
                    return true;
                }
                else
                {
                    return Add_Sub(Node.right, Item);
                }
            }
            else if (_comparer.Compare(Node.item, Item) > 0)
            {
                if (Node.left == null)
                {
                    Node.left = new BNode(Item);
                    _count++;
                    return true;
                }
                else
                {
                    return Add_Sub(Node.left, Item);
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

        public BNode InsertInRoot(BNode node,int val)
        {
            if (node == null) return new BNode(val);
            if (val < node.item)
            {
                node.left = InsertInRoot(node.left, val);
                return rotateRight(node);
            }
            else
            {
                node.right = InsertInRoot(node.right, val);
                return rotateLeft(node);
            }
        }
    }
}
