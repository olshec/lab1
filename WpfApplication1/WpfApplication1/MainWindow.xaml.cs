﻿using System;
using System.Collections.Generic;
using System.IO;
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
            textBox.Text = "     string [, ,,] bbb, a2  , uu ;;;" + '\n' + " float a ff ;; ";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            richTextBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            richTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
        }


        private void run(string query)
        {
            richTextBox.Document.Blocks.Clear();

            RegAnalisator ra = new RegAnalisator();
            List<string> listVars = new List<string>();
            List<string> listTypes = new List<string>();


            InfoAboutError inf = ra.checkQuery(query, listVars, listTypes);

            richTextBox.AppendText("Ошибка: " + ((inf.error) ? "Да" : "Нет"));
            if (inf.error)
            {
                richTextBox.AppendText(Environment.NewLine + "Строка: " + inf.positionLineError);
                richTextBox.AppendText(Environment.NewLine + "Позиция: " + inf.positionError);
                if (inf.message == "")
                    richTextBox.AppendText(Environment.NewLine + 
                        "Неверный символ: " + inf.errorChar);
                else if (inf.typeMessage == "Отсутствует символ")
                    richTextBox.AppendText(Environment.NewLine + 
                        "Отсутствует символ: "+ inf.message);
                else if (inf.typeMessage == "Дубликат переменной")
                    richTextBox.AppendText(Environment.NewLine + 
                        "Дубликат переменной: "+ inf.message);
                else if (inf.typeMessage == "Неверное имя переменной")
                    richTextBox.AppendText(Environment.NewLine + 
                        "Неверное имя переменной: "+ inf.message);
            }

        }


        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                char[] mas = textBox.Text.ToCharArray();
                run(textBox.Text);
            }
            catch
            {
                MessageBox.Show("Произошла какакя-то ошибка (:",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.Document.Blocks.Clear();
            string query = "";
            Microsoft.Win32.OpenFileDialog of =
                new Microsoft.Win32.OpenFileDialog();
            of.ShowDialog();
            if (of.FileName != "")
            {
                File.OpenText(of.FileName);
                StreamReader sr = File.OpenText(of.FileName);
                while (true)
                {
                    string str = sr.ReadLine();
                    if (str == null)
                    {
                        sr.Close();
                        break;
                    }

                    query += str;
                }
                try
                {
                    textBox.Text = query;
                    run(query);
                }
                catch
                {
                    MessageBox.Show("Произошла какакя-то ошибка (:",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}
