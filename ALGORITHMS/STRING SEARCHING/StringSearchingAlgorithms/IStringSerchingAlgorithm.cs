using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringSerchingAlgorithms
{
    public interface IStringSearchingAlgorithm
    {
        int Contains(string text, string word);
    }
}
