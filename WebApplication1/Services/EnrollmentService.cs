using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Mappers;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly UniversityContext _context;

        public EnrollmentService(UniversityContext context)
        {
            _context = context;
        }

        public List<EnrollmentViewModel> GetAllEnrollments()
        {
            return _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Select(EnrollmentMapper.FromEntity)
                .ToList();
        }

        public EnrollmentViewModel? GetEnrollmentById(int id)
        {
            var enrollment = _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefault(e => e.Id == id);
            return enrollment != null ? EnrollmentMapper.FromEntity(enrollment) : null;
        }

        public void AddEnrollment(EnrollmentViewModel enrollment)
        {
            var entity = EnrollmentMapper.ToEntity(enrollment);
            _context.Enrollments.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateEnrollment(EnrollmentViewModel enrollment)
        {
            var entity = EnrollmentMapper.ToEntity(enrollment);
            _context.Enrollments.Update(entity);
            _context.SaveChanges();
        }

        public void DeleteEnrollment(int id)
        {
            var enrollment = _context.Enrollments.Find(id);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                _context.SaveChanges();
            }
        }
    }
}
