using Microsoft.AspNetCore.Http;

namespace ExamProject.Service.DTOs
{
    public class StudentFileForCreation
    {
        public IFormFile Passport { get; set; }
        public IFormFile Image { get; set; }
    }
}
