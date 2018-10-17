using System;
using System.Diagnostics;
using System.Threading;
using ConsoleApp1.Entities;
using ConsoleApp1.Utils;

namespace ConsoleApp1
{
    class Program
    {
        private static DocumentWorker dw = new DocumentWorker();
        private static SortingHelper sort = new SortingHelper();
        private static SearchHelper searcher = new SearchHelper();

        static void Main(string[] args)
        {

            for (int j = 0; j < 30; j++)
            {
                for (int i = 30; i > 1; i--)
                {
                    int[] arr = dw.CreateArrayAndWriteToFile(i);
                    sort.Quicksort(arr, 0, arr.Length - 1);

                    if (CompareSearches(arr))
                    {
                        Console.WriteLine(i);
                    }
                }
            }
            Console.WriteLine("End");

            
            CreateAndFillBTree();

            Console.ReadLine();
        }

        private static bool CompareSearches(int[] arr)
        {
            var watch1 = Stopwatch.StartNew();
            searcher.InterpolationSearch(arr, 5);
            watch1.Stop();
            var tik = watch1.ElapsedTicks;


            var watch2 = Stopwatch.StartNew();
            searcher.BinarySearch(arr, 5);
            watch2.Stop();
            var ti = watch2.ElapsedTicks;


            //Console.WriteLine(tik + " " + ti);
            
            return tik > ti;
        }

        private static void CreateAndFillBTree()
        {
            BTree btr = new BTree();
            btr.Add(6);
            btr.Add(2);
            btr.Add(3);
            btr.Add(11);
            btr.Add(30);
            btr.Add(9);
            btr.Add(13);
            btr.Add(18);
            btr.Add(1);
            btr.Add(6);
            btr.Add(7);
            btr.Add(17);
            btr.Print();
            BTreePrinterSimple.Print(btr.Root);
            BTreePrinterPretty.Print(btr.Root);

            //btr.DeleteNode(6);
            btr._root = btr.RotateLeft(btr.Root);
            Console.WriteLine("space!");
            BTreePrinterPretty.Print(btr.Root);
            //btr.DeleteNode(13);
            //BTreePrinterPretty.Print(btr.Root);
            //btr.DeleteNode(11);
            btr._root = btr.RotateRight(btr.Root);
            btr._root = btr.RotateRight(btr.Root);
            BTreePrinterPretty.Print(btr.Root);

            btr.InsertInRoot(5);
            BTreePrinterPretty.Print(btr.Root);
            //BTreePrinterPretty.Print(btr.Root);
            //btr.DeleteNode(2);
            //BTreePrinterPretty.Print(btr.Root);
        }
    }
}
