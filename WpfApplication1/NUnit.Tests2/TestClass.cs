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

        static object[] DivideCases =
        {
            //begin tests extract symbol
            new object[] { "int ! , a;",'!',true},
            new object[] { "int !b , a;", '!',true},
            new object[] { "int b! , a;", '!',true},
            new object[] { "int a, b^ ,c;", '^',true},
            //end tests extract symbol

            //begin tests without type
            new object[] { "dd , a;", 'd',true},
             new object[] {"int b; dd , a;", 'd',true},
        new object[] {"int j, b d;", 'd',true},
        new object[] {"int j; b d;", 'b',true},
        //end tests without type

        //begin tests without symbol ';'
        new object[] {"int j, d", ';',true},
        //end tests without symbol ';'

        //begin tests without symbol ','
        new object[] {"int j; int b d;", 'd',true},
        //end tests without symbol ','


        new object[] {"float?[,,,] b1!,a2,v3,b1;", '!',true},
        new object[] {"float !?[,,,] b1,a2,v3,b1;", '!',true},
        new object[] {"float?![,,,] b1,a2,v3,b1;", '!',true},
        new object[] {"float?[,!,,] b1,a2,v3,b1;", '!',true},

        new object[] {"string? b1,a2,v3,b1;", '?',true},
        new object[] {"object?[] b1,a2,v3,b1;", '?',true},


        new object[] {"int a,b,c,a,d;", 'a',true},
        new object[] {"string b1,a2,v3,a2;", 'a',true},
        new object[] {"float?[,,,] b1,a2,v3,b1;", 'b',true},
        new object[] {"int a,b; string c,a,d;", 'a',true},
        new object[] {"string b1,a2; string v3,a2;", 'a',true},
        new object[] {"float?[,,,] b1,a2,v3; string b1;", 'b',true},


        new object[] {"string b1,a2,v3;", ';' , false},
        new object[] {"object b1,a2,v3;", ';' , false},
        new object[] {"string[,,,] b1,a2,v3;", ';' , false},
        new object[] {"object[,,,] b1,a2,v3;", ';' , false},
        new object[] {"int[,,,] b1,a2,v3;", ';' , false},
        new object[] {"bool[,,,] b1,a2,v3;", ';' , false},
        new object[] {"int?[,,,] b1,a2,v3;", ';' , false},
        new object[] {"bool?[,,,] b1,a2,v3;", ';' , false},
        new object[] {"int? b1,a2,v3;", ';' , false},
        new object[] {"bool? b1,a2,v3;", ';' , false},

            };



        [Test, TestCaseSource("DivideCases")]
        public void Test_GetErrorSymbol(string query, char errorSymbol, bool hasError)
        {

            RegAnalisator ra = new RegAnalisator();
            List<string> listVars = new List<string>();
            List<string> listTypes = new List<string>();

            InfoAboutError inf = ra.getTrueQuery(query, listVars, listTypes);

            Assert.AreEqual(inf.errorChar, errorSymbol);
        }


        [Test, TestCaseSource("DivideCases")]
        public void Test_QueryWithoutError(string query, char errorSymbol, bool hasError)
        {

            RegAnalisator ra = new RegAnalisator();
            List<string> listVars = new List<string>();
            List<string> listTypes = new List<string>();

            InfoAboutError inf = ra.getTrueQuery(query, listVars, listTypes);

            Assert.AreEqual(inf.error, hasError);
        }






    }
}
