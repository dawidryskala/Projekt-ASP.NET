using Data;
using Data.Entities;
using WebApplication1.Mappers;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class StudentService : IStudentService
    {
        private readonly UniversityContext _context;

        public StudentService(UniversityContext context)
        {
            _context = context;
        }

        public List<StudentViewModel> FindAll()
        {
            return _context.Students.Select(StudentMapper.FromEntity).ToList();
        }

        public StudentViewModel? FindById(int id)
        {
            var student = _context.Students.Find(id);
            return student != null ? StudentMapper.FromEntity(student) : null;
        }

        public void Add(StudentViewModel student)
        {
            var entity = StudentMapper.ToEntity(student);
            _context.Students.Add(entity);
            _context.SaveChanges();
        }

        public void Update(StudentViewModel student)
        {
            var entity = StudentMapper.ToEntity(student);
            _context.Students.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
        }

        public List<StudentViewModel> GetAllStudents()
        {
            return FindAll(); // Alias dla istniejącej metody
        }

        public StudentViewModel? GetStudentById(int id)
        {
            return FindById(id); // Alias dla istniejącej metody
        }
    }
}
