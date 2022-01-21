using StringSerchingAlgorithms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringSerchingAlgorithms
{
    public class RabinKarp : IStringSearchingAlgorithm
    {
        public int Contains(string text, string pattern)
        {
            int p = 113; //1000000007;
            int b = 256;

            var rh = new RollingHash(p, b);

            var n = text.Length;
            var m = pattern.Length;

            var pattern_hash = rh.Hash(pattern);
            var curr_hash = rh.Hash(text.Substring(0, m));

            if (curr_hash == pattern_hash && text.Substring(0, m) == pattern)
                return 0;

            var mul = rh.GetMultiplier(m);

            for (int i = 1; i <= n - m; i++)
            {
                var prev = text[i - 1];
                var next = text[i + m - 1];

                curr_hash = rh.SlideRight(curr_hash, prev, next, mul);

                if (curr_hash == pattern_hash && text.Substring(i, m) == pattern)
                    return i;
            }

            return -1;
        }
    }
}
