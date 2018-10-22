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
            List<string> listTypes = new List<string>();
            //float?[,,,] a, b; ; ; ;
            richTextBox.Document.Blocks.Clear();

            string query = textBox.Text;

            InfoAboutError inf = ra.getTrueQuery(query, listVars, listTypes);

            ra.findDoubleVariable(ref inf, listVars);

            richTextBox.AppendText(Environment.NewLine + "vars: ");
            foreach (string s in listVars)
                if (s != "")
                    richTextBox.AppendText(s + ", ");

            richTextBox.AppendText(Environment.NewLine + "Types: ");
            foreach (string s in listTypes)
                if (s != "")
                    richTextBox.AppendText(s + ", ");
            richTextBox.AppendText(Environment.NewLine);

            richTextBox.AppendText(Environment.NewLine + "trueQuery: "+inf.trueQuery);
            richTextBox.AppendText(Environment.NewLine + "indexLineError: " + inf.indexLineError);
            richTextBox.AppendText(Environment.NewLine + "error position: " + inf.positionError);
            richTextBox.AppendText(Environment.NewLine + "error symbol: " + inf.errorChar);

            

        }



    }
}
