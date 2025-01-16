namespace WebApplication1.Models
{
    public class EnrollmentViewModel
    {
        public int? Id { get; set; }
        public int CourseID { get; set; }
        public string? CourseTitle { get; set; }
        public int StudentID { get; set; }
        public string? StudentName { get; set; }
        public string Grade { get; set; } // Typ zmieniony na string
    }
}
