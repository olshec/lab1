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

        
        private void findDuplicateVariable(ref InfoAboutError inf, List<string> listVars)
        {
            string query = inf.trueQuery;
            bool exit = false;
            for (int i = 0; i < listVars.Count; i++)
            {
                for (int j = 0; j < listVars.Count; j++)
                {
                    if (listVars[i] == listVars[j] && i != j)
                    {
                        int positionFirstVariable = query.IndexOf(listVars[i]);
                        while (!
                            (positionFirstVariable == -1) && !((
                            query.ToCharArray()[positionFirstVariable - 1] == ' ' ||
                            query.ToCharArray()[positionFirstVariable - 1] == ',') &&
                            (positionFirstVariable > query.ToCharArray().Length - 1 ||
                            positionFirstVariable + listVars[i].Length >= query.ToCharArray().Length ||
                            query.ToCharArray()[positionFirstVariable + listVars[i].Length] == ' ' ||
                            query.ToCharArray()[positionFirstVariable + listVars[i].Length] == ','))
                            )
                        {
                            positionFirstVariable++;
                            positionFirstVariable = query.IndexOf(listVars[i],
                                positionFirstVariable + 1);
                        }

                        int positionDoubleVariable = query.IndexOf(listVars[i],
                            positionFirstVariable + 1);
                        while (!
                            ((positionDoubleVariable==-1||
                            query.ToCharArray()[positionDoubleVariable - 1] == ' ' ||
                            query.ToCharArray()[positionDoubleVariable - 1] == ',') &&
                            (
                            positionDoubleVariable > query.ToCharArray().Length - 1 ||
                            positionDoubleVariable + listVars[i].Length >= query.ToCharArray().Length ||
                            query.ToCharArray()[positionDoubleVariable +  listVars[i].Length] == ' ' ||
                            query.ToCharArray()[positionDoubleVariable +  listVars[i].Length] == ',')||
                            query.ToCharArray()[positionDoubleVariable + listVars[i].Length] == ';')
                            )
                        {
                            positionDoubleVariable++;
                            positionDoubleVariable = query.IndexOf(listVars[i],
                                positionDoubleVariable + 1);
                        }


                        if (positionDoubleVariable == -1)
                            positionDoubleVariable = positionFirstVariable;
                        inf.error = true;
                        inf.errorChar = listVars[i].ToCharArray()[0];
                        inf.indexLineError = 0;//=======================
                        inf.positionError = positionDoubleVariable + 1;
                        inf.trueQuery = query.Substring(0, positionDoubleVariable);
                        inf.typeMessage = "Дубликат переменной";
                        inf.message = listVars[i];
                        exit = true;
                        break;
                    }
                }
                if (exit)
                    break;
            }
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

        private bool isIdentifierChar(char chekChar)
        {
            if (Char.IsDigit(chekChar) || Char.IsLetter(chekChar)
                || chekChar == '_' || isKeyChar(chekChar))
                return false;
            return true;
        }

        private bool isKeyChar(char chekChar)
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

        //check error in string
        private InfoAboutError checkString(string query, List<string> listVars, List<string> listType)
        {
            string nameTypeWithNull = String.Join("|", masNameTypeWithNull);
            string nameTypeNotNull = String.Join("|", masNameTypeNotNull);
            string nameAllType = String.Join("|", masNameAllType);

            string symbolInVar = "[a-zA-Z_]+[0-9]*[a-zA-Z_]*";

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

                            if (isKeyChar(query.ToCharArray()[query.Length - 1]))
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
                                //error like: int]
                                if (hasNameType)
                                    inf = new InfoAboutError(true, "", query.Length,
                                query.ToCharArray()[query.Length - 1]);
                                //error like: dfg,dfg,dg
                                else
                                {
                                    string name = query.Substring(0, query.Length - 1);
                                    if (isType(name))
                                        inf = new InfoAboutError(true, name, query.Length,
                                    ';');
                                    else
                                        inf = new InfoAboutError(true, "", 0,
                                        query.ToCharArray()[0]);
                                }
                            }


                            else
                                //error like: int
                                if (isType(query))
                                inf = new InfoAboutError(true, "", query.Length + 1);
                            //error like: integreg
                            else
                                inf = new InfoAboutError(true, "", 0, query.ToCharArray()[0]);

                        }
                    }
                    //g.Value.Length > 0
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

                        string nameGroup2 = "NameType";
                        Group gr2 = m.Groups[nameGroup2];
                        for (int j = 0; j < gr2.Captures.Count; j++)
                        {
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
                                    && !isKeyChar(query.ToCharArray()[indexLenth])
                                    && !isIdentifierChar(query.ToCharArray()[indexLenth]))
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
                            indexLenth++;
                            while (indexLenth < query.ToCharArray().Length &&
                                query.ToCharArray()[indexLenth] == ' ')
                                indexLenth++;

                            //error like: int g, f , ]   , f 
                            if (indexLenth < query.ToCharArray().Length &&
                                (isKeyChar(query.ToCharArray()[indexLenth]) &&
                                !hasVar)
                               )
                            {
                                //error like: string? object?
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
                            //error like: int?[] abv rrr
                            else
                            {
                                string nameVar = "";
                                while (indexLenth < query.ToCharArray().Length)
                                {
                                    if (query.ToCharArray()[indexLenth] != ' '
                                        && !isKeyChar(query.ToCharArray()[indexLenth])
                                        && !isIdentifierChar(query.ToCharArray()[indexLenth])
                                        )
                                    {
                                        nameVar += query.ToCharArray()[indexLenth];
                                        indexLenth++;
                                    }
                                    else break;
                                }
                                
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
                                    if (query.Length > indexLenth)
                                        inf = new InfoAboutError(true,
                                            new String(query.ToCharArray(), 0, indexLenth), indexLenth + 1,
                                            query.ToCharArray()[indexLenth]);
                                    else
                                        inf = new InfoAboutError(true,
                                        new String(query.ToCharArray(), 0, indexLenth), indexLenth + 1,
                                        ';');

                                }
                                else
                                    //error like: int j, b d;
                                    if (indexLenth < query.ToCharArray().Length)
                                    inf = new InfoAboutError(true,
                                        new String(query.ToCharArray(), 0, indexLenth), indexLenth + 1,
                                        query.ToCharArray()[indexLenth]);
                                //error like: int j, d d
                                else
                                    inf = new InfoAboutError(true,
                                        new String(query.ToCharArray(), 0, indexLenth), indexLenth + 1);

                            }

                        }
                    }

                }
                //g.Value.Length >= query.Length
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
            char[] listReplaceString = new char[] { '\r', '\t', '\n' };
            foreach (char s in listReplaceString)
                while (str.IndexOf(s) != -1)
                    str = str.Replace(s, ' ');
            while (str.IndexOf("  ") != -1)
                str = str.Replace("  ", " ");
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
                        (!isKeyChar(masString[i].ToCharArray()[masString[i].Length - 1]) &&
                        !isKeyChar(masString[i + 1].ToCharArray()[0]))
                        ||
                        (isIdentifierChar(masString[i].ToCharArray()[masString[i].Length - 1]) ||
                        isIdentifierChar(masString[i + 1].ToCharArray()[0]))
                        )
                        newString += " ";
                    newString += masString[i + 1];
                }
                str = newString;
            }
            str += "";

        }

        //get position in source query
        private void findRealPositionError(ref InfoAboutError inf,
            string sourceQuery)
        {
            int positionLineError = 0;
            int positionError = 0;

            string[] masStr = sourceQuery.Split(';');
            int countString = masStr.Length;
            for (int i = masStr.Length - 1; i >= 0; i--)
            {
                if (masStr[i] == "")
                    countString--;
                else break;
            }
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
                if (masCharQuery[masCharQuery.Length - 1] != ';')
                    positionError++;
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

                    if (q == ";")
                    {
                        positionError++;
                        inf.indexLineError++;
                        continue;
                    }

                    char[] masCharQuery = q.ToCharArray();
                    if (i < inf.indexLineError)
                    {
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

                        masCharQuery = masStr[i].ToCharArray();
                        char[] masTrueCharQuery = inf.trueQuery.ToCharArray();
                        int positionInSplit = 0;
                        for (int j = 0; j < masTrueCharQuery.Length; j++)
                        {
                            while (masCharQuery[positionInSplit] != masTrueCharQuery[j]
                            && !(masCharQuery[positionInSplit] == '\n' && masTrueCharQuery[j] == ' '))
                            {
                                if (masCharQuery[positionInSplit] == '\n')
                                {
                                    positionError = 0;
                                    positionLineError++;
                                }
                                else
                                    positionError++;
                                positionInSplit++;
                            }
                        }

                        while (masCharQuery[positionInSplit] != inf.errorChar)
                        {
                            if (masCharQuery[positionInSplit] == '\n')
                            {
                                positionError = 0;
                                positionLineError++;
                            }
                            else
                                positionError++;
                            positionInSplit++;
                        }
                        positionError++;
                        break;

                    }
                
                }

            }

            inf.positionError = positionError;
            inf.positionLineError = positionLineError + 1;
        }



        private void findBadVariable(ref InfoAboutError inf, List<string> listVars)
        {
            string query = inf.trueQuery;
            for (int j = 0; j < masNameAllType.Length; j++)
                for (int i = 0; i < listVars.Count; i++)
                {
                    if (listVars[i] == masNameAllType[j] ||
                        !(Char.IsLetter(listVars[i].ToCharArray()[0])||
                        listVars[i].ToCharArray()[0]=='_')
                        )
                    {
                        int positionFirstVariable = query.IndexOf(listVars[i]);
                        int positionDoubleVariable = query.IndexOf(listVars[i],
                            positionFirstVariable + 1);
                        if (positionDoubleVariable == -1)
                            positionDoubleVariable = positionFirstVariable;
                        inf.error = true;
                        inf.errorChar = listVars[i].ToCharArray()[0];
                        inf.indexLineError = -1;
                        inf.positionError = positionDoubleVariable + 1;
                        inf.trueQuery = query.Substring(0, positionDoubleVariable);
                        inf.typeMessage = "Неверное имя переменной";
                        inf.message = listVars[i];
                    }
                }
        }

        private void deleteSymbols(ref string str)
        {
            while (str.IndexOf('\r') != -1)
                str = str.Remove(str.IndexOf('\r'), 1);
            char[] masChar = str.ToCharArray();
            for (int i = masChar.Length - 1; i >= 0; i--)
            {
                if (masChar[i] == ' ' || masChar[i] == '\n' ||
                    masChar[i] == '\r' || masChar[i] == '\t')
                    str = str.Remove(i, 1);
                else break;
            }
        }


        public InfoAboutError getFirstPositionError(InfoAboutError[] masError)
        {
            InfoAboutError iar = new InfoAboutError();
            for (int i = 0; i < masError.Length; i++)
                if (masError[i].error)
                {
                    iar = masError[i];
                    masError[i] = masError[0];
                    masError[0] = iar;
                }

            for (int i = 0; i < masError.Length; i++)
            {
                if (masError[i].error)
                {
                    iar = masError[i];
                    masError[i] = masError[0];
                    masError[0] = iar;
                }
            }

                for (int j = 0; j < masError.Length; j++)
                for (int i = 0; i < masError.Length - 1 - j; i++)
                {
                    if ((masError[i].positionError > masError[i + 1].positionError)
                        && (masError[i].error && masError[i + 1].error))
                    {
                        iar = masError[i];
                        masError[i] = masError[i + 1];
                        masError[i + 1] = iar;
                    }
                }

            for (int j = 0; j < masError.Length; j++)
                for (int i = 0; i < masError.Length-1; i++)
                {
                    if ((masError[i].positionLineError > masError[i + 1].positionLineError)
                            && (masError[i].error && masError[i + 1].error))
                    {
                        iar = masError[i];
                        masError[i] = masError[i + 1];
                        masError[i + 1] = iar;
                    }
                }
            return masError[0];

        }




        public InfoAboutError checkQuery(string query, List<string> listVars, List<string> listTypes)
        {
            RegAnalisator ra = new RegAnalisator();
            InfoAboutError inf = new InfoAboutError();
            deleteSymbols(ref query);

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

                if (inf.error)
                {
                    inf.indexLineError = i;
                    inf.trueQuery += inf.str;
                    InfoAboutError inf3 = inf.Clone();
                    InfoAboutError inf4 = inf.Clone();
                    findRealPositionError(ref inf, queryForFindPosition);
                    findDuplicateVariable(ref inf3, listVars);
                    if (inf3.error)
                    {
                        inf3.indexLineError = i;
                        findRealPositionError(ref inf3, queryForFindPosition);
                        if ((inf3.positionLineError < inf.positionLineError) ||
                        (inf3.error && !inf.error))
                        {
                            inf = inf3;
                        }
                        else if ((inf3.positionError < inf.positionError) ||
                        (inf3.error && !inf.error))
                        {
                            inf = inf3;
                        }

                        for (int k = listVars.Count - 1; k >= 0; k--)
                        {
                            if (listVars[k] == inf.message)
                            {
                                listVars.RemoveAt(k);
                                break;
                            }
                        }
                        findDuplicateVariable(ref inf, listVars);
                        if (inf.error)
                        {
                            inf.indexLineError = i;
                            findRealPositionError(ref inf, queryForFindPosition);
                        }
                    }
                    
                    // inf3 = inf.Clone();
                    findBadVariable(ref inf4, listVars);
                    if (inf4.error)
                    {
                        inf4.indexLineError = i;
                        findRealPositionError(ref inf4, queryForFindPosition);
                    }


                    InfoAboutError[] masError2 = new InfoAboutError[3];
                    masError2[0] = inf;
                    masError2[1] = inf3;
                    masError2[2] = inf4;
                    inf = getFirstPositionError(masError2);
                    break;
                }
                inf.trueQuery += q;

                InfoAboutError[] masError = new InfoAboutError[3];
                masError[2] = inf.Clone();
                masError[1] = inf.Clone();
                masError[0] = inf.Clone();
                findRealPositionError(ref masError[2], queryForFindPosition);
                findDuplicateVariable(ref masError[0], listVars);
                if (masError[0].error)
                {
                    masError[0].indexLineError = i;
                    findRealPositionError(ref masError[0], queryForFindPosition);
                    for (int k = listVars.Count - 1; k >= 0; k--)
                    {
                        if (listVars[k] == masError[0].message)
                        {
                            listVars.RemoveAt(k);
                            break;
                        }
                    }
                    findDuplicateVariable(ref masError[0], listVars);
                    if (masError[0].error)
                    {
                        masError[0].indexLineError = i;
                        findRealPositionError(ref masError[0], queryForFindPosition);
                    }


                }

                findBadVariable(ref masError[1], listVars);
                if (masError[1].error)
                {
                    masError[1].indexLineError = i;
                    findRealPositionError(ref masError[1], queryForFindPosition);
                }

                bool hasError = false;
                foreach (InfoAboutError iar in masError)
                    if (iar.error)
                    {
                        hasError = true;
                        break;
                    }
                        
                if (hasError)
                {
                    inf = getFirstPositionError(masError);
                    break;
                }
            }
            char[] masCharForQuery = queryForFindPosition.ToCharArray();
            for (int i= masCharForQuery.Length -1;i>=0;i--)
            {
                if(masCharForQuery[i]!=' ' && masCharForQuery[i] != '\t')
                {
                    if (masCharForQuery[i] != ';')
                    {
                        inf.typeMessage = "Отсутствует символ";
                        inf.message = ";";  
                    }
                    break;
                }
            }
            return inf;
        }



    }

}
