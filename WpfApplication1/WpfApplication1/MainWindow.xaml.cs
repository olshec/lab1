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

       

        string[] masNameTypeWithNull = new string[] {"sbyte","short","int","long","byte","ushort","uint",
               "ulong","float","double","decimal","bool","char"};

        string[] masNameTypeNotNull = new string[] { "string", "object" };

        string[] masNameAllType = new string[] {"sbyte","short","int","long","byte","ushort","uint",
               "ulong","float","double","decimal","bool","char", "string", "object" };

        void checkString(string query, List<string> listVars)
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

            if (m.Groups.Count > 1)
            {
                Group g = m.Groups[1];
                richTextBox.AppendText(String.Format("True string: '{0}'   Length: '{1}'",
                    g.Value, g.Length));
                richTextBox.AppendText(Environment.NewLine + "QUWERY = " + query.Length + Environment.NewLine);

                string nameGroup = "NameVar";
                g = m.Groups[nameGroup];
                for (int j = 0; j < g.Captures.Count; j++)
                {
                    Capture c = g.Captures[j];
                    listVars.Add(c.Value);
                }

                richTextBox.AppendText("SKOPEN: "+ Environment.NewLine + Environment.NewLine);
                nameGroup = "Sk";
                g = m.Groups[nameGroup];
                for (int j = 0; j < g.Captures.Count; j++)
                {
                    Capture c = g.Captures[j];
                    richTextBox.AppendText(c.Value);

                }

            }


        }

        bool checkKey(char chekChar)
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

        void formatString(ref string str)
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
                    //str = String.Join("", str.ToCharArray(), 0, str.Length - 1);

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


        private void button2_Click(object sender, RoutedEventArgs e)
        {
            List<string> listVars = new List<string>();
            float?[,,,] a, b; ; ; ;
            richTextBox.Document.Blocks.Clear();

            string query = "     string [, ,,] bbb, a2  , uu;;;  float a; ";
            formatString(ref query);

            string[] masStr = query.Split(';');
            for(int i=0;i<masStr.Length;i++)
            {
                checkString(masStr[i]+";", listVars);
            }

            //checkString(query, listVars);
            

            richTextBox.AppendText(Environment.NewLine + "vars: ");
            foreach (string s in listVars)
                richTextBox.AppendText(s + ", ");
            richTextBox.AppendText(Environment.NewLine);

        }



    }
}
