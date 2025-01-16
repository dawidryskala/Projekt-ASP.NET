using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IStudentService
    {
        List<StudentViewModel> FindAll(); // Pobiera listę wszystkich studentów
        StudentViewModel? FindById(int id); // Pobiera studenta po ID
        void Add(StudentViewModel student);
        void Update(StudentViewModel student);
        void Delete(int id);

        // Dodane metody
        List<StudentViewModel> GetAllStudents(); // Pobiera wszystkich studentów (alias FindAll)
        StudentViewModel? GetStudentById(int id); // Pobiera studenta po ID (alias FindById)
    }
}
