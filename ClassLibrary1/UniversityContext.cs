using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Data
{
    public class UniversityContext : DbContext
    {
        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<CourseEntity> Courses { get; set; }
        public DbSet<InstructorEntity> Instructors { get; set; }
        public DbSet<EnrollmentEntity> Enrollments { get; set; } // Definicja tabeli Enrollments


        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var path = System.IO.Path.Join(Environment.CurrentDirectory, "university1.db");
            options.UseSqlite($"Data Source={path}");
        }

        public class UniversityContextFactory : IDesignTimeDbContextFactory<UniversityContext>
        {
            public UniversityContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<UniversityContext>();
                optionsBuilder.UseSqlite("Data Source=university1.db");

                return new UniversityContext(optionsBuilder.Options);
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja relacji dla Enrollments
            modelBuilder.Entity<EnrollmentEntity>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentID);

            modelBuilder.Entity<EnrollmentEntity>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseID);

            // Seed danych dla Studentów
            modelBuilder.Entity<StudentEntity>().HasData(
                new StudentEntity { Id = 1, Name = "John Doe", Email = "john.doe@example.com", IndexNumber = "12345", Birth = new DateTime(1990, 1, 1) },
                new StudentEntity { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com", IndexNumber = "54321", Birth = new DateTime(1992, 5, 10) }
            );

            // Seed danych dla Instruktorów
            modelBuilder.Entity<InstructorEntity>().HasData(
                new InstructorEntity { Id = 1, Name = "Dr. Alan Turing", AcademicTitle = "PhD" },
                new InstructorEntity { Id = 2, Name = "Dr. Grace Hopper", AcademicTitle = "PhD" }
            );

            // Seed danych dla Kursów
            modelBuilder.Entity<CourseEntity>().HasData(
                new CourseEntity { Id = 1, Title = "Mathematics", InstructorId = 1, Credits = 5 },
                new CourseEntity { Id = 2, Title = "Physics", InstructorId = 2, Credits = 4 }
            );

            // Seed danych dla Enrollment (przykładowe zapisy)
            modelBuilder.Entity<EnrollmentEntity>().HasData(
                new EnrollmentEntity { Id = 1, StudentID = 1, CourseID = 1, Grade = "A" },
                new EnrollmentEntity { Id = 2, StudentID = 2, CourseID = 2, Grade = "B" }
            );
        }
    }
}
