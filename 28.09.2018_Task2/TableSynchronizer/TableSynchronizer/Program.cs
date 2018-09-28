using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TableSynchronizer
{
    class Program
    {
        private static int n = 5;
        private static Table1[] table1 = new Table1[n];
        private static Table2[] table2 = new Table2[n];

        static void Main(string[] args)
        {
            InitTables();
            PrintStructures(table1, table2);

            var result = (from t1 in table1
                          join t2 in table2 on t1.Name equals t2.Name
                          select new TableRes { Name = t1.Name, Age = t1.Age, Property0 = t2.Property0, Property1 = t2.Property1, PropertyToBeDefined = t1.PropertyToBeDefined }).ToArray();
                        
            result = Merge_Sort(result);

            table1 = (from res in result
                       select new Table1 { Name = res.Name, Age = res.Age, PropertyToBeDefined = res.PropertyToBeDefined }).ToArray();
            table2 = (from res in result
                     select new Table2 { Name = res.Name, Property0 = res.Property0, Property1 = res.Property1 }).ToArray();
            PrintStructures(table1, table2);
            Console.ReadLine();
        }

        private static void InitTables()
        {
            for (int i = 0; i < n; i++)
            {
                table1[i] = new Table1
                {
                    Age = 19 + i,
                    Name = $"Johny{i}",
                    PropertyToBeDefined = $"S{i}"
                };
            }
            for (int i = 0; i < n; i++)
            {
                table2[i] = new Table2
                {
                    Name = $"Johny{i}",
                    Property0 = $"A{i}",
                    Property1 = $"B{i}",
                };
            }

            MixRandom(table1);
            MixRandom(table2);
        }

        private static void Sort(Table1[] tablee1)
        {
            for (int i = 0; i < table1.Length - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (table1[j - 1].Age > table1[j].Age)
                    {
                        var temp = tablee1[j - 1];
                        tablee1[j - 1] = tablee1[j];
                        //If we have NO key, but in the very beginning tables had the same order - then Swap2ndTableElements
                        PlaceByKeyAndIndex(tablee1[j - 1].Name, j - 1);
                        table1[j] = temp;
                    }
                }
            }
        }

        private void Swap2ndTableElements(int index, int indToRepl)
        {
            Table2 tmp = table2[index];
            table2[index] = table2[indToRepl];
            table2[indToRepl] = tmp;
        }

        private static void PlaceByKeyAndIndex(string key, int index)
        {
            int indToRepl = Array.FindIndex(table2,i => i.Name == key);
            Table2 tmp = table2[index];
            table2[index] = table2[indToRepl];
            table2[indToRepl] = tmp;
        }

        private static void MixRandom<T>(T[] table)
        {
            for (int i = 0; i < table.Length / 2; i++)
            {
                var r = new Random();
                int ind = r.Next(table.Length/2, table.Length);
                T tmp = table[ind];
                table[ind] = table[i];
                table[i] = tmp;
            }
        }

        private static void PrintStructures(Table1[] tbl1, Table2[] tbl2)
        {
            for (int i = 0; i < tbl1.Length; i++)
            {
                tbl1[i].Show();
            }

            Console.WriteLine("-------------------");

            for (int i = 0; i < tbl2.Length; i++)
            {
                tbl2[i].Show();
            }

            Console.WriteLine("-------------------");
            Console.WriteLine("-------------------");
        }

        private static TableRes[] Merge_Sort(TableRes[] arr)
        {
            if (arr.Length == 1)
                return arr;
            int mid_point = arr.Length / 2;
            return Merge(Merge_Sort(arr.Take(mid_point).ToArray()), Merge_Sort(arr.Skip(mid_point).ToArray()));
        }

        private static TableRes[] Merge(TableRes[] arr_1, TableRes[] arr_2)
        {
            int a = 0, b = 0;
            TableRes[] merged = new TableRes[arr_1.Length + arr_2.Length];
            for (int i = 0; i < arr_1.Length + arr_2.Length; i++)
            {
                if (b.CompareTo(arr_2.Length) < 0 && a.CompareTo(arr_1.Length) < 0)
                    if (arr_1[a].Age > arr_2[b].Age)
                        merged[i] = arr_2[b++];
                    else
                        merged[i] = arr_1[a++];
                else
                    if (b < arr_2.Length)
                    merged[i] = arr_2[b++];
                else
                    merged[i] = arr_1[a++];
            }
            return merged;
        }

        struct Table1
        {
            public int Age;
            public string Name;
            public string PropertyToBeDefined;

            public void Show()
            {
                Console.WriteLine($"Name: {Name}, Age: {Age}, Prop: {PropertyToBeDefined}");
            }
        }

        struct Table2
        {
            public string Name;
            public string Property0;
            public string Property1;

            public void Show()
            {
                Console.WriteLine($"Name: {Name}, P0: {Property0}, P1: {Property1}");
            }
        }

        struct TableRes
        {
            public string Name;
            public string Property0;
            public string Property1;
            public int Age;
            public string PropertyToBeDefined;
        }
    }
}
