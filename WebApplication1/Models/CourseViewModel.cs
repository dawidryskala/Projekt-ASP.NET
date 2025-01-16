namespace WebApplication1.Models
{
    public class CourseViewModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; } // Liczba punktów kursu
        public int InstructorId { get; set; } // ID instruktora
        public string? InstructorName { get; set; } // Nazwa instruktora (dla wyświetlania w widoku)
    }
}
