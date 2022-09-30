using ExamProject.Domain.Entities;
using ExamProject.Service.DTOs;

namespace ExamProject.Service.Interfaces
{
    public interface IStudentService : IDisposable
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student> GetAsync(long id);
        Task<Student> CreateAsync(StudentForCreation dto);
        Task<Student> UpdateAsync(long id, StudentForCreation dto);
        Task<bool> DeleteAsync(long id);
        Task UploadFilesAsync(long id, string imagePath, string passportPath);
    }
}
