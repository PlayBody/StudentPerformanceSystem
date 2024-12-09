using Microsoft.EntityFrameworkCore;
using StudentPerformanceSystem.Models;

namespace StudentPerformanceSystem.Data
{
    public class StudentPerformanceContext(DbContextOptions<StudentPerformanceContext> options) : DbContext(options)
    {

        // DbSets for each entity in the system
        public DbSet<User>? Users { get; set; }
        public DbSet<Student>? Students { get; set; }
        public DbSet<Test>? Tests { get; set; }
        public DbSet<Score>? Scores { get; set; }
        public DbSet<Class>? Classes { get; set; }
        public DbSet<Teacher>? Teachers { get; set; }
        public DbSet<School>? Schools { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationships and constraints here

            // Example: One-to-Many relationship between Class and Students
            modelBuilder.Entity<Class>()
                .HasMany(c => c.Students)
                .WithOne()
                .HasForeignKey(s => s.ClassID);

            // Example: One-to-Many relationship between School and Teachers
            modelBuilder.Entity<School>()
                .HasMany(s => s.Teachers)
                .WithOne()
                .HasForeignKey(t => t.SchoolID);

            // Example: One-to-Many relationship between Teacher and Classes
            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Classes)
                .WithOne()
                .HasForeignKey(c => c.TeacherID);

            modelBuilder.Entity<Score>()
                .HasKey(s => new { s.StudentID, s.TestID });
        }
    }
}
