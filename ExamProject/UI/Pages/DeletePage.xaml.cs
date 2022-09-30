using ExamProject.Service.Interfaces;
using ExamProject.Service.Services;
using ExamProject.UI.Windows;
using System.Windows;
using System.Windows.Controls;

namespace ExamProject.Pages
{
    /// <summary>
    /// Interaction logic for DeletePage.xaml
    /// </summary>
    public partial class DeletePage : Page
    {
        private readonly MessageView messageViewer;
        public DeletePage()
        {
            messageViewer = new MessageView();
            InitializeComponent();
        }

        private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var IsValidId = long.TryParse(DeletedStudentId.Text, out long result);

            if (IsValidId)
            {
                using IStudentService studentService = new StudentService();

                var response = await studentService.DeleteAsync(result);

                if (!response)
                {
                    ErrorResponse.Text = "Student not found";
                    ErrorResponse.Visibility = Visibility.Visible;
                }

                else
                    messageViewer.Show();
            }

            else
            {
                ErrorResponse.Text = "Invalid Id";
                ErrorResponse.Visibility = Visibility.Visible;
            }

        }
    }
}
