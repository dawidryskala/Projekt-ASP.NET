using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface ICourseService
    {
        List<CourseViewModel> GetAllCourses();
        CourseViewModel? GetCourseById(int id);
        void AddCourse(CourseViewModel course);
        void UpdateCourse(CourseViewModel course);
        void DeleteCourse(int id);
        List<InstructorViewModel> GetAllInstructors(); // Dodano metodę
    }
}
