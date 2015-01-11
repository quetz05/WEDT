using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WEDT.DBTranslation
{
    class TripleReader
    {
        private StreamReader stream;
        private Match match;
        private String analyzedText;
        private DatabaseWriter db;

        public TripleReader(String filePath, String dbFilePath)
        {
            stream = new StreamReader(filePath);
            db = new DatabaseWriter(dbFilePath);
        }

        public void translate()
        {
            int i = 0;
            while ((analyzedText = stream.ReadLine()) != null)
            {
                ++i;
             //   Console.WriteLine(analyzedText);
             
                if (isWord())
                {
                    String first = findWord(true);
                    if (isWord())
                    {
                        String second = findWord(false);
                       // Console.WriteLine("Slowa: {0} {1}", first, second);
                        db.saveLink(first, second);
                    }
 
                }
             //   if (i > 4) break;
            }

            stream.Close();
            Console.WriteLine("Stworzono baze o {0} rekordach", i);
            Console.ReadLine();
        }

        private Boolean isWord()
        {
            String strRegex = @"\<http:\/\/pl.dbpedia.org\/resource\/.*?\>";
            Regex regex = new Regex(strRegex);
            match = regex.Match(analyzedText);
            return match.Success;
        }

        private String findWord(bool isFirst)
        {
            String matchString = match.Groups[0].ToString();
            if(isFirst)
                analyzedText = analyzedText.Remove(0, matchString.Length);
            matchString = matchString.Remove(0, 32);
            matchString = matchString.Remove(matchString.Length - 1);
            Regex rx = new Regex(@"\\[uU]([0-9A-F]{4})");
            matchString = rx.Replace(matchString, matchh => ((char)Int32.Parse(matchh.Value.Substring(2), NumberStyles.HexNumber)).ToString());
            matchString = matchString.Replace("'", "''");
            return matchString;
        }

    }
}
