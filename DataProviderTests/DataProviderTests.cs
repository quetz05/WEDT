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
            List< String> links = provider.pagelinks("Alergologia");
            Assert.AreEqual(6, links.Count);
            Assert.IsTrue(links.Contains("Alergen"));
            Assert.IsTrue(links.Contains("Alergia"));
            Assert.IsTrue(links.Contains("Kategoria:Alergologia"));
            Assert.IsTrue(links.Contains("Kategoria:Specjalności_lekarskie"), "Polskie znaki");
            Assert.IsTrue(links.Contains("Medycyna"));
            Assert.IsTrue(links.Contains("Leczenie"));

            links = provider.pagelinks("alergologia");
            Assert.AreEqual(6, links.Count);
            Assert.IsTrue(links.Contains("Alergen"));
            Assert.IsTrue(links.Contains("Alergia"));
            Assert.IsTrue(links.Contains("Kategoria:Alergologia"));
            Assert.IsTrue(links.Contains("Kategoria:Specjalności_lekarskie"), "Polskie znaki");
            Assert.IsTrue(links.Contains("Medycyna"));
            Assert.IsTrue(links.Contains("Leczenie"));

            links = provider.pagelinks("Alegogosdf");
            Assert.AreEqual(0, links.Count, "Pusta lista");

        }

        [TestMethod]
        public void wikiPagelinksProviderDisambiguatesTest()
        {
            WikiPagelinksProvider provider = new WikiPagelinksProvider();
            List<String> links = provider.disambiguates("Al");
            Assert.AreEqual(14, links.Count);
            Assert.IsTrue(links.Contains("Alabama"));
            Assert.IsTrue(links.Contains("Albania"));

        }

        [TestMethod]
        public void wikiCategoryProviderSubcategoryTest()
        {
            WikiCategoryProvider provider = new WikiCategoryProvider();
            List< String > subcategories = provider.getSubcategories("fizyka");
            Assert.AreEqual(36, subcategories.Count);

            Assert.IsTrue(subcategories.Contains("Biofizyka"));
            Assert.IsTrue(subcategories.Contains("Fizycy"));

            subcategories = provider.getSubcategories("adfwef");
            Assert.AreEqual(0, subcategories.Count);

        }

        [TestMethod]
        public void wikiCategoryProviderCategoryTest()
        {
            WikiCategoryProvider provider = new WikiCategoryProvider();
            List<String> subcategories = provider.getCategories("Albert Einstein");
            Assert.AreEqual(13, subcategories.Count);
            Assert.IsTrue(subcategories.Contains("Wykładowcy Uniwersytetu Humboldtów w Berlinie"));
            Assert.IsTrue(subcategories.Contains("Niemieccy socjaliści"));

            subcategories = provider.getSubcategories("adfwef");
            Assert.AreEqual(0, subcategories.Count);
        }
    }
}
