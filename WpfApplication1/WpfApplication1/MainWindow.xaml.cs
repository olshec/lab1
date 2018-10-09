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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.Document.Blocks.Clear();
            const string pattern = @"^
            (
                (
                    (          (?<=^|,|\[)          (?<op>[a-z]+)         (?=,|\[|\])  )    |
                    (          (?<=,|\[)            (?<op>\d+)            (?=,|\])     )    |
                    (          (?<=[a-z]|\d)        (?<op>,)              (?=[a-z]|\d) )    |
                    (          (?<=\])              (?(level)(?<op>,))    (?=[a-z]|\d) )    |
                    (?<level>  (?<=[a-z])           (?<op>\[)             (?=[a-z]|\d) )    |
                    (?<-level> (?<=[a-z]|\d|\])     (?<op>\])             (?=,|\]|$)   )
                )+

            )";
            //(?(level)(?!))
            Regex r = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);

            string query = "aaa[g[1],bcd[2]]";
            query = "aaa[1,bcd[2]]!";
            Match m = r.Match(query);
            //MatchCollection mc = r.Matches(query);
            //richTextBox.AppendText(" MatchCollection = " + mc.Count+ 
            //    "     Почему же здесь 0???");
            //richTextBox.AppendText(Environment.NewLine);
            //richTextBox.AppendText(" count groups = " +m.Groups.Count + Environment.NewLine);
            //Group g1 = m.Groups[0];
            //richTextBox.AppendText(String.Format("    Группа {0} = '{1}'", "", g1.Captures.Count) + Environment.NewLine);
            int matchCount = 0;

            while (m.Success)
            {
                richTextBox.AppendText(String.Format("Соответствие {0}, длина {1}", ++matchCount, m.Length));
                richTextBox.AppendText(Environment.NewLine);

                for (int i = 1; i < m.Groups.Count; i++)
                {
                    string nameGroups = "letters";
                    Group g = m.Groups[i];
                    richTextBox.AppendText(String.Format("    Группа {0} = '{1}'", i, g.Value) + Environment.NewLine);
                    for (int j = 0; j < g.Captures.Count; j++)
                    {
                        Capture c = g.Captures[j];
                        richTextBox.AppendText(String.Format("        Захват {0} = '{1}', " +
              "позиция = {2}, длина = {3} ", j, c, c.Index, c.Length) + Environment.NewLine);
                    }
                }
                m = m.NextMatch();
            }

        }




        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            richTextBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            richTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // string text = "One car red car blue car";
            // string pat = @"(\w+)\s+(car)";
            richTextBox.Document.Blocks.Clear();
            string text = "aaa[1,bcd[2]]";
            string pat = @"^
            (
                (
                    (          (?<=^|,|\[)      (?<op>[a-z]+) (?=,|\[|\])  ) |
                    (          (?<=,|\[)        (?<op>\d+)    (?=,|\])     ) |
                    (          (?<=[a-z]|\d|\]) (?<op>,)      (?=[a-z]|\d) ) |
                    (?<level>  (?<=[a-z])       (?<op>\[)     (?=[a-z]|\d) ) |
                    (?<-level> (?<=[a-z]|\d|\]) (?<op>\])     (?=,|\]|$)   )
                )+
                (?(level)(?!))
            )$";

            // Instantiate the regular expression object.
            Regex r = new Regex(pat, RegexOptions.IgnorePatternWhitespace);

            // Match the regular expression pattern against a text string.
            Match m = r.Match(text);
            int matchCount = 0;
            while (m.Success)
            {
                richTextBox.AppendText(String.Format("Match" + (++matchCount)));
                richTextBox.AppendText(Environment.NewLine);
                for (int i = 1; i <= m.Groups.Count; i++)
                {
                    Group g = m.Groups[i];
                    richTextBox.AppendText(String.Format("    Group" + i + "='" + g + "'"));
                    richTextBox.AppendText(Environment.NewLine);
                    CaptureCollection cc = g.Captures;
                    for (int j = 0; j < cc.Count; j++)
                    {
                        Capture c = cc[j];
                        richTextBox.AppendText(String.Format("        Capture" + j + "='" + c + "', Position=" + c.Index));
                        richTextBox.AppendText(Environment.NewLine);
                    }
                }
                m = m.NextMatch();
            }
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

            //string space = @"[\r\n\t\s]+";
            //space = @"[\s]+";
            // string word = @"\w";

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


            // (          (?<=,)                          (?(Sk),)                                  (?=,|\]|\w+)       )    |

            //string pattern = @"\s

            //(
            //    (
            //        (    (?<=" + space  + @")                             (?(SkOpen)(?!))     (" + nameAllType + @")       (?=\?|\[|" + space + @")                              )    |
            //        (    (?<=" + space + "|" + nameTypeWithNull + @")     (?(SkOpen)(?!))     (\?)                         (?=\[|\w+|" + space + @")                             )    |
            //        (    (?<=" + space + @"|\w+|\?|\[|\]|,|;)                                 (" + space + @")             (?=" + space + "|" +  @"|\w+|\?|\[|\]|,|;)            )    |
            //        (    (?<=" + space + "|" + nameAllType + @"|\?)       (?(SkOpen)(?!))     (?<SkOpen>(?<Sk>\[))         (?=,|\]|" + space + @")                               )    |
            //        (    (?<=" + space + "|" + @"\w +|\[|,)               (,)                                              (?=,|\]|\w+|" + space + @")                           )    |
            //        (    (?<=" + space + "|" + @"|,|\[)                   (?<-Sk>\])                                       (?=\w+|" + space + @")                                )    |
            //        (    (?<=" + space + "|" + @"|\?|\]|,)                (?(Sk)(?!))         (?<NameVar>\w+)              (?=,|;|" + space + @")                                )    |
            //        (    (?<=" + space + "|" + @"|\w+)                    (?(Sk)(?!))         (?<-SkOpen>;)                (?=;|$|" + space + @")                                )    |
            //    )+
            //)";


            //string pattern = @"\s
            //(
            //    (
            //        (    (?<=" + space + @")                             (?(SkOpen)(?!))     (" + nameAllType + @")       (?=\?|\[|" + space + @")                              )    |
            //        (    (?<=" + space + "|" + nameTypeWithNull + @")     (?(SkOpen)(?!))     (\?)                         (?=\[|\w+|" + space + @")                             )    |
            //        (    (?<=" + space + @"|\w+|\?|\[|\]|,|;)                                 (" + space + @")             (?=" + space + "|" + @"|\w+|\?|\[|\]|,|;)            )    |
            //        (    (?<=" + space + "|" + nameAllType + @"|\?)       (?(SkOpen)(?!))     (?<SkOpen>(?<Sk>\[))         (?=,|\]|" + space + @")                               )    |
            //        (    (?<=" + space + "|" + @"\w +|\[|,)               (,)                                              (?=,|\]|\w+|" + space + @")                           )    |
            //        (    (?<=" + space + "|" + @"|,|\[)                   (?<-Sk>\])                                       (?=\w+|" + space + @")                                )    |
            //        (    (?<=" + space + "|" + @"|\?|\]|,)                (?(Sk)(?!))         (?<NameVar>\w+)              (?=,|;|" + space + @")                                )    |
            //        (    (?<=" + space + "|" + @"|\w+)                    (?(Sk)(?!))         (?<-SkOpen>;|;)                (?=;|$|" + space + @")                                )    |
            //        (    (?<=" + space + "|" + @"|\w+)                    (?(Sk)(?!))         (;)                          (?=;|$|" + space + @")                                )    |
            //    )+
            //)";

            Regex r = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);

            Match m = r.Match(query);

            if (m.Groups.Count > 1)
            {
                Group g = m.Groups[1];
                richTextBox.AppendText(String.Format("True string: '{0}'   Length: '{1}'",
                    g.Value, g.Length));

                string nameGroup = "NameVar";
                g = m.Groups[nameGroup];
                for (int j = 0; j < g.Captures.Count; j++)
                {
                    Capture c = g.Captures[j];
                    listVars.Add(c.Value);
                }

                richTextBox.AppendText(Environment.NewLine);
                richTextBox.AppendText(String.Format("SKOPEN: "));
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

            string query = "     string [, ,,] bbb, a2  , uu;   ";
            formatString(ref query);
            checkString(query, listVars);
            richTextBox.AppendText(Environment.NewLine + "QUWERY = " + query.Length + Environment.NewLine);
            richTextBox.AppendText(Environment.NewLine);

            //query = "int c1,c2;";
            //checkString(query, listVars);
            //richTextBox.AppendText(Environment.NewLine + "QUWERY = " + query.Length + Environment.NewLine);
            //richTextBox.AppendText(Environment.NewLine);

            //query = "float?[,,,]a,b,c;";
            //checkString(query, listVars);
            //richTextBox.AppendText(Environment.NewLine + "QUWERY = " + query.Length + Environment.NewLine);
            //richTextBox.AppendText(Environment.NewLine);

            richTextBox.AppendText(Environment.NewLine + "vars: ");
            foreach (string s in listVars)
                richTextBox.AppendText(s + ", ");
            richTextBox.AppendText(Environment.NewLine);

        }






    }
}
