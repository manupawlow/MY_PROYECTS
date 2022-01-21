using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgorithm
{
    public class BubbleSort : SortingAlgorithm
    {
        public BubbleSort(int speed) : base(speed) { }

        public override void Sort(int[] arr, int[] indexes)
        {
            Iterations = 0;
            var arr_size = arr.Count();

            int i = 0, j = 0;
            for (i = 0; i < arr_size - 1; i++)
            {
                bool sorted = false;

                for (j = 0; j < arr_size - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        Swap(arr, j + 1, j);
                        
                        indexes[0] = j + 1;

                        System.Threading.Thread.Sleep(SortingSpeed);

                        sorted = true;
                    }
                }

                Iterations += j - 0;

                if (!sorted)
                    break;
            }
        }
    }
}
