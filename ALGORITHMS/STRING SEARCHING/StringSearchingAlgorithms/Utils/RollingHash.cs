using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringSerchingAlgorithms.Utils
{
    public class RollingHash
    {
        private int PRIME_MOD { get; set; }
        private int BASE { get; set; }

        public RollingHash(int p, int b)
        {
            PRIME_MOD = p;
            BASE = b;
        }

        public int Hash(string str)
        {
            var hash_value = 0;

            //for (int i = 0; i < str.Length; hash_value += (str[i] - 'a' + 1) * Math.Pow(10, str.Length - (i++ + 1))) ;

            for (int i = 0; i < str.Length; hash_value = (hash_value * BASE + str[i++]) % PRIME_MOD) ;

            return hash_value;
        }

        public int SlideRight(int prevHash, int prev, int next, int multiplier)
        {
            prevHash += PRIME_MOD;
            prevHash -= (multiplier * prev) % PRIME_MOD;
            prevHash *= BASE;
            prevHash += next;
            prevHash %= PRIME_MOD;

            return prevHash;
        }

        public int GetMultiplier(int str_size)
        {
            var multiplier = 1;
            for (int j = 1; j < str_size; j++)
            {
                multiplier = (multiplier * BASE) % PRIME_MOD;
            }

            return multiplier;
        }
    }
}
