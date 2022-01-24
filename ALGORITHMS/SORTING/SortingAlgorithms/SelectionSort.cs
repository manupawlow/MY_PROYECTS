using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgorithm
{
    public class SelectionSort : SortingAlgorithm
    {
        public SelectionSort(int speed) : base(speed) { }

        public override void Sort(int[] arr, int[] indexes)
        {
            Iterations = 0;
            var arr_size = arr.Count();

            int i, j;
            for (i = 0; i < arr_size; i++)
            {
                var min_index = i;

                for (j = i + 1; j < arr_size; j++)
                {
                    indexes[0] = j;
                    System.Threading.Thread.Sleep(SortingSpeed);

                    if (arr[j] < arr[min_index])
                    {
                        min_index = j;
                    }
                }

                Swap(arr, i, min_index);

                indexes[0] = min_index;

                System.Threading.Thread.Sleep(SortingSpeed);

                Iterations += j - i;
            }
        }
    }
}
