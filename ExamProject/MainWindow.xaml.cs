using ExamProject.Domain.Entities;
using ExamProject.Pages;
using ExamProject.Service.Services;
using ExamProject.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ExamProject
{
#pragma warning disable
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StudentService studentService;
        private Thread thread;
        public MainWindow()
        {
            studentService = new StudentService();

            InitializeComponent();
        }

        private IEnumerable<Student> students;

        

        private void SearchBar_TextChenged(object sender, RoutedEventArgs e)
        {
            StudentsList.Items.Clear();

            string text = SearchBar.Text.ToLower();


            thread = new Thread(async () =>
            {
                students = await studentService.GetAllAsync();

                students = students.Where(p => p.FirstName.ToLower().Contains(text)
                    || p.LastName.ToLower().Contains(text)
                    || p.Faculty.ToLower().Contains(text));

                await LoadStudents(students);
            });
            thread.Start();
        }

        private async Task LoadStudents(IEnumerable<Student> users)
        {
            foreach (var user in users)
            {
                await this.Dispatcher.InvokeAsync(() =>
                {
                    PrivateChat privateChat = new ();
                    privateChat.NameTxt.Text = user.FirstName + " " + user.LastName;
                    privateChat.FacultyMsgTxt.Text = user.Faculty;
                    privateChat.UserImg.ImageSource = user.Image is not null
                        ? new BitmapImage(new Uri("https://talabamiz.uz/" + user.Image.Path))
                        : new BitmapImage(new Uri("https://talabamiz.uz/Images//99daf8ac38de4433aa36a61baf4c9c4d.png"));

                    StudentsList.Items.Add(privateChat);
                });
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            InputerArea.Content = new SavePage();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            InputerArea.Content = new DeletePage();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            InputerArea.Content = new AddPage();
        }

        private void CloseWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            thread = new Thread(async () =>
            {
                Dispatcher.Invoke(() => StudentsList.Items.Clear());

                students = await studentService.GetAllAsync();

                await LoadStudents(students);
            });

            thread.Start();
        }
    }
}
