using System;

namespace ConsoleApp1
{
    class SearchHelper
    {
        //public int InterpolationSearch(int x, int[] arr)
        //{
        //    int start = 0, end = (arr.Length - 1), k = 0;
        //    while (start <= end && x >= arr[start] && x <= arr[end])
        //    {
        //        ++k;
        //        int pos = ((end - start) * (x - arr[start])) / (arr[end] - arr[start]) + start;
        //        if (arr[(int)pos] == x)
        //        {
        //            //Console.WriteLine(k + "iterations;  position - " + pos);
        //            return (int)pos;
        //        }
        //        if (arr[pos] < x)
        //            start = pos + 1;
        //        else
        //            end = pos - 1;
        //    }
        //    Console.WriteLine("Not found");
        //    return -1;
        //}

        //public int InterpolationSearch(int[] list, int data)
        //{
        //    int lo = 0;
        //    int mid = -1;
        //    int hi = list.Length - 1;
        //    int index = -1;

        //    while (lo <= hi)
        //    {
        //        mid = (int)(lo + (((double)(hi - lo) / (list[hi] - list[lo])) * (data - list[lo])));

        //        if (list[mid] == data)
        //        {
        //            index = mid;
        //            break;
        //        }
        //        else
        //        {
        //            if (list[mid] < data)
        //                lo = mid + 1;
        //            else
        //                hi = mid - 1;
        //        }
        //    }

        //    return index;
        //}

        //public int InterpolationSearch(int[] arr, int x)
        //{
        //    // Find indexes of 
        //    // two corners 
        //    int lo = 0, hi = (arr.Length - 1);

        //    // Since array is sorted,  
        //    // an element present in 
        //    // array must be in range 
        //    // defined by corner 
        //    while (lo <= hi &&
        //           x >= arr[lo] &&
        //           x <= arr[hi])
        //    {
        //        // Probing the position  
        //        // with keeping uniform  
        //        // distribution in mind. 
        //        int pos = lo + (((hi - lo) /
        //                         (arr[hi] - arr[lo])) *
        //                        (x - arr[lo]));

        //        // Condition of  
        //        // target found 
        //        if (arr[pos] == x)
        //            return pos;

        //        // If x is larger, x 
        //        // is in upper part 
        //        if (arr[pos] < x)
        //            lo = pos + 1;

        //        // If x is smaller, x  
        //        // is in the lower part 
        //        else
        //            hi = pos - 1;
        //    }
        //    return -1;
        //}
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
