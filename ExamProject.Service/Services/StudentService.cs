using ExamProject.Data.Contexts;
using ExamProject.Data.IRepositories;
using ExamProject.Data.Repositories;
using ExamProject.Domain.Entities;
using ExamProject.Service.Configurations;
using ExamProject.Service.DTOs;
using ExamProject.Service.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace ExamProject.Service.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepo studentRepo;

        public StudentService()
        {
            studentRepo = new StudentRepo();
        }

        public async Task<Student> CreateAsync(StudentForCreation dto)
        {

            using HttpClient client = new();

            string json = JsonConvert.SerializeObject(dto);
            HttpResponseMessage response = await client.PostAsync(AppSettings.API_PATH + "students",
                new StringContent(json, Encoding.Default, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                string message = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Student>(message);

                await studentRepo.CreateAsync(result);

                await studentRepo.SaveChangesAsync();

                return result;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            using HttpClient client = new();
            var result = await client.DeleteAsync(AppSettings.API_PATH + "students/" + id);

            if (result.IsSuccessStatusCode)
            {
                await studentRepo.DeleteAsync(s => s.Id == id);

                await studentRepo.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            using HttpClient client = new();
            string encoded = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(AppSettings.Login + ":" + AppSettings.Password));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);

            var response = await client.GetAsync(AppSettings.API_PATH + "students/");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<Student>>(content);

            }

            return null;

        }
        public async Task<Student> GetAsync(long id)
        {
            using HttpClient client = new ();
            return await client.GetFromJsonAsync<Student>(AppSettings.API_PATH + "students/" + id);
        }

        public async Task<Student> UpdateAsync(long id, StudentForCreation dto)
        {
            using HttpClient client = new ();
            string json = JsonConvert.SerializeObject(dto);
            HttpResponseMessage response = await client.PutAsync(AppSettings.API_PATH + "students/" + id,
                new StringContent(json, Encoding.Default, "application/json"));


            if (response.IsSuccessStatusCode)
            {
                string message = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Student>(message);

                studentRepo.UpdateAsync(result);

                await studentRepo.SaveChangesAsync();

                return result;
            }

            return null;
        }

        public async Task UploadFilesAsync(long id, string imagePath, string passportPath)
        {
            using HttpClient client = new ();
            MultipartFormDataContent formData = new ();
            formData.Add(new StreamContent(File.OpenRead(imagePath)), "image", "image.png");
            formData.Add(new StreamContent(File.OpenRead(passportPath)), "passport", "passport.png");

            HttpResponseMessage response = await client.PostAsync(AppSettings.API_PATH + "students/attachments/" + id, formData);

            string message = await response.Content.ReadAsStringAsync();

        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
