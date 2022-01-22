using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgorithm
{
    public abstract class SortingAlgorithm
    {
        public int SortingSpeed { get; set; }
        public SortingAlgorithm(int sortingSpeed = 0)
        {
            SortingSpeed = sortingSpeed;
        }

        public int Iterations { get; set; }

        public abstract void Sort(int[] arr, int[] indexes);

        public void Swap(int[] arr, int index1, int index2)
        {
            var aux = arr[index1];
            arr[index1] = arr[index2];
            arr[index2] = aux;   
        }

        public static int[] GetRandomArr(int elements) => RandomizeArr(GetInOrderArr(elements));

        public static int[] RandomizeArr(int[] arr)
        {
            var rnd = new Random();
            return arr.OrderBy(x => rnd.Next()).ToArray();
        }

        public static int[] GetInOrderArr(int elements, bool descending = false)
        {
            var arr = new int[elements];
            foreach (var i in Enumerable.Range(1, elements))
                arr[i - 1] = !descending ? i : elements - i;

            return arr;
        }
    }
}
