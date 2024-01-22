using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace WpfSharpGl
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LPointsList = new List<Pair>();
            RPointsList = new List<Pair>();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if ((bool)openFileDialog.ShowDialog())
            {
                Button btnSource = (args.Source as Button);
                if (btnSource.Name[btnSource.Name.Length - 1] == '2')
                {
                    fileLabel2.Content = openFileDialog.FileName;
                    imgOutload2.Source = new ImageSourceConverter().ConvertFromString(openFileDialog.FileName) as ImageSource;
                }
                else
                {
                    fileLabel.Content = openFileDialog.FileName;
                    imgOutload.Source = new ImageSourceConverter().ConvertFromString(openFileDialog.FileName) as ImageSource;
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs args)
        {
            LPointsList.Clear();
            RPointsList.Clear();
            LPoints.Items.Clear();
            RPoints.Items.Clear();
            Cnv.Children.Clear();
            Cnv2.Children.Clear();
        }

        private void btnOpenWin_Click(object sender, RoutedEventArgs args)
        {
            if (LPointsList.Count == RPointsList.Count)
            {
                VertexNet vn = new VertexNet();
                vn.Generate(LPointsList, RPointsList, 5f / 6f);
                SharpGLWin win = new SharpGLWin(this, vn);
                win.Show();
            }
            else
            {
                MessageBox.Show("Count of left points not equals to count of right");
            }
        }

        private void delPoint_List1_handler(object sender, KeyEventArgs e)
        {
            if (LPoints.SelectedIndex != -1 && e.Key == Key.Delete)
            {
                Cnv.Children.RemoveAt(LPoints.SelectedIndex - 1);
                LPointsList.RemoveAt(LPoints.SelectedIndex - 1);
                LPoints.Items.Remove(LPoints.SelectedItem);
            }
        }

        private void delPoint_List2_handler(object sender, KeyEventArgs e)
        {
            if (RPoints.SelectedIndex != -1 && e.Key == Key.Delete)
            {
                Cnv2.Children.RemoveAt(RPoints.SelectedIndex - 1);
                RPointsList.RemoveAt(RPoints.SelectedIndex - 1);
                RPoints.Items.Remove(RPoints.SelectedItem);
            }
        }

        private List<Pair> LPointsList;
        private List<Pair> RPointsList;

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Fill = Brushes.Sienna;
            ellipse.Width = 5;
            ellipse.Height = 5;
            ellipse.StrokeThickness = 2;

            Image outload = args.Source as Image;

            ListBox pointList = null;

            List<Pair> PointsList = null;

            int x = (int)args.GetPosition(outload).X;
            int y = (int)args.GetPosition(outload).Y;

            if (outload.Name[outload.Name.Length - 1] == '2')
            {
                pointList = RPoints;
                Cnv2.Children.Add(ellipse);
                PointsList = RPointsList;
            }
            else
            {
                pointList = LPoints;
                Cnv.Children.Add(ellipse);
                PointsList = LPointsList;
            }
            
            PointsList.Add(new Pair((x - (ellipse.Width / 2))/ outload.ActualWidth, -(y - (ellipse.Width / 2))/ outload.ActualHeight));

            Canvas.SetLeft(ellipse, x - (ellipse.Width / 2));
            Canvas.SetTop(ellipse, y - (ellipse.Height / 2));

            ListBoxItem el = new ListBoxItem() {Content = "X: " + x + ", Y: " + y };            

            pointList.Items.Add(el);
        }
    }
}
