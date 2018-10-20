using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WpfApplication1;

namespace NUnit.Tests2
{
    [TestFixture]
    public class TestClass
    {
        [TestCase("int ! , a;",'!',5)]
        [TestCase("int !b , a;", '!',5)]
        [TestCase("int b! , a;", '!',6)]

        [TestCase("dd , a;", 'd', 6)]
        [TestCase("int b; dd , a;", 'd', 6)]
        [TestCase("int a, b^ ,c;", '^', 6)]

        [TestCase("int j, d", ';', 6)]
        [TestCase("int j, b d;", 'd', 6)]

        [TestCase("int j; b d;", 'b', 6)]
        [TestCase("int j; int b d;", 'd', 6)]
        public void TestMethod(string query, char errorSymbol, int position)
        {

            RegAnalisator ra = new RegAnalisator();
            List<string> listVars = new List<string>();
            List<string> listTypes = new List<string>();

            InfoAboutError inf = ra.getTrueQuery(query,listVars, listTypes);

            Assert.AreEqual(inf.errorChar, errorSymbol);
        }

    }
}
