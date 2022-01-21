using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgorithm
{
    public class InsertionSort : SortingAlgorithm
    {
        public InsertionSort(int speed) : base(speed) { }

        public override void Sort(int[] arr, int[] indexes)
        {
            Iterations = 0;
            var arr_size = arr.Count();

            int i, j;
            for (i = 1; i < arr_size; i++)
            {
                var aux = arr[i];

                j = i - 1;

                while (j >= 0 && arr[j] > aux)
                {
                    //Swap(arr, j, j + 1);

                    arr[j + 1] = arr[j];

                    indexes[0] = j + 1;

                    System.Threading.Thread.Sleep(SortingSpeed);

                    j--;
                }
                arr[j + 1] = aux;
                //Swap(arr, i, j + 1);
            }
        }
    }
}
