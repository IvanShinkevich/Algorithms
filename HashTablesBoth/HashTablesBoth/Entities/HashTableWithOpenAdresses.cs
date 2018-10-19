using System;

namespace HashTablesBoth.Entities
{
    public class HashTableWithOpenAdresses
    {
        public int M = 1024;
        private Pair[] table;
        private double Prime = 3571;
        public double A = 0.6180339887;

        public HashTableWithOpenAdresses()
        {
            table = new Pair[M];
            Console.WriteLine(table.ToString());
        }

        public HashTableWithOpenAdresses(int m)
        {
            table = new Pair[m];
            Console.WriteLine(table.ToString());
        }

        public void ShowTable()
        {
            foreach(var item in table)
            {
                if(item!=null)
                Console.WriteLine($"{GetHash(item.getValue())} {item.getKey()} {item.getValue()}");
                else
                {
                    Console.WriteLine("null");
                }
            }
        }

        public void Insert(int key, int value)
        {
            int h = GetHash(key);
            int i = 1;
            try
            {
                
                if (table[h] == null || table[h].isDeleted())
                {
                    table[h] = new Pair(key, value);
                }
                else if (table[h].getValue() != value)
                {
                    int newHash = (h + i) % M;
                    while (table[newHash] == null || table[newHash].isDeleted() && table[newHash].getValue() != value) {
                        newHash = (h + i * i) % M;
                        i += 1;
                        if (table[newHash] == null || table[newHash].getValue() != value)
                            table[newHash] = new Pair(key, value);
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"Exception occured: {e.Message}");
            }
        }

        public void Delete(int key)
        {
            int h = GetHash(key);
            try
            {
                if (table[h].getKey() == key)
                {
                    table[h].deletePair();
                }
                for (int i = h + 1; i != h; i = (i + 1) % table.Length)
                {
                    if (table[i].getValue() == key && !table[i].isDeleted())
                    {
                        table[i].deletePair();
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }
        }

        public int Find(int key)
        {
            int hash = GetHash(key);
            int i = 1;

            if (table[hash].isDeleted())
            {
                return -1;
            }
            else if (table[hash].getKey() != key)
            {
                int newHash = (hash + i) % M;


                while (table[newHash].isDeleted() && table[newHash].getKey() != key)
                {
                    newHash = (hash + i * i) % M;
                    i += 1;
                }
                if (table[newHash].getKey() == key)
                    return newHash;
                else
                    return -1;

            }
            else
                return hash;
        }

            private int GetHash(int x)
        {
            return (int)Math.Floor((x * 1.0 % Prime * A) % 1 * M);
        }
    }
}