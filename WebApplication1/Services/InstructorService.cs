using Data;
using Data.Entities;
using WebApplication1.Mappers;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly UniversityContext _context;

        public InstructorService(UniversityContext context)
        {
            _context = context;
        }

        public List<InstructorViewModel> GetAllInstructors()
        {
            return _context.Instructors.Select(InstructorMapper.FromEntity).ToList();
        }

        public InstructorViewModel? GetInstructorById(int id)
        {
            var instructor = _context.Instructors.Find(id);
            return instructor != null ? InstructorMapper.FromEntity(instructor) : null;
        }

        public void AddInstructor(InstructorViewModel instructor)
        {
            var entity = InstructorMapper.ToEntity(instructor);
            _context.Instructors.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateInstructor(InstructorViewModel instructor)
        {
            var entity = InstructorMapper.ToEntity(instructor);
            _context.Instructors.Update(entity);
            _context.SaveChanges();
        }

        public void DeleteInstructor(int id)
        {
            var instructor = _context.Instructors.Find(id);
            if (instructor != null)
            {
                Console.WriteLine($"Instructor found for deletion: {instructor.Name}");
                _context.Instructors.Remove(instructor);
                _context.SaveChanges();
            }
            else
            {
                Console.WriteLine($"Instructor with Id: {id} not found.");
            }
        }
    }
}
