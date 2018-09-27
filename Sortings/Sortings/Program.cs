using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sortings
{
    class Program
    {
        private const int maxValue = 1000000000;
        private const int minValue = 0;
        private const int arrAmo = 50;
        private const int arrSize = 100000;
        private static int[] timeCurrentIterationQ = new int[50];
        private static int[] timeCurrentIterationH = new int[50];
        private static int n = 8;

        private static void WriteToFile(int[] arr, int fileNum)
        {
            File.WriteAllText($@".\arr{fileNum}.txt", string.Join(" ", arr));
        }

        private static int[] GenerateArray(int size = 100000)
        {
            int[] arr = new int[size];
            var rnd = new Random();
            for (int i = 0; i < arrSize; i++)
            {
                arr[i] = rnd.Next(minValue, maxValue);
            }
            return arr;
        }

        private static void GenerateArrayAndWriteToFile(int i)
        {
            var arr = GenerateArray();
            WriteToFile(arr, i);
        }

        private static void CreateArrays()
        {
            for (int i = 0; i < arrAmo; i++)
            {
                MeasureExecutionTime(i);
            }
        }

        private static void MeasureExecutionTime(int i)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GenerateArrayAndWriteToFile(i);
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            if(i==49) Console.ReadKey();
        }

        private static int[] ReadArray(int i)
        {
            return System.IO.File
                .ReadAllText($@".\arr{i}.txt").Split(' ').Select(y => int.Parse(y)).ToArray();
        }

        private static void ReadAndSortAndWriteArray(int i)
        {
            var arr4Qs = ReadArray(i);
            var insArr = ReadArray(i);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Quicksort(arr4Qs, 0, arrSize-1);
            watch.Stop();
            var watchIns = System.Diagnostics.Stopwatch.StartNew();
            QuickSortHybrid(insArr, 0, arrSize-1);
            watchIns.Stop();
            Console.WriteLine($"{watch.ElapsedMilliseconds} - {watchIns.ElapsedMilliseconds} - {CompareArr(arr4Qs,insArr)}");
            timeCurrentIterationQ[i] = (int)watch.ElapsedMilliseconds;
            timeCurrentIterationH[i] = (int) watchIns.ElapsedMilliseconds;
            if (i == 49)
            {
                float curIterationQ = ((float)timeCurrentIterationQ.Sum() )/ 50;
                float curIterationH = ((float)timeCurrentIterationH.Sum() )/ 50;
                File.AppendAllText($@".\FileStats.txt", $"{curIterationH < curIterationQ} - QuickSort: {curIterationQ}, HybridSort: {curIterationH}, n: {n}" + Environment.NewLine);
            }
        }

        public static bool CompareArr(int[] arr1,int[] arr2)
        {
            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                    return false;
            }

            return true;
        }

        static void InsertionSort(int[] inputArray, int left, int right)
        {
            for (int i = left; i < right; i++)
            {
                for (int j = i + 1; j > left; j--)
                {
                    if (inputArray[j - 1] > inputArray[j])
                    {
                        int temp = inputArray[j - 1];
                        inputArray[j - 1] = inputArray[j];
                        inputArray[j] = temp;
                    }
                }
            }
        }

       

        public static void Quicksort(int[] elements, int left, int right)
        {
            int i = left, j = right;
            IComparable pivot = elements[(left + right) / 2];

            while (i <= j)
            {
                while (elements[i].CompareTo(pivot) < 0)
                {
                    i++;
                }

                while (elements[j].CompareTo(pivot) > 0)
                {
                    j--;
                }

                if (i <= j)
                {
                    // Swap
                    int tmp = elements[i];
                    elements[i] = elements[j];
                    elements[j] = tmp;

                    i++;
                    j--;
                }
            }

            // Recursive calls
            if (left < j)
            {
                Quicksort(elements, left, j);
            }

            if (i < right)
            {
                Quicksort(elements, i, right);
            }
        }

        public static void QuickSortHybrid(int[] elements, int left, int right)
        {
            if (right - left <= n)
            {
                InsertionSort(elements, left, right);
            }
            else
            {
                int i = left, j = right;
                IComparable pivot = elements[(left + right) / 2];

                while (i <= j)
                {
                    while (elements[i].CompareTo(pivot) < 0)
                    {
                        i++;
                    }

                    while (elements[j].CompareTo(pivot) > 0)
                    {
                        j--;
                    }

                    if (i <= j)
                    {
                        // Swap
                        int tmp = elements[i];
                        elements[i] = elements[j];
                        elements[j] = tmp;

                        i++;
                        j--;
                    }
                }
                // Recursive calls
                if (left < j)
                {
                    QuickSortHybrid(elements, left, j);
                }

                if (i < right)
                {
                        QuickSortHybrid(elements, i, right);
                }
            }
        }
        
        private static void SortAllArraysQuickSort()
        {
            for (int i = 0; i < arrAmo; i++)
            {
                ReadAndSortAndWriteArray(i);
            }
        }

        static void Main(string[] args)
        {

            CreateArrays();
            for (int i = 200; i >= 7; i--)
            {
                n = i;
                SortAllArraysQuickSort();
            }
        }
    }
}
