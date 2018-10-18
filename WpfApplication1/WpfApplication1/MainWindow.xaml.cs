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

       

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            RegAnalisator ra = new RegAnalisator();
            List<string> listVars = new List<string>();
            InfoAboutError inf=new InfoAboutError();
            //float?[,,,] a, b; ; ; ;
            richTextBox.Document.Blocks.Clear();

            string query = textBox.Text;
            ra.formatString(ref query);

           // int positionError = -1;
            int indexLineError = -1;
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
                inf = ra.checkString(q, listVars);
                //int lenth = ra.checkString(q, listVars);
                if (inf.error==true)
                {
                   // positionError = inf.str.Length-1;
                    indexLineError=i;
                    trueQuery += inf.str;
                    break;
                }
                trueQuery += q;
            }

            richTextBox.AppendText(Environment.NewLine + "trueQuery: "+trueQuery);
            richTextBox.AppendText(Environment.NewLine + "indexLineError: " + indexLineError);
            richTextBox.AppendText(Environment.NewLine + "error position: " + inf.positionError);

            richTextBox.AppendText(Environment.NewLine + "vars: ");
            foreach (string s in listVars)
                richTextBox.AppendText(s + ", ");
            richTextBox.AppendText(Environment.NewLine);

        }



    }
}
