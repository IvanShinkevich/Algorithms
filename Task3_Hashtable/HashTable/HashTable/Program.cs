﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashTable.Entities;
using HashTable.Utils;

namespace HashTable
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем новую хеш таблицу.
            var hashTable = new HashTableChains();

            // Добавляем данные в хеш таблицу в виде пар Ключ-Значение.
            hashTable.Insert("Little Prince", "I never wished you any sort of harm; but you wanted me to tame you...");
            hashTable.Insert("Fox", "And now here is my secret, a very simple secret: It is only with the heart that one can see rightly; what is essential is invisible to the eye.");
            hashTable.Insert("Rose", "Well, I must endure the presence of two or three caterpillars if I wish to become acquainted with the butterflies.");
            hashTable.Insert("King", "He did not know how the world is simplified for kings. To them, all men are subjects.");

            // Выводим хранимые значения на экран.
            HashTableHelper.ShowHashTable(hashTable, "Created hashtable.");
            Console.ReadLine();

            // Удаляем элемент из хеш таблицы по ключу
            // и выводим измененную таблицу на экран.
            hashTable.Delete("King");
            HashTableHelper.ShowHashTable(hashTable, "Delete item from hashtable.");
            Console.ReadLine();

            // Получаем хранимое значение из таблицы по ключу.
            Console.WriteLine("Little Prince say:");
            var text = hashTable.Search("Little Prince");
            Console.WriteLine(text);
            Console.ReadLine();
        }
    }
}
