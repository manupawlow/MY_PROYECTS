using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringSerchingAlgorithms
{
    public class CSharpTool : IStringSearchingAlgorithm
    {
        public int Contains(string text, string word) => text.IndexOf(word);
    }
}
