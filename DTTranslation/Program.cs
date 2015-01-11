using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEDT.DBTranslation
{
    class Program
    {
        static void Main(string[] args)
        {
            TripleReader reader = new TripleReader("DBPediaTriple.nt", "dbLink.sqlite");
            reader.translate();

        }
    }
}
