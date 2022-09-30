using ExamProject.Domain.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamProject.Domain.Entities
{
    public class Student : Auditable
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;

        public long? PasspostId { get; set; }
        [ForeignKey("PassportId")]
        public Attachment Passport { get; set; }

        public long? ImageId { get; set; }
        [ForeignKey("ImageId")]
        public Attachment Image { get; set; }

        public Student()
        {
            Passport = new Attachment();
            Image = new Attachment();
        }
    }
}
