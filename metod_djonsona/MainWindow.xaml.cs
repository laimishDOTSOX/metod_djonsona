using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        //списки объектов класса Boxes_detals для хранения ссылок на блоки и послебующего обращения к ним
        List<Boxes_detals> boxes_Detals = new List<Boxes_detals>();
        List<Boxes_detals> mane_boxes_Detals = new List<Boxes_detals>();
        public MainWindow()
        {
            InitializeComponent();
           
            int count = answer_array_one.ColumnDefinitions.Count;
            //создание блоков для времени обработки на первом станке
            for (int i = 0; i < count; i++)
            {
                TextBox box = new TextBox();
                box.BorderBrush = Brushes.Black;
                box.BorderThickness = new Thickness(1);
                box.FontSize = 30;
                box.TextAlignment = TextAlignment.Center;
                box.VerticalContentAlignment = VerticalAlignment.Center;
                box.IsReadOnly = true;
                box.Style = (Style)Resources["TextBoxStyle1"];
                answer_array_one.Children.Add(box);
                Grid.SetColumn(box,i);
                boxes_Detals.Add(new Boxes_detals()
                {
                    time_one = box
                });
            }
            //создание блоков для времени обработки на втором станке
            for (int i = 0; i < count; i++)
            {
                TextBox box = new TextBox();
                box.BorderBrush = Brushes.Black;
                box.BorderThickness = new Thickness(1);
                box.FontSize = 30;
                box.TextAlignment = TextAlignment.Center;
                box.VerticalContentAlignment = VerticalAlignment.Center;
                box.IsReadOnly = true;
                box.Style = (Style)Resources["TextBoxStyle1"];
                answer_array_two.Children.Add(box);
                Grid.SetColumn(box, i);
                boxes_Detals[i].time_two = box;
            }
            //создание блоков для номеров деталей
            for (int i = 0; i < count; i++)
            {
                TextBox box = new TextBox();
                box.BorderBrush = Brushes.White;
                box.BorderThickness = new Thickness(1,0,1,1);
                box.Foreground = Brushes.White;
                box.Background = Brushes.Black;
                box.FontSize = 30;
                box.TextAlignment = TextAlignment.Center;
                box.VerticalContentAlignment = VerticalAlignment.Center;
                box.IsReadOnly = true;
                box.Text = $"{i + 1}";
                box.Style = (Style)Resources["TextBoxStyle1"];
                array_number.Children.Add(box);
                Grid.SetColumn(box, i);
                boxes_Detals[i].number = box;
            }
            //добавляем блоки из первой матрицы в список
            int c = 0;
            foreach (var item in mane_array_one.Children.OfType<TextBox>())
            {
                mane_boxes_Detals.Add(new Boxes_detals
                {
                    time_one = item
                });
            }
            foreach (var item in mane_array_two.Children.OfType<TextBox>())
            {
                mane_boxes_Detals[c].time_two = item;
                c++;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) 
        {
            //проверка на отрицание, ноль, 3-х значные числа и буквы
            int count_children = mane_array_one.Children.Count*2;
            int count = 0;
            foreach (var box in mane_array_one.Children.OfType<TextBox>())
            {
                string old_text = box.Text;
                if (old_text != "")
                {
                    try
                    {
                        int a = Convert.ToInt32(old_text);
                        if (a == 0 || old_text.Length > 2 || old_text.Substring(0,1) == "-") box.Text = old_text.Substring(1, old_text.Length - 1);
                        if (old_text.Substring(0, 1) == "0") box.Text = old_text.Substring(1, old_text.Length - 1);
                        count++;
                    }
                    catch { box.Text = old_text.Substring(0, old_text.Length - 1); }
                }
                
            }
            
            foreach (var box in mane_array_two.Children.OfType<TextBox>())
            {
                string old_text = box.Text;
                if (old_text != "")
                {
                    try
                    {
                        int a = Convert.ToInt32(old_text);
                        if (a == 0 || a > 99) box.Text = old_text.Substring(0, old_text.Length - 1);
                        count++;
                        if (count == count_children) Get_Answer_Array(); //если все ячейки заполнены выполняем метод 
                    }
                    catch { box.Text = old_text.Substring(0, old_text.Length - 1); }
                }

            }
        }

        public class Number_detals
        {
            public int number { get; set; }
            public int time_one { get; set; }
            public int time_two{ get; set; }
        }

        public class Boxes_detals
        {
            public TextBox number { get; set; }
            public TextBox time_one { get; set; }
            public TextBox time_two { get; set; }
        }

        public void Get_Answer_Array()
        {
            int[] downtime= new int[array_number.Children.Count]; //массив для подсчета времени простоя

            List<Number_detals> number_Detals = new List<Number_detals>();//лист для хранения значений деталей (номер, время обработки на 1 и 2 станках)
            //заполняем список
            foreach (var box in mane_array_one.Children.OfType<TextBox>())
            {
                number_Detals.Add(new Number_detals
                {
                    time_one = Convert.ToInt32(box.Text)
                });

            }
            int i = 0;
            int all_time = 0;
            foreach (var box in mane_array_two.Children.OfType<TextBox>())
            {
                number_Detals[i].time_two = Convert.ToInt32(box.Text);
                i++;
                all_time += Convert.ToInt32(box.Text);//считаем время обработки на 2 станке 
            }
            i = 0;
            //заполняем список
            foreach (var box in mane_number.Children.OfType<TextBox>())
            {
                number_Detals[i].number = Convert.ToInt32(box.Text);
                i++;

            }
            //находим времена простоя 
            for (int c = 0; c < downtime.Length; c++)
            {
                if (c > 0)
                {
                    downtime[c] = Convert.ToInt32(mane_boxes_Detals[c].time_one.Text);
                    for (int t = 0; t < c; t++)
                    {
                        downtime[c] += Convert.ToInt32(mane_boxes_Detals[c - (t + 1)].time_one.Text) - Convert.ToInt32(mane_boxes_Detals[c - (t + 1)].time_two.Text);
                    }
                    Debug.WriteLine(downtime[c]);
                }
                else
                {
                    downtime[c] = Convert.ToInt32(mane_boxes_Detals[c].time_one.Text);
                    Debug.WriteLine(downtime[c]);
                }
            }
            downtime = downtime.OrderByDescending(x => x).ToArray();//сортируем массив по убыванию, чтобы максимальное число находилось на 0 индексе 
            Debug.WriteLine("\n");
            foreach (var item in downtime)
            {
                Debug.WriteLine(item);
            }
            all_time += downtime[0];//общее время обработки на обоих станках в 1 матрице
            DT_one.Text = $"F = {downtime[0]}\nT = {all_time}";
            all_time -= downtime[0];
            int count = 0;
            //получаем количество шагов
            if (array_number.Children.Count % 2 == 1) count = array_number.Children.Count / 2 + 1;
            else count = array_number.Children.Count / 2;
            for (int c = 0; c < count ; c++)
            {
                var sort_boxes = number_Detals.OrderBy(e => e.time_one);//сортируем по возрастанию по времени обработки на 1 станке 
                number_Detals = sort_boxes.ToList();
                //заполняем данными детали с начала очереди
                boxes_Detals[c].number.Text = Convert.ToString(number_Detals[0].number);
                boxes_Detals[c].time_one.Text = Convert.ToString(number_Detals[0].time_one);
                boxes_Detals[c].time_two.Text = Convert.ToString(number_Detals[0].time_two);
                number_Detals.RemoveAt(0);//удаляем деталь из списка, тк как мы уже поместили ее в очередь

                sort_boxes = number_Detals.OrderBy(e => e.time_two);
                number_Detals = sort_boxes.ToList();
                boxes_Detals[boxes_Detals.Count - (c+1)].number.Text = Convert.ToString(number_Detals[0].number);
                boxes_Detals[boxes_Detals.Count - (c + 1)].time_one.Text = Convert.ToString(number_Detals[0].time_one);
                boxes_Detals[boxes_Detals.Count - (c + 1)].time_two.Text = Convert.ToString(number_Detals[0].time_two);
                number_Detals.RemoveAt(0);
            }

            Debug.WriteLine("\nDT_TWO_________________________________________");
            for (int c = 0; c < downtime.Length; c++)
            {
                if (c > 0)
                {
                    downtime[c] = Convert.ToInt32(boxes_Detals[c].time_one.Text);
                    for (int t = 0; t < c; t++)
                    {
                        downtime[c] += Convert.ToInt32(boxes_Detals[c - (t + 1)].time_one.Text) - Convert.ToInt32(boxes_Detals[c - (t + 1)].time_two.Text);
                    }
                    Debug.WriteLine(downtime[c]);
                }
                else
                {
                    downtime[c] = Convert.ToInt32(boxes_Detals[c].time_one.Text);
                    Debug.WriteLine(downtime[c]);
                }
            }
            downtime = downtime.OrderByDescending(x => x).ToArray();
            Debug.WriteLine("\n");
            foreach (var item in downtime)
            {
                Debug.WriteLine(item);
            }
            all_time += downtime[0];//общее время обработки на обоих станках в 1 матрице
            DT_two.Text = $"F = {downtime[0]}\nT = {all_time}";
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

            int i = 1;
            boxes = array_number.Children.OfType<TextBox>();
            foreach (var box in boxes)
            {
                box.Text = $"{i}";
                i++;
            }

            DT_one.Text = "";
            DT_two.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button_Click(sender,e);
            Random rn = new Random();
            foreach (var item in mane_array_one.Children.OfType<TextBox>()) item.Text = $"{rn.Next(1, 10)}";
            foreach (var item in mane_array_two.Children.OfType<TextBox>()) item.Text = $"{rn.Next(1, 10)}";
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {

        }
    }
}
