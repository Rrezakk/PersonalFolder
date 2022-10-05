using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFolder
{
    public static class Extensions
    {
        public static byte[] TrimTailingZeros(this byte[] arr)
        {
            return arr.Length == 0 ? arr : arr.Reverse().SkipWhile(x => x == 0).Reverse().ToArray();
        }
    }
}
