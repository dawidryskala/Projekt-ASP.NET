using Data.Entities;
using WebApplication1.Models;

namespace WebApplication1.Mappers
{
    public class CourseMapper
    {
        public static CourseViewModel FromEntity(CourseEntity entity)
        {
            return new CourseViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Credits = entity.Credits,
                InstructorId = entity.InstructorId,
                InstructorName = entity.Instructor?.Name
            };
        }

        public static CourseEntity ToEntity(CourseViewModel model)
        {
            return new CourseEntity
            {
                Id = model.Id ?? 0, // Obsługa nullable Id
                Title = model.Title,
                Credits = model.Credits,
                InstructorId = model.InstructorId
            };
        }
    }
}
