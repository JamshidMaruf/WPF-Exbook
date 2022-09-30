using ExamProject.Domain.Commons;

namespace ExamProject.Domain.Entities
{
    public class Attachment : Auditable
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
    }
}
