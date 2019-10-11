using System;
using System.Collections.Generic;
using System.Linq;

namespace PeriwinkleApp.Core.Test.Utils
{
    public class StringUtils
    {
        public static List <string> ToStringList (string str, char delimiter = '\n')
        {
            var splitted = str.Split (delimiter);

            return splitted.Select (s => "\"" + s + "\", ").ToList ();
        }
    }
}
