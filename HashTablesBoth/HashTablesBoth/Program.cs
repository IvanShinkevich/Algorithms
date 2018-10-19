using System;
using System.IO;
using System.Linq;
using HashTablesBoth.Entities;

namespace HashTablesBoth
{
    class Program
    {
        public static int M = 1000;


        public static double[] definedConst =
        {
            0.6180339887,
            0.694885314464,
            0.23153784587468468,
            0.73185676454654654
        };


        static void Main(string[] args)
        {
            CreateArrays(50,M);

            for (int i = 0; i < definedConst.Length; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    ReadAndMeasureChains(j, definedConst[i]);
                }  
            }
            Console.WriteLine("Second type of table - with open adresses and non-linear insertion alg");
            for (int j = 0; j < 1; j++)
            {
                var arr = ReadArray(j);
                HashTableWithOpenAdresses table = new HashTableWithOpenAdresses(1024);
                foreach (var item in arr)
                {
                    table.Insert(item, item);
                }
                table.ShowTable();
            }
            Console.ReadKey();
        }

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
                int val = rnd.Next(0, arrSize*arrSize*arrSize);
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

        private static void GenerateArrayAndWriteToFile(int i, int arrSize=1000)
        {
            var arr = GenerateArray(arrSize);
            WriteToFile(arr, i);
        }

        private static int[] ReadArray(int i)
        {
            return System.IO.File
                .ReadAllText($@".\arr{i}.txt").Split(' ').Select(y => int.Parse(y)).ToArray();
        }

        private static void CreateArrays(int arrAmo,int arrSize)
        {
            for (int i = 0; i < arrAmo; i++)
            {
                GenerateArrayAndWriteToFile(i,arrSize);
            }
        }

        private static void ReadAndMeasureChains(int i, double constant)
        {
            var arr = ReadArray(i);
            HashTableWithChains table = new HashTableWithChains();
            table.A = constant;
            foreach (var item in arr)
            {
                table.Insert(item,$"Element number {i} in array");
            }

            Console.WriteLine($"Array number:{i}, largest chain - {table.GetLargestChainLength()}");
        }
    }
}
