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
            Sort(table1);
            PrintStructures(table1,table2);
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
    }
}
