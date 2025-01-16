using Data.Entities;
using WebApplication1.Models;

namespace WebApplication1.Mappers
{
    public class StudentMapper
    {
        // Mapowanie z encji (baza danych) na ViewModel (widok)
        public static StudentViewModel FromEntity(StudentEntity entity)
        {
            return new StudentViewModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                IndexNumber = entity.IndexNumber,
                Birth = entity.Birth,
            };
        }

        // Mapowanie z ViewModel (widok) na encję (baza danych)
        public static StudentEntity ToEntity(StudentViewModel model)
        {
            return new StudentEntity()
            {
                Id = model.Id.HasValue ? model.Id.Value : 0, // Obsługa nullable Id
                Name = model.Name,
                Email = model.Email,
                IndexNumber = model.IndexNumber,
                Birth = model.Birth.HasValue ? model.Birth.Value : DateTime.MinValue // Obsługa nullable Birth
            };
        }
    }
}
