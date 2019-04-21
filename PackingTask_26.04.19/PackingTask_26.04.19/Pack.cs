using System;
using System.Collections.Generic;
using System.Text;

namespace PackingTask
{
    public class Pack
    {
        public List<double> packs = new List<double>();
        private Dictionary<int, List<double>> itemsInPack = new Dictionary<int, List<double>>();

        public Pack()
        {
            packs = new List<double>();
            itemsInPack = new Dictionary<int, List<double>>();
        }

        private void AddItemToPackList(int packNumber, double packItem)
        {
            if (itemsInPack.Count == packNumber)
            {
                itemsInPack.Add(packNumber, new List<double>() { packItem });
            }
            else
            {
                itemsInPack[packNumber].Add(packItem);
            }
        }

        public void AddItem(double item)
        {
            if(item > 1 || item < 0)
            {
                Console.WriteLine("Incorrect item size, max allowed size - 1, min allowed size - 0");
                return;
            }

            bool flag = false;
            for(int i=0;i<packs.Count; i++)
            {
                if(packs[i] - item >= 0)
                {
                    packs[i] = packs[i] - item;
                    flag = true;
                    AddItemToPackList(i, item);
                    break;
                }
            }

            if (!flag)
            {
                packs.Add(1 - item);
                AddItemToPackList(itemsInPack.Count, item);
            }
        }

        public void ShowPacksLoad()
        {
            foreach(var el in itemsInPack)
            {
                Console.Write($"Pack number: {el.Key}, pack values: ");
                foreach(var itemValues in el.Value)
                {
                    Console.Write($"{itemValues} ");
                }
            }
            Console.WriteLine();
        }
    }
}
