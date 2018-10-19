using System;

namespace HashTablesBoth.Entities
{
    /// <summary>
    /// Элемент данных хеш таблицы.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Ключ.
        /// </summary>
        public int Key { get; private set; }

        /// <summary>
        /// Хранимые данные.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Создать новый экземпляр хранимых данных Item.
        /// </summary>
        /// <param name="key"> Ключ. </param>
        /// <param name="value"> Значение. </param>
        public Item(int key, string value)
        {
            // Проверяем входные данные на корректность.

            // Устанавливаем значения.
            Key = key;
            Value = value;
        }
    }
}
