using Data.Entities;
using WebApplication1.Models;

namespace WebApplication1.Mappers
{
    public class EnrollmentMapper
    {
        public static EnrollmentViewModel FromEntity(EnrollmentEntity entity)
        {
            return new EnrollmentViewModel
            {
                Id = entity.Id,
                CourseID = entity.CourseID,
                CourseTitle = entity.Course?.Title,
                StudentID = entity.StudentID,
                StudentName = entity.Student?.Name,
                Grade = entity.Grade // Używamy Grade jako string
            };
        }

        public static EnrollmentEntity ToEntity(EnrollmentViewModel model)
        {
            return new EnrollmentEntity
            {
                Id = model.Id ?? 0,
                CourseID = model.CourseID,
                StudentID = model.StudentID,
                Grade = model.Grade // Bez konwersji – zapisujemy jako string
            };
        }
    }
}
