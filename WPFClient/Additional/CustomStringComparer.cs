using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFClient.Additional
{
   public class CustomStringComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return string.Compare(x, y) == 0;
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
