using StringSerchingAlgorithms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAlgorithms
{
    class TestStringSearchingAlgorithms
    {
        public static void Test(string text, string target)
        {
            var aa = System.Reflection.Assembly.GetAssembly(typeof(IStringSearchingAlgorithm));
            var bb = aa.GetTypes();
            var algorithms = bb.Where(p => typeof(IStringSearchingAlgorithm).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
                .ToList();

            Console.WriteLine($"Text = {text.Substring(0, 30)}" + (text.Length > 20 ? "..." : ""));
            Console.WriteLine($"Target = {target.Substring(0, 30) + (text.Length > 20 ? "..." : "")} \n");

            algorithms.ForEach(s =>
            {
                var instance = (IStringSearchingAlgorithm)Activator.CreateInstance(s);

                TestAlgorithm(instance, text, target);

                Console.Write("\n");
            });
        }

        private static void TestAlgorithm(IStringSearchingAlgorithm algorithm, string text, string target)
        {
            Console.Write($"[{algorithm.GetType().Name}] ");

            var sw = new Stopwatch();

            sw.Start();
            var index = algorithm.Contains(text, target);
            sw.Stop();

            var response = index == -1 ? "The text doesnt contain the word" : $"Find at index {index.ToString().PadLeft(10, '0')}";

            Console.WriteLine($"finish in {sw.Elapsed:m\\:ss\\.fff}");
            Console.WriteLine(response);
        }
    }
}
