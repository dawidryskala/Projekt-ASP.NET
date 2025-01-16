namespace WebApplication1.Models
{
    public class InstructorViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string AcademicTitle { get; set; }
        public List<CourseViewModel>? Courses { get; set; } // Opcjonalne, dla wyświetlania powiązanych kursów
    }
}
