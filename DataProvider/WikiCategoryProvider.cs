using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF.Query;
using VDS.RDF.Storage;

namespace WEDT.DataProvider
{
    public class WikiCategoryProvider
    {
        SparqlConnector store;

        public WikiCategoryProvider()
        {
            store = new SparqlConnector(new Uri("http://pl.dbpedia.org/sparql"));
            
        }

        public String[] getSubcategories(String ubercategory)
        {

            List<String> list = new List<string>();
            var client = new WebClient();
            string url = 

                "http://pl.wikipedia.org/w/api.php?format=json&action=query&list=categorymembers&cmtitle=Category:"
                + ubercategory
                + "&cmtype=subcat&cmlimit=200";
            string html = client.DownloadString(url);
            //Console.WriteLine(html);

            dynamic json = System.Web.Helpers.Json.Decode(html);
            dynamic categories = json.query.categorymembers;
            int l = Enumerable.Count(categories);
            for (int i = 0; i < l; ++i)
            {
                dynamic obj = categories[i];
                String category = obj.title;
                list.Add(category.Remove(0, 10));

            }

            return list.ToArray();
        }
        public String[] getCategories(String article)
        {
            List<String> list = new List<string>();
            var client = new WebClient();
            string url = "http://pl.wikipedia.org/w/api.php?format=json&action=query&titles="
                + article
                + "&prop=categories&cllimit=200&continue";
            string html = client.DownloadString(url);
            dynamic json = System.Web.Helpers.Json.Decode(html).query.pages;

            foreach (KeyValuePair<string, dynamic> kvp in json)
            { // enumerating over it exposes the Properties and Values as a KeyValuePair
              //  Console.WriteLine("{0} = {1}", kvp.Key, kvp.Value);
                dynamic categories = kvp.Value.categories;
                int l = Enumerable.Count(categories);
                for (int i = 0; i < l; ++i)
                {
                    dynamic obj = categories[i];
                    String category = obj.title;
                    list.Add(category.Remove(0, 10));

                }
            }

            return list.ToArray();
        }

        public String[] getUbercategory(String a)
        {

            List<String> list = new List<String>();
            a = a.Replace(" ", "_");
            String strQuery;// = String.Format(strFormat, from);
            strQuery =
                "select ?a{ <http://pl.dbpedia.org/resource/Kategoria:"
                + a
                + "> <http://www.w3.org/2004/02/skos/core#broader> ?a }";
            Object results = store.Query(strQuery);
            if (results is SparqlResultSet)
            {
                SparqlResultSet rset = (SparqlResultSet)results;
                foreach (SparqlResult r in rset)
                {
                    String str = (r["a"].ToString());
                    str = str.Remove(0, 41);
                    str = Uri.UnescapeDataString(str);
                    list.Add(str);
                }
            }
            else
            {
                throw new Exception("Did not get a SPARQL Result Set as expected");
            }

            return list.ToArray();
        }
    }
}
