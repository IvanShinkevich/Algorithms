using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashTablesBoth.Entities;

namespace HashTablesBoth
{
    class Program
    {
        //public double A = 0.6180339887;
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


            // Создаем новую хеш таблицу.
            var hashTable = new HashTableWithChains();

            // Добавляем данные в хеш таблицу в виде пар Ключ-Значение.
            hashTable.Insert(1, "I never wished you any sort of harm; but you wanted me to tame you...");
            hashTable.Insert(3, "And now here is my secret, a very simple secret: It is only with the heart that one can see rightly; what is essential is invisible to the eye.");
            hashTable.Insert(2, "Well, I must endure the presence of two or three caterpillars if I wish to become acquainted with the butterflies.");
            hashTable.Insert(15, "He did not know how the world is simplified for kings. To them, all men are subjects.");

            // Выводим хранимые значения на экран.
            HashTableWithChains.ShowHashTable(hashTable, "Created hashtable.");
            Console.ReadLine();

            // Удаляем элемент из хеш таблицы по ключу
            // и выводим измененную таблицу на экран.
            hashTable.Delete(2);
            HashTableWithChains.ShowHashTable(hashTable, "Delete item from hashtable.");
            Console.ReadLine();

            // Получаем хранимое значение из таблицы по ключу.
            Console.WriteLine("Little Prince say:");
            var text = hashTable.Search(1);
            Console.WriteLine(text);
            Console.ReadLine();
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
