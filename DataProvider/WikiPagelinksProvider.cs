using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Query;
using VDS.RDF.Storage;

namespace WEDT.DataProvider
{
    public class WikiPagelinksProvider
    {
        SparqlConnector store;

        public WikiPagelinksProvider()
        {
            store = new SparqlConnector(new Uri("http://pl.dbpedia.org/sparql"));
        }

        public List<String> pagelinks(String from)
        {
            return this.getList(from, "WikiLink");
        }

        public List<String> disambiguates(String from)
        {
            return this.getList(from, "Disambiguates");
        }

        private List<String> getList(String a, String from)
        {

            List<String> list = new List<String>();

            String strQuery;// = String.Format(strFormat, from);
            strQuery = "SELECT * WHERE { <http://pl.dbpedia.org/resource/"
                + DataPreparator.FirstCharToUpper(a)
                + "> <http://dbpedia.org/ontology/wikiPage"
                + from
                + "> ?a . }";
            Object results = store.Query(strQuery);
            if (results is SparqlResultSet)
            {
                SparqlResultSet rset = (SparqlResultSet)results;
                foreach (SparqlResult r in rset)
                {
                    String str = (r["a"].ToString());
                    str = str.Remove(0, 31);
                    str = Uri.UnescapeDataString(str);
                    list.Add(str);
                }
            }
            else
            {
                throw new Exception("Did not get a SPARQL Result Set as expected");
            }

            return list;
        }
    }
}
