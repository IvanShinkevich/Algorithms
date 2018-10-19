using System;
using System.IO;
using System.Linq;
using HashTablesBoth.Entities;
using HashTablesBoth.Utils;

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
            FileUtils.CreateArrays(50,M);
            GetStatsOnTablesWithChains();
            Console.WriteLine("Second type of table - with open adresses and non-linear insertion alg");
            CreateAndFillTableWithOpenAdresses();
            Console.ReadKey();
        }

        private static void GetStatsOnTablesWithChains()
        {
            for (int i = 0; i < definedConst.Length; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    TablesHelper.ReadAndMeasureChains(j, definedConst[i]);
                }
            }
        }

        private static void CreateAndFillTableWithOpenAdresses()
        {
            for (int j = 0; j < 1; j++)
            {
                var arr = FileUtils.ReadArray(j);
                HashTableWithOpenAdresses table = new HashTableWithOpenAdresses(1024);
                foreach (var item in arr)
                {
                    table.Insert(item, item);
                }
                table.ShowTable();

                int var = table.Find(arr[532]);
                Console.WriteLine(var);
            }
        }
    }
}
