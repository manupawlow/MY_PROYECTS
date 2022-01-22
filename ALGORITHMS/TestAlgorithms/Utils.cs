using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAlgorithms
{
    class Utils
    {
        public static string arrToString<T>(T[] arr)
        {
            var sb = new StringBuilder();

            if (arr.Count() > 20)
            {
                for (int i = 0; i < 6; i++)
                {
                    sb.Append(arr[i] + " ");
                }

                sb.Append("... ");

                for (int i = arr.Count() - 6; i < arr.Count(); i++)
                {
                    sb.Append(arr[i] + " ");
                }
            }
            else
            {
                for (int i = 0; i < arr.Count(); i++)
                {
                    sb.Append(arr[i] + " ");
                }
            }

            return sb.ToString();
        }
    }
}
