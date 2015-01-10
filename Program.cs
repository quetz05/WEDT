﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEDT
{
    class Program
    {
        static void Main(string[] args)
        {
            String docPath = "../../simple.xml";
            String xpath = "//breakfast_menu/food/name";


            String [] data = XMLParser.ParseDocument(docPath, xpath);

            foreach (String line in data)
                Console.WriteLine(line);

            Console.ReadLine();

        }
    }
}