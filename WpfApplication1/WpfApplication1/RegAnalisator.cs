using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WpfApplication1
{

  public  class RegAnalisator
    {

        string[] masNameTypeWithNull = new string[] {"sbyte","short","int","long","byte","ushort","uint",
               "ulong","float","double","decimal","bool","char"};

        string[] masNameTypeNotNull = new string[] { "string", "object" };

        string[] masNameAllType = new string[] {"sbyte","short","int","long","byte","ushort","uint",
               "ulong","float","double","decimal","bool","char", "string", "object" };


        private bool checkKey(char chekChar)
        {
            char[] listReplaceString = new char[] { '?', '[', ']', ',', ';' };
            foreach (char c in listReplaceString)
            {
                if (c == chekChar)
                {
                    return true;
                }
            }
            return false;
        }

        public InfoAboutError checkString(string query, List<string> listVars)
        {
            string nameTypeWithNull = String.Join("|", masNameTypeWithNull);
            string nameTypeNotNull = String.Join("|", masNameTypeNotNull);
            string nameAllType = String.Join("|", masNameAllType);


            string pattern = @"^
            (
                (
                    (          (?<=^)                          (" + nameAllType + @")                    (?=\?|\[|\s)       )    |
                    (          (?<=" + nameTypeWithNull + @")  (\?)                                      (?=\[|\w+)         )    |
                    (          (?<=" + nameAllType + @")       (\s)                                      (?=\w+)            )    |
                    (          (?<=\?|" + nameAllType + @")    (?<Sk>\[)                                 (?=,|\])           )    |
                    (          (?<=\w+|\[)                     (,)                                       (?=,|\]|\w+)       )    |
                    (          (?<=,)                          (?(Sk),)                                  (?=,|\])           )    |
                    (          (?<=,|\[)                       (?<-Sk>\])                                (?=\w+)            )    |
                    (          (?<=\?|\s|\]|,)  (?(Sk)(?!))    (?<NameVar>\w+)                           (?=,|;)            )    |
                    (          (?<=\w+)         (?(Sk)(?!))    (;)                                       (?=;|$)            )    |
                )+
            )";



            Regex r = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);

            Match m = r.Match(query);

            InfoAboutError inf = null;
            if (m.Groups.Count > 1)
            {
                Group g = m.Groups[1];

                if (g.Value.Length < query.Length)
                {
                    if (g.Value.Length == 0) {
                        if (query == ";")
                            inf = new InfoAboutError(false, query);
                        else
                            inf = new InfoAboutError(true, query);
                    }
                    else
                    {
                        if (query.ToCharArray()[g.Value.Length] == ' ')
                            inf = new InfoAboutError(true,
                                new String(query.ToCharArray(), 0, g.Value.Length));
                        else
                            inf = new InfoAboutError(true,
                                new String(query.ToCharArray(), 0, g.Value.Length));
                    }
                        
                }
                else
                {
                    inf = new InfoAboutError(false, query);
                }
                string nameGroup = "NameVar";
                g = m.Groups[nameGroup];
                for (int j = 0; j < g.Captures.Count; j++)
                {
                    Capture c = g.Captures[j];
                    listVars.Add(c.Value);
                }

                
                return inf;
            }
            return null;

        }




        

        public void formatString(ref string str)
        {
            string[] listReplaceString = new string[] { "\n", "  " };
            foreach (string s in listReplaceString)
                while (str.IndexOf(s) != -1)
                    str = str.Replace(s, " ");
            while (str.IndexOf(";;") != -1)
                str = str.Replace(";;", ";");

            if (str.Length > 1)
                if (str.ToCharArray()[0] == ' ')
                    str = str.Remove(0, 1);

            if (str.Length > 1)
                if (str.ToCharArray()[str.Length - 1] == ' ')
                {
                    str = str.Remove(str.Length - 1, 1);
                }


            if (str.Length > 0)
            {
                string[] masString = str.Split(' ');
                string newString = "";
                newString += masString[0];
                for (int i = 0; i < masString.Length - 1; i++)
                {

                    if (!checkKey(masString[i].ToCharArray()[masString[i].Length - 1]) &&
                        !checkKey(masString[i + 1].ToCharArray()[0]))
                        newString += " ";
                    newString += masString[i + 1];
                }
                str = newString;
            }
            str += "";

        }




    }

}
