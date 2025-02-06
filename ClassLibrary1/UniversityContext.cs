using Data.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Data
{
    public class UniversityContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<CourseEntity> Courses { get; set; }
        public DbSet<InstructorEntity> Instructors { get; set; }
        public DbSet<EnrollmentEntity> Enrollments { get; set; } // Definicja tabeli Enrollments

        public DbSet<ExamEntity> Exams { get; set; }


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
            base.OnModelCreating(modelBuilder);
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


            string ADMIN_ID = Guid.NewGuid().ToString();
            string ROLE_ID = Guid.NewGuid().ToString();

            // dodanie roli administratora
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "admin",
                NormalizedName = "ADMIN",
                Id = ROLE_ID,
                ConcurrencyStamp = ROLE_ID
            });

            // utworzenie administratora jako użytkownika
            var admin = new IdentityUser
            {
                Id = ADMIN_ID,
                Email = "adminuser@wsei.edu.pl",
                EmailConfirmed = true,
                UserName = "adminuser@wsei.edu.pl",
                NormalizedUserName = "ADMINUSER@WSEI.EDU.PL",
                NormalizedEmail = "ADMINUSER@WSEI.EDU.PL"
            };

            // haszowanie hasła, najlepiej wykonać to poza programem i zapisać gotowy
            // PasswordHash
            PasswordHasher<IdentityUser> ph = new PasswordHasher<IdentityUser>();
            admin.PasswordHash = ph.HashPassword(admin, "S3cretPassword");

            // zapisanie użytkownika
            modelBuilder.Entity<IdentityUser>().HasData(admin);

            // przypisanie roli administratora użytkownikowi
            modelBuilder.Entity<IdentityUserRole<string>>()
            .HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });
        }
    }
}
