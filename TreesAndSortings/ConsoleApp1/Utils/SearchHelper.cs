using System;

namespace ConsoleApp1
{
    class SearchHelper
    {
        public int InterpolationSearch(int[] array, int value)
        {
            int index = -1;
            int left = 0;
            int right = array.Length - 1;
            int middle;
            while (array[left] < value && array[right] > value)
            {
                middle = left + ((value - array[left]) * (right - left)) / (array[right] - array[left]);
                if (value == array[middle])
                {
                    index = middle;
                    break;
                }
                else if (value > array[middle])
                {
                    left = middle + 1;
                }
                else {
                    right = middle - 1;
                }
            }
            if (array[left] == value)
            {
                index = left;
            }
            else if (array[right] == value)
            {
                index = right;
            }
            return index;
        }

        public int BinarySearch(int[] array, int val)
        {
            int index = -1;
            int left = 0;
            int right = array.Length - 1;
            int middle;
            while (array[left] < val && array[right] > val)
            {
                middle = left + (right - left) / 2;
                if (val == array[middle])
                {
                    index = middle;
                    break;
                }
                else if (val < array[middle])
                {
                    right = middle - 1;
                }
                else {
                    left = middle + 1;
                }
            }
            if (val == array[left])
            {
                index = left;
            }
            else if (val == array[right])
            {
                index = right;
            }
            return index;
        }
    }
}
