using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WEDT.DataProvider;
using System.Collections.Generic;

namespace DataProviderTests
{
    [TestClass]
    public class DataProviderTests
    {
        [TestMethod]
        public void wikiRedirectsProviderTest()
        {
            WikiRedirectsProvider provider = new WikiRedirectsProvider();
            String result = provider.redirect("Akronim");
            Assert.IsTrue(result.Equals("Skrótowiec"), "Akronim -> Skrótowiec");
            result = provider.redirect("akronim");
            Assert.IsTrue(result.Equals("Skrótowiec"), "akronim -> Skrótowiec");
            result = provider.redirect("Akrnim");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void wikiPagelinksProviderTest()
        {
            WikiPagelinksProvider provider = new WikiPagelinksProvider();
            List< String> links = new List<string> ( provider.pagelinksFrom("Alergologia"));
            Assert.AreEqual(6, links.Count);
            Assert.IsTrue(links.Contains("Alergen"));
            Assert.IsTrue(links.Contains("Alergia"));
            Assert.IsTrue(links.Contains("Kategoria:Alergologia"));
            Assert.IsTrue(links.Contains("Kategoria:Specjalności_lekarskie"), "Polskie znaki");
            Assert.IsTrue(links.Contains("Medycyna"));
            Assert.IsTrue(links.Contains("Leczenie"));

            links = new List<string> ( provider.pagelinksFrom("alergologia"));
            Assert.AreEqual(6, links.Count);
            Assert.IsTrue(links.Contains("Alergen"));
            Assert.IsTrue(links.Contains("Alergia"));
            Assert.IsTrue(links.Contains("Kategoria:Alergologia"));
            Assert.IsTrue(links.Contains("Kategoria:Specjalności_lekarskie"), "Polskie znaki");
            Assert.IsTrue(links.Contains("Medycyna"));
            Assert.IsTrue(links.Contains("Leczenie"));

            links = new List<string> ( provider.pagelinksFrom("Alegogosdf"));
            Assert.AreEqual(0, links.Count, "Pusta lista");

        }

        [TestMethod]
        public void wikiPagelinksProviderToTest()
        {
            WikiPagelinksProvider provider = new WikiPagelinksProvider();
            List<String> links = new List<string>(provider.pagelinksTo("Alergologia"));
            Assert.AreEqual(22, links.Count);

            Assert.IsTrue(links.Contains("Alergen"));
            Assert.IsTrue(links.Contains("Alergia"));
            Assert.IsTrue(links.Contains("Lekarz"));

            links = new List<string>(provider.pagelinksFrom("Alegogosdf"));
            Assert.AreEqual(0, links.Count, "Pusta lista");

        }

        [TestMethod]
        public void wikiPagelinksProviderDisambiguatesTest()
        {
            WikiPagelinksProvider provider = new WikiPagelinksProvider();
            List<String> links = new List<string> ( provider.disambiguates("Al"));
            Assert.AreEqual(14, links.Count);
            Assert.IsTrue(links.Contains("Alabama"));
            Assert.IsTrue(links.Contains("Albania"));

        }

        [TestMethod]
        public void wikiCategoryProviderSubcategoryTest()
        {
            WikiCategoryProvider provider = new WikiCategoryProvider();
            List< String > subcategories = new List<string> ( provider.getSubcategories("fizyka"));
            Assert.AreEqual(36, subcategories.Count);

            Assert.IsTrue(subcategories.Contains("Biofizyka"));
            Assert.IsTrue(subcategories.Contains("Fizycy"));

            subcategories = new List<string> ( provider.getSubcategories("adfwef"));
            Assert.AreEqual(0, subcategories.Count);

        }

        [TestMethod]
        public void wikiCategoryProviderCategoryTest()
        {
            WikiCategoryProvider provider = new WikiCategoryProvider();
            List<String> subcategories = new List<string> ( provider.getCategories("Ptaki"));
            Assert.AreEqual(3, subcategories.Count);
            Assert.IsTrue(subcategories.Contains("Artykuły na medal"));
            Assert.IsTrue(subcategories.Contains("Ptaki"));

            subcategories = new List<string> ( provider.getSubcategories("adfwef") );
            Assert.AreEqual(0, subcategories.Count);
        }

        [TestMethod]
        public void wikiCategoryProviderUberategoriesTest()
        {
            WikiCategoryProvider provider = new WikiCategoryProvider();
            List<String> subcategories = new List<string>(provider.getUbercategory("Tytuły monarsze"));
            Assert.AreEqual(3, subcategories.Count);
            Assert.IsTrue(subcategories.Contains("Władcy"));
            Assert.IsTrue(subcategories.Contains("Tytuły"));

            subcategories = new List<string>(provider.getSubcategories("adfwef"));
            Assert.AreEqual(0, subcategories.Count);
        }
    }
}
