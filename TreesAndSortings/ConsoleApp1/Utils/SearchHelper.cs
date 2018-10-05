using System;

namespace ConsoleApp1
{
    class SearchHelper
    {
        public int InterpolationSearch(int[] x, int searchValue)
        {
            // Returns index of searchValue in sorted input data
            // array x, or -1 if searchValue is not found
            int low = 0;
            int high = x.Length - 1;
            int mid;

            while (x[low] < searchValue && x[high] >= searchValue)
            {
                mid = low + ((searchValue - x[low]) * (high - low)) / (x[high] - x[low]);

                if (x[mid] < searchValue)
                    low = mid + 1;
                else if (x[mid] > searchValue)
                    high = mid - 1;
                else
                    return mid;
            }

            if (x[low] == searchValue)
                return low;
            else
                return -1; // Not found
        }

        public int BinarySearch(int x, int[] arr)
        {
            int i = 0;
            int j = -1;
            if (arr != null)
            {
                int start = 0, end = arr.Length, mid;
                while (start < end)
                {
                    ++i;
                    mid = (start + end) / 2;
                    if (x == arr[mid])
                    {
                        j = mid;
                        //Console.WriteLine(i + " iterations, position - " + j);
                        break;
                    }
                    else
                    {
                        if (x < arr[mid])
                        {
                            end = mid;
                        }
                        else
                        {
                            start = mid + 1;
                        }
                    }
                }
            }
            if (j == -1)
                Console.WriteLine("Not found.");
            return j;
        }
    }
}
