using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IInstructorService
    {
        List<InstructorViewModel> GetAllInstructors();
        InstructorViewModel? GetInstructorById(int id);
        void AddInstructor(InstructorViewModel instructor);
        void UpdateInstructor(InstructorViewModel instructor);
        void DeleteInstructor(int id);
    }
}
