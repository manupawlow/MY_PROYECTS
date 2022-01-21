using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgorithm
{
    public class HeapSort : SortingAlgorithm
    {
        public HeapSort(int speed) : base(speed) { }

        public override void Sort(int[] arr, int[] indexes)
        {
            var arr_size = arr.Length;

            int i;
            for (i = arr_size / 2 - 1; i >= 0; i--)
                heapify(arr, arr_size, i, indexes);

            for (i = arr_size - 1; i > 0; i--)
            {
                Swap(arr, 0, i);
                heapify(arr, i, 0, indexes);
            }
        }

        private void heapify(int[] arr, int n, int root, int[] indexes)
        {
            var indexL = 2 * root + 1;
            var indexR = 2 * root + 2;

            var largestIndex = root;

            if (indexL < n && arr[indexL] > arr[largestIndex])
            {
                largestIndex = indexL;
            }

            if (indexR < n && arr[indexR] > arr[largestIndex])
            {
                largestIndex = indexR;
            }

            if(largestIndex != root)
            {
                Swap(arr, largestIndex, root);

                indexes[0] = root;
                System.Threading.Thread.Sleep(SortingSpeed);

                heapify(arr, n, largestIndex, indexes);
            }
        }

    }
}
