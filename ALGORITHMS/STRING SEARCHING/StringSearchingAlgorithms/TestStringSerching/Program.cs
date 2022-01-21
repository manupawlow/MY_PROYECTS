using StringSerchingAlgorithms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStringSerching
{
    class Program
    {
        static void Main(string[] args)
        {
            var TEXT = File.ReadAllText("Files/BIBLIA COMPLETA.txt", Encoding.Default);

            var WORD = "25 ¿Me ofrecisteis sacrificios y ofrendas en el desierto en cuarenta años, oh casa de Israel? 26 Antes bien, llevabais el tabernáculo de vuestro Moloc y Quiún, ídolos vuestros, la estrella de vuestros dioses que os hicisteis. 27 Os haré, pues, transportar más allá de Damasco, ha dicho Jehová, cuyo nombre es Dios de los ejércitos.";

            //TEXT = "abcdefghijk";

            //WORD = "bcd";

            IStringSearchingAlgorithm stringSearchingAlgorithm;

            //C SHARP TOOL
            stringSearchingAlgorithm = new CSharpTool();
            TestAlgorithm(stringSearchingAlgorithm, TEXT, WORD, "C# Tool");

            //BRUTE FORCE
            stringSearchingAlgorithm = new BruteForce();
            TestAlgorithm(stringSearchingAlgorithm, TEXT, WORD, "Brute Force");

            //RABIN-KARP
            stringSearchingAlgorithm = new RabinKarp();
            TestAlgorithm(stringSearchingAlgorithm, TEXT, WORD, "Rabin-Karp");

            Console.ReadLine();
        }

        static string TimeSpanToString(TimeSpan ts) => String.Format("{0:00}:{1:00}.{2:000000}", ts.Minutes, ts.Seconds, ts.Milliseconds * 1.0 / 10.0);

        static private void TestAlgorithm(IStringSearchingAlgorithm stringSerchingAlgorithm, string text, string word, string algorithm_name)
        {
            Console.WriteLine($"Starting {algorithm_name}...");
            var sw = new Stopwatch();

            sw.Start();
            var rabinKarp = stringSerchingAlgorithm.Contains(text, word);
            sw.Stop();

            var result = rabinKarp == -1 ? "The text doesnt contain the word" : $"Word find at index {rabinKarp.ToString().PadLeft(10, '0')}";

            Console.WriteLine($"{result} in {sw.Elapsed:m\\:ss\\.fff} [{algorithm_name}]");
        }
    }
}
