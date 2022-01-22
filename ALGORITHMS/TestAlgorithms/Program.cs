using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TestAlgorithms
{
    class Program
    {
        private static int[] GetIntArray()
        {
            //return SortingAlgorithm.SortingAlgorithm.GetRandomArr(10000);
            return SortingAlgorithm.SortingAlgorithm.GetInOrderArr(10000, descending: !true);
        }

        private static string[] GetTextAndTarget()
        {
            var response = new string[2];

            //text
            response[0] = File.ReadAllText("Files/BIBLIA COMPLETA.txt", Encoding.Default);

            //target
            response[1] = "25 ¿Me ofrecisteis sacrificios y ofrendas en el desierto en cuarenta años, oh casa de Israel? 26 Antes bien, llevabais el tabernáculo de vuestro Moloc y Quiún, ídolos vuestros, la estrella de vuestros dioses que os hicisteis. 27 Os haré, pues, transportar más allá de Damasco, ha dicho Jehová, cuyo nombre es Dios de los ejércitos2.";

            return response;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("[TESTING ALGORITHMS]\n");

            var original_arr = GetIntArray();
            var textAndTarget = GetTextAndTarget();

            Console.WriteLine("\n===SORTING ALGORITHMS===\n");

            TestSortingAlgorithms.Test(original_arr);

            Console.WriteLine("\n===STRING SEARCHING ALGORITHMS===\n");

            TestStringSearchingAlgorithms.Test(textAndTarget[0], textAndTarget[1]);

            Console.ReadLine();
        }
    }
}
