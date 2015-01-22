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

        public List<String> pagelinksFrom(String from)
        {
            return this.getList(from, "WikiLink", false);
        }

        public List<String> pagelinksTo(String to)
        {
            return this.getList(to, "WikiLink", true);
        }

        public String[] disambiguates(String from)
        {
            return this.getList(from, "Disambiguates", false).ToArray();
        }


        private List<String> getList(String a, String from, bool reverse)
        {

            List<String> list = new List<String>();
            a = a.Replace(" ", "_");
            String strQuery;// = String.Format(strFormat, from);
            strQuery = "SELECT * WHERE {"
                + (!reverse ? resource(a) : " ?a ")
                + " <http://dbpedia.org/ontology/wikiPage"
                + from
                + "> "
                + (reverse ? resource(a) : " ?a ")
                + ". }";
            Object results;
            try
            {
                results = store.Query(strQuery);
            }
            catch
            {
                return null;
            }

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

        private String resource(String name)
        {
            return "<http://pl.dbpedia.org/resource/" + DataPreparator.FirstCharToUpper(name) + ">";
        }
    }
}
