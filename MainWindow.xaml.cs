using EasyBookFront.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace EasyBookFront
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var responseGetAll = InteractionWithApi.GetAll();
            CreateCards(responseGetAll);
        }

        public void CreateCards(List<Book> books)
        {
            exlporer.Children.Clear();
            foreach(var book in books)
            {
                exlporer.Children.Add(createBtn(book));
            }
        }

        private Border createBtn(Book item)
        {
            Border border = new Border()
            {
                BorderBrush = Brushes.Red,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(4),
                Height = 64
            };

            var stck = new DockPanel() {
                LastChildFill = true,
                Margin = new Thickness(4),
            };
            var img = new Image() { Source = LoadImage(item.ImageByte) };
            DockPanel.SetDock(img, Dock.Left);
            stck.Children.Add(img);

            Button delBtn = new Button() { Tag = item.BookName, Margin = new Thickness(4), Width = 36, Content = "X" };
            DockPanel.SetDock(delBtn, Dock.Right);
            delBtn.Click += DeleteBtnClick;
            stck.Children.Add(delBtn);

            Button editBtn = new Button() { Tag = item.BookName, Margin = new Thickness(4), Width = 36, Content = "Edit" };
            DockPanel.SetDock(editBtn, Dock.Right);
            editBtn.Click += EditBtnClick;
            stck.Children.Add(editBtn);

            var text = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 18,
                Text = item.BookName,
                Margin = new Thickness(4)
            };
            DockPanel.SetDock(text, Dock.Left);
            stck.Children.Add(text);
            border.Child = stck;
            return border;
        }

        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        private void DeleteBtnClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var btn = sender as Button;
            var name = btn.Tag.ToString();

            InteractionWithApi.Delete(name);
            var responseGetAll = InteractionWithApi.GetAll();
            CreateCards(responseGetAll);
        }

        private void EditBtnClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var btn = sender as Button;
            var name = btn.Tag.ToString();

            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Files (*.jpg)|*.jpg"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    byte[] bytes = File.ReadAllBytes(filename);
                    Book book = new Book() { BookName = name, ImageByte = bytes };
                    InteractionWithApi.Put(name, book);
                }
            }

            var responseGetAll = InteractionWithApi.GetAll();
            CreateCards(responseGetAll);
        }

        private void InputFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Files (*.jpg)|*.jpg"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    ((Button)sender).Tag = filename;
                }
            }
        }

        private void SendFile(object sender, RoutedEventArgs e)
        {
            byte[] bytes;
            try
            {
                bytes = File.ReadAllBytes(OutputFile.Text);
            }
            catch
            {
                MessageBox.Show($"Не удалось прочитать файл {OutputFile.Text}");
                return;
            }

            if(string.IsNullOrWhiteSpace(FileNameBox.Text))
            {
                MessageBox.Show($"Название не может быть пустым");
                return;
            }

            Book book = new Book() { BookName = FileNameBox.Text, ImageByte = bytes };
            InteractionWithApi.Post(book);

            var responseGetAll = InteractionWithApi.GetAll();
            CreateCards(responseGetAll);
        }
    }
}
