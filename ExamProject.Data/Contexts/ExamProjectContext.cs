
using ExamProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamProject.Data.Contexts
{
    public class ExamProjectContext : DbContext
    {
        private readonly string ConnectionString = "Host=localhost;Port=5432;Database=StudentDb;Username=postgres;Password=jama1226;";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
        }

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Attachment> Attachements { get; set; }

    }
}
