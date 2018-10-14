using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            richTextBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            richTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
        }

       

        //string[] masNameTypeWithNull = new string[] {"sbyte","short","int","long","byte","ushort","uint",
        //       "ulong","float","double","decimal","bool","char"};

        //string[] masNameTypeNotNull = new string[] { "string", "object" };

        //string[] masNameAllType = new string[] {"sbyte","short","int","long","byte","ushort","uint",
        //       "ulong","float","double","decimal","bool","char", "string", "object" };

        //int checkString(string query, List<string> listVars)
        //{



        //    string nameTypeWithNull = String.Join("|", masNameTypeWithNull);
        //    string nameTypeNotNull = String.Join("|", masNameTypeNotNull);
        //    string nameAllType = String.Join("|", masNameAllType);


        //    string pattern = @"^
        //    (
        //        (
        //            (          (?<=^)                          (" + nameAllType + @")                    (?=\?|\[|\s)       )    |
        //            (          (?<=" + nameTypeWithNull + @")  (\?)                                      (?=\[|\w+)         )    |
        //            (          (?<=" + nameAllType + @")       (\s)                                      (?=\w+)            )    |
        //            (          (?<=\?|" + nameAllType + @")    (?<Sk>\[)                                 (?=,|\])           )    |
        //            (          (?<=\w+|\[)                     (,)                                       (?=,|\]|\w+)       )    |
        //            (          (?<=,)                          (?(Sk),)                                  (?=,|\])           )    |
        //            (          (?<=,|\[)                       (?<-Sk>\])                                (?=\w+)            )    |
        //            (          (?<=\?|\s|\]|,)  (?(Sk)(?!))    (?<NameVar>\w+)                           (?=,|;)            )    |
        //            (          (?<=\w+)         (?(Sk)(?!))    (;)                                       (?=;|$)            )    |
        //        )+
        //    )";

            

        //    Regex r = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);

        //    Match m = r.Match(query);

        //    int lenthTrueQuery = query.Length;
        //    if (m.Groups.Count > 1)
        //    {
        //        Group g = m.Groups[1];
                
        //        richTextBox.AppendText(String.Format("True string: '{0}'   Length: '{1}'",
        //            g.Value, g.Length));
        //        richTextBox.AppendText(Environment.NewLine + "QUWERY = " + query.Length + Environment.NewLine);
        //        if (g.Value.Length < query.Length)
        //        {
        //            if(g.Value.Length==0)
        //                lenthTrueQuery = 1;
        //            else
        //               // if(g.Value.ToCharArray()[g.Value.Length-1]==' ')
        //               //     lenthTrueQuery = g.Value.Length + 3;
        //               // else
        //                    lenthTrueQuery = g.Value.Length + 2;
        //        }
        //        string nameGroup = "NameVar";
        //        g = m.Groups[nameGroup];
        //        for (int j = 0; j < g.Captures.Count; j++)
        //        {
        //            Capture c = g.Captures[j];
        //            listVars.Add(c.Value);
        //        }

        //       // richTextBox.AppendText("SKOPEN: ");
        //        nameGroup = "Sk";
        //        g = m.Groups[nameGroup];
        //        for (int j = 0; j < g.Captures.Count; j++)
        //        {
        //            Capture c = g.Captures[j];
        //            richTextBox.AppendText("SKOPEN: "+c.Value);

        //        }
        //        return lenthTrueQuery;
        //    }
        //    return lenthTrueQuery;

        //}

        //bool checkKey(char chekChar)
        //{
        //    char[] listReplaceString = new char[] { '?', '[', ']', ',', ';' };
        //    foreach (char c in listReplaceString)
        //    {
        //        if (c == chekChar)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //void formatString(ref string str)
        //{
        //    string[] listReplaceString = new string[] { "\n", "  " };
        //    foreach (string s in listReplaceString)
        //        while (str.IndexOf(s) != -1)
        //            str = str.Replace(s, " ");
        //    while (str.IndexOf(";;") != -1)
        //        str = str.Replace(";;", ";");

        //    if (str.Length > 1)
        //        if (str.ToCharArray()[0] == ' ')
        //            str = str.Remove(0, 1);

        //    if (str.Length > 1)
        //        if (str.ToCharArray()[str.Length - 1] == ' ')
        //        {
        //            str = str.Remove(str.Length - 1, 1);
        //        }
                   

        //    if (str.Length > 0)
        //    {
        //        string[] masString = str.Split(' ');
        //        string newString = "";
        //        newString += masString[0];
        //        for (int i = 0; i < masString.Length - 1; i++)
        //        {

        //            if (!checkKey(masString[i].ToCharArray()[masString[i].Length - 1]) &&
        //                !checkKey(masString[i + 1].ToCharArray()[0]))
        //                newString += " ";
        //            newString += masString[i + 1];
        //        }
        //        str = newString;
        //    }
        //    str += "";

        //}


        private void button2_Click(object sender, RoutedEventArgs e)
        {
            RegAnalisator ra = new RegAnalisator();
            List<string> listVars = new List<string>();
            //float?[,,,] a, b; ; ; ;
            richTextBox.Document.Blocks.Clear();

            string query = textBox.Text;
            ra.formatString(ref query);

            int positionError = -1;
            int indexQuery = -1;
            string trueQuery = ""; 
            string[] masStr = query.Split(';');
            int countString = masStr.Length;
            if (masStr[masStr.Length - 1] == "")
                countString--;
            for (int i=0;i< countString; i++)
            {
                string q = masStr[i];
                if (i == countString - 1)
                {
                    if (query.ToCharArray()[query.Length - 1] == ';')
                        q += ";";
                }
                else q += ";" ;
                InfoAboutError inf = ra.checkString(q, listVars);
                //int lenth = ra.checkString(q, listVars);
                if (inf.error==true)
                {
                    positionError = inf.str.Length-1;
                    indexQuery=i;
                    trueQuery += inf.str;
                    break;
                }
                trueQuery += q;
            }

            richTextBox.AppendText(Environment.NewLine + "--trueQuery: "+trueQuery);
            richTextBox.AppendText(Environment.NewLine + "--indexQuery: " + indexQuery);


            richTextBox.AppendText(Environment.NewLine + "vars: ");
            foreach (string s in listVars)
                richTextBox.AppendText(s + ", ");
            richTextBox.AppendText(Environment.NewLine);

        }



    }
}
