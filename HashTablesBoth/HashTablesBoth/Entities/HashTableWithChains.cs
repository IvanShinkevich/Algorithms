using System;
using System.Collections.Generic;
using System.Linq;

namespace HashTablesBoth.Entities
{
    public class HashTableWithChains
    {
        private double Prime = 3571;
        public double A = 0.6180339887;
        public int M = 1000;

        /// <summary>
        /// Коллекция хранимых данных.
        /// </summary>
        /// <remarks>
        /// Представляет собой словарь, ключ которого представляет собой хеш ключа хранимых данных,
        /// а значение это список элементов с одинаковым хешем ключа.
        /// </remarks>
        private static Dictionary<int, List<Item>> _items = null;

        /// <summary>
        /// Коллекция хранимых данных в хеш-таблице в виде пар Хеш-Значения.
        /// </summary>
        public IReadOnlyCollection<KeyValuePair<int, List<Item>>> Items => _items?.ToList()?.AsReadOnly();

        /// <summary>
        /// Создать новый экземпляр класса HashTable.
        /// </summary>
        public HashTableWithChains()
        {
            // Инициализируем коллекцию максимальным количество элементов.
            _items = new Dictionary<int, List<Item>>(M);
        }

        /// <summary>
        /// Добавить данные в хеш таблицу.
        /// </summary>
        /// <param name="key"> Ключ хранимых данных. </param>
        /// <param name="value"> Хранимые данные. </param>
        public void Insert(int key, string value)
        {
            // Создаем новый экземпляр данных.
            var item = new Item(key, value);

            // Получаем хеш ключа
            var hash = GetHash(item.Key);

            // Получаем коллекцию элементов с таким же хешем ключа.
            // Если коллекция не пустая, значит заначения с таким хешем уже существуют,
            // следовательно добавляем элемент в существующую коллекцию.
            // Иначе коллекция пустая, значит значений с таким хешем ключа ранее не было,
            // следовательно создаем новую пустую коллекцию и добавляем данные.
            List<Item> hashTableItem = null;
            if (_items.ContainsKey(hash))
            {
                // Получаем элемент хеш таблицы.
                hashTableItem = _items[hash];

                // Проверяем наличие внутри коллекции значения с полученным ключом.
                // Если такой элемент найден, то сообщаем об ошибке.
                var oldElementWithKey = hashTableItem.SingleOrDefault(i => i.Key == item.Key);
                if (oldElementWithKey != null)
                {
                    throw new ArgumentException($"Хеш-таблица уже содержит элемент с ключом {key}. Ключ должен быть уникален.", nameof(key));
                }

                // Добавляем элемент данных в коллекцию элементов хеш таблицы.
                _items[hash].Add(item);
            }
            else
            {
                // Создаем новую коллекцию.
                hashTableItem = new List<Item> { item };

                // Добавляем данные в таблицу.
                _items.Add(hash, hashTableItem);
            }
        }

        /// <summary>
        /// Удалить данные из хеш таблицы по ключу.
        /// </summary>
        /// <param name="key"> Ключ. </param>
        public void Delete(int key)
        {

            // Получаем хеш ключа.
            var hash = GetHash(key);

            // Если значения с таким хешем нет в таблице, 
            // то завершаем выполнение метода.
            if (!_items.ContainsKey(hash))
            {
                return;
            }

            // Получаем коллекцию элементов по хешу ключа.
            var hashTableItem = _items[hash];

            // Получаем элемент коллекции по ключу.
            var item = hashTableItem.SingleOrDefault(i => i.Key == key);

            // Если элемент коллекции найден, 
            // то удаляем его из коллекции.
            if (item != null)
            {
                hashTableItem.Remove(item);
            }
        }

        /// <summary>
        /// Поиск значения по ключу.
        /// </summary>
        /// <param name="key"> Ключ. </param>
        /// <returns> Найденные по ключу хранимые данные. </returns>
        public string Search(int key)
        {
            // Получаем хеш ключа.
            var hash = GetHash(key);

            // Если таблица не содержит такого хеша,
            // то завершаем выполнения метода вернув null.
            if (!_items.ContainsKey(hash))
            {
                return null;
            }

            // Если хеш найден, то ищем значение в коллекции по ключу.
            var hashTableItem = _items[hash];

            // Если хеш найден, то ищем значение в коллекции по ключу.
            if (hashTableItem != null)
            {
                // Получаем элемент коллекции по ключу.
                var item = hashTableItem.SingleOrDefault(i => i.Key == key);

                // Если элемент коллекции найден, 
                // то возвращаем хранимые данные.
                if (item != null)
                {
                    return item.Value;
                }
            }

            // Возвращаем null если ничего найдено.
            return null;
        }

        /// <summary>
        /// Хеш функция.
        /// </summary>
        /// <remarks>
        /// Возвращает длину строки.
        /// </remarks>
        /// <param name="value"> Хешируемая строка. </param>
        /// <returns> Хеш строки. </returns>
        private int GetHash(int value)
        {     
            return (int)Math.Floor((value*1.0 % Prime * A) % 1 * M);
        }

        /// <summary>
        /// Вывод хеш-таблицы на экран.
        /// </summary>
        /// <param name="hashTable"> Хеш таблица. </param>
        /// <param name="title"> Заголовок перед выводом таблицы. </param>
        public static void ShowHashTable(HashTableWithChains hashTable, string title)
        {
            // Проверяем входные аргументы.
            if (hashTable == null)
            {
                throw new ArgumentNullException(nameof(hashTable));
            }

            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            // Выводим все имеющие пары хеш-значение
            Console.WriteLine(title);
            foreach (var item in hashTable.Items)
            {
                // Выводим хеш
                Console.WriteLine(item.Key);

                // Выводим все значения хранимые под этим хешем.
                foreach (var value in item.Value)
                {
                    Console.WriteLine($"\t{value.Key} - {value.Value}");
                }
            }
            Console.WriteLine();
        }

        public int GetLargestChainLength()
        {
            int largestChainLength = 0;
            foreach (var keyValuePair in _items)
            {
                if (keyValuePair.Value.Count > largestChainLength)
                {
                    largestChainLength = keyValuePair.Value.Count;
                }
            }
            return largestChainLength;
        }
    }
}
