using HashTablesBoth.Entities;
using System;

namespace HashTablesBoth.Utils
{
    public class TablesHelper
    {
        public static void ReadAndMeasureChains(int i, double constant)
        {
            var arr = FileUtils.ReadArray(i);
            HashTableWithChains table = new HashTableWithChains();
            table.A = constant;
            foreach (var item in arr)
            {
                table.Insert(item, $"Element number {i} in array");
            }

            Console.WriteLine($"Array number:{i}, largest chain - {table.GetLargestChainLength()}");
        }
    }
}