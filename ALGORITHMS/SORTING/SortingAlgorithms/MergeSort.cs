using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgorithm
{
    public class MergeSort : SortingAlgorithm
    {
        public MergeSort(int speed) : base(speed) { }

        public override void Sort(int[] arr, int[] indexes)
        {
            mergeSort(arr, 0, arr.Length - 1, indexes);
        }

        private void mergeSort(int[] arr, int start, int end, int[] indexes)
        {
            if (start >= end)
                return;

            var mid = start + (end - start) / 2;

            mergeSort(arr, start, mid, indexes);
            mergeSort(arr, mid + 1, end, indexes);

            merge(arr, start, mid, end, indexes);
        }

        private void merge(int[] arr, int start, int mid, int end, int[] indexes)
        {
            var sizeL = mid - start + 1;
            var sizeR = end - mid;

            var left = new int[sizeL];
            var right = new int[sizeR];

            int i, j, k;

            for (i = 0; i < sizeL; i++)
                left[i] = arr[start + i];
            for (j = 0; j < sizeR; j++)
                right[j] = arr[mid + 1 + j];

            i = 0; j = 0; k = start;

            while(i < sizeL && j < sizeR)
            {
                if(left[i] <= right[j])
                {
                    arr[k] = left[i];
                    i++;

                }
                else
                {
                    arr[k] = right[j];
                    j++;
                }

                indexes[0] = k;
                System.Threading.Thread.Sleep(SortingSpeed);

                k++;
            }

            for(; i < sizeL; i++)
            {
                arr[k] = left[i];

                indexes[0] = k;
                System.Threading.Thread.Sleep(SortingSpeed);

                k++;
            }

            for (; j < sizeR; j++)
            {
                arr[k] = right[j];

                indexes[0] = k;
                System.Threading.Thread.Sleep(SortingSpeed);

                k++;
            }

        }
    }
}
