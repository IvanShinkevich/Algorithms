using System;
using System.Diagnostics;
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

            //for (int j = 0; j < 30; j++)
            //{
            //    for (int i = 30; i > 1; i--)
            //    {
            //        int[] arr = dw.CreateArrayAndWriteToFile(i);
            //        sort.Quicksort(arr, 0, arr.Length - 1);

            //        if (CompareSearches(arr))
            //        {
            //            Console.WriteLine(i);
            //        }
            //    }
            //}
            //Console.WriteLine("End");

            //int[] arr1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10000000};
            //Stopwatch stopwatch = Stopwatch.StartNew();
            //searcher.InterpolationSearch(arr1, 9);
            //stopwatch.Stop();
            CreateAndFillBTree();

            Console.ReadLine();
        }

        private static bool CompareSearches(int[] arr)
        {
            var watch1 = System.Diagnostics.Stopwatch.StartNew();

            searcher.InterpolationSearch(arr, 5);
            watch1.Stop();
            //Console.WriteLine((int)stopwatch.ElapsedMilliseconds);

            var watch2 = Stopwatch.StartNew();
            searcher.BinarySearch(5, arr);
            watch2.Stop();
            //Console.WriteLine((int)stopwatch2.ElapsedMilliseconds);
            Console.WriteLine(watch1.ElapsedMilliseconds);
            Console.WriteLine(watch2.ElapsedMilliseconds);Console.WriteLine("-----------------");


            return watch1.ElapsedMilliseconds < watch2.ElapsedMilliseconds;
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
