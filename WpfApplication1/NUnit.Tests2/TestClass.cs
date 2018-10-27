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
            #region // extract symbol
            new object[] { "int ! , a;",'!',true, 1, 5},
            new object[] { "int !b , a;", '!',true, 1, 5},
            new object[] { "int b! , a;", '!',true, 1, 6},
            new object[] { "int a, b^ ,c;", '^',true, 1, 9},
            #endregion  // extract symbol

            #region //without type
            new object[] { "dd , a;", 'd',true, 1, 1},
            new object[] {"int b; dd , a;", 'd',true, 1, 8},
            new object[] {"int j, b d;", 'd',true, 1, 10},
            new object[] {"int j; b d;", 'b',true, 1, 8},
            #endregion //without type

            #region // without symbol ';'
            new object[] {"int j, d", ';',true, 1, 9},
            #endregion //without symbol ';'

            #region //without symbol ','
            new object[] {"int j; int b d;", 'd',true, 1, 14},
            #endregion //without symbol ','

            #region //symbol '?' in object and string
            new object[] {"string? b1,a2,v3,b1;", '?',true, 1, 7},
            new object[] {"object?[] b1,a2,v3,b1;", '?',true, 1, 7},
            #endregion //symbol '?' in object and string

            #region //double variable
            new object[] {"int a,b,c,a,d;", 'a',true, 1, 11},
            new object[] {"string b1,a2,v3,a2;", 'a',true, 1, 17},
            new object[] {"float?[,,,] b1,a2,v3,b1;", 'b',true, 1, 22},
            new object[] {"int a,b; string c,a,d;", 'a',true, 1, 19},
            new object[] {"string b1,a2; string v3,a2;", 'a',true, 1, 25},
            new object[] {"float?[,,,] b1,a2,v3; string b1;", 'b',true, 1, 30},
            #endregion //double variable

            #region //other query with error
            new object[] { "     string [, ,,] bbb, a2  , uu ;;;"+'\n'+
             " float a, ff ;; string? s;", '?' , true,2,23},
            new object[] { "     string [, ,,] bbb, a2  , uu ;;;"+'\n'+
             " float a, ff ;; "+'\n'+"string? s;", '?' , true,3,7},
            new object[] { "     string [, ,,] bbb, a2  , uu ;;;"+'\n'+
             " float a, "+'\n'+"ff ;; string"+'\n'+"? s;", '?' , true,4,1},
            new object[] { "     string! [, ,,] bbb, a2  , uu ;;;"+'\n'+
             " float a, ff ;; string? s;", 's' , true,1,6},
            new object[] { "  hhfh   string [, ,,] bbb, a2  , uu ;;;"+'\n'+
             " float a, ff ;; "+'\n'+"string? s;", 'h' , true,1,3},
            new object[] { "     string [, ,, ] bbb, &a2  , uu ;;;"+'\n'+
                 " float a, "+'\n'+"ff ;; string"+'\n'+"? s;", '&' , true,1,26},
            new object[] { " ; int ; ", ';' , true,1,8},
            new object[] { " ;"+'\n'+" int ; ", ';' , true,2,6},
            new object[] { '\n'+"int; "+'\n', ';' , true,2,4},
            new object[] { '\n' + "int a , a ;", 'a' , true,2,9},
            new object[] { " float[,] c , b;"+'\n'+" " + '\n' +
                " float a, a; ", 'a' , true,3,11},
            new object[] { "float a, a; ", 'a' , true,1,10},
            new object[] { '\n' + "  object  a , " +'\n'+
                " b1 " + '\n', ';' , true,3,4},
            new object[] { '\t'+ "int "+'\n' +
                '\t'+'\t'+"a,d,c"+'\n'+'\n'+
                "      a;", 'a' , true, 4, 7},
            new object[] {"float?[,,,] b1!,a2,v3,b1;", '!',true, 1, 15},
            new object[] {"float !?[,,,] b1,a2,v3,b1;", '!',true, 1, 7},
            new object[] {"float?![,,,] b1,a2,v3,b1;", '!',true, 1, 7},
            new object[] {"float?[,!,,] b1,a2,v3,b1;", '!',true, 1, 9},

            #endregion //other query with error

            #region //query without error
            new object[] {"string b1,a2,v3;", ';' , false, 0, 0},
            new object[] {"object b1,a2,v3;", ';' , false, 0, 0},
            new object[] {"string[,,,] b1,a2,v3;", ';' , false, 0, 0},
            new object[] {"object[,,,] b1,a2,v3;", ';' , false, 0, 0},
            new object[] {"int[,,,] b1,a2,v3;", ';' , false, 0, 0},
            new object[] {"bool[,,,] b1,a2,v3;", ';' , false, 0, 0},
            new object[] {"int?[,,,] b1,a2,v3;", ';' , false, 0, 0},
            new object[] {"bool?[,,,] b1,a2,v3;", ';' , false, 0, 0},
            new object[] {"int? b1,a2,v3;", ';' , false, 0, 0},
            new object[] {"bool? b1,a2,v3;", ';' , false, 0, 0},
            new object[] { '\n' + "int a23 , v_7 ;", ';' , false, 0, 0},
            new object[] { " float[,] c_4 , b;"+'\n'+" " + '\n' +
                " float adgdg, g1; ", ';' , false, 0, 0},
            new object[] { "float " + '\n' + "a54gd, " + '\n' +
                "hj2; "+""+'\n', ';' , false, 0, 0},
            new object[] { '\n'+ "  object  a , " +
                " b1 ;" + '\n', ';' , false, 0, 0},
            #endregion //query without error
        };



        [Test, TestCaseSource("DivideCases")]
        public void Test_hasError(string query,
           char errorSymbol, bool hasError, int positionLine,
           int positionError)
        {

            RegAnalisator ra = new RegAnalisator();
            List<string> listVars = new List<string>();
            List<string> listTypes = new List<string>();

            InfoAboutError inf = ra.getTrueQuery(query,
                listVars, listTypes);

            Assert.AreEqual(inf.error, hasError);
        }

        [Test, TestCaseSource("DivideCases")]
        public void Test_symbolError(string query,
            char errorSymbol, bool hasError, int positionLine,
            int positionError)
        {

            RegAnalisator ra = new RegAnalisator();
            List<string> listVars = new List<string>();
            List<string> listTypes = new List<string>();

            InfoAboutError inf = ra.getTrueQuery(query,
                listVars, listTypes);

            Assert.AreEqual(inf.errorChar, errorSymbol);
        }


        [Test, TestCaseSource("DivideCases")]
        public void Test_linePositionError(string query,
            char errorSymbol, bool hasError, int positionLineError,
            int positionError)
        {

            RegAnalisator ra = new RegAnalisator();
            List<string> listVars = new List<string>();
            List<string> listTypes = new List<string>();

            InfoAboutError inf = ra.getTrueQuery(query,
                listVars, listTypes);

            Assert.AreEqual(inf.positionLineError, positionLineError);
        }

        [Test, TestCaseSource("DivideCases")]
        public void Test_positionError(string query,
            char errorSymbol, bool hasError,
            int positionLineError, int positionError)
        {

            RegAnalisator ra = new RegAnalisator();
            List<string> listVars = new List<string>();
            List<string> listTypes = new List<string>();

            InfoAboutError inf = ra.getTrueQuery(query,
                listVars, listTypes);

            Assert.AreEqual(inf.positionError, positionError);
        }





    }
}
