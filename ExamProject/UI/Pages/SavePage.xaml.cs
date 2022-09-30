using ExamProject.Service.DTOs;
using ExamProject.Service.Interfaces;
using ExamProject.Service.Services;
using ExamProject.UI.Windows;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ExamProject.Pages
{
    /// <summary>
    /// Interaction logic for SavePage.xaml
    /// </summary>
    public partial class SavePage : Page
    {
        private string imagePath;
        private string passportImagePath;
        private readonly MessageView messageView;

        public SavePage()
        {
            messageView = new MessageView();
            InitializeComponent();
        }

        private async void StudentSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var IsValidId = long.TryParse(StudentId.Text, out long result);

            if (IsValidId)
            {
                using IStudentService studentService = new StudentService();

                var oldStudent = await studentService.GetAsync(result);

                if (oldStudent is null)
                {
                    ErrorResponse.Text = "Student not fount";
                    ErrorResponse.Visibility = Visibility.Visible;

                    return;
                }

                var updateStudentInfo = new StudentForCreation();

                if (!string.IsNullOrEmpty(StudentFirstName.Text))
                    updateStudentInfo.FirstName = StudentFirstName.Text;
                else
                    updateStudentInfo.FirstName = oldStudent.FirstName;

                if (!string.IsNullOrEmpty(StudentLastName.Text))
                    updateStudentInfo.LastName = StudentLastName.Text;
                else
                    updateStudentInfo.LastName = oldStudent.LastName;

                if (!string.IsNullOrEmpty(StudentFaculty.Text))
                    updateStudentInfo.Faculty = StudentFaculty.Text;
                else
                    updateStudentInfo.Faculty = oldStudent.Faculty;


                var response = await studentService.UpdateAsync(result, updateStudentInfo);

                if (imagePath != null && passportImagePath != null)
                    await studentService.UploadFilesAsync(oldStudent.Id, imagePath, passportImagePath);

                else if (imagePath != null && passportImagePath == null ||
                    imagePath == null && passportImagePath != null)
                {
                    ErrorResponse.Text = "Please upload both images";
                    ErrorResponse.Visibility = Visibility.Visible;
                }

                if (response is not null)
                    messageView.Show();

            }
            else
            {
                ErrorResponse.Text = "Please enter a valid ID";
                ErrorResponse.Visibility = Visibility.Visible;
            }
        }

        private void ImageUploader_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files(*.PNG,*.JPG;)|*.JPG;*.PNG",

                InitialDirectory = Environment.GetFolderPath
                (Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() is true)
            {
                imagePath = openFileDialog.FileName;

                Image.Text = imagePath.Split('\\').Last();
            }
        }

        private void PasspostImageBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files(*.PNG,*.JPG;)|*.JPG;*.PNG",
                InitialDirectory = Environment.GetFolderPath
                (Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                passportImagePath = openFileDialog.FileName;

                PassportImage.Text = passportImagePath.Split('\\').Last();
            }
        }
    }
}
