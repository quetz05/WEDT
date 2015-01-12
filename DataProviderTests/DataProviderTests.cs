using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WEDT.DataProvider;

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
    }
}
