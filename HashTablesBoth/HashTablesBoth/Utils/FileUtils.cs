using System;
using System.IO;
using System.Linq;

namespace HashTablesBoth.Utils
{
    public class FileUtils
    {
        private static void WriteToFile(int[] arr, int fileNum)
        {
            File.WriteAllText($@".\arr{fileNum}.txt", string.Join(" ", arr));
        }

        private static int[] GenerateArray(int arrSize)
        {
            int[] arr = new int[arrSize];
            var rnd = new Random();
            for (int i = 0; i < arrSize; i++)
            {
                int val = rnd.Next(0, arrSize * arrSize * arrSize);
                if (!arr.Contains(val))
                {
                    arr[i] = val;
                }
                else
                {
                    i--;
                }
            }
            return arr;
        }

        private static void GenerateArrayAndWriteToFile(int i, int arrSize = 1000)
        {
            var arr = GenerateArray(arrSize);
            WriteToFile(arr, i);
        }

        public static int[] ReadArray(int i)
        {
            return System.IO.File
                .ReadAllText($@".\arr{i}.txt").Split(' ').Select(y => int.Parse(y)).ToArray();
        }

        public static void CreateArrays(int arrAmo, int arrSize)
        {
            for (int i = 0; i < arrAmo; i++)
            {
                GenerateArrayAndWriteToFile(i, arrSize);
            }
        }
    }
}