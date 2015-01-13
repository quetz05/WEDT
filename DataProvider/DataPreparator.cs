using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WEDT.DataProvider
{
    class DataPreparator
    {
        static public String utfConversion(String str)
        {
            Regex rx = new Regex(@"\\[uU]([0-9A-F]{4})");
            String matchString = rx.Replace(str, match => ((char)Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString());
            return matchString;
        }

        public static String FirstCharToUpper(String input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}
