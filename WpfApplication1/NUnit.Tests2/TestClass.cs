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
        //begin tests extract symbol
        [TestCase("int ! , a;",'!')]
        [TestCase("int !b , a;", '!')]
        [TestCase("int b! , a;", '!')]
        [TestCase("int a, b^ ,c;", '^')]
        //end tests extract symbol

        //begin tests without type
        [TestCase("dd , a;", 'd')]
        [TestCase("int b; dd , a;", 'd')]
        [TestCase("int j, b d;", 'd')]
        [TestCase("int j; b d;", 'b')]
        //end tests without type

        //begin tests without symbol ';'
        [TestCase("int j, d", ';')]
        //end tests without symbol ';'

        //begin tests without symbol ','
        [TestCase("int j; int b d;", 'd')]
        //end tests without symbol ','

        
        [TestCase("float?[,,,] b1!,a2,v3,b1;", '!')]
        [TestCase("float !?[,,,] b1,a2,v3,b1;", '!')]
        [TestCase("float?![,,,] b1,a2,v3,b1;", '!')]
        [TestCase("float?[,!,,] b1,a2,v3,b1;", '!')]

        [TestCase("string? b1,a2,v3,b1;", '?')]
        [TestCase("object?[] b1,a2,v3,b1;", '?')]


        [TestCase("int a,b,c,a,d;", 'a')]
        [TestCase("string b1,a2,v3,a2;", 'a')]
        [TestCase("float?[,,,] b1,a2,v3,b1;", 'b')]
        [TestCase("int a,b; string c,a,d;", 'a')]
        [TestCase("string b1,a2; string v3,a2;", 'a')]
        [TestCase("float?[,,,] b1,a2,v3; string b1;", 'b')]


        public void Test_GetErrorSymbol(string query, char errorSymbol)
        {

            RegAnalisator ra = new RegAnalisator();
            List<string> listVars = new List<string>();
            List<string> listTypes = new List<string>();

            InfoAboutError inf = ra.getTrueQuery(query,listVars, listTypes);

            Assert.AreEqual(inf.errorChar, errorSymbol);
        }




        [TestCase("string b1,a2,v3;", false)]
        [TestCase("object b1,a2,v3;", false)]
        [TestCase("string[,,,] b1,a2,v3;", false)]
        [TestCase("object[,,,] b1,a2,v3;", false)]
        [TestCase("int[,,,] b1,a2,v3;", false)]
        [TestCase("bool[,,,] b1,a2,v3;", false)]
        [TestCase("int?[,,,] b1,a2,v3;", false)]
        [TestCase("bool?[,,,] b1,a2,v3;", false)]
        [TestCase("int? b1,a2,v3;", false)]
        [TestCase("bool? b1,a2,v3;", false)]
        public void Test_QueryWithoutError(string query, bool hasError)
        {

            RegAnalisator ra = new RegAnalisator();
            List<string> listVars = new List<string>();
            List<string> listTypes = new List<string>();

            InfoAboutError inf = ra.getTrueQuery(query, listVars, listTypes);

            Assert.AreEqual(inf.error, hasError);
        }






    }
}
