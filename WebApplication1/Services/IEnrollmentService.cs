using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IEnrollmentService
    {
        List<EnrollmentViewModel> GetAllEnrollments();
        EnrollmentViewModel? GetEnrollmentById(int id);
        void AddEnrollment(EnrollmentViewModel enrollment);
        void UpdateEnrollment(EnrollmentViewModel enrollment);
        void DeleteEnrollment(int id);
    }
}
