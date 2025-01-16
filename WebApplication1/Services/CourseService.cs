using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Mappers;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class CourseService : ICourseService
    {
        private readonly UniversityContext _context;

        public CourseService(UniversityContext context)
        {
            _context = context;
        }

        public List<CourseViewModel> GetAllCourses()
        {
            return _context.Courses
                .Include(c => c.Instructor)
                .Select(c => new CourseViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Credits = c.Credits,
                    InstructorId = c.InstructorId,
                    InstructorName = c.Instructor.Name
                })
                .ToList();
        }

        public CourseViewModel? GetCourseById(int id)
        {
            var course = _context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefault(c => c.Id == id);

            if (course == null) return null;

            return new CourseViewModel
            {
                Id = course.Id,
                Title = course.Title,
                Credits = course.Credits,
                InstructorId = course.InstructorId,
                InstructorName = course.Instructor.Name
            };
        }

        public void AddCourse(CourseViewModel course)
        {
            var entity = CourseMapper.ToEntity(course);
            _context.Courses.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateCourse(CourseViewModel course)
        {
            var entity = CourseMapper.ToEntity(course);
            _context.Courses.Update(entity);
            _context.SaveChanges();
        }

        public void DeleteCourse(int id)
        {
            var course = _context.Courses.Find(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
        }

        public List<InstructorViewModel> GetAllInstructors() // Dodano implementację metody
        {
            return _context.Instructors
                .Select(InstructorMapper.FromEntity)
                .ToList();
        }
    }
}
