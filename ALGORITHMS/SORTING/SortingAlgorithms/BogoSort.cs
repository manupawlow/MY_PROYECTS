using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgorithm
{
    public class BogoSort : SortingAlgorithm
    {
        private const int Attempts = 1000;
        
        public BogoSort(int speed) : base(speed) { }

        public override void Sort(int[] arr, int[] indexes)
        {
            var attempt = 0;

            while (!IsSorted(arr) && attempt++ <= Attempts)
            {
                Array.Copy(RandomizeArr(arr), arr, arr.Length);
                System.Threading.Thread.Sleep(SortingSpeed);
                Iterations++;
            }
        }

        private bool IsSorted(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if(arr[i] > arr[i + 1])
                    return false;
            }

            return true;
        }
    }
}
