using ExamProject.Data.Contexts;
using ExamProject.Data.IRepositories;
using ExamProject.Domain.Entities;

namespace ExamProject.Data.Repositories
{
    public class StudentRepo : GenericRepo<Student>, IStudentRepo
    {

    }
}
