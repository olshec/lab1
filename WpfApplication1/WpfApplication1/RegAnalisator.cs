using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WpfApplication1
{

    public class RegAnalisator
    {

        string[] masNameTypeWithNull = new string[] {"sbyte","short","int","long","byte","ushort","uint",
               "ulong","float","double","decimal","bool","char"};

        string[] masNameTypeNotNull = new string[] { "string", "object" };

        string[] masNameAllType = new string[] {"sbyte","short","int","long","byte","ushort","uint",
               "ulong","float","double","decimal","bool","char", "string", "object" };


        private void findDoubleVariable(ref InfoAboutError inf, List<string> listVars)
        {
            string query = inf.trueQuery;
            for (int i = 0; i < listVars.Count; i++)
                for (int j = 0; j < listVars.Count; j++)
                {
                    if (listVars[i] == listVars[j] && i != j)
                    {
                        int positionFirstVariable = query.IndexOf(listVars[i]);
                        int positionDoubleVariable = query.IndexOf(listVars[i],
                            positionFirstVariable + 1);
                        if (positionDoubleVariable == -1)
                            positionDoubleVariable = positionFirstVariable;
                        inf.error = true;
                        inf.errorChar = listVars[i].ToCharArray()[0];
                        inf.indexLineError = -1;//=======================
                        inf.positionError = positionDoubleVariable + 1;
                        inf.trueQuery = query.Substring(0, positionDoubleVariable);
                    }
                }
            //return inf;
        }

        private bool hasVarInList(string nameVar, List<string> listVars)
        {
            foreach (string s in listVars)
                if (s == nameVar)
                    return true;
            return false;
        }


        private bool isType(string str)
        {
            foreach (string s in masNameAllType)
            {
                if (s == str)
                {
                    return true;
                }
            }
            return false;
        }

        private bool isUserKey(char chekChar)
        {
            if (Char.IsDigit(chekChar) || Char.IsLetter(chekChar)
                || chekChar == '_' || isKey(chekChar))
                return false;
            return true;
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

        private InfoAboutError checkString(string query, List<string> listVars, List<string> listType)
        {
            string nameTypeWithNull = String.Join("|", masNameTypeWithNull);
            string nameTypeNotNull = String.Join("|", masNameTypeNotNull);
            string nameAllType = String.Join("|", masNameAllType);

            string symbolInVar = "[a-zA-Z_]+[0-9]*";

            string pattern = @"^
            (
                (
                    (          (?<=^)                          (?<NameType>" + nameAllType + @")         (?=\?|\[|\s)       )    |
                    (          (?<=" + nameTypeWithNull + @")  (\?)                                      (?=\[|" + symbolInVar + @")         )    |
                    (          (?<=" + nameAllType + @")       (\s)                                      (?=" + symbolInVar + @")            )    |
                    (          (?<=\?|" + nameAllType + @")    (?<Sk>\[)                                 (?=,|\])           )    |
                    (          (?<=" + symbolInVar + @"|\[)                     (,)                                       (?=,|\]|" + symbolInVar + @")       )    |
                    (          (?<=,)                          (?(Sk),)                                  (?=,|\])           )    |
                    (          (?<=,|\[)                       (?<-Sk>\])                                (?=" + symbolInVar + @")            )    |
                    (          (?<=\?|\s|\]|,)  (?(Sk)(?!))    (?<NameVar>" + symbolInVar + @")                           (?=,|;)            )    |
                    (          (?<=" + symbolInVar + @")         (?(Sk)(?!))    (;)                                       (?=;|$)            )    |
                )+
            )";


            Regex r = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);

            Match m = r.Match(query);

            InfoAboutError inf = new InfoAboutError();
            if (m.Groups.Count > 1)
            {
                Group g = m.Groups[1];

                if (g.Value.Length < query.Length)
                {
                    if (g.Value.Length == 0)
                    {
                        if (query == ";")
                            inf = new InfoAboutError(false, query);
                        else
                        {

                            if (isKey(query.ToCharArray()[query.Length - 1]))
                            {
                                bool hasNameType = false;
                                string nameGroup = "NameType";
                                Group gr = m.Groups[nameGroup];
                                for (int j = 0; j < gr.Captures.Count; j++)
                                {
                                    hasNameType = true;
                                    Capture c = gr.Captures[j];
                                    listType.Add(c.Value);
                                }
                                //int]
                                if (hasNameType)
                                    inf = new InfoAboutError(true, "", query.Length,
                                query.ToCharArray()[query.Length - 1]);
                                //dfg,dfg,dg
                                else
                                {
                                    inf = new InfoAboutError(true, "", 0,
                                    query.ToCharArray()[0]);
                                }
                            }


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
                        Group gr = m.Groups[nameGroup];
                        for (int j = 0; j < gr.Captures.Count; j++)
                        {
                            hasVar = true;
                            Capture c = gr.Captures[j];
                            listVars.Add(c.Value);
                        }

                        // bool hasNameType = false;
                        string nameGroup2 = "NameType";
                        Group gr2 = m.Groups[nameGroup2];
                        for (int j = 0; j < gr2.Captures.Count; j++)
                        {
                            //   hasNameType = true;
                            Capture c = gr2.Captures[j];
                            listType.Add(c.Value);
                        }

                        int indexLenth = g.Value.Length - 1;
                        if (query.ToCharArray()[indexLenth] == ' ')
                        {
                            indexLenth++;
                            string nameVar = "";
                            while (indexLenth < query.ToCharArray().Length)
                            {
                                if (query.ToCharArray()[indexLenth] != ' '
                                    && !isKey(query.ToCharArray()[indexLenth])
                                    && !isUserKey(query.ToCharArray()[indexLenth]))
                                {
                                    nameVar += query.ToCharArray()[indexLenth];
                                    indexLenth++;
                                }
                                else break;
                            }
                            listVars.Add(nameVar);

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
                               )
                            {
                                //string? object?
                                if (query.ToCharArray()[indexLenth] == '?'
                                    && indexLenth - 6 >= 0)
                                {
                                    string nameType = query.Substring(indexLenth - 6, 6);
                                    if (isType(nameType))
                                        inf = new InfoAboutError(true,
                                   new String(query.ToCharArray(), 0, indexLenth), indexLenth + 1,
                                   query.ToCharArray()[indexLenth]);
                                }
                                else
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
                            }
                            //int?[] abv rrr
                            else
                            {
                                string nameVar = "";
                                while (indexLenth < query.ToCharArray().Length)
                                {
                                    if (query.ToCharArray()[indexLenth] != ' '
                                        && !isKey(query.ToCharArray()[indexLenth])
                                        && !isUserKey(query.ToCharArray()[indexLenth])
                                        )
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
                                if (indexLenth < query.ToCharArray().Length
                                    && query.ToCharArray()[indexLenth] == ',')
                                {
                                    indexLenth++;
                                    while (indexLenth < query.ToCharArray().Length &&
                                    query.ToCharArray()[indexLenth] == ' ')
                                        indexLenth++;
                                    inf = new InfoAboutError(true,
                                  new String(query.ToCharArray(), 0, indexLenth), indexLenth + 1,
                                  query.ToCharArray()[indexLenth]);

                                }
                                else
                                    //int j, b d;
                                    if (indexLenth < query.ToCharArray().Length)
                                    inf = new InfoAboutError(true,
                                        new String(query.ToCharArray(), 0, indexLenth), indexLenth + 1,
                                        query.ToCharArray()[indexLenth]);
                                //int j, d d
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
                    string nameGroup = "NameVar";
                    Group gr = m.Groups[nameGroup];
                    for (int j = 0; j < gr.Captures.Count; j++)
                    {
                        Capture c = gr.Captures[j];
                        listVars.Add(c.Value);
                    }

                    string nameGroup2 = "NameType";
                    Group gr2 = m.Groups[nameGroup2];
                    for (int j = 0; j < gr2.Captures.Count; j++)
                    {
                        //   hasNameType = true;
                        Capture c = gr2.Captures[j];
                        listType.Add(c.Value);
                    }

                }



                return inf;
            }
            return null;

        }






        private void formatString(ref string str)
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
                        (isUserKey(masString[i].ToCharArray()[masString[i].Length - 1]) ||
                        isUserKey(masString[i + 1].ToCharArray()[0]))
                        )
                        newString += " ";
                    newString += masString[i + 1];
                }
                str = newString;
            }
            str += "";

        }


        private void findRealPositionError(ref InfoAboutError inf,
            string sourceQuery)
        {
            int positionLineError = inf.positionLineError;
            int positionError = inf.positionError;

            string[] masStr = sourceQuery.Split(';');
            int countString = masStr.Length;
            //if (masStr[masStr.Length - 1] == "")
            //    countString--;
            for (int i = masStr.Length - 1; i >= 0; i--)
                if (masStr[masStr.Length - 1] == "")
                    countString--;
            if (inf.errorChar == ';')
            {
                char[] masCharQuery = sourceQuery.ToCharArray();
                foreach (char c in masCharQuery)
                {
                    if (c == '\n')
                    {
                        positionError = 0;
                        positionLineError++;
                    }
                    else
                        positionError++;
                }
                positionError++;
                // inf.positionLineError = positionLineError;
                // inf.positionError = positionError;
            }

            else
            {

                for (int i = 0; i < countString; i++)
                {
                    string q = masStr[i];
                    if (i == countString - 1)
                    {
                        if (sourceQuery.ToCharArray()[sourceQuery.Length - 1] == ';')
                            q += ";";
                    }
                    else q += ";";


                    char[] masCharQuery = q.ToCharArray();
                    if (i < inf.indexLineError)
                    {
                        // masCharQuery = masStr[i].ToCharArray();
                        foreach (char c in masCharQuery)
                        {
                            if (c == '\n')
                            {
                                positionError = 0;
                                positionLineError++;
                            }
                            else
                                positionError++;
                        }
                    }
                    else
                    {
                        if (q == ";")
                            positionError++;
                        else
                        {
                            char[] masTrueCharQuery = inf.trueQuery.ToCharArray();
                            int positionInSplit = 0;
                            for (int j = 0; j < masTrueCharQuery.Length; j++)
                            {
                                while (masCharQuery[positionInSplit] != masTrueCharQuery[j])
                                {
                                    char c = masCharQuery[positionInSplit];
                                    if (c == '\n')
                                    {
                                        positionError = 0;
                                        positionLineError++;
                                    }
                                    else
                                        positionError++;
                                    positionInSplit++;
                                }
                            }
                            //positionError++;
                            while (masCharQuery[positionInSplit] != inf.errorChar)
                            { positionInSplit++; positionError++; }
                            //positionError += positionInSplit;
                            positionError++;
                            break;
                        }
                    }

                }

            }

            inf.positionError = positionError;
            inf.positionLineError = positionLineError;
        }

        public InfoAboutError getTrueQuery(string query, List<string> listVars, List<string> listTypes)
        {
            RegAnalisator ra = new RegAnalisator();
            InfoAboutError inf = new InfoAboutError();
            //float?[,,,] a, b; ; ; ;
            string queryForFindPosition = query;
            ra.formatString(ref query);

            string[] masStr = query.Split(';');
            int countString = masStr.Length;
            if (masStr[masStr.Length - 1] == "")
                countString--;
            for (int i = 0; i < countString; i++)
            {
                string q = masStr[i];
                if (i == countString - 1)
                {
                    if (query.ToCharArray()[query.Length - 1] == ';')
                        q += ";";
                }
                else q += ";";
                inf = ra.checkString(q, listVars, listTypes);

                if (inf.error == true)
                {
                    inf.indexLineError = i;
                    inf.trueQuery += inf.str;
                    findRealPositionError(ref inf, queryForFindPosition);
                    break;
                }
                inf.trueQuery += q;

                findDoubleVariable(ref inf, listVars);
                if (inf.error == true)
                {
                    inf.indexLineError = i;
                    findRealPositionError(ref inf, queryForFindPosition);
                    break;
                }

            }

            return inf;
        }



    }

}
