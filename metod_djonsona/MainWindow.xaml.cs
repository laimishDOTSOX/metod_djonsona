using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace metod_djonsona
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBox[,] all_boxes = new TextBox[2, 10];
        public MainWindow()
        {
            InitializeComponent();
            int count = answer_array_one.ColumnDefinitions.Count;
            for (int i = 0; i < count; i++)
            {
                TextBox box = new TextBox();
                box.BorderBrush = Brushes.Black;
                box.BorderThickness = new Thickness(1);
                box.FontSize = 30;
                box.TextAlignment = TextAlignment.Center;
                box.VerticalContentAlignment = VerticalAlignment.Center;
                box.IsReadOnly = true;
                answer_array_one.Children.Add(box);
                Grid.SetColumn(box,i);
                all_boxes[0, i] = box;
            }
            for (int i = 0; i < count; i++)
            {
                TextBox box = new TextBox();
                box.BorderBrush = Brushes.Black;
                box.BorderThickness = new Thickness(1);
                box.FontSize = 30;
                box.TextAlignment = TextAlignment.Center;
                box.VerticalContentAlignment = VerticalAlignment.Center;
                box.IsReadOnly = true;
                answer_array_two.Children.Add(box);
                Grid.SetColumn(box, i);
                all_boxes[1, i] = box;
            }
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var boxes = mane_array_one.Children.OfType<TextBox>();
            int count_children = mane_array_one.Children.Count*2;
            int count = 0;
            foreach (var box in boxes)
            {
                string old_text = box.Text;
                if (old_text != "")
                {
                    try
                    {
                        int a = Convert.ToInt32(old_text);
                        if (a == 0 || old_text.Length > 2) box.Text = old_text.Substring(0, old_text.Length - 1);
                        if (old_text.Substring(0, 1) == "0") box.Text = old_text.Substring(1, old_text.Length - 1);
                        count++;
                    }
                    catch { box.Text = old_text.Substring(0, old_text.Length - 1); }
                }
                
            }
            boxes = mane_array_two.Children.OfType<TextBox>();
            foreach (var box in boxes)
            {
                string old_text = box.Text;
                if (old_text != "")
                {
                    try
                    {
                        int a = Convert.ToInt32(old_text);
                        if (a == 0 && a > 99) box.Text = old_text.Substring(0, old_text.Length - 1);
                        count++;
                        if (count == count_children) Get_Answer_Array();
                    }
                    catch { box.Text = old_text.Substring(0, old_text.Length - 1); }
                }

            }
        }

        public void Get_Answer_Array()
        {
            int[] one_st = new int[10];
            int[] two_st = new int[10];
            int i = 0;
            var boxes = mane_array_one.Children.OfType<TextBox>();
            foreach (var box in boxes)
            {
                one_st[i] = Convert.ToInt32(box.Text);
                i++;

            }
            i = 0;
            boxes = mane_array_two.Children.OfType<TextBox>();
            foreach (var box in boxes)
            {
                two_st[i] = Convert.ToInt32(box.Text);
                i++;

            }
            one_st = one_st.OrderBy(x => x).ToArray();
            two_st = two_st.OrderByDescending(x => x).ToArray();

            for (int c = 0; c < answer_array_one.Children.Count; c++)
            {
                all_boxes[0, c].Text = Convert.ToString(one_st[c]);
                all_boxes[1, c].Text = Convert.ToString(one_st[c]);
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var boxes = mane_array_one.Children.OfType<TextBox>();
            foreach (var box in boxes) box.Text = "";

            boxes = mane_array_two.Children.OfType<TextBox>();
            foreach (var box in boxes) box.Text = "";

            boxes = answer_array_two.Children.OfType<TextBox>();
            foreach (var box in boxes) box.Text = "";

            boxes = answer_array_one.Children.OfType<TextBox>();
            foreach (var box in boxes) box.Text = "";
        }
    }
}
