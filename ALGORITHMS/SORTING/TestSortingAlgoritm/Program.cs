using SortingAlgorithm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SA = SortingAlgorithm.SortingAlgorithm;

namespace TestSortingAlgoritm
{
    class Program
    {
        static int[] arr;

        static void Main(string[] args)
        {
            //arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            //arr = new int[] { 9, 4, 1, 12, 15, 2, 6, 11, 3, 10, 8, 7, 13, 14, 5 };
            //arr = new int[] { 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            arr = new int[] { 5, 4, 3, 2, 1 };
            //arr = SA.GetRandomArr(100);
            //arr = SA.GetInOrderArr(10000, descending: !true);

            while (true)
            {
                Console.Clear();

                showArr(arr);
                Console.WriteLine($"Sorting an array with {arr.Count()} elements...\n");

                var sortingAlgorithms = new List<Type>()
                {
                    typeof(BogoSort),
                    typeof(HeapSort),
                    typeof(SelectionSort),
                    typeof(QuickSort),
                    typeof(BubbleSort),
                };

                sortingAlgorithms.ForEach(s =>
                {
                    var copy_arr = new int[arr.Count()];
                    Array.Copy(arr, 0, copy_arr, 0, arr.Length);

                    var instance = (SA)Activator.CreateInstance(s, 0);

                    TestAlgorithm(instance, copy_arr, s.Name);

                    Console.Write("\n");
                });

                Console.Write("Finish all sorting algorithms!");
                Console.Read();
            }

        }

        static private void TestAlgorithm(SA sortingAlgorithm, int[] arr, string algorithm_name)
        {
            Console.WriteLine($"Starting {algorithm_name}...");
            var sw = new Stopwatch();

            sw.Start();
            sortingAlgorithm.Sort(arr, new int[] { -1, -1 });
            sw.Stop();

            showArr(arr);
            Console.WriteLine($"Sorted in {sortingAlgorithm.Iterations.ToString("N0")} iterations in {sw.Elapsed:m\\:ss\\.fff}");
        }

        public static void showArr(int[] arr)
        {
            if(arr.Count() > 20)
            {
                for(int i = 0; i < 6; i++)
                    Console.Write(arr[i] + " ");

                Console.Write("... ");

                for (int i = arr.Count() - 6; i < arr.Count(); i++)
                    Console.Write(arr[i] + " ");
            }
            else
                for(int i = 0; i < arr.Count(); i++)
                    Console.Write(arr[i] + " ");
            
            Console.Write("\n");
        }
    }
}
