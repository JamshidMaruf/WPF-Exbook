using ExamProject.Service.DTOs;
using ExamProject.Service.Interfaces;
using ExamProject.Service.Services;
using ExamProject.UI.Windows;
using System.Windows;
using System.Windows.Controls;

namespace ExamProject.Pages
{
    /// <summary>
    /// Interaction logic for AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        private readonly MessageView messageView;
        public AddPage()
        {
            messageView = new MessageView();

            InitializeComponent();
        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NewStudentFirstName.Text) &&
                !string.IsNullOrEmpty(NewStudentLastName.Text) &&
                !string.IsNullOrEmpty(NewStudentFaculty.Text))
            {

                var newStudent = new StudentForCreation()
                {
                    FirstName = NewStudentFirstName.Text,
                    LastName = NewStudentLastName.Text,
                    Faculty = NewStudentFaculty.Text
                };

                using IStudentService studentService = new StudentService();

                var response = await studentService.CreateAsync(newStudent);

                if (response is null)
                {
                    ErrorResponse.Text = "Something went wrong";
                    ErrorResponse.Visibility = Visibility.Visible;
                }
                else
                    messageView.Show();
            }
            else
            {
                ErrorResponse.Text = "Please fill all fields";
                ErrorResponse.Visibility = Visibility.Visible;
            }

        }
    }
}
