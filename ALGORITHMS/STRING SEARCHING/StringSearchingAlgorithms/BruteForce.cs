using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringSerchingAlgorithms
{
    public class BruteForce : IStringSearchingAlgorithm
    {
        public int Contains(string text, string word)
        {
            var n = text.Length;
            var m = word.Length;

            for(int i = 0; i < n - m; i++)
            {
                //if (text.Substring(i, m) == word)
                //    return i;

                var diferent = false;
                for(int j=0; j < m; j++)
                {
                    if (text[i + j] != word[j])
                    {
                        diferent = true;
                        break;
                    }
                }

                if (!diferent)
                    return i;
            }

            return -1;
        }
    }
}
