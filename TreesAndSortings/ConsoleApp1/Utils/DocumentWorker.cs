using System;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class DocumentWorker
    {
        public int[] CreateRandomArray(int n)
        {
            Random rnd = new Random();
            int[] arr = new int[n];
            for (int i = 0; i < n; i++)
            {
                int value = rnd.Next(0, n);
                arr[i] =  value;
            }
            return arr;
        }

        public int[] CreateArrayAndWriteToFile(int n)
        {
            int[] arr = CreateRandomArray(n);
            File.WriteAllText($@".\number{n}.txt", string.Join(" ", arr));
            return arr;
        }

        public int[] ReadArrayFromFile(int num)
        {
            return File.ReadAllText($@".\numbers{num}.txt").Split(' ').Select(y => int.Parse(y)).ToArray();
        }
    }
}
