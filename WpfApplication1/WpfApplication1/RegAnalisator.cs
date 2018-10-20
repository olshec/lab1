﻿using System;
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


        private bool isType(string str)
        {
            //char[] listReplaceString = new char[] { '?', '[', ']', ',', ';' };
            foreach (string s in masNameAllType)
            {
                if (s == str)
                {
                    return true;
                }
            }
            return false;
        }

        private bool isUseKey(char chekChar)
        {
            char[] listKey = new char[] { '!', '`', '~', '\'', '.', '\\', '/' };
            foreach (char c in listKey)
            {
                if (c == chekChar)
                {
                    return true;
                }
            }
            return false;
        }

        private bool isKey(char chekChar)
        {
            char[] listKey = new char[] { '?', '[', ']', ',', ';' };
            foreach (char c in listKey)
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

            string symbolInVar = "[abc]+";

            string pattern = @"^
            (
                (
                    (          (?<=^)                          (?<NameType>" + nameAllType + @")         (?=\?|\[|\s)       )    |
                    (          (?<=" + nameTypeWithNull + @")  (\?)                                      (?=\[|"+symbolInVar+@")         )    |
                    (          (?<=" + nameAllType + @")       (\s)                                      (?="+symbolInVar+@")            )    |
                    (          (?<=\?|" + nameAllType + @")    (?<Sk>\[)                                 (?=,|\])           )    |
                    (          (?<="+symbolInVar+@"|\[)                     (,)                                       (?=,|\]|"+symbolInVar+@")       )    |
                    (          (?<=,)                          (?(Sk),)                                  (?=,|\])           )    |
                    (          (?<=,|\[)                       (?<-Sk>\])                                (?="+symbolInVar+@")            )    |
                    (          (?<=\?|\s|\]|,)  (?(Sk)(?!))    (?<NameVar>"+symbolInVar+@")                           (?=,|;)            )    |
                    (          (?<="+symbolInVar+@")         (?(Sk)(?!))    (;)                                       (?=;|$)            )    |
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
                        {
                            //int]
                            if (isKey(query.ToCharArray()[query.Length - 1]))
                                inf = new InfoAboutError(true, "", query.Length,
                                    query.ToCharArray()[query.Length - 1]);
                            
                            else
                                //int
                                if (isType(query))
                                    inf = new InfoAboutError(true, "", query.Length + 1);
                                //integreg
                                else
                                    inf = new InfoAboutError(true, "", 0, query.ToCharArray()[0]);

                        }
                            
                    }
                    else
                    {
                        bool hasVar = false;
                        string nameGroup = "NameVar";
                        Group  gr = m.Groups[nameGroup];
                        for (int j = 0; j < gr.Captures.Count; j++)
                        {
                            hasVar = true;
                            Capture c = gr.Captures[j];
                            listVars.Add(c.Value);
                        }
                        
                        int indexLenth = g.Value.Length-1;
                        if (query.ToCharArray()[indexLenth] == ' ')
                        {
                            indexLenth++;
                            string nameVar = "";
                            while (indexLenth<query.ToCharArray().Length)
                            {
                                if (query.ToCharArray()[indexLenth] != ' '
                                    && !isKey(query.ToCharArray()[indexLenth])
                                    && !isUseKey(query.ToCharArray()[indexLenth]))
                                {
                                    nameVar += query.ToCharArray()[indexLenth];
                                    indexLenth++;
                                }
                                else break;
                            }
                            listVars.Add(nameVar);

                            while (indexLenth < query.ToCharArray().Length&&
                                query.ToCharArray()[indexLenth] == ' ')
                                indexLenth++;

                            if(indexLenth < query.ToCharArray().Length)
                                inf = new InfoAboutError(true,
                               new String(query.ToCharArray(), 0, indexLenth), indexLenth + 1,
                               query.ToCharArray()[indexLenth]);
                            else
                            inf = new InfoAboutError(true,
                               new String(query.ToCharArray(), 0, indexLenth), indexLenth+1);
                        }
                        
                        else
                        {
                            //if(!isKey(query.ToCharArray()[indexLenth]))
                                indexLenth++;
                            while (indexLenth < query.ToCharArray().Length &&
                                query.ToCharArray()[indexLenth] == ' ')
                                indexLenth++;

                            //int g, f , ]   , f 
                            if (indexLenth < query.ToCharArray().Length &&
                                (isKey(query.ToCharArray()[indexLenth]) &&
                                !hasVar)
                                //&&query.ToCharArray()[indexLenth]!='?'
                                //&&query.ToCharArray()[indexLenth]!=']'
                               )
                            {
                                indexLenth++;
                                while (indexLenth < query.ToCharArray().Length &&
                                query.ToCharArray()[indexLenth] == ' ')
                                    indexLenth++;
                                if (indexLenth < query.ToCharArray().Length)
                                    inf = new InfoAboutError(true,
                                   new String(query.ToCharArray(), 0, indexLenth), indexLenth + 1,
                                   query.ToCharArray()[indexLenth]);
                                else
                                    inf = new InfoAboutError(true,
                                    new String(query.ToCharArray(), 0, indexLenth), indexLenth + 1);

                            }
                            //int?[] abv rrr
                            else
                            {
                                string nameVar = "";
                                while (indexLenth < query.ToCharArray().Length)
                                {
                                    if (query.ToCharArray()[indexLenth] != ' '
                                        && !isKey(query.ToCharArray()[indexLenth])
                                        && !isUseKey(query.ToCharArray()[indexLenth]))
                                    {
                                        nameVar += query.ToCharArray()[indexLenth];
                                        indexLenth++;
                                    }
                                    else break;
                                }
                                //indexLenth++;
                                while (indexLenth < query.ToCharArray().Length &&
                                query.ToCharArray()[indexLenth] == ' ')
                                    indexLenth++;
                                listVars.Add(nameVar);
                                if (indexLenth < query.ToCharArray().Length)
                                    inf = new InfoAboutError(true,
                                   new String(query.ToCharArray(), 0, indexLenth), indexLenth + 1,
                                   query.ToCharArray()[indexLenth]);
                                else
                                    inf = new InfoAboutError(true,
                                   new String(query.ToCharArray(), 0, indexLenth), indexLenth + 1);

                            }

                        }
                    }
                        
                }
                else
                {
                    inf = new InfoAboutError(false, query);
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

                    if (
                        (!isKey(masString[i].ToCharArray()[masString[i].Length - 1]) &&
                        !isKey(masString[i + 1].ToCharArray()[0]))
                        ||
                        (isUseKey(masString[i].ToCharArray()[masString[i].Length - 1])||
                        isUseKey(masString[i + 1].ToCharArray()[0]))
                        )
                        newString += " ";
                    newString += masString[i + 1];
                }
                str = newString;
            }
            str += "";

        }
        

    }

}
