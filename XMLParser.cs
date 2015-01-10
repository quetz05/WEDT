using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace WEDT
{
    static class XMLParser
    {
        static public String[] ParseDocument(String docPath, String xpath)
        {
            List<String> data = new List<String>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(docPath);
            XmlNodeList nodes = xmlDoc.SelectNodes(xpath);

            foreach (XmlNode node in nodes)
                data.Add(node.InnerText);

            return data.ToArray();
        }
        

    }
}
