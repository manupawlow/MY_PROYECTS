using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAlgorithms
{
    class TestSortingAlgorithms
    {
        public static void Test(int[] original_arr)
        {
            var sortingAlgorithms = System.Reflection.Assembly.GetAssembly(typeof(SortingAlgorithm.SortingAlgorithm))
                .GetTypes().Where(t => t.IsSubclassOf(typeof(SortingAlgorithm.SortingAlgorithm))).ToList();

            Console.WriteLine($"Arr = {Utils.arrToString(original_arr)}\n");

            sortingAlgorithms.ForEach(s =>
            {
                var copy_arr = new int[original_arr.Count()];

                Array.Copy(original_arr, 0, copy_arr, 0, original_arr.Length);

                var instance = (SortingAlgorithm.SortingAlgorithm)Activator.CreateInstance(s, 0);

                TestAlgorithm(instance, copy_arr);

                Console.Write("\n");
            });
        }

        private static void TestAlgorithm(SortingAlgorithm.SortingAlgorithm algorithm, int[] arr)
        {
            Console.Write($"[{algorithm.GetType().Name}] ");

            var sw = new Stopwatch();

            sw.Start();
            algorithm.Sort(arr, new int[] { -1, -1 });
            sw.Stop();

            Console.WriteLine($"finish in {sw.Elapsed:m\\:ss\\.fff}");
            Console.WriteLine(Utils.arrToString(arr));
        }
    }
}
