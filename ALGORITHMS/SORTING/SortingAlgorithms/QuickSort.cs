using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgorithm
{
    public class QuickSort : SortingAlgorithm
    {
        public QuickSort(int speed) : base(speed) { }

        public override void Sort(int[] arr, int[] indexes) => quickSort(arr, 0, arr.Count() - 1, indexes);

        private void quickSort(int[] arr, int start, int end, int[] indexes)
        {
            if (start >= end)
                return;

            var index = divide(arr, start, end, indexes);

            indexes[1] = index;

            quickSort(arr, start, index - 1, indexes);
            quickSort(arr, index + 1, end, indexes);


            //var leftSorting = Task.Run( () => quickSort(arr, start, index - 1));
            //var rightSorting = Task.Run(() => quickSort(arr, index, end));

            //Task.WaitAll(new Task[] { leftSorting, rightSorting });
        }

        private int divide(int[] arr, int start, int end, int[] indexes)
        {
            var pivot_ix = start;
            var pivot = arr[end];

            for (int i = start; i < end; i++)
            {
                if(arr[i] < pivot)
                {
                    Swap(arr, i, pivot_ix);

                    indexes[0] = i;

                    System.Threading.Thread.Sleep(SortingSpeed);

                    pivot_ix++;
                }
            }

            Iterations += end - start;

            Swap(arr, pivot_ix, end);

            return pivot_ix;
        }
    }
}
