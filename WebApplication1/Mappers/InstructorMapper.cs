using Data.Entities;
using WebApplication1.Models;

namespace WebApplication1.Mappers
{
    public class InstructorMapper
    {
        public static InstructorViewModel FromEntity(InstructorEntity entity)
        {
            return new InstructorViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                AcademicTitle = entity.AcademicTitle
            };
        }

        public static InstructorEntity ToEntity(InstructorViewModel model)
        {
            return new InstructorEntity
            {
                Id = model.Id ?? 0,
                Name = model.Name,
                AcademicTitle = model.AcademicTitle
            };
        }
    }
}
